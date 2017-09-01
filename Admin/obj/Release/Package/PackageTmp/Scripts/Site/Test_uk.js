
var app = angular.module('EditTest', ['ngRoute', 'angular-autogrow', 'chieffancypants.loadingBar'])
    .config(function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    });

app.controller('TestAPIController', function ($scope, ContactService, $timeout) {

    var testID = $("#test_id").val()

    $scope.editTestTitle = function () {
        $scope.isEdit = true;
    }

    $scope.saveTest = function () {
        $scope.isEdit = false;
        ContactService.editTest($scope.test).then(function () {
        });
    }

    $scope.GetTest = function GetTest(id) {
        ContactService.getTest(id).then(function (response) {
            $scope.test = response.data;
            GetQuestions(id);
        }, function (response) {
            $scope.errorTest = response.data.Message;
        });
    }

    function GetQuestions(id) {
        ContactService.getQuestions(id).then(function (response) {
            $scope.questions = response.data;
        }, function (error) {
            alert(error.statusText)
        });
    }

    $scope.question_form = {
        TestID: "",
        Question: "",
        Answers: ["", "", "", ""],
        Correct: [false, false, false, false],
        Points: null
    }

    $scope.addQuestion = function (id) {
        $scope.question_form.TestID = id;
        if ($scope.question_form.Question == "") {
            $scope.add_error = "Питання не може бути порожнім."
            $scope.clear($scope);
            return;
        }
        if ($scope.question_form.Points == null || $scope.question_form.Points == "") {
            $scope.add_error = "Бали не можуть бути порожніми."
            $timeout(function () {
                $scope.add_error = "";
            }, 3000);
            return;
        }
        for (i = 0; i < $scope.question_form.Correct.length; i++) {
            if ($scope.question_form.Correct[i] === true && $scope.question_form.Answers[i] != "") {
                var flag = true;
                break;
            }
        }
        if (!flag) {
            $scope.add_error = "Один з відповідей повинен бути правильним."
            $timeout(function () {
                $scope.add_error = ""
            }, 3000);
        }
        else
            ContactService.addQuestion($scope.question_form).then(function () {
                GetQuestions(id);
            });
    }

    $scope.editQuestion = function (question) {
        if (question.Content != "")
            for (i = 0; i < question.Answer.length; i++) {
                if (question.Answer[i].Correct === true) {
                    var flag = true;
                    break;
                }
            }
        if (flag)
            ContactService.editQuestion(question).then(function () {
                $scope.successful = "Дані успішно оновлені"
                $timeout(function () {
                    $scope.successful = ""
                }, 3000);
            });
        else {
            $scope.edit_error = "Один з відповідей повинен бути правильним."
            $timeout(function () {
                $scope.edit_error = ""
            }, 3000);
        }

    }

    $scope.getQuestion = function (question, index) {
        $scope.question = question;
        $scope.question.index = index;
        $scope.show_question = true;

        ContactService.getImages($scope.question.ID).then(function (response) {
            $scope.question.images = response.data;
            Masonry();
        })
        SelectedButton(index - 1);
    }



    $scope.evaluation = function () {
        if (typeof $scope.questions === "undefined")
            return;

        var totalPoints = 0;
        for (i = 0; i < $scope.questions.length; i++) {
            if ($scope.questions[i].Points == "")
                totalPoints += 0;
            else
                totalPoints += parseFloat($scope.questions[i].Points);
        }

        return totalPoints;
    }

    $scope.imageUpload = function () {
        var data = new FormData();
        var files = document.getElementById('imageUpload').files;

        for (var i = 0; i < files.length; i++) {
            data.append("file" + i, files[i]);
        }
        $.ajax({
            type: "POST",
            url: '/api/TestAPI/ImageUpload/' + $scope.question.ID,
            data: data,
            contentType: false,
            processData: false,
            success: function () {
                ContactService.getImages($scope.question.ID).then(function (response) {
                    $scope.question.images = response.data;
                })
            },
            error: function (result) {
                $scope.question.uploadError = $.parseJSON(result.responseText).ExceptionMessage;
                angular.element(document.querySelector("#uploadError")).scope().$apply();

                $('#uploadError').modal('show');
                $timeout(function () {
                    $('#uploadError').modal('hide');
                }, 3000);
            }
        });
    }


    $scope.showImageModal = function (image) {
        $scope.imageModal = image;
        $('#imageModal').modal('show');
    }

    $scope.deleteImage = function () {
        ContactService.deleteImage($scope.imageModal.ImageID).then(function (response) {
            $('#imageModal').modal('hide');

            $scope.question.images = $scope.question.images.filter(function (img) {
                return img.ImageID !== $scope.imageModal.ImageID;
            });
        });
    }

    function SelectedButton(id) {
        $("#button" + $scope.previosButton).removeClass("blueSelect");
        $("#button" + $scope.previosButton).addClass("blue");

        //id button that was selected before
        $scope.previosButton = id;

        $("#button" + id).removeClass("blue");
        $("#button" + id).addClass("blueSelect");
    }


    $scope.clear = function (el) {
        $timeout(function () {
            el.add_error = "";
        }, 3000);
    }
});

app.factory('ContactService', function ($http) {

    this.addQuestion = function (question) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/AddQuestion",
            data: question
        });
    }

    this.editQuestion = function (question) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/EditQuestion",
            data: question
        });
    }

    this.getImages = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetImages/" + id
        });
    }

    this.deleteImage = function (id) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/DeleteImage/" + id
        });
    }

    this.getQuestions = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetQuestions/" + id,
        });
    }

    this.getTest = function (id) {
        return $http({
            method: "GET",
            url: "/api/TestAPI/GetTest/" + id,
        });
    }

    this.editTest = function (testTitle) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/EditTest",
            data: testTitle
        });
    }

    return this;
});


function Masonry() {
    $('.grid').masonry({
        // options
        itemSelector: '.grid-item',
    });
}

$(function () {
    $('.pop').on('click', function () {
        $('.imagepreview').attr('src', $(this).find('img').attr('src'));
        $('#imagemodal').modal('show');
    });
});

function UploadImage() {
    var scope = angular.element($("#testapi")).scope();
    scope.$apply(function () {
        scope.imageUpload();
    });
}