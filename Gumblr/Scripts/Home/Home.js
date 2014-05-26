var Home = function () {
    this.init = function () {
        if (location.search.indexOf("invite=true") > 0) {
            toastr.success("Waiting for Shachar now", "Your request was submitted");
        }
    };
};