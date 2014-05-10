var RankingOverview = function (model) {

    this.model = model;

    this.init = function () {
        ko.applyBindings(new RankingOverviewViewModel(this.model));
    }
};