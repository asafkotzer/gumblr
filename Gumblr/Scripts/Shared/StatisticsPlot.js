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

    this.showStatisticsDistribution = function (data, target) {
        var barChartData = [];
        for (var score in data) {
            var dataItem = [];
            dataItem.push(score, data[score]);
            barChartData.push(dataItem);
        }
        var dataPoints = [{ 
            data: barChartData, 
            bars: { show: true, barWidth: 2, align: "center", fillColor: "#4572A7", fill: true, lineWidth: 0 }
        }];
        $.plot(target, dataPoints, {
            xaxis: { tickLength: 0 },
            series: { shadowSize: 1 },
        });
    };
}