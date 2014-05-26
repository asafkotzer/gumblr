var PlaceBetsViewModel = function (model) {
    var self = this;

    this.model = model;

    var getStatistics = function (matchId, loader, target) {
        mixpanel.track('statistics request', {
            'matchId': matchId,
        });

        request = $.ajax({
            url: "/BetStatistics/Match/" + matchId,
            type: "get",
        }).done(function (result) {
            loader.hide();

            var data = [
                { label: result.Match.Host, data: 0 },
                { label: "X", data: 0 },
                { label: result.Match.Visitor, data: 0 }];

            for (var key in result.ExpectedResultByUserId) {
                var expectedResult = result.ExpectedResultByUserId[key];
                if (expectedResult == -1) continue;
                data[expectedResult].data++;
            }
                      
            $.plot(target, data, 
                {
                    series: {
                        pie: {
                            show: true,
                            radius: 1,
                            label: {
                                show: true,
                                formatter: function (label, series) { return label.substr(0, 3).toUpperCase(); },
                            }
                        }
                    },
                    legend: {
                        show: false
                    }

                });

        }).fail(function (result) {
            loader.hide();

            console.log("Fail: " + JSON.stringify(result));
        });
    };

    var onShowStatisticsClick = function (viewModel, event) {
        var parent = $(event.target.parentElement.parentElement)
        var target = parent.children(".LoaderContainer");

        if (target.children(".Spinner").length > 0) {
            return;
        }

        var statisticsLoader = new loader();
        statisticsLoader.show(target.get(0));

        // TODO: use promise here
        getStatistics(viewModel.match.MatchId, statisticsLoader, target);
    };

    var onGameBetClick = function (viewModel, event) {
        var match = viewModel.match;
        var startTime = new Date(parseInt(match.StartTime.substr(6)));
        if (startTime.getTime() > new Date().getTime()) {
            mixpanel.track('late bet change', {
                'matchId': viewModel.match.MatchId,
            });
            alert("Too late");
        }

        var target = $(event.currentTarget);
        if (target.hasClass('Host')) {
            match.ExpectedResult(0);
        } else if (target.hasClass('Visitor')) {
            match.ExpectedResult(2);
        } else {
            match.ExpectedResult(1);
        }

        mixpanel.track('bet change', {
            'matchId': viewModel.match.MatchId,
            'expectedResult': match.ExpectedResult()
        });

    };

    var matches = [];
    model.Matches.forEach(function (matchItem) {
        var currentExpectedResult = matchItem.ExpectedResult;
        matchItem.ExpectedResult = ko.observable(currentExpectedResult);
        matchItem.SuccessIndicator = ko.computed(function () {
            if (matchItem.ActualResult == -1) {
                return null;
            } else if (matchItem.ExpectedResult() == matchItem.ActualResult) {
                return "/Images/success.png";
            } else {
                return "/Images/fail.png";
            }
        }, this);

        matches.push({
            match: matchItem,
            selected: onGameBetClick,
            showStatistics: onShowStatisticsClick
        });
    });
    this.games = ko.observableArray(matches);
    this.winner = ko.observable(model.Winner);
    this.possibleWinners = ko.observable(model.PossibleWinners);
    this.winnerLogo = ko.computed(function () {
        if (self.winner() == null) {
            return "/Images/QuestionMark.png";
        }
        return model.TeamLogoUrlByTeamName[self.winner()];
    });

    var prepareModelForUpload = function (model) {
        model.Matches.forEach(function (x) { x.StartTime = moment(x.StartTime).format(); });
        model.Winner = self.winner();
        return ko.toJSON(model);
    };

    this.uploadBets = function () {
        mixpanel.track('upload bets');

        request = $.ajax({
            url: "/Betting/PlaceBets",
            type: "post",
            data: prepareModelForUpload(model),
            contentType: 'application/json',
        }).done(function (result) {
            if (result && result.status == "success") {
                toastr.success("Your bets were submitted", "Got it");
            }
            else {
                toastr.fail("Please try again later", "We can't update your bets right now");
            }
        }).fail(function (result) {
            toastr.fail("Please try again later", "We can't update your bets right now");
        });
    }

    this.statusLine = ko.computed(function () {
        var betsLeft = [];
        self.model.Matches.forEach(function (x) { if (x.ExpectedResult() == -1) betsLeft.push(x); });
        
        if (betsLeft.length == 0) {
            return "submit";
        } else {
            return betsLeft.length + " more to go";
        }
    }, this);

};

