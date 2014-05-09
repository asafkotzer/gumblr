var PlaceBetsViewModel = function (model) {
    var self = this;

    this.model = model;

    var getStatistics = function (matchId, loader, target) {
        console.log(matchId);
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
        statisticsLoader.show(target.get(0))

        // TODO: use promise here
        getStatistics(viewModel.match.MatchId, statisticsLoader, target)
    };

    var onGameBetClick = function (viewModel, event) {
        var match = viewModel.match;
        var startTime = new Date(parseInt(match.StartTime.substr(6)));
        if (startTime.getTime() > new Date().getTime()) {
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

    var prepareModelForUpload = function (model) {
        model.Matches.forEach(function (x) { x.StartTime = moment(x.StartTime).format(); });
        return ko.toJSON(model);
    };

    this.uploadBets = function () {
        request = $.ajax({
            url: "/Betting/PlaceBets",
            type: "post",
            data: prepareModelForUpload(model),
            contentType: 'application/json',
        }).done(function (result) {
            if (result && result.redirectUrl) {
                window.location = result.redirectUrl;
            }
            else {
                console.log("Upload bets redirect failed: " + JSON.stringify(result));
            }
        }).fail(function (result) {
            console.log("failed to upload bets: " + JSON.stringify(result));
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

