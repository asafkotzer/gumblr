var ViewModel = function (model) {
    var self = this;

    this.model = model;
    this.currentGameIndex = ko.observable(0);
    this.currentGame = ko.observable(model.Matches[0]);

    this.uploadBets = function () {
        request = $.ajax({
            url: "/Betting/PlaceBets",
            type: "post",
            data: JSON.stringify(model),
            contentType: 'application/json',
        }).done(function () {
            console.log("success");
        }).fail(function () {
            console.log("failure");
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
        var matchesLeft = self.model.Matches.length - self.currentGameIndex();
        if (matchesLeft == 1) {
            return "last bet";
        }
        return matchesLeft + " more to go";
    }, this);

};

