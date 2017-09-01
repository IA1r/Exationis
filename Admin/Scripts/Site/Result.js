
var app = angular.module('Result', ['ngRoute', 'ui.bootstrap', 'angular.filter', 'chieffancypants.loadingBar'])
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


app.controller('ResultAPIController', function ($scope, TestService, filterFilter) {

    $scope.GetDisciplines = function () {
        TestService.getDisciplines().then(function (response) {
            $scope.disciplines = response.data;
        });
    };
    $scope.DisID = null;

    $scope.GetTests = function () {
        TestService.getTests($scope.DisID).then(function (response) {
            $scope.tests = response.data;

            // pagination controls
            $scope.currentPage = 1;
            $scope.totalItems = $scope.tests.length;
            $scope.entryLimit = 5; // items per page
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
        }, function (response) {
            if (response.status == 403)
                $scope.errorTests = response.data.Message;
            if (response.status == 404)
                $scope.isExist = response.data.Message;
        });
    }

    $scope.GetShortResult = function (id) {
        TestService.getShortResult(id).then(function (response) {
            $scope.results = response.data;

            // pagination controls
            $scope.currentPage_1 = 1;
            $scope.totalItems_1 = $scope.results.length;
            $scope.entryLimit_1 = 5; // items per page
            $scope.noOfPages_1 = Math.ceil($scope.totalItems_1 / $scope.entryLimit_1);
        });
    }

    $scope.GetFullResult = function (userResult) {
        TestService.getFullResult(userResult.TestID, userResult.UserID).then(function (response) {
            $scope.fullResult = response.data;
            $scope.user = {
                Name: userResult.Name,
                SurName: userResult.SurName,
                Points: userResult.Points,
                Evaluation: userResult.Evaluation
            };
            $scope.show_question = false;
        });
    }

    $scope.getQuestion = function (question, index) {
        $scope.question = question;
        $scope.show_question = true;

        SelectedButton(index);
    }


    $scope.$watch('searchTest', function (newVal, oldVal) {
        if ($scope.tests) {
            $scope.filtered = filterFilter($scope.tests, newVal);
            $scope.totalItems = $scope.filtered.length;
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
            $scope.currentPage = 1;
        }
    }, true);

    $scope.$watch('searchResult', function (newVal, oldVal) {
        if ($scope.results) {
            $scope.filtered_1 = filterFilter($scope.results, newVal);
            $scope.totalItems_1 = $scope.filtered_1.length;
            $scope.noOfPages_1 = Math.ceil($scope.totalItems_1 / $scope.entryLimit_1);
            $scope.currentPage_1 = 1;
        }
    }, true);

    function SelectedButton(id) {
        $("#button" + $scope.previosButton).removeClass("blueSelect");
        $("#button" + $scope.previosButton).addClass("blue");

        //id button that was selected before
        $scope.previosButton = id;

        $("#button" + id).removeClass("blue");
        $("#button" + id).addClass("blueSelect");
    }

});

app.factory('TestService', function ($http) {

    this.getTests = function (id) {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetTests/" + id
        });
    }

    this.getDisciplines = function () {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetActualDisciplines"
        });
    }

    this.getShortResult = function (id) {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetShortResult/" + id
        });
    }

    this.getFullResult = function (testID, userID) {
        return $http({
            method: "GET",
            url: "/api/ResultAPI/GetFullResult/" + testID + "/" + userID
        });
    }

    return this;
});