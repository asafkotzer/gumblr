var Home = function () {

    this.init = function () {

        // apply KO bindings
        ko.applyBindings(new HomeViewModel());

        $("a[rel]").overlay();
    }
};