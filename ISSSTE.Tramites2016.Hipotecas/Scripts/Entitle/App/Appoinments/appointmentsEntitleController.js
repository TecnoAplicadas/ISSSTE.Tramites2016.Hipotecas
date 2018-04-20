(function () {
    'use strict';

    var controllerId = 'appointmentsEntitleController';
    angular
        .module(appName)
        .controller(controllerId, ['common', 'appointmentsEntitleDataService', appointmentsEntitleController]);

    function appointmentsEntitleController(common, appointmentsEntitleDataService) {
        var vm = this;

        //Informacion propiedades o elementos de la Vista
        vm.appointments = []; // Informacion de las citas
        vm.cancelNotAttendedAppointments = [];
        vm.NumberAppointments = 3;
        //vm.usersoperator = {};
        //vm.userselected = {};


        //Inicializacion de metodos
        vm.init = init;
        vm.SelectCancelApp = SelectCancelApp;
        vm.cancelApp = cancelApp;
        vm.addApp = addApp;
        vm.initSearch = initSearch;
        vm.appointmentSelected = "";
        //vm.getUsersOperator = getUsersOperator;


        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }


        function initSearch() {
            common.displayLoadingScreen();
            var appointmentsPromise = getAppointments();
            var cancelAndNotAttendedPromise = getCancelAndNotAttendedAppointments();
            //var usersPromise = getUsersOperator();
            //vm.requestassig = null;
            //vm.userselected = null;

            common.$q.all([appointmentsPromise, cancelAndNotAttendedPromise])
                .finally(function () {
                    init();
                    common.hideLoadingScreen();
                    //UI.initStatusDropdown();
                });
        }


        function getAppointments(appId) {
            common.displayLoadingScreen();
            var actualAppointment = {};

            if (appId == null || appId == undefined)
                actualAppointment = $("#appointmentSearch").val();
            else actualAppointment = appId;


            return appointmentsEntitleDataService.getAppointments(common.config.entitleInformation.NoISSSTE, actualAppointment)
                .success(function (data) {
                    vm.appointments = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.hideLoadingScreen();
                    //common.showErrorMessage(reason, Messages.error.requests); // Comento MFP 04/01/2017
                });
        }


        function getCancelAndNotAttendedAppointments(appId) {
            common.displayLoadingScreen();
            var actualAppointment = {};

            if (appId == null || appId == undefined)
                actualAppointment = $("#appointmentSearch").val();
            else actualAppointment = appId;


            return appointmentsEntitleDataService.getCancelAndNotAttendedAppointments(common.config.entitleInformation.NoISSSTE, actualAppointment)
                .success(function (data) {
                    vm.cancelNotAttendedAppointments = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    // common.showErrorMessage(reason, Messages.error.requests); // Comento MFP 04/01/2017
                    common.hideLoadingScreen();
                });
        }

        function cancelAppointment(appointmentId) {
            common.displayLoadingScreen();
            return appointmentsEntitleDataService.cancelAppointment(appointmentId, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    //vm.opinionRequest = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                });
        }

        function SelectCancelApp(appID) {
            vm.appointmentSelected = appID;
        }

        function cancelApp(appId) {
            common.displayLoadingScreen();
            if (appId == null || appId == undefined) {
                vm.appointmentSelected = $("#appointmentSelected").val();                
            }
            else {
                vm.appointmentSelected = appId;                
            }

            if (vm.appointmentSelected == undefined || vm.appointmentSelected == "" || vm.appointmentSelected == null) {
                common.showWarningMessage(Messages.info.SeleccionarCitaCancel);
                common.hideLoadingScreen();
                return;
            }


            var promise = cancelAppointment(vm.appointmentSelected);
            promise.success(function () {
                initSearch();
                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();
            });

        }

        function addApp(appId) {
            common.displayLoadingScreen();
            if (vm.appointments.length > 0) {
                common.showWarningMessage(Messages.info.ShouldCancelAppointment);
                common.hideLoadingScreen();
            }
            else if (vm.appointments.length + vm.cancelNotAttendedAppointments.length >= vm.NumberAppointments) {
                common.showWarningMessage(Messages.info.NumberOfAppointmentsReached);
                common.hideLoadingScreen();
            }
            else window.location.href = common.getBaseUrl() + "Entitle/Calendario?NoIssste=" + common.config.entitleInformation.NoISSSTE + "&RequestId=" + appId;
                       

        }

        function completeControllerInit() {
            common.logger.log(Messages.info.contollerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }


        function completeControllerSuccess() {
            completeControllerInit();
        }

        function completeControllerFail() {
            completeControllerInit();
        }


        //function getUsersOperator() {
        //    return webApiService.makeRetryRequest(1, function () {
        //        return appointmentsDataService.usersOperator();
        //    })
        //        .then(function (data) {
        //            vm.usersoperator = data;
        //        })
        //        .catch(function (reason) {
        //            common.showErrorMessage(reason, Messages.error.statusList);
        //        });
        //}

    }
})();