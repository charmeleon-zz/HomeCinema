(function (app) {
    'use strict';

    app.controller('customerEditCtrl', customerEditCtrl);

    customerEditCtrl.$inject = ['$scope', '$modalInstance', '$timeout',
        'apiService', 'notificationService'];

    function customerEditCtrl($scope, $modalInstance, $timeout, apiService,
        notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateCustomer = updateCustomer;
        $scope.openDatePicker = openDatePicker;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

        function updateCustomer() {
            apiService.post('/api/customers/update/', $scope.EditedCustomer,
                updateCustomerCompleted, updateCustomerLoadFailed);
        }

        function updateCustomerCompleted(response) {
            notificationService.displaySuccess($scope.EditedCustomer.FirstName + ' ' + $scope.EditedCustomer.LastName + ' has been updated');
            $scope.EditedCustomer = {};
            $modalInstance.dismiss();
        }

        function updateCustomerLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $timeout(function () {
                $scope.datepicker.opened = true;
            });
        }
    }
})
(angular.module('homeCinema'));