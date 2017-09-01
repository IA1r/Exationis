
var app = angular.module('Faculty', ['ngRoute', 'angular.filter', 'Account', 'ui.bootstrap', 'chieffancypants.loadingBar', 'ngAnimate'])
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

app.controller('FacultyAPIController', function ($scope, ContactService, filterFilter, cfpLoadingBar) {

    $scope.slide = function (parent, child) {
        var elementToShow = '#item-' + parent + child;
        $(elementToShow).children(".discipline").slideToggle();
    }

    $scope.getFaculties = function () {
        ContactService.getFaculties().then(function (response) {
            cfpLoadingBar.start();
            $scope.faculties = response.data;
            cfpLoadingBar.complete();
        });
    };

    $scope.getFaculties();

    $scope.SearchDiscipline = function () {
        ContactService.searchDiscipline($scope.searchDiscipline).then(function (response) {
            $scope.disciplines = response.data;

            // pagination controls
            $scope.currentPage = 1;
            $scope.totalItems = $scope.disciplines.length;
            $scope.entryLimit = 5; // items per page
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
        });
    }

    $scope.$watch('searchDiscipline', function (newVal, oldVal) {
        if ($scope.disciplines) {
            $scope.filtered = filterFilter($scope.disciplines, newVal);
            $scope.totalItems = $scope.filtered.length;
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
            $scope.currentPage = 1;
        }
    }, true);

    $scope.redirect = function (url) {
        window.location.href = url;
    }

});

app.factory('ContactService', function ($http) {

    this.getFaculties = function () {
        return $http({
            method: "GET",
            url: "/api/HomeAPI/GetFaculties",
        });
    }

    this.searchDiscipline = function (id) {
        return $http({
            method: "POST",
            url: "/api/HomeAPI/SearchDiscipline/" + id
        });
    }

    return this;
});

angular.element(document).ready(function () {
    var myDiv1 = document.getElementById("account");
    angular.bootstrap(myDiv1, ["Account"]);

    var myDiv2 = document.getElementById("faculty");
    angular.bootstrap(myDiv2, ["Faculty"]);
});