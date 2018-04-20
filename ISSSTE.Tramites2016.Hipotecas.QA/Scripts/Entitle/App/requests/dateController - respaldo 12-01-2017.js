//####################################################################
//      ## Fecha de creación: 18-03-2016
//      ## Fecha de última modificación: 30-03-2016
//      ## Responsable: Emanuel De la Isla Vértiz
//      ## Módulos asociados: Información general, Deudos, Beneficiarios, Historial Laboral.
//      ## Id Tickets asociados al cambio: R-013042
//####################################################################
(function () {
    "use strict";
    var controllerId = "dateController";

    angular
        .module(appName)
        .controller(controllerId, ["$routeParams", "common", "homeDataService", "requestDataService", "dateDataService", "commonDataService", dateController]);

    function dateController($routeParams, common, homeDataService, requestDataService, dateDataService, commonDataService) {
        //#region Controller Members
        var date = new Date();
        var vm = this;
        vm.dates = {};
        vm.FromDate = (date.getDate());
        vm.opinionRequest = {};
        vm.initDates = initDates;
        vm.getDates = getDates;
        vm.getOpinionRequest = getOpinionRequest;
        vm.loadData = loadData;
        vm.saveStatusRequest = saveStatusRequest;
        vm.cancelAppointment = cancelAppointment;
        vm.aproveDiagnostic = aproveDiagnostic;
        vm.rejectDiagnostic = rejectDiagnostic;
        vm.cancelApp = cancelApp;
        vm.getDelegations = getDelegations;
        vm.getDelegation = getDelegation;
        vm.entitle = {};
        vm.getEntitleInformation = getEntitleInformation;
        vm.initCalendar = initCalendar;
        vm.selectDelegation = selectDelegation;
        vm.getCalendarMont = getCalendarMonth;
        vm.getTimesCalendar = getTimesCalendar;


        vm.getCalendar = getCalendar;
        vm.getTimes = getTimes;
        vm.DelegationSelect = {};
        vm.calendar = {};
        vm.times = {};
        vm.dateSelected = {};
        vm.dateDelegation = {};
        vm.timeselection = {};
        vm.month = {};
        vm.year = {};
        vm.month = {};
        vm.months = {};
        vm.years = {};
        vm.yearSelect = {};
        vm.monthSelect = {};
        vm.getYears = getYears;
        vm.getMonths = getMonths;
        vm.initCombos = initCombos;
        vm.reqId = {};
        vm.dayCalendar = {};
        vm.app = {};
        vm.saveAppointment = saveAppointment;
        vm.selectTime = selectTime;


        //#endregion
        //#region Constants

        //#endregion

        //#region Fields


        //#endregion

        //#region Properties


        //#endregion        

        //#region Public Functions

        function getCalendar() {
            vm.times = {};
            vm.calendar = {};
            vm.dateDelegation = {};
            vm.dayCalendar = {};
            vm.dateSelected = {};
            vm.timeselection = null;

            common.displayLoadingScreen();
            if (vm.dateSelected == null || vm.DelegationSelect == null) {
                common.showErrorMessage(Messages.error.selectDateOrDelegation);
                common.hideLoadingScreen();
            } else {
                vm.dateSelected = vm.yearSelect + "/" + vm.monthSelect + "/" + 1;
                vm.dateDelegation.Date = vm.dateSelected;
                vm.dateDelegation.DelegationId = vm.DelegationSelect;
                var promise = getCalendarMonth();
                promise.success(function () {
                    common.hideLoadingScreen();
                }).error(function () {
                    common.hideLoadingScreen();
                });
            }


        }


        function initDates() {
            $('#appoiments').removeClass("hidden");
            $('#appoiments').addClass("active");

            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hideen");
            $('#infoGeneral').addClass("hidden");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");

            completeControllerInit();
            common.overrideNavigationMenu(false);
            loadData();

        }

        function initCombos() {
            common.displayLoadingScreen();
            var promise = getMonths();
            promise.success(function () {
                var promise2 = getYears();
                promise2.success(function () {
                    common.hideLoadingScreen();
                }).error(function () {
                    common.hideLoadingScreen();
                });
            }).error(function () {
                common.hideLoadingScreen();
            });
        }

        function initCalendar() {
            $('#home').addClass("active");
            $('#appoiments').removeClass("hidden");
            $('#appoiments').addClass("inactive");
            $('#scheduleAppointment').removeClass("hidden");
            $('#scheduleAppointment').addClass("active");

            $('#request').addClass("hidden");
            $('#infoGeneral').addClass("hidden");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hidden");
            //$('#').removeClass("hidden");

            vm.delegations = {};
            vm.requestAll = {};
            vm.dayCalendar = {};
            vm.dateSelected = {};
            vm.dateDelegation = {};
            vm.timeselection = {};
            vm.app = {};
            vm.reqId = $routeParams["requestId"];
            var datesample = new Date();
            vm.month = datesample.getMonth() + 1;
            vm.year = datesample.getFullYear();
            vm.monthSelect = datesample.getMonth() + 1;
            vm.yearSelect = datesample.getFullYear();
            vm.dateSelected = vm.year + "/" + vm.month + "/" + 1;
            //var promisesarray = [];
            //promisesarray.push(getDelegations());
            //promisesarray.push(getDelegation(vm.entitle.DelegationId));
            //common.$q.all(promisesarray).then(function () {
            //    completeControllerInit();
            //    common.overrideNavigationMenu(false);
            //    vm.DelegationSelect = vm.delegation.DelegationId;
            //    initCombos();
            //    getCalendar();
            //});


            var promise = getDelegations();
            promise.success(function () {
                var promisent = getEntitleInformation();
                promisent.success(function () {
                    var promisedel = getDelegation(vm.entitle.DelegationId);
                    promisedel.success(function () {
                        vm.DelegationSelect = vm.delegation.DelegationId;
                        var promise = getMonths();
                        promise.success(function () {
                            var promise2 = getYears();
                            promise2.success(function () {
                                common.hideLoadingScreen();
                                vm.times = {};
                                vm.calendar = {};
                                vm.dateDelegation = {};
                                vm.dayCalendar = {};
                                vm.dateSelected = {};
                                vm.timeselection = null;
                                common.displayLoadingScreen();
                                if (vm.dateSelected == null || vm.DelegationSelect == null) {
                                    common.showErrorMessage(Messages.error.selectDateOrDelegation);
                                    common.hideLoadingScreen();
                                } else {
                                    vm.dateSelected = vm.yearSelect + "/" + vm.monthSelect + "/" + 1;
                                    vm.dateDelegation.Date = vm.dateSelected;
                                    vm.dateDelegation.DelegationId = vm.DelegationSelect;
                                    var promise = getCalendarMonth();
                                    promise.success(function () {
                                        common.hideLoadingScreen();
                                    }).error(function () {
                                        common.hideLoadingScreen();
                                    });
                                }
                            }).error(function () {
                                common.hideLoadingScreen();
                            });
                        }).error(function () {
                            common.hideLoadingScreen();
                        });
                    }).error(function () {
                        //completeControllerInit();
                        //common.overrideNavigationMenu(false);
                    });
                }).error(function () {
                    //completeControllerInit();
                    //common.overrideNavigationMenu(false);

                });
            }
            ).error(
                function () {
                    //completeControllerInit();
                    //common.overrideNavigationMenu(false);

                });
        }

        function selectDelegation() {

            common.displayLoadingScreen();
            var promise = getDelegation(vm.DelegationSelect);
            promise.success(function () {
                common.hideLoadingScreen();
                getCalendar();
            })
                .error(function () {
                    common.hideLoadingScreen();
                });
        };

        function getTimes(day) {
            if (day.Availability == 2 || day.Availability == 1) {
                vm.times = {};
                vm.dateDelegation = {};
                vm.timeselection = null;
                common.displayLoadingScreen();
                vm.dayCalendar = day;
                vm.dateDelegation.Date = vm.dayCalendar.Date;
                vm.dateDelegation.DelegationId = vm.DelegationSelect;
                var promise = getTimesCalendar();
                promise.success(function () {
                    common.hideLoadingScreen();
                }).error(function () {
                    common.hideLoadingScreen();
                });
            }

        }

        function selectTime(time) {
            common.displayLoadingScreen();
            vm.timeselection = time;
            common.hideLoadingScreen();

        }

        function saveAppointment(navigate) {
            common.displayLoadingScreen();
            var app = {};
            app.RequestId = vm.reqId;
            app.Delegationid = vm.DelegationSelect;
            app.Time = vm.timeselection.Time;
            app.Date = vm.dateDelegation.Date;
            app.IsAttended = false;
            app.DaIsCancelledte = false;
            var promise = saveDate(app);
            promise.success(function () {
                navigate(vm.reqId);
                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();

            });


        }

        function loadData() {
            common.displayLoadingScreen();
            vm.dates = {};
            vm.opinionRequest = {};
            common.config.requestInf = {};
            var requestId = $routeParams["requestId"];
            var promiseDates = getDates(requestId);
            promiseDates.success(function () {
                var promiseEntitle = getEntitleInformation();
                promiseEntitle.success(function () {
                    var promiseOpinion = getOpinionRequest(requestId, vm.entitle.State);
                    promiseOpinion.success(function () {
                        common.hideLoadingScreen();
                    }).error(function () {
                        common.hideLoadingScreen();
                    });
                });

            }).error(function () {
                common.hideLoadingScreen();
            });
        }

        function aproveDiagnostic() {


            common.displayLoadingScreen();
            var promise = saveStatusRequest(vm.opinionRequest.Request.RequestId, 140, true);
            promise.success(function () {
                loadData();
                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();
            });
        }

        function rejectDiagnostic(navigate) {
            common.displayLoadingScreen();
            var promise = saveStatusRequest(vm.opinionRequest.Request.RequestId, 131, true);
            promise.success(function () {
                navigate();
                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();
            });
        }

        function cancelApp(appId) {
            common.displayLoadingScreen();
            var promise = cancelAppointment(appId);
            promise.success(function () {
                var promisec = saveStatusRequest(vm.opinionRequest.Request.RequestId, 140, true);
                promisec.success(function () {
                    loadData();
                }).error(function () {
                    common.hideLoadingScreen();
                });


            }).error(function () {
                common.hideLoadingScreen();
            });

        }

        //#endregion

        //#region Helper Functions

        function getDates(requestId) {
            common.displayLoadingScreen();
            return dateDataService.getDates(requestId, common.config.entitleInformation.CURP)
                .success(function (data, status, headers, config) {
                    vm.dates = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                });
        }

        function getOpinionRequest(requestId, State) {
            common.displayLoadingScreen();
            return requestDataService.getOpinionRequest(requestId, common.config.entitleInformation.CURP, State)
                .success(function (data, status, headers, config) {
                    vm.opinionRequest = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                });
        }


        function saveStatusRequest(requestId, idstatus, happy) {
            return requestDataService.saveStatusRequest(requestId, idstatus, happy, common.config.entitleInformation.CURP)
                .success(function (data, status, headers, config) {
                    //vm.opinionRequest = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function cancelAppointment(appointmentId) {
            return dateDataService.cancelAppointment(appointmentId, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    //vm.opinionRequest = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function getDelegations() {
            return commonDataService.getDelegations(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.delegations = data;
                }).error(function () {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function getDelegation(delegationId) {
            return commonDataService.getDelegation(delegationId, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.delegation = data;

                }).error(function () {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function getEntitleInformation() {
            common.displayLoadingScreen();
            return homeDataService.getEntitleInformation(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {

                    vm.entitle = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.getEntitleInformation);
                    common.hideLoadingScreen();
                });
        }

        function getCalendarMonth() {
            return dateDataService.calendarByMonthAndDelegation(vm.dateDelegation, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.calendar = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function getTimesCalendar() {
            return dateDataService.getTimesCalendar(vm.dateDelegation, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.times = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function getMonths() {
            return dateDataService.getMonths(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.months = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }

        function getYears() {
            return dateDataService.getYears(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.years = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                });
        }


        function saveDate(appoinment) {
            return dateDataService.saveDate(appoinment, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.app = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.saveDate);
                });
        }


        function completeControllerInit() {
            common.logger.log(Messages.info.contollerLoaded, null, controllerId);
            common.overrideNavigationMenu(true);
            common.activateController([], controllerId);
        }


        function completeControllerSuccess() {
            completeControllerInit();
            common.overrideNavigationMenu(false);
        }

        function completeControllerFail() {
            completeControllerInit();
            common.overrideNavigationMenu(true);
        }

        //#endregion
    };
})();