
angular.module('andpApps.Services').
    factory('provisionService', ['config', '$http', function (config, $http) {
        var provisionService = {};
        var baseUrl = config.apiUrl;

        //Uses Controller in project
        provisionService.retrieveAllCompaniesExtIds = function () {
            return $http({
                method: 'GET',
                url: baseUrl + '/api/company/'
            });
        }

        //Uses Controller in project
        provisionService.provisionOrder = function (externalCompanyId, externalOrderId, testMode, forceProvision) {
            var data = {
                ExternalCompanyId: externalCompanyId,
                ExternalOrderId: externalOrderId,
                TestMode: testMode,
                ForceProvision: forceProvision
            };

            return $http({
                method: 'POST',
                data: 'OrderDto=' + data,
                url: baseUrl + '/api/engine/provision/order'
            });
        }

        //Uses Controller in project
        provisionService.provisionService = function (externalCompanyId, externalOrderId, externalServiceId, testMode, forceProvision) {
            var data = {
                ExternalCompanyId: externalCompanyId,
                ExternalOrderId: externalOrderId,
                ExternalServiceId: externalServiceId,
                TestMode: testMode,
                ForceProvision: forceProvision
            };
            return $http({
                method: 'POST',
                data: 'ServiceDto=' + data,
                url: baseUrl + '/api/engine/provision/service'
            });
        }

        //Uses Controller in project
        provisionService.stopProvisioningEngine = function (externalCompanyId) {
            return $http({
                method: 'POST',
                data: '"' + externalCompanyId + '"',
                url: baseUrl + '/api/engine/stop'
            });
        }

        provisionService.startProvisioningEngine = function (externalCompanyId) {
            return $http({
                method: 'POST',
                data: '"' + externalCompanyId + '"',
                url: baseUrl + '/api/engine/start'
            });
        }

        provisionService.pauseProvisioningEngine = function (externalCompanyId) {
            return $http({
                method: 'POST',
                data: '"' + externalCompanyId + '"',
                url: baseUrl + '/api/engine/pause'
            });
        }

        provisionService.unpauseProvisioningEngine = function (externalCompanyId) {
            return $http({
                method: 'POST',
                data: '"' + externalCompanyId + '"',
                url: baseUrl + '/api/engine/unpause'
            });
        }

        provisionService.retrieveProvisioningEngineStatus = function (externalCompanyId) {
            return $http({
                method: 'GET',
                url: baseUrl + '/api/engine/status/company/' + externalCompanyId
            });
        }

        return provisionService;
    }]);
