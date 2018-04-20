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
        .factory("dateDataService", ["$http", "common", "appConfig", dateDataService]);

    function dateDataService($http, common, appConfig) {
        //#region Members
        var factory = {
            getDates: getDates,
            cancelAppointment: cancelAppointment,
            calendarByMonthAndDelegation: calendarByMonthAndDelegation,
            getTimesCalendar: getTimesCalendar,
            getYears: getYears,
            getMonths: getMonths,
            saveDate: saveDate
        };

        return factory;

        //#endregion
        //#region Constants


        //#endregion

        //#region Fields

        //#endregion

        //#region Functions
        function getDates(requestId, isssteNumber) {
            var url;
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/DatesRequest/" + requestId;
            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/DatesRequest/" + requestId;
            }

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function cancelAppointment(appointmentId, isssteNumber) {
            var url;
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/CancelAppointment/" + appointmentId;

            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/CancelAppointment/" + appointmentId;
            }
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }


        function calendarByMonthAndDelegation(dateDelegation, isssteNumber) {
            var url
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/CalendarByMonthAndDelegation";
            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/CalendarByMonthAndDelegation";
            }
            return $http.post(url, dateDelegation, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }


        function getTimesCalendar(dateDelegation, isssteNumber) {
            var url
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/TimesCalendar";
            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/TimesCalendar";
                }
            return $http.post(url, dateDelegation, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function getYears(isssteNumber) {
            var url
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/Years/";
            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/Years/";
            }
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function getMonths(isssteNumber) {
            var url
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/Months/";
            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/Months/";
            }
            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        function saveDate(appoinment, isssteNumber) {
            var url
            if (common.getBaseUrl() != undefined) {
                url = common.getBaseUrl() + "api/Calendar/Appointment";
            }
            else {
                url = "http://" + window.location.host + "/api/Calendar/Appointment";
            }

            return $http.post(url, appoinment, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Issste-Tramites2016-UserIdentity': isssteNumber
                }
            });
        }

        //#endregion      CurrentRequest
    }

})();