let data = [];
this.getData(0);

am4core.ready(function () {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("chartdiv", am4charts.XYChart);

    // Add data
    //chart.data = this.data;

    chart.dataSource.url = "/Chart/GetHistory?index=0";
    chart.dataSource.parser = new am4core.JSONParser();

    // Set input format for the dates
    chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

    // Create axes
    var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.LineSeries());
    series.dataFields.valueY = "drop";
    series.dataFields.dateX = "date";
    series.tooltipText = "{drop}"
    series.strokeWidth = 2;
    series.minBulletDistance = 15;

    // Drop-shaped tooltips
    series.tooltip.background.cornerRadius = 20;
    series.tooltip.background.strokeOpacity = 0;
    series.tooltip.pointerOrientation = "vertical";
    series.tooltip.label.minWidth = 40;
    series.tooltip.label.minHeight = 40;
    series.tooltip.label.textAlign = "middle";
    series.tooltip.label.textValign = "middle";

    // Make bullets grow on hover
    var bullet = series.bullets.push(new am4charts.CircleBullet());
    bullet.circle.strokeWidth = 2;
    bullet.circle.radius = 4;
    bullet.circle.fill = am4core.color("#fff");

    var bullethover = bullet.states.create("hover");
    bullethover.properties.scale = 1.3;

    // Make a panning cursor
    chart.cursor = new am4charts.XYCursor();
    chart.cursor.behavior = "panXY";
    chart.cursor.xAxis = dateAxis;
    chart.cursor.snapToSeries = series;

    // Create vertical scrollbar and place it before the value axis
    chart.scrollbarY = new am4core.Scrollbar();
    chart.scrollbarY.parent = chart.leftAxesContainer;
    chart.scrollbarY.toBack();

    // Create a horizontal scrollbar with previe and place it underneath the date axis
    chart.scrollbarX = new am4charts.XYChartScrollbar();
    chart.scrollbarX.series.push(series);
    chart.scrollbarX.parent = chart.bottomAxesContainer;

    dateAxis.start = 0.79;
    dateAxis.keepSelection = true;


});

//function getData(position) {
//    var data = { index: position };
//    var chData = [];
//    $.ajax({
//        url: "Loto/Chart/GetHistory",
//        data: data,
//        async: false,
//        success: function (response) {
//            $.each(response, function (i, value) {
//                var date = new Date(value.date);
//                chData.push({
//                    date: date,
//                    drop: value.drop,
//                    diff: value.diff
//                });
//            });
//        },
//        datatype: 'json'
//    });

//    return chData;
//};

function getData(position) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == XMLHttpRequest.DONE) {   // XMLHttpRequest.DONE == 4
            if (xmlhttp.status == 200) {
                var array = JSON.parse(xmlhttp.responseText);
                array.forEach(function (obj, index) {
                    data.push({
                        date: obj.date,
                        drop: obj.drop
                    });
                });
            }
            else if (xmlhttp.status == 400) {
                alert('There was an error 400');
            }
            else {
                alert('something else other than 200 was returned');
            }
        }
    };

    xmlhttp.open("GET", "http://localhost:50139/Chart/GetHistory?index=" + position, true);
    xmlhttp.send();
}