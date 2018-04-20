(function () {
    'use strict';

    angular
        .module(appName)
        .factory('catalogsDataService', ['$http', 'common', 'appConfig', 'authenticationService', catalogsDataService]);

    function catalogsDataService($http, common, appConfig, authenticationService) {

        //#region Constants

        var STRING_CONTENT_TEMPLATE = "'{0}'";

        //#endregion

        //#region Members

        var factory = {
            getCatalogItems: getCatalogItems,
            getCatalogItemDetail: getCatalogItemDetail,
            addOrUpdateCatalogItem: addOrUpdateCatalogItem,
            deleteCatalogItemDetail: deleteCatalogItemDetail,
            getDependentCatalogs: getDependentCatalogs
        };

        return factory;

        //#endregion

        //#region Methods

        function getCatalogItems(catalogName) {
            var url = common.getBaseUrl() + 'api/Administrator/Catalogs/{0}'.format(catalogName);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getCatalogItemDetail(catalogName, itemKey) {
            var url = common.getBaseUrl() + 'api/Administrator/Catalogs/{0}/{1}'.format(catalogName, itemKey);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function addOrUpdateCatalogItem(catalogName, itemData) {
            var url = common.getBaseUrl() + 'api/Administrator/Catalogs/{0}'.format(catalogName);
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url,
                STRING_CONTENT_TEMPLATE.format(JSON.stringify(itemData)),
                {
                    headers: {
                        'Content-Type': JSON_CONTENT_TYPE,
                        'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                    }
                }
            );
        }

        function deleteCatalogItemDetail(catalogName, itemData) {
            var url = common.getBaseUrl() + 'api/Administrator/Catalogs/{0}'.format(catalogName);
            var accessToken = authenticationService.getAccessToken();

            return deleteWithBody(url,
                STRING_CONTENT_TEMPLATE.format(JSON.stringify(itemData)), {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            );
        }

        function getDependentCatalogs(catalogName, dependentPropertyNames) {
            var url = common.getBaseUrl() + 'api/Administrator/Catalogs/{0}/Dependents'.format(catalogName);
            var accessToken = authenticationService.getAccessToken();

            return $http.put(url,
                dependentPropertyNames, {
                    headers: {
                        'Content-Type': JSON_CONTENT_TYPE,
                        'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                    }
                }
            );
        }

        //#endregion

        //#region Helper Methods

        function getWithBody(url, body, headers) {
            return $http({
                url: url,
                method: 'GET',
                data: body,
                headers: headers
            });
        }

        function deleteWithBody(url, body, headers) {
            return $http({
                url: url,
                method: 'DELETE',
                data: body,
                headers: headers
            });
        }

        //#endregion

    }
})();