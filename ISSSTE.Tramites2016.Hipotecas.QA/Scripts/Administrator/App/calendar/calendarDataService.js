(function () {
    'use strict';

    angular
        .module(appName)
        .factory("calendarDataService", ["$http", "common", "appConfig", "authenticationService", calendarDataService]);

    function calendarDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getExistingData: getExistingData,
            getDelegations: getDelegations,
            saveSchedules: saveSchedules,
            deleteSchedule: deleteSchedule,
            saveSpecialDays: saveSpecialDays,
            deleteSpecialDay: deleteSpecialDay,
            saveSpecialScheduleDays: saveSpecialScheduleDays,
            deleteSpecialScheduleDays: deleteSpecialScheduleDays,
            getAppointmentsByDelegation: getAppointmentsByDelegation,
            getDeleteSpecialDaysSchedules: getDeleteSpecialDaysSchedules,
        };

        return factory;

        function getExistingData(delegationId) {
            var url = common.getBaseUrl() + 'api/Administrator/AdmninistratorSchedule/{0}'.format(delegationId);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getDelegations() {
            var url = common.getBaseUrl() + 'api/Administrator/Delegations';
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function saveSchedules(schedules) {
            var url = common.getBaseUrl() + '/api/Administrator/SaveSchedules';
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, schedules, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function deleteSchedule(id) {
            var url = common.getBaseUrl() + 'api/Administrator/DeleteSchedule/{0}'.format(id);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getDeleteSpecialDaysSchedules(id) {
            var url = common.getBaseUrl() + 'api/Administrator/DeleteSpecialDaysSchedules/{0}'.format(id);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }
        function saveSpecialDays(nonWorkingDays) {
            var url = common.getBaseUrl() + '/api/Administrator/SaveNonLaborableDays';
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, nonWorkingDays, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function deleteSpecialDay(info) {
            var url = common.getBaseUrl() + 'api/Administrator/DeleteNonLaboraleDays';
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, info, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function saveSpecialScheduleDays(specialScheduleDays) {
            var url = common.getBaseUrl() + '/api/Administrator/SaveSpecialScheduleDays';
            var accessToken = authenticationService.getAccessToken();

            return $http.post(url, specialScheduleDays, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function deleteSpecialScheduleDays(specialScheduleDays) {
            var url = common.getBaseUrl() + 'api/Administrator/DeleteSpecialScheduleDays/{0}'.format(specialScheduleDays);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getAppointmentsByDelegation(id) {
            var url = common.getBaseUrl() + 'api/Administrator/Appointments/Delegation/{0}'.format(id);
            var accessToken = authenticationService.getAccessToken();

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }
    }
})();