var MatchDetails = function (model) {

    this.model = model;

    this.init = function () {
        ko.applyBindings(new MatchDetailsViewModel(this.model));
    }
};