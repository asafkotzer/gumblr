var RankingOverviewViewModel = function (model) {
    var self = this;
    this.model = model;

    var currentUserScore = -1;
    var users = [];
    model.Users.forEach(function (userItem) {
        var userScoreItem = {
            user: userItem,
            userScore: model.ScoreByUserId[userItem.Id],
        };

        userScoreItem.isWinner = userScoreItem.userScore.Score == model.MaxScore;
        userScoreItem.isLoser = userScoreItem.userScore.Score == model.MinScore;
        userScoreItem.isCurrentUser = userItem.Id == model.CurrentUserId;
        if (userScoreItem.isCurrentUser) {
            currentUserScore = model.ScoreByUserId[userItem.Id];
        }

        /*
        Must use ko.calculated because using IDs makes it impossible to distinguish between elements in the list
        */
        userScoreItem.detailsInvisible = ko.observable(true);
        userScoreItem.toggleDetails = function () {
            userScoreItem.detailsInvisible(!userScoreItem.detailsInvisible());
        };

        userScoreItem.getDetailsClass = ko.computed(function () {
            if (userScoreItem.detailsInvisible()) {
                return "invisible";
            } else {
                return "";
            }
        });

        users.push(userScoreItem);
    });

    function compare(a, b) {
        if (a.userScore.Score < b.userScore.Score)
            return 1;
        if (a.userScore.Score > b.userScore.Score)
            return -1;
        return 0;
    }

    var sortedUsers = users.sort(compare);

    this.users = ko.observableArray(users);

    var countByScore = {};
    for (var key in model.ScoreByUserId) {
        var score = model.ScoreByUserId[key].Score;

        if (!countByScore[score]) {
            countByScore[score] = 0;
        }

        countByScore[score]++;
    }

    var graphContainer = $("#score-distribution-container");
    var plot = new StatisticsPlot();
    plot.showStatisticsDistribution({countByScore: countByScore, currentUserScore : currentUserScore}, graphContainer);
};