var EnterResultsViewModel = function (model) {
    var self = this;
    this.model = model;

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
        });
    });
    this.matches = ko.observableArray(matches);

};