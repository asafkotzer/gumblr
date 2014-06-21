var PendingMatchesViewModel = function (model) {
    var self = this;
    this.model = model;

    var matches = [];
    model.NewMatches.forEach(function (matchItem) {
        matches.push({
            match: matchItem,
        });
    });

    this.matches = ko.observableArray(matches);

    this.submit = function () {
        request = $.ajax({
            url: "/GroupAdmin/SendNewMatchUpdate/" + self.model.Id,
            type: "post",
        }).done(function (result) {
            if (result && result.status == "success") {
                //TODO: indicate success/failure better
                alert("Sent emails");
                document.location.href = "/GroupAdmin/Matches";
            }
            else {
                console.log("Upload results redirect failed: " + JSON.stringify(result));
            }
        }).fail(function (result) {
            console.log("failed to upload results: " + JSON.stringify(result));
        });
    };

}