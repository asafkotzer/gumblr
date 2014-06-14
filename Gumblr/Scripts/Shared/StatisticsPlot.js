var StatisticsPlot = function () {
    this.show =  function (matchData, target) {
        var data = [
            { label: matchData.Match.Host, data: 0 },
            { label: "X", data: 0 },
            { label: matchData.Match.Visitor, data: 0 }];

        for (var key in matchData.ExpectedResultByUsername) {
            var expectedResult = matchData.ExpectedResultByUsername[key];
            if (expectedResult == -1) continue;
            data[expectedResult].data++;
        }
                      
        $.plot(target, data, 
            {
                series: {
                    pie: {
                        show: true,
                        radius: 1,
                        label: {
                            show: true,
                            formatter: function (label, series) { return label.substr(0, 3).toUpperCase(); },
                        }
                    }
                },
                legend: {
                    show: false
                }

            });

    }
}