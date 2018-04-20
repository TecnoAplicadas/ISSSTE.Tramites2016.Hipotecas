(function () {
    "use strict";

    angular
        .module(appName)
        .factory("requestsDataService", ["$http", "common", "appConfig", "authenticationService", requestsDataService]);

    function requestsDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getRequestEntitleInformation: getRequestEntitleInformation,
            getRequestInformation: getRequestInformation,
            getOpinionRequest: getOpinionRequest,
            getMessagesOpinion: getMessagesOpinion,
            saveStatusRequest: saveStatusRequest,
            getOpinion: getOpinion,
            getMessagesOpinionParameter: getMessagesOpinionParameter,
            updateDocument, updateDocument,
            updateRequestDocumentValidation: updateRequestDocumentValidation,
            saveTempObservations: saveTempObservations,

        };

        return factory;


        function saveTempObservations(Observations) {
            var url = common.getBaseUrl() + 'api/Administrator/SaveTempObservations';
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, Observations,
                {
                    headers: {
                        'Content-Type': JSON_CONTENT_TYPE,
                        'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });

        }
        //------------
        function updateRequestDocumentValidation(requestId, documentType, isValid, observations) {
            var url = common.getBaseUrl() + 'api/Administrator/Request/{0}/Documents/{1}'.format(requestId, documentType);
            var accessToken = authenticationService.getAccessToken();

            return $http.put(url,
                {
                    IsValid: isValid,
                    Comment: observations
                }, {
                    headers: {
                        'Content-Type': JSON_CONTENT_TYPE,
                        'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                    }
                });
        }
        //------------
        //#endregion

        //#region Fields

        //#endregion

        //#region Methods

        function getRequestEntitleInformation(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/AllInformation/" + requestId;
            var accessToken = authenticationService.getAccessToken();
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        function getRequestInformation(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/Request/" + requestId;
            var accessToken = authenticationService.getAccessToken();
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getOpinionRequest(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/OpinionAndRequest/" + requestId;
            var accessToken = authenticationService.getAccessToken();
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getMessagesOpinion() {
            var url = common.getBaseUrl() + "api/Administrator/OpinionMessages/";
            var accessToken = authenticationService.getAccessToken();
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getOpinion(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/OpinionAndRequest/" + requestId;
            var accessToken = authenticationService.getAccessToken();
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        function saveStatusRequest(requestId, idstatus, happy, opinion) {
            var url = common.getBaseUrl() + "api/Administrator/SaveStatusRequest";
            var staOpinion = {};
            staOpinion.requestId = requestId;
            staOpinion.idstatus = idstatus;
            staOpinion.happy = happy;
            staOpinion.opinion = opinion;

            var accessToken = authenticationService.getAccessToken();
            return $http.post(url, staOpinion, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function updateDocument(requestId, documentId, isValid, observations) {
            var url = common.getBaseUrl() + "api/Administrator/Documents";
            var dataDoc = {};
            dataDoc.RequestId = requestId;
            dataDoc.DocumentId = documentId;
            dataDoc.Observations = observations;
            dataDoc.IsValid = isValid;

            var accessToken = authenticationService.getAccessToken();

            return $http.post(url,
                dataDoc,
                {
                    headers: {
                        'Content-Type': JSON_CONTENT_TYPE,
                        'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                    }
                });
        }

        function getMessagesOpinionParameter(id) {
            var url = common.getBaseUrl() + "api/Administrator/OpinionsMessagesConfig/" + id;
            var accessToken = authenticationService.getAccessToken();
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        //#endregion

    }
})();