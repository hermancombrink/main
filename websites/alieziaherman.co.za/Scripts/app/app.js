var appRoot = angular.module('main', ['ngRoute', 'ngResource', 'ngAnimate', 'ui.bootstrap']);     //Define the main module

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
        $routeProvider
            .when('/home', { templateUrl: '/home/main', controller: 'MainController' })
            .when('/home/date', { templateUrl: '/home/date', controller: 'MainController' })
            .when('/home/gettingthere', { templateUrl: '/home/gettingthere', controller: 'MainController' })
            .when('/home/ourstory', { templateUrl: '/home/ourstory', controller: 'MainController' })
            .when('/home/event', { templateUrl: '/home/event', controller: 'MainController' })
            .when('/home/rsvp', { templateUrl: '/home/rsvp', controller: 'MainController' })
            .otherwise({ redirectTo: '/home' });
    }])
    .controller('RootController', ['$scope', '$route', '$routeParams', '$location', function ($scope, $route, $routeParams, $location) {
        var self = this;
        /* region Loading */
        $scope.$on("LOAD", function () { self.startLoading(); });
        $scope.$on("UNLOAD", function () { self.stopLoading(); });

        $scope.$on('$routeChangeStart', function () {
            console.log("Go router");
            $(".view-container").hide();
            self.startLoading();
        });
        $scope.$on('$routeChangeSuccess', function () {
            $scope.toggleMainView();
            self.stopLoading();
        });
        $scope.$on('$routeChangeError', function () {
             $scope.toggleMainView();
            self.stopLoading();
        });
        self.startLoading = function () {
           // $.blockUI({ "message": "<h3>Loading...</h3>" });
        }
        self.stopLoading = function () {
           // $.unblockUI();
        }
        /* end region Loading */

        $scope.toggleFade = function ()
        {
            $(".view-container").fadeOut("slow");
        }
        $scope.toggleMainView = function ()
        {
            $(".view-container").fadeIn("slow");
        }
       
    }]);
