var MatchesViewModel = function (model) {
    var self = this;
    this.model = model;

    var createResultByEnumValue = function (host, visitor) {
        return {
            "-1": "Unknown",
            "0": host,
            "1": "Draw",
            "2": visitor,
        };
    };
    var createEnumValueByResult = function (host, visitor) {
        var enumValueByResult = {};
        enumValueByResult["Unknown"] = '-1';
        enumValueByResult[host] = '0';
        enumValueByResult["Draw"] = '1';
        enumValueByResult[visitor] = '2';
        return enumValueByResult;
    };

    var matches = [];
    model.Matches.forEach(function (matchItem) {
        matchItem.resultByEnumValue = createResultByEnumValue(matchItem.Host, matchItem.Visitor);
        matchItem.enumValueByResult = createEnumValueByResult(matchItem.Host, matchItem.Visitor);
        
        matchItem.selectedResult = ko.observable(matchItem.resultByEnumValue[matchItem.ActualResult]);

        matchItem.updateActualResult = function () {
            matchItem.ActualResult = matchItem.enumValueByResult[matchItem.selectedResult()];
        };

        matches.push({
            match: matchItem,
            possibleResults: ["Unknown", matchItem.Host, "Draw", matchItem.Visitor],
        });
    });

    this.matches = ko.observableArray(matches);

    var prepareModelForUpload = function (model) {
        model.Matches.forEach(function (x) {
            x.updateActualResult();
            x.StartTime = moment(x.StartTime).format();
        });
        return ko.toJSON(model);
    };

//    var getPossibleTeams = function () {
//        var possibleTeamsUnique = {};
//        model.Matches.forEach(function (x) {
//            possibleTeamsUnique[x.Host] = true;
//            possibleTeamsUnique[x.Visitor] = true;
//        });
//
//        var possibleTeams = [];
//        for (var key in possibleTeamsUnique) {
//            possibleTeams.push(key);
//        }
//
//        return possibleTeams;
//    };
//
//    this.possibleTeams = getPossibleTeams();
//
//    this.addMatch = function () {
//        var matchItem = { ActualResult: -1, Group: "A", Host: "NewHost", Stage: 0, StartTime: "/Date(1356991200000)/", Venue: "NewVenue", Visitor: "NewVisitor" };
//        self.matches.push({ match: matchItem, possibleResults: ["Unknown"] });
//    };

    this.submit = function () {
        request = $.ajax({
            url: "/GroupAdmin/UpdateMatches",
            type: "post",
            data: prepareModelForUpload(model),
            contentType: 'application/json',
        }).done(function (result) {
            if (result && result.status == "success") {
                //TODO: indicate success/failure better

                alert("Upload results redirect succeeded");
            }
            else {
                console.log("Upload results redirect failed: " + JSON.stringify(result));
            }
        }).fail(function (result) {
            console.log("failed to upload results: " + JSON.stringify(result));
        });
    };
};