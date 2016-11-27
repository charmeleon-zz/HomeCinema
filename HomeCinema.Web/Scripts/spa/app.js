(function () {
    'use strict';

    angular.module('homeCinema', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "/Scripts/spa/home/index.html",
                controller: "indexCtrl"
            })
            .when("/login", {
                templateUrl: "/Scripts/spa/account/login.html",
                controller: "loginCtrl"
            })
            .when("/register", {
                templateUrl: "/Scripts/spa/account/register.html",
                controller: "registerCtrl"
            })
            .when("/movies", {
                templateUrl: "/Scripts/spa/movies/movies.html",
                controller: "moviesCtrl"
            })
            .when("/movies/add", {
                templateUrl: "/Scripts/spa/movies/add.html",
                controller: "moviesAddCtrl"
            })
            .when("/movies/:id", {
                templateUrl: "/Scripts/spa/movies/details.html",
                controller: "moviesDetailsCtrl"
            })
            .when("/movies/edit/:id", {
                templateUrl: "/Scripts/spa/movies/edit.html",
                controller: "movieEditCtrl"
            })
            .when("/rental", {
                templateUrl: "/Scripts/spa/rental/rental.html",
                controller: "rentStatsCtrl"
            })
            .when("/customers", {
                templateUrl: "/Scripts/spa/customers/customers.html",
                controller: "customersCtrl"
            })
            .when("/customers/register", {
                templateUrl: "/Scripts/spa/customers/register.html",
                controller: "customersRegCtrl",
                // Check that the user is authenticated before the route changes
                resolve: {isAuthenticated: isAuthenticated}
            })
            .otherwise({ redirectTo: "/" });
    }

    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http'];

    function run($rootScope, $location, $cookieStore, $http) {
        $rootScope.repository = $cookieStore.get('repository') || {};

        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
        }

        $(document).ready(function () {
            $('.fancybox').fancybox({
                openEffect: 'none',
                closeEffect: 'none'
            });

            $('.fancybox-media').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: {}
                }
            });
        });
    }

    isAuthenticated.$inject = ['membershipService', '$rootScope', '$location'];

    function isAuthenticated(membershipService, $rootScope, $location) {
        if (!membershipService.isUserLoggedIn()) {
            $rootScope.previousState = $location.path();
            $location.path('/login');
        }
    }
})();