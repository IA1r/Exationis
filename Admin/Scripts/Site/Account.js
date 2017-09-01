
var app = angular.module('Account', ['ngRoute', 'Discipline']);

app.controller('AccountAPIController', function ($scope, AccountService, $timeout) {

    GetFacultyList();
    function GetFacultyList() {
        AccountService.getFacultyList().then(function (response) {
            $scope.registration_form.Facultys = response.data;
        });
    };

    $scope.registration_form = {
        Login: undefined,
        Email: undefined,
        Name: "",
        SurName: "",
        MiddleName: "",
        Password: "",
        ConfirmPassword: "",
        Group: null,
        FacultyID: null,
        Facultys: $scope.facultyList,
        KeyWord:""
    };

    $scope.signin_form = {
        Login: undefined,
        Password: "",
        RememberMe: false
    };

    $scope.patterns = {
        Login: "([A-Z0-9]){1}([A-Za-z0-9]){2,}",
        Email: "([A-Za-z0-9])+@([a-z])+\.(ua|net|com|ru)",
        Name: "^([А-Я]|Ї|І|Є|Ґ)[а-ї]{1,}$|^[A-Z][a-z]{1,}$",
        SurName: "^([А-Я]|Ї|І|Є|Ґ)[а-ї]{1,}$|^[A-Z][a-z]{1,}$",
        MiddleName: "^([А-Я]|Ї|І|Є|Ґ)[а-ї]{1,}$|^[A-Z][a-z]{1,}$",
        Password: "",
        ConfirmPassword: $scope.registration_form.Password,
        KeyWord:""
    }

    $scope.access = function (type) {
        $scope.accessType = type;
        if (type == "registration") {
            $("#registration").css("border-bottom", "solid 3px white");
            $("#signin").css("border-bottom", "#507299");
        }
        if (type == "signin") {
            $("#signin").css("border-bottom", "solid 3px white");
            $("#registration").css("border-bottom", "#507299");
        }
    }

    $scope.registration = function () {
        if ($scope.form1.$valid && $scope.errorLogin == "" && $scope.errorEmail == "") {
            AccountService.registration($scope.registration_form).then(function (response) {
                $('#myModal').modal('hide');
                location.reload();
            }, function (response) {
                $scope.regError = response.data.Message;
                $timeout(function () {
                    $scope.regError = '';
                }, 3000);
            });
        }
    };

    $scope.signin = function () {
        if ($scope.form2.$valid) {
            AccountService.signin($scope.signin_form).then(function (response) {
                $('#myModal').modal('hide');
                if (response.data != null)
                    window.location.replace(response.data);
                else
                    location.reload();
            }, function (response) {
                $scope.errorSignIn = response.data.Message;
                $timeout(function () {
                    $scope.errorSignIn = "";
                }, 3000);
            });
        }
    };

    //$scope.signout = function () {
    //    AccountService.signout().then(function () {
    //        location.reload();
    //    });
    //};

    $scope.isLoginExist = function () {
        AccountService.isLoginExist($scope.registration_form.Login).then(function (response) {
            $scope.errorLogin = response.data;
        }, function (response) {
            $scope.errorLogin = response.data.Message;
        });
    };

    $scope.isEmailExist = function () {
        AccountService.isEmailExist($scope.registration_form.Email).then(function (response) {
            $scope.errorEmail = response.data;
        }, function (response) {
            $scope.errorEmail = response.data.Message;
        });
    };

});


app.factory('AccountService', function ($http) {

    this.getFacultyList = function () {
        return $http({
            method: "GET",
            url: "/api/HomeAPI/GetFacultyList",
        });
    }

    this.registration = function (registration_form) {
        return $http({
            method: "POST",
            url: "/api/AccountAPI/Registration",
            data: registration_form
        });
    }

    this.signin = function (signin_form) {
        return $http({
            method: "POST",
            url: "/api/AccountAPI/SignIn",
            data: signin_form
        });
    }

    this.isLoginExist = function (login) {
        return $http({
            method: "POST",
            url: "/api/AccountAPI/IsLoginExist",
            data: { Login: login }
        });
    }

    this.isEmailExist = function (email) {
        return $http({
            method: "POST",
            url: "/api/AccountAPI/IsEmailExist",
            data: { Email: email }
        });
    }

    return this;
});


$(document).ready(function () {
    $('#myModal').modal('show');
});

$(document).click(function (evt) {
    if (evt.target.parentElement.id == "avatar") {
        $("#profile_menu").show("fast");
        return;
    }

    $("#profile_menu").hide("fast");
});
