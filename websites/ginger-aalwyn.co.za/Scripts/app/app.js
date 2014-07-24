// Main configuration file. Sets up AngularJS module and routes and any other config objects
var clock;
var appRoot = angular.module('main', ['ngRoute', 'ngGrid', 'ngResource', 'angularStart.services', 'angularStart.directives', 'ngAnimate', 'ui.bootstrap']);     //Define the main module

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
        $routeProvider
            .when('/home', { templateUrl: '/home/main', controller: 'MainController' })
             .when('/home/guestbook', { templateUrl: '/home/guestbook', controller: 'MainController' })
            .otherwise({ redirectTo: '/home' });
    }])
    .controller('RootController', ['$scope', '$route', '$routeParams', '$location', function ($scope, $route, $routeParams, $location) {
        var self = this;

        $scope.MenuItems = [{ "MenuLink": "SomeLink", "MenuText": "TEst" }, {}, {}];
        $scope.TestText = "abc";
        /* region Loading */
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
        self.startLoading = function () {
            $.blockUI({ "message": "<h3>Loading...</h3>"});
        }
        self.stopLoading = function () {
            $.unblockUI();
        }
        /* end region Loading */
    }]);




function goToByScroll(id) {
    try {
       if (window.location.hash.indexOf("guestbook") > -1 || window.location.hash.indexOf("rsvp") > -1)
           window.location = window.location.protocol + "//" + window.location.host + "/#/home";
       var obj1 = $("#" + id).offset().top;
       obj1 -= 150;
       var obj = { scrollTop: obj1 };
       $('html,body').stop().animate(obj,{ duration: 1500, easing: "easeInOutExpo" });
        // return false;
    }
    catch (e) {
        console.log(e);
        window.location.reload();
    }
}