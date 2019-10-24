(function (app) {
    app.controller('mainCtrl', function mainCtrl($state, authData, loginService, $scope, authenticationService) {
    
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;
       
    });
})(angular.module('app'));