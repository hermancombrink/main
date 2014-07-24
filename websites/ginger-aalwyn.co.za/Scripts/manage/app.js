// Main configuration file. Sets up AngularJS module and routes and any other config objects

var clock;
var appRoot = angular.module('main', ['ngRoute', 'ngGrid', 'ngResource', 'angularStart.services', 'angularStart.directives', 'ngAnimate', 'ui.bootstrap']);     //Define the main module

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
        $routeProvider
            .when('/Wedding', { templateUrl: '/Manage/Wedding', controller: 'WeddingController' })
            .when('/BrideGroom', { templateUrl: '/Manage/Main', controller: 'MainController' })
            .when('/BridesMaids', { templateUrl: '/Manage/Bridesmaids', controller: 'BridesmaideController' })
            .when('/GroomsMen', { templateUrl: '/Manage/Groomsmen', controller: 'GroomsmenController' })
            .when('/Guests', { templateUrl: '/Manage/Guests', controller: 'GuestController' })
            .when('/Photos', { templateUrl: '/Manage/Photos', controller: 'PhotoController' })
            .when('/SaveDate', { templateUrl: '/Manage/SaveDate', controller: 'MainController' })
            .when('/CheckList', { templateUrl: '/Manage/CheckList', controller: 'MainController' })
            .otherwise({ redirectTo: '/Wedding' });
    }])
    .controller('RootController', ['$scope', '$route', '$routeParams', '$location', '$http', function ($scope, $route, $routeParams, $location, $http) {
        var self = this;

        self.startLoading = function () {
            $.blockUI({ "message": "<h3>Loading...</h3>" });
        }
        self.stopLoading = function () {
            $.unblockUI();
        }

        $scope.debug = true;

        $scope.MenuItems = [{ "MenuLink": "SomeLink", "MenuText": "TEst" }];

        $scope.Title = "Manage...";

        self.startLoading();
        //$http({ method: "GET", url: "/api/ManageApi/GetMenuItems" })
        //    .success(function (data) {
        //        if (data.NodeItems)
        //            $scope.MenuItems = data.NodeItems;
        //        self.stopLoading();
        //    })
        //    .error(function (data, status) {
        //        if ($scope.debug)
        //            return;
        //        if (status === 404) {
        //            window.location.pathname = "/Error/NotFound";
        //        }
        //        else
        //            if (status > 499) {

        //                window.location.pathname = "/Error/Server";
        //            }
        //    });
        /* region Loading */
        $scope.$on("SETTITLE", function (e, title) {
            $scope.Title = title;
        });
        $scope.$on("LOAD", function () { self.startLoading(); });
        $scope.$on("UNLOAD", function () { self.stopLoading(); });
        $scope.$on('$routeChangeStart', function () {
            self.startLoading();
        });
        $scope.$on('$routeChangeSuccess', function () {
            self.stopLoading();
        });
        $scope.$on('$routeChangeError', function () {
            self.stopLoading();
        });
        $scope.$on("SUCCESS", function (e, content) {
            $scope.MTitle = content.Title;
            $scope.MBody = content.Body;
            $('#smallModal').modal('show');
        });
        $scope.$on("ERROR", function (e, content) {
            $scope.MTitle = content.Title;
            $scope.MBody = content.Body;
            $('#smallModal').modal('show');
        });
        /* end region Loading */
    }]);

