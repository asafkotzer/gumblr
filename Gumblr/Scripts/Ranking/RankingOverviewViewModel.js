var RankingOverviewViewModel = function (model) {
    var self = this;
    this.model = model;

    var users = [];
    model.Users.forEach(function (userItem) {
        var userScoreItem = {
            user: userItem,
            userScore: model.ScoreByUserId[userItem.Id],
        };

        userScoreItem.isWinner = userScoreItem.userScore.Score == model.MaxScore;
        userScoreItem.isLoser = userScoreItem.userScore.Score == model.MinScore;

        users.push(userScoreItem);
    });

    this.users = ko.observableArray(users);
};