var Users = function (model) {

    this.init = function () {
        ko.applyBindings(new UsersViewModel(model));
    }
};