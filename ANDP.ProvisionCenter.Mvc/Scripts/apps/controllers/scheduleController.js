
angular.module('andpApps.Controllers').
    controller('scheduleController', [
        '$scope', 'provisionService', 'orderService', function($scope, provisionService, orderService) {
            $scope.companies = [];
            $scope.externalCompanyId = "";

            toastr.options = {
                "positionClass": "toast-bottom-right"
            };

            $scope.initialize = function() {
                provisionService.retrieveAllCompaniesExtIds().
                    success(function(companies) {
                        $scope.companies = companies;
                        $scope.externalCompanyId = companies[0].ExternalCompanyId;
                        $scope.refresh();
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                        console.debug(data);
                    });
            };

            $scope.onCompanyChange = function() {
                $scope.refresh();
            };

            $scope.refresh = function() {
                //retrieve provisioning orders
                orderService.retrieveOrdersThatAreProvisioning($scope.externalCompanyId).
                    success(function(orders) {
                        $scope.orders = orders;
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    });

                //Get Provisioning Engine status
                provisionService.retrieveProvisioningEngineStatus($scope.externalCompanyId).
                    success(function(status) {
                        $scope.schedulerStatus = status.replace(/["']/g, "");

                        if ($scope.schedulerStatus == "running") {
                            $scope.showUnPauseButton = false;
                            $scope.showPauseButton = true;
                            $scope.showStopButton = true;
                            $scope.showStartButton = false;
                        } else if ($scope.schedulerStatus == "paused") {
                            $scope.showUnPauseButton = true;
                            $scope.showPauseButton = false;
                            $scope.showStopButton = false;
                            $scope.showStartButton = false;
                        } else {
                            $scope.showUnPauseButton = false;
                            $scope.showPauseButton = false;
                            $scope.showStopButton = false;
                            $scope.showStartButton = true;
                        }
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                        $scope.showUnPauseButton = false;
                        $scope.showPauseButton = false;
                        $scope.showStopButton = false;
                        $scope.showStartButton = false;
                        $scope.schedulerStatus = 'Error';
                    });
            };

            $scope.startProvisioningEngine = function() {
                provisionService.startProvisioningEngine($scope.externalCompanyId).
                    success(function(response) {
                        $scope.refresh();
                        toastr.success('Service started');
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    });
            };

            $scope.stopProvisioningEngine = function() {
                provisionService.stopProvisioningEngine($scope.externalCompanyId).
                    success(function(response) {
                        $scope.refresh();
                        toastr.success('Service stopped');
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    });
            };

            $scope.unpauseProvisioningEngine = function () {
                provisionService.unpauseProvisioningEngine($scope.externalCompanyId).
                    success(function (response) {
                        $scope.refresh();
                        toastr.success('Service unpaused');
                    }).error(function (data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    });
            };

            $scope.pauseProvisioningEngine = function () {
                provisionService.pauseProvisioningEngine($scope.externalCompanyId).
                    success(function (response) {
                        $scope.refresh();
                        toastr.success('Service paused');
                    }).error(function (data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    });
            };

            $scope.initialize();
        }
    ]);
