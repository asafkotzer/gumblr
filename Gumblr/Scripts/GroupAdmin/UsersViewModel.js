var UsersViewModel = function(model) {
    var self = this;

    this.users = ko.observableArray();

    var addUserInternal = function (username, emailAddress, password) {
        self.users.push({ Name: username, EmailAddress: emailAddress, Password: password });
    };

    // this is a callback, so we can't overload - if we put username and emailAddress in signature, we get viewmodel etc.
    this.addUser = function () {
        addUserInternal();
    };

    var prepareModelForUpload = function (addedUsers) {
        var model = { Users: addedUsers };
        return ko.toJSON(model);
    };

    this.submit = function () {
        request = $.ajax({
            url: "/GroupAdmin/UpdateUsers",
            type: "post",
            data: prepareModelForUpload(self.users),
            contentType: 'application/json',
        }).done(function (result) {
            if (result && result.status == "success") {
                //TODO: indicate success/failure better

                alert("Upload users redirect succeeded");
            }
            else {
                console.log("Upload users redirect failed: " + JSON.stringify(result));
            }
        }).fail(function (result) {
            console.log("failed to users results: " + JSON.stringify(result));
        });
    };

    addUserInternal(model.username, model.emailAddress, model.password);
};