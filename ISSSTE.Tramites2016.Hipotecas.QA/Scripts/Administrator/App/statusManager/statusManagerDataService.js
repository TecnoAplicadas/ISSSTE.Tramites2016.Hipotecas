(function () {
    'use strict';

    angular
        .module(appName)
        .factory('statusManagerDataService', ['$http', 'common', 'authenticationService', statusManagerDataService]);

    function statusManagerDataService($http, common, authenticationService) {

        //#region Members

        var factory = {
            changeRequestStatus: changeRequestStatus,
            getAvailableNextStatus: getAvailableNextStatus
        };

        return factory;

        //#endregion

        //#region Fields

        //#endregion

        //#region Methods

        function changeRequestStatus(statusId,requestId) {
            var url = common.getBaseUrl() + "api/Administrator/ChangeRequestStatus";
            var accessToken = authenticationService.getAccessToken();
            var parameter = {
                newStatus: statusId,
                requestId: requestId
            };
            //aqui van los parámetros que sean necesarios
            return $http.post(url, parameter, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }
        function getAvailableNextStatus(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/GetAvailableNextStatus";
            var accessToken = authenticationService.getAccessToken();
            var parameter = {
                requestId: requestId
            };
            var header = {
                'Content-Type': JSON_CONTENT_TYPE,
                'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
            };
            //aqui van los parámetros que sean necesarios
            return $http.post(url, parameter, {
                headers: header
            });
        }

        //#endregion
    }
})();