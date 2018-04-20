/// <reference path="requestDataService.js" />
//####################################################################
//      ## Fecha de creación: 18-03-2016
//      ## Fecha de última modificación: 30-03-2016
//      ## Responsable: Emanuel De la Isla Vértiz
//      ## Módulos asociados: Información general, Deudos, Beneficiarios, Historial Laboral.
//      ## Id Tickets asociados al cambio: R-013042
//####################################################################
(function () {
    "user strict";
    angular
        .module(appName)
        .factory("requestDataService", ["$http", "common", "appConfig", requestDataService]);

    function requestDataService($http, common, appConfig) {
        //#region Members
        var factory = {
            saveRequest: saveRequest,
            getRequest: getRequest,
            getPastRequests: getPastRequests,
            getRequests: getRequests,
            getCurrentRequests: getCurrentRequests,
            getRequestAll: getRequestAll,
            getNotifications: getNotifications,
            getOpinionRequest: getOpinionRequest,
            saveStatusRequest: saveStatusRequest,
            getCurpRequests: getCurpRequests,
            getDocumentsByRelationship: getDocumentsByRelationship,
            getModalidadPension: getModalidadPension,
            getRequestDebtors: getRequestDebtors,
            getRequestDebtorsByRequestId: getRequestDebtorsByRequestId,
            getDocumentsForInfo: getDocumentsForInfo,
            getTypeProperty: getTypeProperty,
            getUrbanCenter: getUrbanCenter,
            uploadDocument: uploadDocument
        };

        return factory;

        //#endregion
        //#region Constants


        //#endregion

        //#region Fields

        //#endregion

        //#region Functions

        function uploadDocument(curp, file, requestId, documentType) {
            common.displayLoadingScreen();
            var deferred = common.$q.defer();
            var promise = deferred.promise;

            if (file) {
                var url = common.getBaseUrl() + 'api/Entitle/Requests/{0}/Documents/{1}'.format(requestId, documentType);

                promise = Upload.upload({
                    url: url,
                    fields: {
                        'Issste-Tramites2015-UserIdentity': curp
                    },
                    file: file
                });
            }
            else
                deferred.resolve();

            return promise;
        }

        function getRequest(requestId, isssteNumber) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/Request/" + requestId;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function saveRequest(request, isssteNumber) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/Save";

            return $http.post(url, request, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function getPastRequests(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/PastRequestEntitled";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }


        function getRequests(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/RequestEntitled";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }

        function getCurrentRequests(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/CurrentRequest";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }



        function getRequestAll(requestId, state) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/RequestAllEntitle/" + requestId + "/" + state;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': requestId
                }
            });
        }



        function getNotifications(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/Notifications";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }

        //function getOpinionRequest(requestId) {
        //    var url = common.getBaseUrl() + "api/Administrator/OpinionAndRequest/" + requestId;
        //    //var accessToken = authenticationService.getAccessToken();
        //    return $http.get(url, {
        //        headers: {
        //            'Content-Type': JSON_CONTENT_TYPE,
        //            'Issste-Tramites2016-UserIdentity': requestId
        //            //'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
        //        }
        //    });
        //}

        function getOpinionRequest(requestId, curp, state) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/OpinionAndRequest/" + requestId + "/" + state;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }


        function saveStatusRequest(requestId, idstatus, happy, curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/SaveStatusRequest/" + requestId + "/" + idstatus + "/" + happy;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }

        function getCurpRequests(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/CurpRequest";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }

        function getDocumentsByRelationship(Pension, KeyRelationship) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/DocumentsByRelationship/" + Pension + "/" + KeyRelationship;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': Pension
                }
            });
        }
        function getModalidadPension(isssteNumber, KeyRelationship) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/ModalidadPension/" + KeyRelationship;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }
        function getRequestDebtors(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/RequestDebtors";

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }

        function getRequestDebtorsByRequestId(requestId) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + "api/Request/RequestDebtorsRequestId/" + requestId;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': requestId
                }
            });
        }

        function getDocumentsForInfo() {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + 'api/Entitle/Documents';

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE
                }
            });
        }

        function getTypeProperty(curp) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + 'api/Entitle/CatalogsProperty/' + curp;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }


        function getUrbanCenter(id) {
            common.displayLoadingScreen();
            var url = common.getBaseUrl() + 'api/Entitle/UrbanCenter/' + id;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': curp
                }
            });
        }




        //#endregion      CurrentRequest
    }

})();