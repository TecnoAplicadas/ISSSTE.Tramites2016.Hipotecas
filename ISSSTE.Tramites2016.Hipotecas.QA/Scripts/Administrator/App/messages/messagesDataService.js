(function () {
    'use strict';

    angular
        .module(appName)
        .factory("messagesDataService", ["$http", "common", "appConfig", "authenticationService", messagesDataService]);

    function messagesDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getmessages: getmessages,
            getmessagesById: getmessagesById,
            savemessages: savemessages,
            changeDescription: changeDescription
        };

        return factory;



        function getmessages(pageSize, page, query) {
            var url = common.getBaseUrl() + 'api/Administrator/Messages?pageSize={0}&page={1}&&query={2}'.format(pageSize, page, query);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getmessagesById(id) {
            var url = common.getBaseUrl() + 'api/Administrator/MessagesId/{0}'.format(id);
            var accessToken = authenticationService.getAccessToken()

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function savemessages(messages) {
            var url = common.getBaseUrl() + 'api/Administrator/Savemessages';
            var accessToken = authenticationService.getAccessToken()

            return $http.post(url, messages, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function changeDescription(message) {
            var url = common.getBaseUrl() + 'api/Administrator/SaveDescription';
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, message, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }




    }
})();