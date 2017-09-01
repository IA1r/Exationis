
var app = angular.module('TestResult', ['ngRoute', 'angular.filter']);

app.controller('ResultAPIController', function ($scope, TestResultService) {


    $scope.GetDisciplines = function () {
        TestResultService.getDisciplines().then(function (response) {
            $scope.disciplines = response.data;
        });
    };
    
    //$scope.GetTests = function () {
    //    TestResultService.getTests().then(function (response) {
    //        $scope.tests = response.data;
    //    });
    //}

    $scope.slide = function (id) {
        var elementToShow = '#item-' + id;
        $(elementToShow).children(".discipline").slideToggle();
    }


});

app.factory('TestResultService', function ($http) {

    this.getDisciplines = function () {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetActualDisciplines"
        });
    }

    //this.getTests = function (id) {
    //    return $http({
    //        method: "GET",
    //        url: "/api/ResultAPI/GetTests/" + id
    //    });
    //}

    return this;
});