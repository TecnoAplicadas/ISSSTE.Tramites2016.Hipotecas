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
        vm.FromDate = ((date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) + "/" + ((date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "/" + date.getFullYear());
        vm.opinionRequest = {};
        vm.selectDay = -1;
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

        vm.appointmentCreated = false;
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
            common.displayLoadingScreen();
            vm.times = {};
            vm.calendar = {};
            vm.dateDelegation = {};
            vm.dayCalendar = {};
            vm.dateSelected = {};
            vm.timeselection = null;
            //vm.DelegationSelect = " ";

            if (vm.dateSelected == null || vm.DelegationSelect == null) {
                common.showErrorMessage(Messages.error.selectDateOrDelegation);
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
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
            common.displayLoadingScreen();
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
                    //var promisedel = getDelegation(vm.entitle.DelegationId);
                    //promisedel.success(function () {
                    //  vm.DelegationSelect = vm.delegation.IdLegalUnit;
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
                            //if (vm.dateSelected == null || vm.DelegationSelect == null) {
                            //    common.showErrorMessage(Messages.error.selectDateOrDelegation);
                            //    common.hideLoadingScreen();
                            //} else {
                            vm.dateSelected = vm.yearSelect + "/" + vm.monthSelect + "/" + 1;
                            vm.dateDelegation.Date = vm.dateSelected;
                            vm.dateDelegation.DelegationId = vm.delegations[0].IdDelegation;
                            var promise = getCalendarMonth();
                            promise.success(function () {
                                common.hideLoadingScreen();
                            }).error(function () {
                                common.hideLoadingScreen();
                            });
                            //  }
                        }).error(function () {
                            common.hideLoadingScreen();
                        });
                    }).error(function () {
                        common.hideLoadingScreen();
                    });
                    //}).error(function () {
                    //    //completeControllerInit();
                    //    //common.overrideNavigationMenu(false);
                    //});
                }).error(function () {
                    //completeControllerInit();
                    //common.overrideNavigationMenu(false);
                    common.hideLoadingScreen();
                });
            }
            ).error(
                function () {
                    //completeControllerInit();
                    //common.overrideNavigationMenu(false);
                    common.hideLoadingScreen();
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

            if (day.Availability < 3) {
                vm.selectDay = day.Day;
                common.displayLoadingScreen();
                if (day.Availability == 2 || day.Availability == 1) {
                    vm.times = {};
                    vm.dateDelegation = {};
                    vm.timeselection = null;
                    vm.dayCalendar = day;
                    vm.dateDelegation.Date = vm.dayCalendar.Date;
                    vm.dateDelegation.DelegationId = vm.DelegationSelect;
                    var promise = getTimesCalendar();
                    promise.success(function () {
                        common.hideLoadingScreen();
                    }).error(function () {
                        common.hideLoadingScreen();
                    });
                } else {
                    common.hideLoadingScreen();
                }
            } else {
                common.hideLoadingScreen();
            }


        }

        function selectTime(time) {

            vm.timeselection = time;
            common.hideLoadingScreen();

        }

        function saveAppointment(RequestId) {
            common.displayLoadingScreen();
            var app = {};
            //vm.reqId = window.location.search.substring(window.location.search.indexOf("RequestId"), window.location.length).replace("RequestId=", "");
            vm.reqId = RequestId;
            app.RequestId = vm.reqId;
            app.Delegationid = vm.DelegationSelect;
            app.Time = vm.timeselection.Time;
            app.Date = vm.dateDelegation.Date;
            app.IsAttended = false;
            app.DaIsCancelledte = false;
            var promise = saveDate(app);
            promise.success(function () {
                //navigate(vm.reqId);
                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();
            });

            //common.hideLoadingScreen();


        }

        function loadData() {
            common.displayLoadingScreen();
            vm.dates = {};
            vm.opinionRequest = {};
            common.config.requestInf = {};
            var requestId = vm.reqId = window.location.search.substring(window.location.search.indexOf("RequestId"), window.location.length).replace("RequestId=", "");
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
            common.hideLoadingScreen();
        }

        function aproveDiagnostic() {
            common.displayLoadingScreen();
            var promise = saveStatusRequest(vm.opinionRequest.Request.RequestId, 203, true);
            promise.success(function () {
                loadData();
                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();
            });
            common.hideLoadingScreen();
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
            common.hideLoadingScreen();
        }

        function cancelApp(appId) {
            common.displayLoadingScreen();
            var promise = cancelAppointment(appId);
            promise.success(function () {
                var promisec = saveStatusRequest(vm.opinionRequest.Request.RequestId, 203, true);
                promisec.success(function () {
                    loadData();
                    common.hideLoadingScreen();
                }).error(function () {
                    common.hideLoadingScreen();
                });

                common.hideLoadingScreen();
            }).error(function () {
                common.hideLoadingScreen();
            });

        }

        //#endregion

        //#region Helper Functions

        function getDates(requestId) {
            common.displayLoadingScreen();
            return dateDataService.getDates(requestId, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.dates = data;
                    common.hideLoadingScreen();
                    //  navigate(requestId);
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getOpinionRequest(requestId, State) {
            common.displayLoadingScreen();
            return requestDataService.getOpinionRequest(requestId, common.config.entitleInformation.NoISSSTE, State)
                .success(function (data, status, headers, config) {
                    vm.opinionRequest = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }


        function saveStatusRequest(requestId, idstatus, happy) {
            common.displayLoadingScreen();
            return requestDataService.saveStatusRequest(requestId, idstatus, happy, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    //vm.opinionRequest = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function cancelAppointment(appointmentId) {
            common.displayLoadingScreen();
            return dateDataService.cancelAppointment(appointmentId, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    //vm.opinionRequest = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getDelegations() {
            common.displayLoadingScreen();
            return commonDataService.getDelegations(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.delegations = data;
                    common.hideLoadingScreen();
                }).error(function () {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getDelegation(delegationId) {
            common.displayLoadingScreen();
            return commonDataService.getDelegation(delegationId, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.delegation = data;
                    common.hideLoadingScreen();
                }).error(function () {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
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
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getCalendarMonth() {
            common.displayLoadingScreen();
            return dateDataService.calendarByMonthAndDelegation(vm.dateDelegation, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.calendar = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getTimesCalendar() {
            common.displayLoadingScreen();
            return dateDataService.getTimesCalendar(vm.dateDelegation, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.times = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getMonths() {
            common.displayLoadingScreen();
            return dateDataService.getMonths(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.months = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getYears() {
            common.displayLoadingScreen();
            return dateDataService.getYears(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.years = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }


        function saveDate(appoinment) {
            common.displayLoadingScreen();
            return dateDataService.saveDate(appoinment, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.app = data;
                    common.showSuccessMessage(Messages.success.appointmentCreated);
                    vm.appointmentCreated = true;
                    window.location.href = common.getBaseUrl() + "Entitle/AgendarCita?NoIssste=" + common.config.entitleInformation.NoISSSTE + "&RequestId=" + appoinment.RequestId;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.saveDate);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
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