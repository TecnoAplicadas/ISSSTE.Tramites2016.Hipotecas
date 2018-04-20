(function() {
    "use strict";

    angular
        .module(appName)
        .factory("searchDataService", ["$http", "common", "appConfig", "authenticationService", searchDataService]);

    function searchDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getStatusList: getStatusList,
            getRequests: getRequests,
            usersOperator: usersOperator,
            assign: assign
        };

        return factory;

        //#endregion 

        //#region Fields

        //#endregion

        //#region Methods

        function getStatusList() {
            var url = common.getBaseUrl() + "api/Administrator/Status";
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getRequests(pageSize, page, query, statusId, orden) {
            var url = common.getBaseUrl() + "api/Administrator/Requests?pageSize={0}&page={1}&orden={2}&query={3}&statusId={4}".format(pageSize, page,orden, query, statusId);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        function usersOperator() {
            var url = common.getBaseUrl() + "api/Administrator/UsersOperator";
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function assign(requestId, user) {
            var userrequest = {};
            userrequest.User = user;
            userrequest.RequestId = requestId;
            var url = common.getBaseUrl() + "api/Administrator/AsingRequest";
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, userrequest, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


    } //#endregion

})();