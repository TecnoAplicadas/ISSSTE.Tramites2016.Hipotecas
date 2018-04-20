(function() {
    "user strict";
    angular
        .module(appName)
        .factory("commonDataService", ["$http", "common", "appConfig", commonDataService]);

    function commonDataService($http, common, appConfig) {
        //#region Members
        var factory = {
            getDelegation: getDelegation,
            getDelegations: getDelegations
        };

        return factory;

//#endregion
        //#region Constants


        //#endregion

        //#region Fields

        //#endregion

        //#region Functions
        function getDelegations(isssteNumber) {
            var url = common.getBaseUrl() + "/api/Common/Delegations/";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function getDelegation(idDelegation, isssteNumber) {
            var url = common.getBaseUrl() + "/api/Common/Delegation/" + idDelegation;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }


//#endregion      CurrentRequest
    }

})();