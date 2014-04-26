var PlaceBetsViewModel = function (model) {
    var self = this;

    this.model = model;
    this.currentGameIndex = ko.observable(0);
    this.currentGame = ko.observable(model.Matches[0]);

    var matches = [];
    model.Matches.forEach(function (matchItem) {
        matchItem.ExpectedResult = ko.observable(-1);
        matches.push({
            match: matchItem,
            selected: function (viewModel, event) {
                var target = $(event.currentTarget);
                if (target.hasClass('Host')) {
                    viewModel.match.ExpectedResult(0);
                } else if (target.hasClass('Visitor')) {
                    viewModel.match.ExpectedResult(2);
                } else {
                    viewModel.match.ExpectedResult(1);
                }
            }
        });
    });
    this.games = ko.observableArray(matches);

    this.uploadBets = function () {
        request = $.ajax({
            url: "/Betting/PlaceBets",
            type: "post",
            data: JSON.stringify(model),
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

    this.selected = function (viewModel, event) {
        if (event.currentTarget == $(".Host")[0]) {
            self.currentGame().ExpectedResult = 0;
        } else if (event.currentTarget == $(".Visitor")[0]) {
            self.currentGame().ExpectedResult = 2;
        } else {
            self.currentGame().ExpectedResult = 1;
        }
        
        if (self.currentGameIndex() == self.model.Matches.length - 1) {
            self.uploadBets();
        }
        else {
            var newValue = self.currentGameIndex() < self.model.Matches.length ? self.currentGameIndex() + 1 : 0;
            self.currentGameIndex(newValue);
            self.currentGame(model.Matches[self.currentGameIndex()]);
        }
    };

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

