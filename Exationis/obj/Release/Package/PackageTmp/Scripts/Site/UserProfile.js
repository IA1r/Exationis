
var app = angular.module('Profile', ['ngRoute', 'ui.bootstrap', 'chieffancypants.loadingBar', 'ngAnimate'])
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

app.controller('AccountAPIController', function ($scope, ProfileService, cfpLoadingBar) {
    //var user_id = $('#user_id').val();

    $scope.slide = function () {
        var elementToShow = '#results';
        $(elementToShow).children(".tableResults").slideToggle();
    }

    $scope.getUserProfile = function (id) {
        ProfileService.getUserProfile(id).then(function (response) {
            cfpLoadingBar.start();
            $scope.profile = response.data;
            $scope.backupProfile = {
                Name: response.data.Name,
                SurName: response.data.SurName,
                Login: response.data.Login,
                Email: response.data.Email,
                Password: response.data.Password,
                ConfirmPassword: response.data.ConfirmPassword
            };

            // pagination controls
            $scope.currentPage = 1;
            $scope.totalItems = $scope.profile.Results.length;
            $scope.entryLimit = 10; // items per page
            $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
            $scope.getLanguages();
            cfpLoadingBar.complete();
        }, function (response) {
            $scope.profileError = response.data.Message;
        });
    };

    $scope.patterns = {
        Login: "([A-Z0-9]){1}([A-Za-z0-9]){2,}",
        Email: "([A-Za-z0-9])+@([a-z])+\.(ua|net|com|ru)",
        Name: "^([А-Я]|Ї|І|Є|Ґ)[а-ї]{1,}$|^[A-Z][a-z]{1,}$",
        SurName: "^([А-Я]|Ї|І|Є|Ґ)[а-ї]{1,}$|^[A-Z][a-z]{1,}$",
        MiddleName: "^([А-Я]|Ї|І|Є|Ґ)[а-ї]{1,}$|^[A-Z][a-z]{1,}$",
        Password: ""
    }

    $scope.editUserProfile = function () {
        if ($scope.form1.$valid && $scope.errorLogin == null && $scope.errorEmail == null) {
            ProfileService.editUserProfile($scope.profile).then(function (response) {
                $scope.IsEdit = false;
            }, function (response) {
                alert(response.data.Message);
            });
        };
    }
    $scope.cancel = function () {
        $scope.profile.Name = $scope.backupProfile.Name;
        $scope.profile.SurName = $scope.backupProfile.SurName;
        $scope.profile.Login = $scope.backupProfile.Login;
        $scope.profile.Email = $scope.backupProfile.Email;
        $scope.profile.Password = $scope.backupProfile.Password;
        $scope.profile.ConfirmPassword = $scope.backupProfile.ConfirmPassword;
        $scope.IsEdit = false;
    }

    

    $scope.isLoginExist = function () {
        ProfileService.isLoginExist($scope.profile.Login).then(function (response) {
            $scope.errorLogin = null;
        }, function (response) {
            $scope.errorLogin = response.data.Message;
        });
    };

    $scope.isEmailExist = function () {
        ProfileService.isEmailExist($scope.profile.Email).then(function (response) {
            $scope.errorEmail = null;
        }, function (response) {
            $scope.errorEmail = response.data.Message;
        });
    };

    $scope.getLanguages = function () {
        ProfileService.getLanguages().then(function (response) {
            $scope.languages = response.data;
        });
    }
    $scope.changeLanguage = function () {
        ProfileService.changeLanguage($scope.profile.Language.LanguageID).then(function (response) {
            window.location.reload();
        });
    }
});

app.factory('ProfileService', function ($http) {

    this.getUserProfile = function (id) {
        return $http({
            method: "GET",
            url: "/api/AccountAPI/GetUserPrfile/" + id,
        });
    }

    this.editUserProfile = function (profile) {
        return $http({
            method: "POST",
            url: "/api/AccountAPI/EditUserProfile",
            data: profile
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

    this.getLanguages = function () {
        return $http({
            method: "GET",
            url: "/api/AccountAPI/GetLanguages"
        });
    }

    this.changeLanguage = function (id) {
        return $http({
            method: "POST",
            url: "/api/AccountAPI/changeLanguage/" + id
        });
    }

    return this;
});


function AvatarUpload (id) {
    var data = new FormData();
    var files = document.getElementById('avatarUpload').files;

    for (var i = 0; i < files.length; i++) {
        data.append("file" + i, files[i]);
    }

    $.ajax({
        type: "POST",
        url: '/api/AccountAPI/AvatarUpload/' + id,
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            location.reload();
        },
        error: function (response) {
            alert(response.statusText);
        }
    });
}