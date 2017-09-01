

//var app = angular.module('StudentsResults', ['ngRoute', 'ui.bootstrap']);

//app.filter('startFrom', function () {
//    return function (input, start) {
//        if (input) {
//            start = +start;
//            return input.slice(start);
//        }
//        return [];
//    };
//});


//app.controller('ResultAPIController', function ($scope, StudentsResultsService, filterFilter) {

//    $scope.GetShortResult = function (id) {
//        StudentsResultsService.getShortResult(id).then(function (response) {
//            $scope.results = response.data;

//            // pagination controls
//            $scope.currentPage = 1;
//            $scope.totalItems = $scope.results.length;
//            $scope.entryLimit = 5; // items per page
//            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
//        });
//    }

//    $scope.$watch('searchResult', function (newVal, oldVal) {
//        if ($scope.results) {
//            $scope.filtered = filterFilter($scope.results, newVal);
//            $scope.totalItems = $scope.filtered.length;
//            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
//            $scope.currentPage = 1;
//        }
//    }, true);

//    $scope.GetFullResult = function (userResult) {
//        StudentsResultsService.getFullResult(userResult.TestID, userResult.UserID).then(function (response) {
//            $scope.fullResult = response.data;
//            $scope.user = {
//                Name: userResult.Name,
//                SurName: userResult.SurName,
//                Points: userResult.Points,
//                Evaluation: userResult.Evaluation
//            };
//            $scope.show_question = false;
//        });
//    }


//    $scope.getQuestion = function (question, index) {
//        $scope.question = question;
//        $scope.show_question = true;

//        SelectedButton(index);
//    }



//    //$scope.filterSearch = function () {
//    //    $scope.filtered = filterFilter($scope.results, $scope.search);
//    //    $scope.totalItems = $scope.filtered.length;
//    //    $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
//    //    $scope.currentPage = 1;
//    //};



//    function SelectedButton(id) {
//        $("#button" + $scope.previosButton).removeClass("blueSelect");
//        $("#button" + $scope.previosButton).addClass("blue");

//        //id button that was selected before
//        $scope.previosButton = id;

//        $("#button" + id).removeClass("blue");
//        $("#button" + id).addClass("blueSelect");
//    }

//});

//app.factory('StudentsResultsService', function ($http) {

//    this.getShortResult = function (id) {
//        return $http({
//            method: "GET",
//            url: "/api/ResultAPI/GetShortResult/" + id
//        });
//    }

//    this.getFullResult = function (testID, userID) {
//        return $http({
//            method: "GET",
//            url: "/api/ResultAPI/GetFullResult/" + testID + "/" + userID
//        });
//    }

//    return this;
//});