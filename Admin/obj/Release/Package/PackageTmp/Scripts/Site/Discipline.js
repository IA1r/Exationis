

var app = angular.module('Discipline', ['ngRoute', 'chieffancypants.loadingBar'])
    .config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    });

app.controller('HomeAPIController', function ($scope, DisciplineService, $timeout, HelpService) {


    $scope.GetDisciplines = function () {
        DisciplineService.isLogged().then(function (response) {
            if (response.data) {
                DisciplineService.getDisciplines().then(function (response) {
                    $scope.Disciplines = response.data;
                    GetFacultyList();
                    $scope.groupedDis = GroupDis();
                });
            }
            else { $('#myModal').modal('show'); }
        })
    };

    function GroupDis() {
        var obj = [];
        var j = -1;
        for (var i = 0; i < $scope.Disciplines.length; i++) {
            if (i % 5 == 0) {
                q = 0;
                j++;
                obj[j] = [];
            }
            obj[j][i] = $scope.Disciplines[i];
        }
        return obj;
    }

    function GetFacultyList() {
        DisciplineService.getFacultyList().then(function (response) {
            $scope.discipline_form.Facultys = response.data;
        });
    };

    $scope.patterns = {
        DisciplineName: "^([А-Я]|Ї|І|Є|Ґ)[а-ї0-9 *]{1,}$|^[A-Z][a-z0-9 *]{1,}$"
    }

    $scope.createDiscipline = function () {
        if ($scope.form2.$valid) {
            DisciplineService.createDiscipline($scope.discipline_form).then(function (response) {
                $('#newDiscipline').modal('hide');
                location.reload();
            }, function (response) {
                $scope.errorDiscipline = response.data.Message;
                $timeout(function () {
                    $scope.errorDiscipline = null;
                }, 3000);
            });
        }
    }

    $scope.slide = function (id) {
        var elementToShow = '#item-' + id;
        $(elementToShow).children(".test").slideToggle();
    }

    //$scope.setDisID = function (id) {
    //    HelpService.setDisID(id);
    //}

    $scope.setDisciplines = function () {
        HelpService.setDisciplines($scope.Disciplines);
    }

    $scope.discipline_form = {
        Name: "",
        Course: "",
        Facultys: ""
    };

    $scope.discipline_patterns = {
        Name: "",
        Course: "([1-6])"
    }

});


app.factory('DisciplineService', function ($http) {

    this.getDisciplines = function () {
        return $http({
            method: "GET",
            url: "/api/HomeAPI/GetDisciplines",
        });
    }

    this.getFacultyList = function () {
        return $http({
            method: "GET",
            url: "/api/HomeAPI/GetFacultyList",
        });
    }

    this.createDiscipline = function (discipline) {
        return $http({
            method: "POST",
            url: "/api/HomeAPI/CreateDiscipline",
            data: discipline
        });
    }

    this.isLogged = function () {
        return $http({
            method: "GET",
            url: "/api/AccountAPI/IsLogged"
        });
    }

    return this;
});

app.service('HelpService', function () {

    var dis_ID = null;
    var Disciplines = null;

    this.setDisID = function (id) {
        dis_ID = id;
    }
    this.setDisciplines = function (disciplines) {
        Disciplines = disciplines;
    }

    this.getDisID = function () {
        return dis_ID;
    }
    this.getDisciplines = function () {
        return Disciplines;
    }
});


$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});


angular.element(document).ready(function () {
    var myDiv1 = document.getElementById("account");
    angular.bootstrap(myDiv1, ["Account"]);

    var myDiv2 = document.getElementById("discipline");
    angular.bootstrap(myDiv2, ["Discipline"]);
});