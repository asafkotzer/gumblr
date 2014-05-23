var Matches = function (model) {

    this.model = model;

    this.init = function () {
        ko.applyBindings(new MatchesViewModel(this.model));
    }
};