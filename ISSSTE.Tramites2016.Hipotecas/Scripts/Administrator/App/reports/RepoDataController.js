(function () {
    "use strict";

    angular
        .module(appName)
        .factory("RepoDataController", ["$http", "common", "appConfig", "authenticationService", RepoDataController]);

    function RepoDataController($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getAllDelegationList: getAllDelegationList,
            getStatusList: getStatusList,
            getTipoPensionList: getTipoPensionList,
            getRepo: getRepo,
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


        function getTipoPensionList() {
            var url = common.getBaseUrl() + "api/Administrator/TipoPension";
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getAllDelegationList() {
            var url = common.getBaseUrl() + "api/Administrator/AllDelegation";
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

        function getRepo(pageSize, page, Gender, delID, pensionId, statusId, nameEntiti, numIssste, inicio, final, banderaSoloDelegacion) {
            var url = common.getBaseUrl() + "api/Administrator/Reportes?pageSize={0}&page={1}&Gender={2}&delID={3}&pensionId={4}&statusId={5}&nameEntiti={6}&numIssste={7}&inicio={8}&final={9}&banderaSoloDelegacion={10}".format(pageSize, page, Gender, delID, pensionId, statusId, nameEntiti, numIssste, inicio, final, banderaSoloDelegacion);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


    } //#endregion

})();