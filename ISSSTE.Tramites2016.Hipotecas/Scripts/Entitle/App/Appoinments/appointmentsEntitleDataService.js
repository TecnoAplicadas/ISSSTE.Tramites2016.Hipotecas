(function () {
    'use strict';

    angular
        .module(appName)
        .factory("appointmentsEntitleDataService", ["$http", "common", "appConfig",appointmentsEntitleDataService]);

    function appointmentsEntitleDataService($http, common, appConfig) {

        //#region Members

        var factory = {
            getAppointments: getAppointmentsByEntitleId,
            getCancelAndNotAttendedAppointments: getCancelAndNotAttendedAppointmentsByEntitleId,
            cancelAppointment: cancelAppointment
        };

        return factory;


        function getAppointmentsByEntitleId(isssteNumber,requestId) {
            var url = common.getBaseUrl() + "api/Calendar/CurrentsAppointmentsByRequestId/" + requestId;
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function getCancelAndNotAttendedAppointmentsByEntitleId(isssteNumber,requestId) {
            var url = common.getBaseUrl() + "api/Calendar/CancelAndNotAttendedAppointmentsByRequestId/" + requestId;
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function cancelAppointment(appointmentId, isssteNumber) {
            var url = common.getBaseUrl() + "api/Calendar/CancelAppointment/" + appointmentId;
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }


        //function usersOperator() {
        //    var url = common.getBaseUrl() + "api/Administrator/UsersOperator";
        //    var accessToken = authenticationService.getAccessToken();
        //    return $http.get(url, {
        //        headers: {
        //            'Content-Type': JSON_CONTENT_TYPE,
        //            'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
        //        }
        //    });
        //}   

    }
})();