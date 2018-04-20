(function() {
    "use strict";

    angular.module("common").factory("logger",
    ["$log", "appConfig", logger]);

    function logger($log, appConfig) {
        var service = {
            log: log,
            logError: logError,
            logSuccess: logSuccess,
            logWarning: logWarning
        };

        return service;

        //#region Methods

        function log(message, data, source, showNotification) {
            writeLog(message, data, source, showNotification, "info");
        }

        function logError(message, data, source, showNotification) {
            writeLog(message, data, source, showNotification, "error");
        }

        function logSuccess(message, data, source, showNotification) {
            writeLog(message, data, source, showNotification, "success");
        }

        function logWarning(message, data, source, showNotification) {
            writeLog(message, data, source, showNotification, "warning");
        }

        //#endregion

        //#region Helpers

        function writeLog(message, data, source, showNotification, notificationType) {
            var iconUrl, notiTitle, sticky = false;
            showNotification = showNotification || true;

            var write = (notificationType === "error") ? $log.error : $log.log;
            source = source ? "[" + source + "] " : "";
            write(source, message, data);

            if (showNotification) {
                if (notificationType === "info") {
                    if (!appConfig.showDebugNotiSetting) {
                        return;
                    } else {
                        iconUrl = "../Images/info.png";
                        notiTitle = "Gastos Generales: DEBUG LOG";
                    }
                } else if (notificationType === "error") {
                    iconUrl = "../Images/error.png";
                    notiTitle = "Gastos Generales: ERROR";
                    //sticky = true;
                } else if (notificationType === "warning") {
                    iconUrl = "../Images/warning.png";
                    notiTitle = "Gastos Generales: WARNING";
                } else if (notificationType === "success") {
                    iconUrl = "../Images/success.png";
                    notiTitle = "Gastos Generales";
                }

                //TODO: Colocar código para lanzar notificaciones
                alert(message);
            }
        }

        //#endregion
    }
})();