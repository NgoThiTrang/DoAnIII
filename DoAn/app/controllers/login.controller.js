(function (app) {
    app.controller('loginCtrl', ['$scope', 'loginService', '$injector', 'notificationService',
        function ($scope, loginService, $injector, notificationService) {

            $scope.loginData = {
                UserName: "",
                Password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.UserName, $scope.loginData.Password).then(function (response) {
                    if (response != null && response.data.error != undefined) {
                        notificationService.displayError(response.data.error_description);
                        console.log(response.data)
                    }
                    else {
                        var stateService = $injector.get('$state');
                        stateService.go('admin.dashboards');
                    }
                });
            }
        }]);
})(angular.module('app'));