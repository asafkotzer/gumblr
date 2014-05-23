var Users = function () {

    this.init = function () {
        ko.applyBindings(new UsersViewModel());
    }
};