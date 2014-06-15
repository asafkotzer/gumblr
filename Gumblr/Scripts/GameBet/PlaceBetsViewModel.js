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
            var plot = new StatisticsPlot();
            plot.showStatisticsPie(result, target);

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

        if (match.HasStarted) { return; }

        var startTime = new Date(parseInt(match.StartTime.substr(6, 12)));
        if (startTime.getTime() > new Date().getTime()) {
            mixpanel.track('late bet change', {
                'matchId': viewModel.match.MatchId,
            });
            toastr.error("Too late, the game already started", "That's low");
        }

        var target = $(event.currentTarget);
        if (target.hasClass('Host')) {
            match.ExpectedResult(0);
        } else if (target.hasClass('Visitor')) {
            match.ExpectedResult(2);
        } else {
            if (match.Stage > 1) {
                console.log("Draw is not a valid result on a playoff match");
                return;
            }
            match.ExpectedResult(1);
        }
    };

    var getFlag = function (country, modifier) {
        var suffix = "";
        if (modifier == "glow") suffix = "-glow";
        if (modifier == "bw") suffix = "-bw";
        return "/Images/Flags/" + country.toLowerCase().replace(' ', '-') + suffix + ".png";
    };

    var getDrawIcon = function (modifier) {
        var suffix = "";
        if (modifier == "glow") suffix = "-glow";
        if (modifier == "bw") suffix = "-bw";
        return "/Images/draw" + suffix + ".png";
    };

    var matches = [];
    model.Matches.forEach(function (matchItem) {
        var currentExpectedResult = matchItem.ExpectedResult;

        matchItem.onMouseIn = function (viewModel, event) {

            if (matchItem.HasStarted) { return;}

            var isHost = $(event.target).hasClass("Host");
            var isVisitor = $(event.target).hasClass("Visitor");
            var isDraw = $(event.target).hasClass("Tie");
            if (isHost) {
                matchItem.hostLogoUrl(getFlag(matchItem.Host, "glow"));
            }
            else if (isVisitor) {
                matchItem.visitorLogoUrl(getFlag(matchItem.Visitor, "glow"));
            }
            else if (isDraw) {
                matchItem.drawLogoUrl(getDrawIcon("glow"));
            }
        };

        matchItem.onMouseOut = function (viewModel, event) {

            if (matchItem.HasStarted) { return; }

            var isHost = $(event.target).hasClass("Host");
            var isVisitor = $(event.target).hasClass("Visitor");
            var isDraw = $(event.target).hasClass("Tie");
            if (isHost) {
                matchItem.hostLogoUrl(getFlag(matchItem.Host));
            }
            else if (isVisitor) {
                matchItem.visitorLogoUrl(getFlag(matchItem.Visitor));
            }
            else if (isDraw) {
                matchItem.drawLogoUrl(getDrawIcon());
            }
        };

        matchItem.hostLogoUrl = ko.observable(getFlag(matchItem.Host));
        matchItem.visitorLogoUrl = ko.observable(getFlag(matchItem.Visitor));

        matchItem.ExpectedResult = ko.observable(currentExpectedResult);
        matchItem.getSuccessIndicatorClass = ko.computed(function () {
            if (matchItem.ActualResult == -1) {
                return "MatchFrame unknown-bet-result";
            } else if (matchItem.ExpectedResult() == matchItem.ActualResult) {
                return "MatchFrame successful-bet";
            } else {
                return "MatchFrame failed-bet";
            }
        }, this);

        matchItem.showMatchDetailsButtonClick = function () {
            var url = '/MatchDetails/' + matchItem.MatchId;
            window.open(url, '_blank');
        }

        matchItem.showMatchDetailsBackgroundClick = function () {
            if (!matchItem.HasStarted) {
                return;
            }
            var url = '/MatchDetails/' + matchItem.MatchId;
            window.open(url, '_blank');
        }

        matchItem.Stage = ko.observable(matchItem.Stage);
        matchItem.drawLogoUrl = ko.observable("/Images/draw.png");
        matchItem.isHidden = ko.observable(false);

        matches.push({
            match: matchItem,
            selected: onGameBetClick,
            showStatistics: onShowStatisticsClick,
            getMatchContainerClass: ko.computed(function () {
                if (matchItem.isHidden() == true) {
                    return "HiddenMatchContainer";
                }
                return matchItem.HasStarted ? "StartedMatchContainer" : "MatchContainer";
            }),
        });
    });
    this.matches = ko.observableArray(matches);
    this.showingStartedMatches = ko.observable(true);
    this.hideButtonText = ko.computed(function () {
        if (self.showingStartedMatches()) {
            return "hide started matches";
        } else {
            return "show all matches";
        }
    });
    this.hideStartedMatches = function () {
        if (self.showingStartedMatches()) {
            self.showingStartedMatches(false);
            self.matches().forEach(function (matchWrapper) {
                if (matchWrapper.match.HasStarted) {
                    matchWrapper.match.isHidden(true);
                }
            });
        } else {
            self.showingStartedMatches(true);
            self.matches().forEach(function (matchWrapper) {
                matchWrapper.match.isHidden(false);
            });
        }
    };
    this.winner = ko.observable(model.Winner);
    this.possibleWinners = ko.observable(model.PossibleWinners);
    this.winnerLogo = ko.computed(function () {
        if (self.winner() == null) {
            return "/Images/QuestionMark.png";
        }
        return "/Images/Flags/" + self.winner().toLowerCase().replace(' ', '-') + ".png";
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
                toastr.error("Please try again later", "We can't update your bets right now");
            }
        }).fail(function (result) {
            toastr.error("Please try again later", "We can't update your bets right now");
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

