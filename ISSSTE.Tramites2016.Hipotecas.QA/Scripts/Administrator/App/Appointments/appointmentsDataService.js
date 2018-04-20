(function () {
    "use strict";

    angular
        .module(appName)
        .factory("appointmentsDataService", ["$http", "common", "appConfig", "authenticationService", appointmentsDataService]);

    function appointmentsDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getRequests: getRequests,
            changueSta: changueSta,
            getStatusList: getStatusList,
            usersOperator: usersOperator,
            getRequestsAppoinmets: getRequestsAppoinmets

        };

        return factory;

        //#endregion 

        //#region Fields

        //#endregion

        //#region Methods


        function getRequests(pageSize, page, query, date, time, statusId) {
            var url = common.getBaseUrl() + "api/Administrator/Appointments?pageSize={0}&page={1}&query={2}&date={3}&time={4}&statusId={5}".format(pageSize, page, query, date, time, statusId);
            var accessToken = authenticationService.getAccessToken();
            //var parameter = {};
            //parameter.pageSize = pageSize;
            //parameter.page = page;
            //parameter.query = query;
            //parameter.date = date;
            //parameter.time = time;

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }
        function getStatusList() {
            var url = common.getBaseUrl() + "api/Administrator/DateStatus";
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        //function getRequests(pageSize, page, query, statusId) {
        //    var url = common.getBaseUrl() + "api/Administrator/Appointments?pageSize={0}&page={1}&query={2}&statusId={3}".format(pageSize, page, query, statusId);
        //    var accessToken = authenticationService.getAccessToken();

        //    return $http.get(url, {
        //        headers: {
        //            'Content-Type': JSON_CONTENT_TYPE,
        //            'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
        //        }
        //    });
        //}

        function getRequestsAppoinmets(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/AppointmentsRequest?RequestId={0}".format(requestId);
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