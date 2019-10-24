

(function () {
    angular.module('app',
           ['oc.lazyLoad',
            'ui.router',
            'ngBootbox',
            'ngCkeditor',
            'LocalStorageModule',
            'ui.bootstrap',
            'ui.bootstrap.datetimepicker',
            'angular-loading-bar',       
            'ngSanitize',          
            'datatables',
            'datatables.buttons',
            'checklist-model',
            'localytics.directives'])
        .config(config)
        .config(configAuthentication);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, IdleProvider, KeepaliveProvider, $locationProvider) {
        
        $urlRouterProvider.otherwise('/login');
        $stateProvider
            .state('admin', {
                abstract: true,
                url: "/admin",
                templateUrl: "app/views/common/content.html",
            })
            .state('admin.dashboards', {
                url: "/dashboards",
                controller: "dashboardCtrl",
                templateUrl: "app/views/admin/dashboard.html",
            })
            .state('login', {
                url: "/login",
                templateUrl: "app/views/admin/login.html",
                data: { specialClass: 'gray-bg' },
                controller: "loginCtrl"
            })
            .state('admin.system-management', {
                abstract: true,
                url: "/system-management",
                template: '<div ui-view></div>'
            })
            .state('admin.system-management.user', {
                url: "/user",
                templateUrl: "app/views/admin/user.html",
                controller: "userCtrl"
            })  
            .state('admin.system-management.group', {
                url: "/group",
                templateUrl: "app/views/admin/group.html",
                controller: "groupCtrl"
            })  
            .state('admin.system-management.role', {
                url: "/role",
                templateUrl: "app/views/admin/role.html",
                controller: "roleCtrl"
            })  
            .state('admin.general-management', {
                abstract: true,
                url: "/general-management",
                template: '<div ui-view></div>'
            })
            .state('admin.general-management.province', {
                url: "/province",
                templateUrl: "app/views/admin/province.html",
                controller: "provinceCtrl"
            })
            .state('admin.general-management.district', {
                url: "/district",
                templateUrl: "app/views/admin/district.html",
                controller: "districtCtrl"
            })                               
            .state('admin.general-management.device', {
                url: "/device",
                templateUrl: "app/views/admin/device.html",
                controller: "deviceCtrl"
            })
            .state('admin.general-management.warningprofile', {
                url: "/warningprofile",
                templateUrl: "app/views/admin/warningprofile.html",
                controller: "warningprofileCtrl"
            })
            .state('admin.activitylog', {
                url: "/activitylog",
                templateUrl: "app/views/admin/activitylog.html",
                controller:"activitylog"
            })
            
           
    };

    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location, localStorageService) {
            return {
                request: function (config) {
                    config.headers = config.headers || {};
                    var authData = localStorageService.get('TokenInfo');
                    if (authData) {
                        authData = JSON.parse(authData);
                        config.headers.Authorization = 'Bearer ' + authData.accessToken;                     
                    }

                    return config;
                },
                responseError: function (rejection) {
                    if (rejection.status === 401) {
                        $location.path('/login');
                        console.log("Không có quyền !")
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }
})();


