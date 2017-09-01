
var app = angular.module('Charts', ['ngRoute', 'chieffancypants.loadingBar'])
    .config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    });


app.controller('ResultAPIController', function ($scope, Charts, $compile) {

    GetDisciplines();
    function GetDisciplines() {
        Charts.getDisciplines().then(function (response) {
            $scope.disciplines = response.data;
        });
    }

    $scope.DrawChartRequestModel = {
        TestID: null,
        Groups: [],
        Group: null,
        OneOrMany: null
    }

    $scope.ChengeType = function () {
        if (!$scope.groups)
            Charts.getGroups().then(function (response) {
                $scope.groups = response.data;
            });
    }

    $scope.Draw = function () {
        Charts.drawCharts($scope.DrawChartRequestModel).then(function (response) {
            if ($scope.DrawChartRequestModel.OneOrMany == 1) {
                LineChartResultsGroup(response.data.GroupChartData);
                BarChartResultsGroup(response.data.GroupChartData);
            }
            if ($scope.DrawChartRequestModel.OneOrMany == 2) {
                LineChartResultsGroups(response.data.GroupsChartData);
                BarChartResultsGroups(response.data.GroupsChartData);
            }

        });
    }
});

app.factory('Charts', function ($http) {

    this.getDisciplines = function () {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetDisciplines"
        });
    }

    this.getGroups = function () {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetGroups"
        });
    }

    this.drawCharts = function (data) {
        return $http({
            method: "POST",
            url: "/api/ResultAPI/DrawCharts",
            data: data
        });
    }

    return this;
});

function BarChartResultsGroup(chartData) {
    var data = [];
    $.each(chartData.StudentsData, function (key, val) {

        var trace = {
            x: [],
            y: [],
            name: '',
            type: 'bar',
            text: [],
            marker: {
                color: []
            }
        };

        trace.x.push(val.SurName);
        trace.y.push(val.StudentPoints);
        trace.text.push(val.Percent + "%");
        if (val.Percent < 60) {
            trace.marker.color.push('rgba(255, 34, 43, 1)');
            trace.name = val.SurName + ' (' + val.Percent + '%' + ')';
        }
        if (val.Percent >= 90) {
            trace.marker.color.push('rgba(0, 163, 0, 1)');
            trace.name = val.SurName + ' (' + val.Percent + '%' + ')';
        }
        if (val.Percent >= 60 && val.Percent < 90) {
            trace.marker.color.push('rgba(0, 170, 255, 1)');
            trace.name = val.SurName + ' (' + val.Percent + '%' + ')';
        }

        data.push(trace);
    });

    var layout = {
        title: 'Оцінка Студента',
        yaxis: {
            autorange: false,
            range: [0, chartData.TestEvaluation + 1],
            tick0: chartData.TestEvaluation,
            dtick: 1,
            title: 'Бали',
        },
        xaxis: {
            title: 'Студенти',
        }
    };


    Plotly.newPlot('everyStudent', data, layout);
}


function LineChartResultsGroups(chartData) {
    var data = [];
    var colors = Plotly.d3.scale.category20();

    $.each(chartData, function (i) {
        var trace_line = {
            x: [],
            y: [],
            mode: 'lines+markers',
            name: '',
            line: {
                color: '',
            }
        };

        var trace_dashdot = {
            x: [],
            y: [],
            mode: 'lines',
            name: '',
            line: {
                dash: 'dashdot',
                width: 3.5,
                color: ''
            }
        };
        $.each(chartData[i].StudentsData, function (key, val) {
            trace_line.x.push(key);
            trace_line.y.push(val.StudentPoints);
            trace_line.name = chartData[i].Group;

            trace_dashdot.x.push(key);
            trace_dashdot.y.push(chartData[i].Avarage);
        });
        trace_line.line.color = colors(i + 3);
        trace_dashdot.line.color = colors(i);
        trace_dashdot.name = 'Середній бал - ' + chartData[i].Avarage;

        data.push(trace_line);
        data.push(trace_dashdot);
    });
    var layout = {
        title: 'Лінійний графік - Результати студентів',
        yaxis: {
            autorange: false,
            range: [0, chartData[0].TestEvaluation + 1],
            tick0: chartData[0].TestEvaluation,
            dtick: 1,
            title: 'Бали',
        },
        xaxis: {
            title: 'Студенти',
        }
    };

    Plotly.newPlot('lineChart', data, layout);
}


function BarChartResultsGroups(chartData) {
    var data = [];
    var colors = Plotly.d3.scale.category10();

    $.each(chartData, function (i) {
        var trace = {
            x: [],
            y: [],
            name: '',
            type: 'bar',
            text: [],
            marker: {
                color: []
            }
        };
        $.each(chartData[i].StudentsData, function (key, val) {
            trace.x.push(val.SurName);
            trace.y.push(val.StudentPoints);
            trace.text.push(val.Percent + "%");

        });
        trace.name = chartData[i].Group;
        trace.marker.color = colors(i);
        data.push(trace);
    });

    var layout = {
        title: 'Гістограма - Student Results',
        yaxis: {
            autorange: false,
            range: [0, chartData[0].TestEvaluation + 1],
            tick0: chartData[0].TestEvaluation,
            dtick: 1,
            title: 'Бали',
        },
        xaxis: {
            title: 'Студенти',
        },
    };


    Plotly.newPlot('barChart', data, layout);
}

function LineChartResultsGroup(chartData) {
    var data = [];

    var trace_line = {
        x: [],
        y: [],
        mode: 'lines+markers',
        name: '',
        line: {
            color: '',
        }
    };

    var trace_dashdot = {
        x: [],
        y: [],
        mode: 'lines',
        name: '',
        text: '',
        line: {
            dash: 'dashdot',
            width: 3.5,
            color: ''
        }
    };

    $.each(chartData.StudentsData, function (key, val) {
        trace_line.x.push(val.SurName);
        trace_line.y.push(val.StudentPoints);

        trace_dashdot.x.push(val.SurName);
        trace_dashdot.y.push(chartData.Avarage);
    });
    trace_line.name = chartData.Group;
    trace_line.line.color = 'rgba(0, 0, 0, 1)';
    if (chartData.Percent < 60) {
        trace_dashdot.line.color = 'rgba(255, 34, 43, 1)';
        trace_dashdot.text = chartData.Percent + "%";
    }
    if (chartData.Percent > 90) {
        trace_dashdot.line.color = 'rgba(0, 163, 0, 1)';
        trace_dashdot.text = chartData.Percent + "%";
    }
    if (chartData.Percent > 60 && chartData.Percent < 90) {
        trace_dashdot.line.color = 'rgba(0, 170, 255, 1)';
        trace_dashdot.text = chartData.Percent + "%";
    }
    trace_dashdot.name = 'Середній бал - ' + chartData.Avarage;

    data.push(trace_line);
    data.push(trace_dashdot);
    var layout = {
        title: 'Лінійний графік - Результати студентів',
        yaxis: {
            autorange: false,
            range: [0, chartData.TestEvaluation + 1],
            tick0: chartData.TestEvaluation,
            dtick: 1,
            title: 'Бали',
        },
        xaxis: {
            title: 'Студенти',
        }

    };

    Plotly.newPlot('lineChartResultsGroup', data, layout);
}