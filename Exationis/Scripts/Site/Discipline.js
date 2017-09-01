
var app = angular.module('Test-List', ['ngRoute', 'ui.bootstrap', 'chieffancypants.loadingBar', 'ngAnimate', 'angular.filter'])
    .config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    });

app.filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = +start;
            return input.slice(start);
        }
        return [];
    };
});


app.controller('TestAPIController', function ($scope, TestListService, filterFilter, cfpLoadingBar) {

    $scope.GetTests = function (id) {
        TestListService.getTests(id).then(function (response) {
            $scope.tests = response.data;

            // pagination controls
            $scope.currentPage = 1;
            $scope.totalItems = $scope.tests.length;
            $scope.entryLimit = 10; // items per page
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
        }, function (response) {
            $scope.testError = response.data.Message;
        });
    }

    $scope.GetDiscipline = function (id) {
        TestListService.getDiscipline(id).then(function (response) {
            cfpLoadingBar.start();
            $scope.discipline = response.data;
            $scope.GetTests(id);
            cfpLoadingBar.complete();
        }, function (response) {
            $scope.disciplineError = response.data.Message;
        });
    }

    $scope.$watch('searchTest', function (newVal, oldVal) {
        if ($scope.tests) {
            $scope.filtered = filterFilter($scope.tests, newVal);
            $scope.totalItems = $scope.filtered.length;
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
            $scope.currentPage = 1;
        }
    }, true);

    $scope.redirect = function (test) {
        if (test.Access)
            window.location.href = '/Test/' + test.ID;
        else
            return;
    }

});

app.factory('TestListService', function ($http) {

    this.getDiscipline = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetDiscipline/" + id,
        });
    }

    this.getTests = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetTests/" + id,
        });
    }

    return this;
});
