var chartData = [];
this.getChartData();
var chart = AmCharts.makeChart("chartdiv", {
    type: "serial",
    language: "eu",
    dataProvider: this.chartData,
    categoryField: "date",
    categoryAxis: {
        parseDates: true,
        gridAlpha: 0.15,
        minorGridEnabled: true,
        axisColor: "#DADADA"
    },
    valueAxes: [{
        axisAlpha: 0.2,
        id: "v1"
    }],
    graphs: [{
        title: "red line",
        id: "g1",
        valueAxis: "v1",
        valueField: "drop",
        bullet: "round",
        bulletBorderColor: "#FFFFFF",
        bulletBorderAlpha: 1,
        lineThickness: 2,
        lineColor: "#b5030d",
        negativeLineColor: "#0352b5",
        hideBulletsCount: 30,
        balloonText: "[[category]]<br><b><span style='font-size:14px;'>value: [[value]]</span></b>"
    }],
    chartCursor: {
        fullWidth: true,
        cursorAlpha: 0.1
    },
    chartScrollbar: {
        scrollbarHeight: 40,
        color: "#FFFFFF",
        autoGridCount: true,
        graph: "g1"
    }
});
chart.addListener("dataUpdated", zoomChart);

function getChartData() {
    $.ajax({
        url: "Loto/Chart/GetHistory",
        async: false,
        success: function (response) {
            $.each(response, function (i, value) {
                var date = new Date(value.date);
                chartData.push({
                    date: date,
                    drop: value.drop
                });
            });
        },
        datatype: 'json'
    });
}

function zoomChart() {
    chart.zoomToIndexes(chartData.length - 40, chartData.length - 1);
}

function setPanSelect() {
    var chartCursor = chart.chartCursor;

    if (document.getElementById("rb1").checked) {
        chartCursor.pan = false;
        chartCursor.zoomable = true;

    } else {
        chartCursor.pan = true;
    }
    chart.validateNow();
}