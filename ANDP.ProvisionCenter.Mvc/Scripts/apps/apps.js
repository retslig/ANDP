
var app = angular.module('andpApps', [
    'andpApps.Controllers',
    'andpApps.Services',
    'ngGrid',
    'ngCookies',
    'ngResource'
]);

app.constant('config', {
    apiUrl: (function () {
        var baseUrl = '';
        if (document.location.hostname == "localhost") {
            baseUrl = 'http://localhost:60109';
        } else {
            baseUrl = 'https://' + window.location.host + '/ANDP.API.Rest.v2000/';
        }

        return baseUrl;
    })(),
    provisioningApiUrl: (function () {
        var baseUrl = '';
        if (document.location.hostname == "localhost") {
            baseUrl = 'http://localhost:60109';
        } else {
            baseUrl = 'https://' + window.location.host + '/ANDP.API.Rest.v2000/';
        }

        return baseUrl;
    })()
});

app.config(function ($provide, $httpProvider) {

    // Intercept http calls.
    $provide.factory('MyHttpInterceptor', function ($q, $cookies) {
        return {
            // On request success
            request: function (config) {
                var token = $cookies.AngularAuthToken;
                config.headers['Authorization'] = 'Bearer ' + token;

                // Return the config or wrap it in a promise if blank.
                return config || $q.when(config);
            },

            // On request failure
            //requestError: function (rejection) {
            //    //console.log('requestError:' + JSON.stringify(rejection));

            //    // Return the promise rejection.
            //    return $q.reject(rejection);
            //},

            // On response success
            //response: function (response) {
            //    //console.log('response:' + JSON.stringify(response));

            //    // Return the response or promise.
            //    return response || $q.when(response);
            //},

            // On response failure
            responseError: function (rejection) {
                var status = rejection.status;
                if (status == 401) {
                    window.location = "/Auth/login?returnUrl=" + window.location.pathname + '&renewSession=true';
                    return;
                }

                // Return the promise rejection.
                return $q.reject(rejection);
            }
        };
    });

    // Add the interceptor to the $httpProvider.
    $httpProvider.interceptors.push('MyHttpInterceptor');
});
