//####################################################################
//      ## Fecha de creación: 18-03-2016
//      ## Fecha de última modificación: 30-03-2016
//      ## Responsable: Emanuel De la Isla Vértiz
//      ## Módulos asociados: Información general, Deudos, Beneficiarios, Historial Laboral.
//      ## Id Tickets asociados al cambio: R-013042
//####################################################################
(function () {
    "use strict";

    angular
        .module(appName)
        .factory("homeDataService", ["$http", "common", "appConfig", homeDataService]);

    function homeDataService($http, common, appConfig) {

        //#region Members

        var factory = {
            getEntitleInformation: getEntitleInformation,
            getAllEntitleInformation: getAllEntitleInformation,
            saveEntitle: saveEntitle,
            saveAllEntitle: saveAllEntitle,
            validateCurp: validateCurp,
            getAllSavedEntitleInformation: getAllSavedEntitleInformation,
            updateEntitleInformation: updateEntitleInformation,
            getDocumentsForInfo: getDocumentsForInfo,
            getEntitledAlerts: getEntitledAlerts,
        };

        return factory;

        //#endregion

        //#region Constants


        //#endregion

        //#region Fields

        //#endregion

        //#region Functions

        function getEntitleInformation(isssteNumber) {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Entitle/Information";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        //CAP function getAllEntitleInformation(isssteNumber, idPension) {
        function getAllEntitleInformation(isssteNumber) {
            //common.displayLoadingScreen();
            //CAP var url = common.getBaseUrl() + "api/Entitle/AllInformationNew/"  + idPension;
            var url = common.getBaseUrl() + "api/Entitle/AllInformationNew/" + isssteNumber;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }
        function getEntitledAlerts(isssteNumber) {
            // var url = common.getBaseUrl() + 'api/Entitle/' + isssteNumber + '/Alerts';
            common.displayLoadingScreen();
            var url = common.$location.$$absUrl.substring(0, (common.$location.$$absUrl.indexOf("/Entitle"))) + '/api/Entitle/' + isssteNumber + '/Alerts';

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE
                }
            });
        }
        function saveEntitle(entitle) {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Entitle/Save";

            return $http.post(url, entitle, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function saveAllEntitle(entitleData, isssteNumber) {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Entitle/SaveAll";

            return $http.post(url, entitleData, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function validateCurp(curp, isssteNumber) {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Entitle/ValidateCurp/" + curp;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function updateEntitleInformation(curp, telephone, email) {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + 'api/Entitle/Information';

            return $http.get(url, {
                Telephone: telephone,
                Email: email
            }, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }

        function getAllSavedEntitleInformation(isssteNumber, requestId) {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Entitle/AllInformation/" + requestId;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function getDocumentsForInfo() {
            //common.displayLoadingScreen();
            var url = common.getBaseUrl() + 'api/Entitle/Documents';

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE
                }
            });
        }

        //#endregion
    }
})();