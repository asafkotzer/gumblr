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

    function compare(a, b) {
        if (a.userScore.Score < b.userScore.Score)
            return 1;
        if (a.userScore.Score > b.userScore.Score)
            return -1;
        return 0;
    }

    console.log(users);
    var sortedUsers = users.sort(compare);
    console.log(sortedUsers);

    this.users = ko.observableArray(users);
};