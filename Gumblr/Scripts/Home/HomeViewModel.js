var HomeViewModel = function () {
    var self = this;

    this.emailAddress = ko.observable("");
    this.comments = ko.observable("");

    this.submit = function () {
        var model = {
            EmailAddress: self.emailAddress(),
            Comments: self.comments(),
        };

        request = $.ajax({
            url: "/Home/RequestInvite",
            type: "post",
            data: JSON.stringify(model),
            contentType: 'application/json',
        }).done(function (result) {
            if (result && result.status == "success") {
                //TODO: call remove overlay here
                console.log("submit succeeded");
            }
            else {
                console.log("submit failed: " + JSON.stringify(result));
            }
        }).fail(function (result) {
            console.log("failed to submit: " + JSON.stringify(result));
        });
    }

};
