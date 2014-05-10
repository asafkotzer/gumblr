var RankingOverviewViewModel = function (model) {
    var self = this;
    this.model = model;

    var users = [];
    model.Users.forEach(function (userItem) {
        users.push({
            user: userItem,
            userScore: model.ScoreByUserId[userItem.Id],
        });
    });

    this.users = ko.observableArray(users);
};