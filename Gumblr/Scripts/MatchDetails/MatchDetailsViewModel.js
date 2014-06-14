var MatchDetailsViewModel = function (model) {
    var self = this;
    this.model = model;

    this.match = ko.observable(model.Match);

    var getUserArray = function (userList) {
        var result = [];
        if (userList) {
            userList.forEach(function (user) {
                result.push({ username: user });
            });
        }

        return result;
    }

    this.hostUsers = ko.observableArray(getUserArray(model.UserNamesByExpectedResult.Host));
    this.drawUsers = ko.observableArray(getUserArray(model.UserNamesByExpectedResult.Draw));
    this.visitorUsers = ko.observableArray(getUserArray(model.UserNamesByExpectedResult.Visitor));

    //var plot = function () {
    //    var statisticsPlot = new StatisticsPlot();
    //    statisticsPlot.show(model, $("#loader-container"));
    //}

    //plot();
}