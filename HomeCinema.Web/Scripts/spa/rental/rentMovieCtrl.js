(function (app) {
    'use strict';

    app.controller('rentMovieCtrl', rentMovieCtrl);

    rentMovieCtrl.$inject = ['$scope', '$modalInstance', '$location',
        'apiService', 'notificationService'];

    function rentMovieCtrl($scope, $modalInstance, $location, apiService,
        notificationService) {

        $scope.Title = $scope.movie.Title;
        $scope.loadStockItems = loadStockItems;
        $scope.selectCustomer = selectCustomer;
        $scope.rentMovie = rentMovie;
        $scope.cancelRental = cancelRental;
        $scope.stockItems = [];
        $scope.selectedCustomer = -1;
        $scope.isEnabled = false;

        function loadStockItems() {
            notificationService.displayInfo('Loading available stock items ' +
                'for ' + $scope.movie.Title);

            apiService.get('/api/stocks/movie/' + $scope.movie.ID, null,
                loadCompleted, loadFailed);

            function loadCompleted(response) {
                $scope.stockItems = response.data;
                $scope.selectedStockItem = $scope.stockItems[0].ID;
            }

            function loadFailed(response) {
                notificationService.displayError(response.data);
            }
        }

        function rentMovie() {
            apiService.post('/api/rentals/rent/' + $scope.selectCustomer + '/' +
                $scope.selectedStockItem, null, rentSucceeded, rentFailed);

            function rentSucceeded(response) {
                notificationService.displaySuccess('Rental completed successfully');
                $modalInstance.close();
            }

            function rentFailed(response) {
                notificationService.displayError(response.data.Message);
            }
        }

        function selectCustomer($item) {
            if ($item) {
                $scope.selectCustomer = $item.originalObject.ID;
                $scope.isEnabled = true;
            }
            else {
                $scope.selectedCustomer = -1;
                $scope.isEnabled = false;
            }
        }

        function selectionChanged($item) {

        }

        function cancelRental() {
            $scope.stockItems = [];
            $scope.selectedCustomer = -1;
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

        loadStockItems();
    }
})(angular.module('homeCinema'));