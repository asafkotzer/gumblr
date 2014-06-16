var StatisticsPlot = function () {
    this.showStatisticsPie = function (matchData, target) {
        var data = [
            { label: matchData.Match.Host, data: 0 },
            { label: "X", data: 0 },
            { label: matchData.Match.Visitor, data: 0 }];

        for (var key in matchData.ExpectedResultByUsername) {
            var expectedResult = matchData.ExpectedResultByUsername[key];
            if (expectedResult == -1) continue;
            data[expectedResult].data++;
        }

        $.plot(target, data, {
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
    };

    this.showStatisticsDistribution = function (model, target) {
        var barChartData = [];
        for (var score in model.countByScore) {
            var dataPair = [[score, model.countByScore[score]]];
            var color = model.currentUserScore.Score == score ? "red" : "#4572A7";
            var dataItem = { data: dataPair, color: color, bars: { show: true, barWidth: 2, align: "center", fill: true, lineWidth: 0 } };
            barChartData.push(dataItem);
        }

        $.plot(target, barChartData, {
            xaxis: { tickLength: 0 },
            series: { shadowSize: 1 },
        });
    };
}