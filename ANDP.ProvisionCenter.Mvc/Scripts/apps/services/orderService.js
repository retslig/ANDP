
angular.module('andpApps.Services').
    factory('orderService', ['config', '$http', function (config, $http) {
        var orderService = {};
        var baseUrl = config.apiUrl;

        //Uses Controller in project
        orderService.retrieveAllCompaniesExtIds = function() {
            return $http({
                method: 'GET',
                url: baseUrl + '/api/company/'
            });
        }

        //Uses Controller in project
        orderService.retrieveOrdersByStatusTypeAndExtCompanyId = function(statusType, externalCompanyId) {
            return $http({
                method: 'GET',
                url: baseUrl + '/api/order/statustype/' + statusType + '/company/' + externalCompanyId
            });
        }
        
        orderService.retrieveOrdersThatAreProvisioning = function (externalCompanyId) {
            return $http({
                method: 'GET',
                url: baseUrl + '/api/order/processing/company/' + externalCompanyId
            });
        }

        return orderService;
    }]);
