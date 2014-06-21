var PendingMatches = function (model) {

    this.model = model;

    this.init = function () {
        ko.applyBindings(new PendingMatchesViewModel(this.model));
    }
};