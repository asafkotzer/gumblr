var EnterResults = function (model) {

    this.model = model;

    this.init = function () {
        ko.applyBindings(new EnterResultsViewModel(this.model));
    }
};