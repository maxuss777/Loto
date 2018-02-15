var chartData = this.getData(0);

var chart = AmCharts.makeChart("chartdiv", {
    type: "serial",
    theme: "dark",
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
        balloonText: "[[category]]<br><b><span style='font-size:14px;'>value: [[value]]</span></b><br><b>[[diff]]</b>"
    }],
    chartCursor: {
        fullWidth: true,
        cursorAlpha: 0.5,
        valueLineEnabled: true,
        valueLineBalloonEnabled: true
    },
    chartScrollbar: {
        scrollbarHeight: 80,
        color: "#FFFFFF",
        autoGridCount: true,
        graph: "g1"
    }
});
chart.addListener("dataUpdated", zoomChart);

function getData(position) {
    var data = { index: position };
    var chData = [];
    $.ajax({
        url: "Chart/GetHistory",
        data: data,
        async: false,
        success: function (response) {
            $.each(response, function (i, value) {
                var date = new Date(value.date);
                chData.push({
                    date: date,
                    drop: value.drop,
                    diff: value.diff
                });
            });
        },
        datatype: 'json'
    });

    return chData;
}

function setData(position)
{
    chart.dataProvider = this.getData(position);
    chart.validateData();
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
    chart.validateData();
}

function switchToDifferanceOnly()
{
    var data = { index: 1 };
    var chData = [];
    $.ajax({
        url: "Chart/GetDiffOnly",
        data: data,
        async: false,
        success: function (response) {
            $.each(response, function (i, value) {
                var date = new Date(value.date);
                chData.push({
                    date: date,
                    drop: value.drop,
                    diff: 0
                });
            });
        },
        datatype: 'json'
    });

    chart.dataProvider = chData;
    chart.validateData();
}