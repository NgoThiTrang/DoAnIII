(function (app) {
    app.controller('dashboardCtrl', function dashboardCtrl($http, $scope, authData, $location) {
        if (!authData.authenticationData.IsAuthenticated) {
            $location.path('/login');
           
        }
       
    });
})(angular.module('app'));