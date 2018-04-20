(function () {
    'use strict';

    angular
        .module(appName)
        .factory('supportDataService', ['$http', 'common', 'authenticationService', supportDataService]);

    function supportDataService($http, common, authenticationService) {

        //#region Members

        var factory = {
            validateIsssteNumber: validateIsssteNumber,
            getEntitleIsssteNumberByCurp: getEntitleIsssteNumberByCurp,
            getEntitleIsssteNumberByRfc: getEntitleIsssteNumberByRfc,
            getEntitleApplicationUrl: getEntitleApplicationUrl,
            getNotEntitleApplicationUrl: getNotEntitleApplicationUrl
        };

        return factory;

        //#endregion

        //#region Fields

        //#endregion

        //#region Methods

        function validateIsssteNumber(issteNumber) {
            var url = common.getBaseUrl() + 'api/Administrator/Entitle/{0}'.format(issteNumber);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getEntitleIsssteNumberByCurp(curp) {
            var url = common.getBaseUrl() + 'api/Administrator/Entitle?curp={0}&rfc='.format(curp);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getEntitleIsssteNumberByRfc(rfc) {
            var url = common.getBaseUrl() + 'api/Administrator/RFC?rfc={0}'.format(rfc);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getEntitleApplicationUrl(noIssste) {
            var url = common.getBaseUrl() + "Entitle/Index/?noissste={0}".format(noIssste);

            return url;
        }

        function getNotEntitleApplicationUrl(curp) {
            var url = common.getBaseUrl() + "Entitle/Index/?curp={0}".format(curp);

            return url;
        }

        //#endregion
    }
})();