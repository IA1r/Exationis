
app.controller('TestAPIController', function ($scope, TestService, filterFilter, HelpService) {

    $scope.TestModel = {
        DisciplineID: "",
        Name: "",
        Limitation: "",
        Evaluation: 0.00
    }

    $scope.AccessModel = {
        TestID: "",
        Group: "",
        UserID: []
    }

    $scope.testPatterns = {
        TestName: "^([А-Я]|Ї|І|Є|Ґ)[а-ї0-9 *]{1,}$|^[A-Z][a-z0-9 *]{1,}$",
        Limitation: "(([0-1][0-9])|([2][0-3]))(\:[0-5][0-9]\:[0-5][0-9])",
        Evaluation: "^[1-9][0-9]?$|^100$",
        Group: "^[1-6][0-9][0-9] - 20[0-9][0-9]$"
    }

    $scope.GetDisciplines = function () {
        $scope.disciplines = HelpService.getDisciplines();
    }

    $scope.createTest = function () {
        if ($scope.form1.$valid && $scope.errorName == null) {
            TestService.createTest($scope.TestModel).then(function (response) {
                location.replace(response.data.testPage);
            });
        }
    }

    $scope.isTestExist = function () {
        TestService.isTestExist($scope.TestModel).then(function (response) {
            $scope.errorTest = response.data;
        }, function (response) {
            $scope.errorTest = response.data.Message;
        });
    };

    $scope.GetStudents = function () {
        if ($scope.form3.$valid) {
            //$scope.AccessModel.TestID = HelpService.getTestID();
            TestService.GetStudents($scope.AccessModel).then(function (response) {
                $scope.students = response.data;
            });
        }
    };

    $scope.SetAccess = function () {
        TestService.SetAccess($scope.students).then(function (response) {
            $('#Access').modal('hide');
        });

    };

    function ClrearError() {
        /// <summary>
        /// Clrears the error.
        /// </summary>
        /// <param name="label_id">The field of error.</param>
        setTimeout(function () {
            $scope.error = "";
        }, 3000);
    }
});

app.factory('TestService', function ($http) {

    this.createTest = function (test) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/CreateTest",
            data: test
        });
    }

    this.isTestExist = function (test) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/IsTestExist",
            data: test
        });
    }

    this.GetStudents = function (model) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/GetStudents",
            data: model
        });
    }

    this.SetAccess = function (model) {
        return $http({
            method: "POST",
            url: "/api/TestAPI/SetAccess",
            data: model
        });
    }

    return this;
});
