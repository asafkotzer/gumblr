var UsersViewModel = function() {
    var self = this;

    this.users = ko.observableArray();

    this.addUser = function () {
        self.users.push({ Name: "", EmailAddress: "", Password: "" });
    };

    var prepareModelForUpload = function (addedUsers) {
        var model = { Users: addedUsers };
        debugger;
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
};