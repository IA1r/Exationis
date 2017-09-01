
var app = angular.module('Account', ['ngRoute', 'Faculty']);

app.controller('AccountAPIController', function ($scope, ContactService, $timeout) {


    $scope.GetFacultyList = function () {
        ContactService.getFacultyList().then(function (response) {
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
        Group: "",
        FacultyID: null,
        Facultys: $scope.facultyList
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
        Group: "^([1-6])[0-9][1-9]$"
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
            ContactService.registration($scope.registration_form).then(function (response) {
                $('#btn_close').trigger('click');
                window.location.reload();
            });
        }
    };

    $scope.signin = function () {
        if ($scope.form2.$valid) {
            ContactService.signin($scope.signin_form).then(function (response) {
                $('#btn_close').trigger('click');
                if (response.data != null)
                    window.location.replace(response.data);
                else
                    location.reload();
            }, function (response) {
                $scope.errorSignIn = response.data.Message;
            });
        }
    };

    $scope.isLoginExist = function () {
        ContactService.isLoginExist($scope.registration_form.Login).then(function (response) {
            $scope.errorLogin = response.data;
        }, function (response) {
            $scope.errorLogin = response.data.Message;
        });
    };

    $scope.isEmailExist = function () {
        ContactService.isEmailExist($scope.registration_form.Email).then(function (response) {
            $scope.errorEmail = response.data;
        }, function (response) {
            $scope.errorEmail = response.data.Message;
        });
    };

});


app.factory('ContactService', function ($http) {

    this.getFacultyList = function () {
        return $http({
            method: "GET",
            url: "/api/AccountAPI/GetFacultyList",
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

$(function () {
    if (window.location.href.toLowerCase().indexOf("returnurl") > 0)
        $("#myModal").modal("show");
})


$(document).click(function (evt) {
    if (evt.target.parentElement.id == "avatar")
    {
        $("#profile_menu").show("fast");
        return;
    }    

    $("#profile_menu").hide("fast");
});
