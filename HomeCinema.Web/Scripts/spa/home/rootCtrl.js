(function (app) {
    'use strict';

    app.controller('rootCtrl', rootCtrl);

    rootCtrl.$inject = ['$scope', 'membershipService', '$rootScope', '$location'];

    function rootCtrl($scope, membershipService, $rootScope, $location) {
        $scope.userData = {};

        $scope.userData.displayUserInfo = displayUserInfo;
        $scope.logout = logout;

        function displayUserInfo() {
            $scope.userData.isUserLoggedIn = membershipService.isUserLoggedIn();

            if ($scope.userData.isUserLoggedIn)
            {
                $scope.username = $rootScope.repository.loggedUser.username;
            }
        }

        function logout() {
            membershipService.removeCredentials();
            $location.path('#/');
            $scope.userData.displayUserInfo();
        }

        $scope.userData.displayUserInfo();
    }
})(angular.module('homeCinema'));