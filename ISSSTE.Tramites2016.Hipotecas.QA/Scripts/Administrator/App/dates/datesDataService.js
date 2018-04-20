(function() {
    "use strict";

    angular
        .module(appName)
        .factory("datesDataService", ["$http", "common", "appConfig", "authenticationService", datesDataService]);

    function datesDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getRequests: getRequests,
            changueSta: changueSta

        };

        return factory;

        //#endregion 

        //#region Fields

        //#endregion

        //#region Methods


        function getRequests(pageSize, page, query, date, time) {
            var url = common.getBaseUrl() + "api/Administrator/Dates?pageSize={0}&page={1}&query={2}&date{3}&time{4}".format(pageSize, page, query,date,time);
            var accessToken = authenticationService.getAccessToken();
            var parameter = {};
            parameter.pageSize = pageSize;
            parameter.page = page;
            parameter.query = query;
            parameter.date = date;
            parameter.time = time;

            return $http.get(url,  {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getMessages(pageSize, page, query, date, time) {
            var url = common.getBaseUrl() + "api/Administrator/Messages";
            var accessToken = authenticationService.getAccessToken();
            var parameter = {};
            parameter.pageSize = pageSize;
            parameter.page = page;
            parameter.query = query;
            parameter.date = date;
            parameter.time = time;

            return $http.post(url, parameter, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        function changueSta(appointmentId, isAttended, requestId) {
            var url = common.getBaseUrl() + "api/Administrator/Changue?appointmentId={0}&isAttended={1}&requestId={2}".format(appointmentId, isAttended, requestId);
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