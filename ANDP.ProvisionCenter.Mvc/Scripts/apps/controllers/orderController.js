
angular.module('andpApps.Controllers').
    controller('orderGridController', ['$scope', 'provisionService', 'orderService', function ($scope, provisionService, orderService) {
        $scope.GridLoading = false;
        $scope.resultMessage = "";
        $scope.orders = [];
        $scope.mySelections = [];
        $scope.companies = [];
        $scope.externalCompanyId = "";
        $scope.statusTypes = [{ code: 'processing', name: 'Processing' }, { code: 'pending', name: 'Pending' }, { code: 'error', name: 'Error' }, { code: 'success', name: 'Success' }, { code: 'deleted', name: 'Deleted' }];
        $scope.statusType = "";
        $scope.testModes = [{ code: true, name: 'true' }, { code: false, name: 'false' }];
        $scope.testMode = false;
        $scope.forceProvisions = [{ code: true, name: 'true' }, { code: false, name: 'false' }];
        $scope.ForceProvision = false;

        toastr.options = {
            "positionClass": "toast-bottom-right"
        };

        $scope.initialize = function() {
            orderService.retrieveAllCompaniesExtIds().
                success(function(companies) {
                    $scope.companies = companies;
                }).error(function(data, status, headers, config) {
                    toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    console.debug(data);
                });
        };


        $scope.ToJavaScriptDate = function(value) {
            if (value.indexOf('Date') != -1) {
                var pattern = /Date\(([^)]+)\)/;
                var results = pattern.exec(value);
                var dt = new Date(parseFloat(results[1]));
                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
            } else if (value.indexOf('T') != -1) {
                var dt = new Date(value);
                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
            }

            return value;
        };

        $scope.refresh = function () {
            $scope.GridLoading = !$scope.GridLoading;

            orderService.retrieveOrdersByStatusTypeAndExtCompanyId($scope.statusType, $scope.externalCompanyId).
                success(function(orders) {
                    var services = [];
                    angular.forEach(orders, function(order) {
                        angular.forEach(order.Services, function(service) {
                            service.ExternalOrderId = order.ExternalOrderId;
                            service.ExternalCompanyId = order.ExternalCompanyId;
                            service.ProvisionDate = $scope.ToJavaScriptDate(service.ProvisionDate);
                            services.push(service);
                        });
                    });

                    $scope.orders = orders;
                    $scope.services = services;
                    $scope.GridLoading = !$scope.GridLoading;
                }).error(function(data, status, headers, config) {
                    toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    $scope.GridLoading = !$scope.GridLoading;
                });
        };

        $scope.nCompanyChange = function() {
            $scope.GridLoading = !$scope.GridLoading;
            orderService.retrieveOrdersByStatusTypeAndExtCompanyId($scope.statusType, $scope.externalCompanyId).
                success(function(orders) {
                    var services = [];
                    angular.forEach(orders, function(order) {
                        angular.forEach(order.Services, function(service) {
                            service.ExternalOrderId = order.ExternalOrderId;
                            service.ExternalCompanyId = order.ExternalCompanyId;
                            service.ProvisionDate = $scope.ToJavaScriptDate(service.ProvisionDate);
                            services.push(service);
                        });
                    });

                    $scope.orders = orders;
                    $scope.services = services;
                    $scope.GridLoading = !$scope.GridLoading;
                }).error(function(data, status, headers, config) {
                    toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                    $scope.GridLoading = !$scope.GridLoading;
                });
        };

        $scope.gridOptions = {
            data: 'services',
            columnDefs: [
                { field: 'ExternalOrderId', displayName: 'External Order Id' },
                { field: 'ExternalServiceId', displayName: 'External Service Id' },
                { field: 'ExternalCompanyId', displayName: 'External Company Id', visible: false },
                { field: 'Priority', displayName: 'Priority', visible: false },
                { field: 'ActionType', displayName: 'Action Type', visible: false },
                { field: 'ProvisionSequence', displayName: 'Provision Sequence', visible: false },
                { field: 'ProvisionDate', displayName: 'Provision Date' },
                { field: 'StatusType', displayName: 'Status Type' },
                { field: 'CreatedByUser', displayName: 'Created By User', visible: false }
            ],
            groups: ['ExternalOrderId'],
            showFooter: true,
            showGroupPanel: true,
            enablePinning: true,
            enableColumnResize: true,
            enableColumnReordering: true,
            showColumnMenu: true,
            showFilter: true,
            //jqueryUIDraggable: true, bug: when used grouping stop work, but column reordering start to work:(
            selectedItems: $scope.mySelections,
            multiSelect: false,
            jqueryUITheme: false
        };

        $scope.provisionOrder = function() {
            $scope.ProvisioningLoading = !$scope.ProvisioningLoading;

            //If they have not selected anything don't allow the to provisioning anything.
            if ($scope.mySelections.length < 1) {
                toastr.error('Please select a row first!');
                $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
            } else {
                orderService.provisionOrder($scope.externalCompanyId, $scope.mySelections[0].ExternalOrderId, $scope.testMode, $scope.ForceProvision).
                    success(function(response) {
                        $scope.resultMessage = response;
                        $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                        console.debug(data);
                        $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
                    });
            }
        };

        $scope.provisionService = function() {
            $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
            //If they have not selected anything don't allow the to provisioning anything.
            if ($scope.mySelections.length < 1) {
                toastr.error('Please select a row first!');
                $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
            } else {
                orderService.provisionService($scope.externalCompanyId, $scope.mySelections[0].ExternalOrderId, $scope.mySelections[0].ExternalServiceId, $scope.testMode, $scope.ForceProvision).
                    success(function(response) {
                        $scope.resultMessage = response;
                        $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
                    }).error(function(data, status, headers, config) {
                        toastr.error('Sorry we have run into a error! ' + data.ExceptionMessage);
                        console.debug(data);
                        $scope.ProvisioningLoading = !$scope.ProvisioningLoading;
                    });
            }
        };

        $scope.initialize();
    }]);
