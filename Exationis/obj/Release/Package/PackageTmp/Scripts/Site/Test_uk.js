
var app = angular.module('Testing', ['ngRoute', 'chieffancypants.loadingBar', 'ngAnimate'])
    .config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    });

app.controller('TestAPIController', function ($scope, TestingService, $timeout, cfpLoadingBar) {

    $scope.GetTest = function (id) {
        TestingService.getTest(id).then(function (response) {
            cfpLoadingBar.start();
            $scope.test = response.data.Test;
            GetQuestions(id);
            cfpLoadingBar.complete();
        }, function (response) {
            $scope.error = response.data.Message;
        });
    }


    function GetQuestions(id) {
        TestingService.getQuestions(id).then(function (response) {
            if (response.data.Time <= 0)
                $scope.sendResult(id);
            else {
                $scope.questions = response.data.Questions;

                CountDown(response.data.Time);
            }
        }, function (response) {
            $scope.error = response.data.Message;
        });
    }

    $scope.getImages = function (id) {
        TestingService.getImages(id).then(function (response) {
            $scope.question.images = response.data;
            Masonry();
        });
    }

    $scope.getQuestion = function (id, question) {
        $scope.question = question;
        $scope.getImages(question.ID);
        $scope.show_question = true;
        SelectedButton(id);
        $scope.updateResult();
    }


    $scope.sendResult = function (id) {
        /// <summary>
        /// </summary>
        /// <param name="id">The discipline identifier. Needed for redirect after showing successful message about sends answers.</param>
        if (CheckNullResults($scope.questions))
            TestingService.sendResult($scope.questions).then(function (response) {
                $scope.successful = response.data;
                $timeout(function () {
                    window.location = '/Discipline/' + id;
                }, 3000);

            }, function (response) {
                alert(response.statusText)
            });
        else {
            $scope.nullResults = "Ви не відповіли на всі питання.";
            $timeout(function () {
                $scope.nullResults = null;
            }, 5000);
        }


    }

    $scope.updateResult = function () {
        TestingService.updateResult($scope.questions).then(function () {

        }, function (error) {
            alert(error.statusText)
        });
    }

    function CheckNullResults(questions) {
        for (i = 0; i < questions.length; i++) {
            var flag = false;
            for (j = 0; j < questions[i].Answer.length; j++) {
                if (questions[i].Answer[j].Result === true) {
                    flag = true;
                    break;
                }
                if (j == 3 & flag == false)
                    return flag;
            }
        }

        return flag;
    }

    function CountDown(time) {
        var fiveSeconds = new Date().getTime() + time;
        $('#clock').countdown(fiveSeconds, function (event) {
            $(this).html(event.strftime('%H:%M:%S'));
        }).on('finish.countdown', function (event) {
            TestingService.SendResults(id, $scope.words).then(function () {
                // window.location = "/Home/Result/" + id;
            });
        });
    };



    function SelectedButton(id) {
        $("#button" + $scope.previosButton).removeClass("blueSelect");
        $("#button" + $scope.previosButton).addClass("blue");

        //id button that was selected before
        $scope.previosButton = id;

        $("#button" + id).removeClass("blue");
        $("#button" + id).addClass("blueSelect");
    }
});

app.factory('TestingService', function ($http) {

    this.getQuestions = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetQuestions/" + id,
            data: { isTesting: true },
        });
    }

    this.sendResult = function (result) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/SendResult",
            data: result,
        });
    }

    this.updateResult = function (questions) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/UpdateResult",
            data: questions,
        });
    }

    this.getTest = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetTest/" + id,
        });
    }

    this.getImages = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetImages/" + id,
        });
    }

    return this;
});


function Masonry() {
    $('.grid').masonry({
        // options
        itemSelector: '.grid-item'
    });
}