(function (app) {
    'use strict';

    app.controller('movieDetailsCtrl', movieDetailsCtrl);

    movieDetailsCtrl.$inject = ['$scope', '$location', '$routeParams',
        '$modal', 'apiService', 'notificationService'];

    function movieDetailsCtrl($scope, $location, $routeParams, $modal,
        apiService, notificationService) {
        $scope.pageClass = 'page-movies';
        $scope.movie = {};
        $scope.loadingMovie = true;
        $scope.loadingRentals = true;
        $scope.isReadOnly = true;
        $scope.openRentDialog = openRentDialog;
        $scope.returnMovie = returnMovie;
        $scope.rentalHistory = [];
        $scope.getStatusColor = getStatusColor;
        $scope.clearSearch = clearSearch;
        $scope.isBorowed = isBorrowed;

        function loadMovie() {
            $scope.loadingMovie = true;

            apiService.get('/api/movies/details/' + $routeParams.id, null,
                loadCompleted, loadFailed);

            function loadCompleted(response) {
                $scope.movie = response.data;
                $scope.loadingMovie = false;
            }

            function loadFailed(response) {
                notificationService.displayError(response.data);
            }
        }

        function clearSearch() {
            $scope.filterRentals = '';
        }

        function openRentDialog() {
            $modal.open({
                templateUrl: '/Scripts/spa/rental/rentMovieModal.html',
                controller: 'rentMovieCtrl',
                scope: $scope
            }).result.then(loadMovieDetails, function () { });
        }

        function loadMovieDetails() {
            loadMovie();
            loadRentalHistory();
        }

        function loadRentalHistory() {
            $scope.loadingRentals = true;
            apiService.get('/api/rentals/' + $routeParams.id + '/rentalhistory',
                null, loadCompleted, loadFailed);

            function loadCompleted(response) {
                $scope.rentalHistory = result.data;
                $scope.loadingRentals = false;
            }

            function loadFailed(response) {
                notificationService.displayError(response);
            }
        }

        function returnMovie() {
            apiService.post('/api/rentals/return/' + rentalID, null,
                returnCompleted, returnFailed);

            function returnCompleted(response) {
                notificationService.displaySuccess('Movie returned successfully');
                loadMovieDetails();
            }

            function returnFailed(response) {
                notificationService.displayError(response.data);
            }
        }

        function getStatusColor(status) {
            if ('Borrowed' == status) {
                return 'red';
            }
            return 'green';
        }

        function isBorrowed(rental) {
            return 'Borrowed' == rental.Status;
        }
    }
})(angular.module('homeCinema'));