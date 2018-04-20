(function () {
    'use strict';

    var controllerId = 'calendarController';
    angular
        .module(appName)
        .controller(controllerId, ['common', 'calendarDataService', 'webApiService', calendarController]);

    function calendarController(common, calendarDataService, webApiService) {
        var vm = this;

        vm.delegations = [];

        vm.schedules = [];
        vm.deleteSchedules = [];

        vm.specialDays = [];
        vm.deleteSpecialDays = [];

        vm.specialSchedues = [];
        vm.deleteSpecialSchedues = [];

        vm.weekDays = [];
        vm.delegationId = -1;
        vm.capacityPerSchedule = 0;

        vm.Schedule = {};
        vm.SpecialDay = {};
        vm.SpecialDaySchedule = {};
        vm.Quota = {};
        vm.specialDayScheduleEdit = {};

        vm.init = init;
        vm.saveNewSchedule = saveNewSchedule;
        vm.saveNewSpecialDay = saveNewSpecialDay;
        vm.saveNewSpecialSchedule = saveNewSpecialSchedule;
        vm.deleteSchedule = deleteSchedule;
        vm.deleteSpecialDay = deleteSpecialDay;
        vm.deleteSpecialSchedule = deleteSpecialSchedule;
        vm.delegationChange = delegationChange;
        vm.getWeekDayDescription = getWeekDayDescription;
        vm.weekdayChange = weekdayChange;
        vm.selectSpecialScheduleDetail = selectSpecialScheduleDetail;
        vm.saveNewQuota = saveNewQuota;
        vm.deleteQuota = deleteQuota;
        vm.formValid = formValid;
        vm.validateAppointments = validateAppointments;
        vm.saveCalendar = saveCalendar;
        vm.validateSpecialDay = validateSpecialDay;
        vm.validateSpecialDaySchedule = validateSpecialDaySchedule;

        function init() {
            debugger;
            $('#appoiments').removeClass("hidden");
            $('#appoiments').addClass("inactive");
            $('#scheduleAppointment').removeClass("hidden");
            $('#scheduleAppointment').addClass("active");

            $('#request').removeClass("hidden");
            $('#request').addClass("inactive");
            $('#infoGeneral').removeClass("hidden");
            $('#infoGeneral').addClass("active");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hidden");

            initCalendar();

            getDelegations()
                .finally(function () {
                    common.logger.log(Messages.info.controllerLoaded, null, controllerId);
                    common.activateController([], controllerId);
                });
        }

        function initSchudele() {
            var today = new Date();
            vm.weekDays = Constants.weekDays;

            var stringHour = today.getHours() < 10 ? "0" + String(today.getHours()) : String(today.getHours());
            var stringMinutes = today.getMinutes() < 10 ? "0" + String(today.getMinutes()) : String(today.getMinutes());
            var stringSeconds = today.getSeconds() < 10 ? "0" + String(today.getSeconds()) : String(today.getSeconds());
            var currentTime = stringHour + ":" + stringMinutes + ":" + stringSeconds;

            var stringDay = today.getDate() < 10 ? "0" + String(today.getDate()) : String(today.getDate());
            var stringMonth = today.getMonth() + 1 < 10 ? "0" + String(today.getMonth() + 1) : String(today.getMonth() + 1);
            var stringYear = today.getFullYear() < 10 ? "0" + String(today.getFullYear()) : String(today.getFullYear());
            var currentDate = stringDay + "/" + stringMonth + "/" + stringYear;

            vm.Schedule = {
                ScheduleId: '',
                DelegationId: 0,
                WeekdayId: 2,
                Weekday: '',
                Time: currentTime,
                Capacity: 1,
                IsNew: false
            };

            vm.SpecialDay = {
                DelegationId: 0,
                Date: currentDate,
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false
            };

            vm.SpecialDaySchedule = {
                DelegationId: 0,
                Date: currentDate,
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false,
                Quota: [],
                DeleteQuota: []
            };

            vm.Quota = {
                Id: '',
                Time: currentTime,
                Capacity: 0,
                IsNew: false
            }
        }

        function initCalendar() {

            initSchudele();

            vm.schedules = [];
            vm.deleteSchedules = [];

            vm.specialDays = [];
            vm.deleteSpecialDays = [];

            vm.specialSchedues = [];
            vm.deleteSpecialSchedues = [];

            vm.specialDayScheduleEdit = {};
        }

        function validateSpecialDay() {
            vm.SpecialDay.Date = vm.SpecialDay.Date.substring(0, vm.SpecialDay.Date.length - 1);

        }

        function validateSpecialDaySchedule() {
            vm.SpecialDaySchedule.Date = vm.SpecialDaySchedule.Date.substring(0, vm.SpecialDaySchedule.Date.length - 1);

        }

        function validDate() {
            var myString = " 1234567890/";
            var valid = false;
            for (var i = 0; i < myString.length; i++) {
                if (e.toLowerCase() == myString[i]) {
                    valid = true;
                    break;
                }
            }
            return valid;
        }

        function getExistingData() {
            if (vm.delegationId != null) {
                return webApiService.makeRetryRequest(1, function () {
                    common.displayLoadingScreen();

                    return calendarDataService.getExistingData(vm.delegationId);
                }).then(function (data) {
                    data.Schedules.forEach(function (schedule, index) {
                        vm.schedules.push({
                            ScheduleId: schedule.ScheduleId,
                            DelegationId: schedule.DelegationId,
                            WeekdayId: schedule.WeekdayId,
                            Weekday: getWeekDayDescription(schedule.WeekdayId),
                            Time: getTimeFromStringEditor(schedule.Time),
                            Capacity: schedule.Capacity,
                            IsNew: false
                        });
                    });

                    data.NonLaborableDays.forEach(function (nonLabourDay, index) {
                        if (nonLabourDay.IsNonWorking) {
                            vm.specialDays.push({
                                DelegationId: nonLabourDay.DelegationId,
                                Date: nonLabourDay.Date,
                                IsNonWorking: nonLabourDay.IsNonWorking,
                                IsOverrided: nonLabourDay.IsOverrided,
                                IsNew: false
                            });
                        }
                    });

                    data.SpecialSchedules.forEach(function (specialSchedule, index) {
                        var overridenDay = {
                            DelegationId: specialSchedule.SpecialDay.DelegationId,
                            Date: specialSchedule.SpecialDay.Date,
                            IsNonWorking: specialSchedule.SpecialDay.IsNonWorking,
                            IsOverrided: specialSchedule.SpecialDay.IsOverrided,
                            IsNew: false,
                            Quota: [],
                            NewQuota: [],
                            DeleteQuota: []
                        };

                        specialSchedule.Schedules.forEach(function (currentSchedule, scheduleIndex) {
                            var overridenDayQuota = {
                                Id: currentSchedule.SpecialDayScheduleId,
                                Time: currentSchedule.Time,
                                Capacity: currentSchedule.Capacity,
                                IsNew: false
                            }
                            overridenDay.Quota.push(overridenDayQuota);
                        });

                        vm.specialSchedues.push(overridenDay);
                    });
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }).catch(function (reason) {
                    $('.alerts').empty();
                     common.showErrorMessage('', Messages.error.errorGetCalendarData);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
            }

        }

        function getDelegations() {
            return webApiService.makeRetryRequest(1, function () {
                common.displayLoadingScreen();
                return calendarDataService.getDelegations();
            }).then(function (data) {
                vm.delegations = data;
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
            }).catch(function (reason) {
                $('.alerts').empty();
                 common.showErrorMessage('', Messages.error.errorGetDelegations);
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
            });
        }

        function delegationChange() {
            vm.specialDays = [];
            vm.specialSchedues = [];
            vm.schedules = [];
            getExistingData();
        }

        function saveNewSchedule() {
            var error = validateNewSchedule();
            if (error != "") {
                $('.alerts').empty();
                common.showErrorMessage(error);
                $("html, body").animate({ scrollTop: 700 }, 2000);
                return;
            }

            vm.schedules.push({
                ScheduleId: '',
                DelegationId: vm.delegationId,
                WeekdayId: vm.Schedule.WeekdayId,
                Weekday: getWeekDayDescription(vm.Schedule.WeekdayId),
                Time: getTimeFromStringEditor(vm.Schedule.Time),
                Capacity: vm.Schedule.Capacity,
                IsNew: true
            });

            initSchudele();
        }

        function saveNewSpecialDay() {
            var error = validateNewSpecialDay();
            if (error != "") {
                $('.alerts').empty();
                common.showErrorMessage(error);
                $("html, body").animate({ scrollTop: 700 }, 2000);
                return;
            }

            vm.specialDays.push({
                DelegationId: vm.delegationId,
                Date: vm.SpecialDay.Date,
                IsNonWorking: true,
                IsOverrided: false,
                IsNew: true
            });

            vm.SpecialDay = {
                DelegationId: 0,
                Date: new Date(),
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false
            };
        }

        function saveNewSpecialSchedule() {
            var error = validateNewSpecialSchedule();
            if (error != "") {
                $('.alerts').empty();
                common.showErrorMessage(error);
                $("html, body").animate({ scrollTop: 700 }, 2000);
                return;
            }

            vm.specialSchedues.push({
                DelegationId: vm.delegationId,
                Date: vm.SpecialDaySchedule.Date,
                IsNonWorking: false,
                IsOverrided: true,
                IsNew: true,
                Quota: [],
                DeleteQuota: []
            });

            vm.SpecialDaySchedule = {
                DelegationId: 0,
                Date: new Date(),
                IsNonWorking: false,
                IsOverrided: false,
                IsNew: false,
                Quota: [],
                DeleteQuota: []
            };
        }

        function saveNewQuota() {
            var error = validateNewQuota();
            if (error != "") {
                $('.alerts').empty();
                common.showErrorMessage(error);
                $("html, body").animate({ scrollTop: 700 }, 2000);
                return;
            }

            vm.Quota.IsNew = true;
            vm.Quota.Time = getTimeFromStringEditor(vm.Quota.Time);
            vm.specialDayScheduleEdit.Quota.push(vm.Quota);

            vm.Quota = {
                Time: '',
                Capacity: 0,
                IsNew: false
            }
        }

        function deleteSchedule(schedule) {
            if (!schedule.IsNew) {
                vm.deleteSchedules.push(schedule);
            }

            var index = vm.schedules.indexOf(schedule);
            if (index > -1) {
                vm.schedules.splice(index, 1);
            }
        }

        function deleteSpecialDay(specialDay) {
            if (!specialDay.IsNew) {
                vm.deleteSpecialDays.push(specialDay);
            }

            var index = vm.specialDays.indexOf(specialDay);
            if (index > -1) {
                vm.specialDays.splice(index, 1);
            }
        }

        function deleteSpecialSchedule(specialSchedule) {
            if (!specialSchedule.IsNew) {
                vm.deleteSpecialSchedues.push(specialSchedule);
            }

            var index = vm.specialSchedues.indexOf(specialSchedule);
            if (index > -1) {
                vm.specialSchedues.splice(index, 1);
            }
        }

        function deleteQuota(quota) {
            if (!quota.IsNew) {
                vm.specialDayScheduleEdit.DeleteQuota.push(quota);
                for (var x = 0; x <= vm.specialDayScheduleEdit.Quota.length - 1; x++) {
                    if (vm.specialDayScheduleEdit.Quota[x].Id == quota.Id) {
                        vm.specialDayScheduleEdit.Quota.splice(x, 1);
                        calendarDataService.getDeleteSpecialDaysSchedules(quota.Id)
                        .success(function (data, status, headers, config) {
                            common.showSuccessMessage(Messages.success.deleteSpecialDaysSchedules);
                            $("html, body").animate({ scrollTop: 700 }, 2000);
                        })
                        .error(function (data, status, headers, config) {
                            $('.alerts').empty();
                            common.showErrorMessage(Messages.error.errorDeleteSpecialDaysSchedules);
                            $("html, body").animate({ scrollTop: 700 }, 2000);
                        });
                        break;
                    }
                }
            }

            var index = vm.specialDayScheduleEdit.Quota.indexOf(quota);
            if (index > -1) {
                vm.specialDayScheduleEdit.Quota.splice(index, 1);
            }
        }

        function validateNewSchedule() {
            var error = "";
            if (vm.Schedule.Capacity == undefined) {
                error = Messages.error.errorCalendarSchedule;
            }
            if (vm.Schedule.Capacity < 1 || vm.Schedule.WeekdayId < 1) {
                error = Messages.error.errorCalendarSchedule;
            }

            vm.Schedule.Time = getTimeFromStringEditor(vm.Schedule.Time);

            if (error == "") {
                vm.schedules.forEach(function (currentSchedule, index) {
                    if (currentSchedule.WeekdayId == vm.Schedule.WeekdayId && currentSchedule.Time == vm.Schedule.Time) {
                        error = Messages.error.errorCalendarSchedule;
                        return false;
                    }
                });
            }

            return error;
        }

        function validateNewSpecialDay() {
            var error = "";
            var today = new Date();
            var selectedStringDate = getDateFromStringEditor(vm.SpecialDay.Date);

            if (selectedStringDate < today) {
                error = Messages.error.errorCalendarSchedule;
            }

            if (error == "") {
                vm.specialDays.forEach(function (currentSpecialDay, index) {
                    if (vm.SpecialDay.Date == currentSpecialDay.Date) {
                        error = Messages.error.errorCalendarSchedule;
                        return false;
                    }
                });
            }

            return error;
        }

        function validateNewSpecialSchedule() {
            var error = "";
            var today = new Date();
            var selectedStringDate = getDateFromStringEditor(vm.SpecialDaySchedule.Date);
            if (selectedStringDate < today) {
                error = Messages.error.errorCalendarSchedule;
            }

            if (error == "") {
                vm.specialSchedues.forEach(function (currentspecialSchedule, index) {
                    if (vm.SpecialDaySchedule.Date == currentspecialSchedule.Date) {
                        error = Messages.error.errorCalendarSchedule;
                        return false;
                    }
                });
            }

            return error;
        }

        function validateNewQuota() {
            var error = "";

            if (vm.specialDayScheduleEdit.Quota == null) {
                error = "Seleccione una fecha";
            }

            if (error == "" && vm.Quota.Capacity < 1) {
                error = Messages.error.errorCalendarSchedule;
            }

            vm.Quota.Time = getTimeFromStringEditor(vm.Quota.Time);
            if (error == "") {
                vm.specialDayScheduleEdit.Quota.forEach(function (currentQuota, index) {
                    if (vm.Quota.Time == currentQuota.Time) {
                        error = Messages.error.errorCalendarSchedule;
                        return false;
                    }
                });
            }

            return error;
        }
        function saveCalendar(delegationId) {
            common.displayLoadingScreen();

            //var promises = [];            
            //vm.deleteSchedules.forEach(function (actualSchedule, index) {
            //    promises.push(calendarDataService.deleteSchedule(actualSchedule.ScheduleId));
            //});

            //promises.push(calendarDataService.saveSchedules(vm.newSchedules));

            //vm.deleteSpecialDays.forEach(function (nonLabourDay, index) {
            //    promises.push(calendarDataService.deleteSpecialDay({
            //        Id: nonLabourDay.DelegationId,
            //        Date: nonLabourDay.Date
            //    }));
            //});

            //promises.push(calendarDataService.saveSpecialDays(vm.newSpecialDays));

            //vm.deleteSpecialSchedues.forEach(function (specialSchedule, index) {
            //    if (specialSchedule.Quota.length > 0) {
            //        specialSchedule.Quota.forEach(function (quota, indexQuota) {
            //            if (quota.Id != '') {
            //                promises.push(calendarDataService.deleteSpecialScheduleDays(quota.Id));
            //            }                        
            //        });                    
            //    }

            //    promises.push(calendarDataService.deleteSpecialDay({
            //        Id: specialSchedule.DelegationId,
            //        Date: specialSchedule.Date
            //    }));                
            //});

            //promises.push(calendarDataService.saveSpecialDays(vm.newSpecialSchedues));
            //vm.newSpecialSchedues.forEach(function (newSpecialSchedue, index) {
            //    if (newSpecialSchedue.NewQuota.length > 0) {
            //        promises.push(calendarDataService.saveSpecialScheduleDays(newSpecialSchedue.NewQuota));
            //    }
            //});            

            //common.$q.all(promises)
            //    .then(function () {
            //        common.hideLoadingScreen();
            //    }).catch(function (reason) {
            //         common.showErrorMessage('', Messages.error.errorSaveCalendar);
            //        common.hideLoadingScreen();
            //    });

            saveSchedules().then(function () {
                saveNonLabourDays().then(function () {
                    saveSpecialDays().then(function () {
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                        delegationChange();
                    }, function (reason) {
                        $('.alerts').empty();
                        common.showErrorMessage(Messages.error.errorCalendarDaysNoWorking);
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    });
                }, function (reason) {
                    $('.alerts').empty();
                    common.showErrorMessage(Messages.error.errorCalendarDaysSpecial);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
            }, function (reason) {
                $('.alerts').empty();
                 common.showErrorMessage('', Messages.error.errorSaveCalendar);
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
            });
        }
        function getWeekDayDescription(ordinal) {
            var descrption = '';
            switch (ordinal) {
                case "1":
                case 1:
                    descrption = Constants.weekDays.Domingo.Name;
                    break;
                case "2":
                case 2:
                    descrption = Constants.weekDays.Lunes.Name;
                    break;
                case "3":
                case 3:
                    descrption = Constants.weekDays.Martes.Name;
                    break;
                case "4":
                case 4:
                    descrption = Constants.weekDays.Miercoles.Name;
                    break;
                case "5":
                case 5:
                    descrption = Constants.weekDays.Jueves.Name;
                    break;
                case "6":
                case 6:
                    descrption = Constants.weekDays.Viernes.Name;
                    break;
                case "7":
                case 7:
                    descrption = Constants.weekDays.Sabado.Name;
                    break;
            }
            return descrption;
        }

        function weekdayChange() {
            vm.Schedule.WeeydayDescription = getWeekDayDescription(vm.Schedule.WeekdayId);
        }

        function selectSpecialScheduleDetail(specialDay) {
            vm.specialDayScheduleEdit = specialDay;
        }

        function formValid(form) {
            return form.$valid;
        }

        function getDateFromStringEditor(editorDate) {
            //editorDate DD/MM/YYYY
            var dateParts = editorDate.split("/");
            var stringDate = dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2]
            var milliseconds = Date.parse(stringDate);
            var newDate = new Date(milliseconds);
            return newDate;
        }

        function getTimeFromStringEditor(time) {
            var timeParts = time.split(":");
            var hour = Number(timeParts[0]);
            var minutes = Number(timeParts[1].substring(0, 2));
            var amPmParts = timeParts[1].split(" ");
            if (amPmParts[1] == "PM") {
                hour += 12;
                if (hour == 24)
                    hour = 12;

            }
            var stringHour = hour < 10 ? "0" + String(hour) : String(hour);
            var stringMinutes = minutes < 10 ? "0" + String(minutes) : String(minutes);

            var newDate = stringHour + ":" + stringMinutes;
            return newDate;
        }

        function validateAppointments() {
            calendarDataService.getAppointmentsByDelegation(vm.delegationId)
                .success(function (data, status, headers, config) {
                    schedules

                })
                .error(function (data, status, headers, config) {
                    $('.alerts').empty();
                    common.showErrorMessage(data, Messages.error.getDelegations);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }).finally(function () {
                    common.hideLoadingScreen();

                });
        }

        function saveSchedules() {
            var promises = [];
            vm.deleteSchedules.forEach(function (actualSchedule, index) {
                promises.push(calendarDataService.deleteSchedule(actualSchedule.ScheduleId));
            });

            var newSchedules = [];
            vm.schedules.forEach(function (currentSchedule, index) {
                if (currentSchedule.IsNew) {
                    //newSchedule.Time = getTimeFromStringEditor(newSchedule.Time);
                    newSchedules.push(currentSchedule);
                }
            });

            promises.push(calendarDataService.saveSchedules(newSchedules));
            return common.$q.all(promises);
        }

        function saveNonLabourDays() {
            var promises = [];
            vm.deleteSpecialDays.forEach(function (nonLabourDay, index) {
                promises.push(calendarDataService.deleteSpecialDay({
                    Id: nonLabourDay.DelegationId,
                    Date: nonLabourDay.Date
                })
                .success(function (data, status, headers, config) {
                    common.showSuccessMessage(Messages.success.catalogItemUpdated);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .error(function (data, status, headers, config) {
                    $('.alerts').empty();
                    common.showErrorMessage(data);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }));
            });

            var newSpecialDays = [];
            vm.specialDays.forEach(function (currentSpecialDay, index) {
                if (currentSpecialDay.IsNew) {
                    newSpecialDays.push(currentSpecialDay);
                }
            });

            for (var x = 0; x <= newSpecialDays.length - 1; x++) {
                for (var y = 0; y <= vm.specialSchedues.length - 1; y++) {
                    if (newSpecialDays[x].Date == vm.specialSchedues[y].Date) {
                        $('.alerts').empty();
                        common.showErrorMessage(Messages.error.errorCalendarDaysSpecial);

                        newSpecialDays.splice(x, 1);
                        for (var x = 0; x <= newSpecialDays.length - 1; x++) {
                            for (var y = 0; y <= vm.specialSchedues.length - 1; y++) {
                                if (newSpecialDays[x].Date == vm.specialSchedues[y].Date) {
                                    $('.alerts').empty();
                                    common.showErrorMessage(Messages.error.errorCalendarDaysSpecial);
                                    newSpecialDays.splice(x, 1);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }

            if (newSpecialDays.length > 0) {
                promises.push(calendarDataService.saveSpecialDays(newSpecialDays)
                .success(function (data) {
                    common.showSuccessMessage(Messages.success.catalogItemUpdated);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .error(function (data) {
                    var count = vm.specialDays.length - 1;
                    for (var x = 0; x <= count; x++) {
                        if (vm.specialDays[x].Date == newSpecialDays[0].Date) {
                            vm.specialDays.splice(x, 1);
                        }
                    }

                }));
            }
            return common.$q.all(promises);
        }

        function saveSpecialDays() {
            var promises = [];
            var deleteSpecialScheduesAndHour = [];

            vm.deleteSpecialSchedues.forEach(function (specialSchedule, index) {
                if (specialSchedule.Quota.length > 0) {
                    specialSchedule.Quota.forEach(function (quota, indexQuota) {
                        if (quota.Id != '') {
                            promises.push(calendarDataService.deleteSpecialScheduleDays(quota.Id)
                                .success(function (data, status, headers, config) {
                                })
                                .error(function (data, status, headers, config) {
                                    $('.alerts').empty();
                                    common.showErrorMessage(data);
                                    $("html, body").animate({ scrollTop: 700 }, 2000);
                                }));
                            deleteSpecialScheduesAndHour.push(quota);
                        }
                    });
                }

                promises.push(calendarDataService.deleteSpecialDay({
                    Id: specialSchedule.DelegationId,
                    Date: specialSchedule.Date
                }));
            });

            var newSpecialSchedues = [];
            var quotas = [];
            var count = vm.Quota.length;
            vm.specialSchedues.forEach(function (currentSpecialSchedule, index) {
                if (currentSpecialSchedule.IsNew) {
                    newSpecialSchedues.push(currentSpecialSchedule);
                } else {
                    currentSpecialSchedule.Quota.forEach(function (currentQuota, quotaIndex) {
                        if (currentQuota.IsNew) {
                            quotas.push({
                                SpecialDayScheduleId: '',
                                DelegationId: vm.delegationId,
                                Date: currentSpecialSchedule.Date,
                                Time: getTimeFromStringEditor(currentQuota.Time),
                                Capacity: currentQuota.Capacity
                            });
                        }
                    });
                }
            });

            if (quotas.length > 0)
                promises.push(calendarDataService.saveSpecialScheduleDays(quotas));


            for (var x = 0; x <= newSpecialSchedues.length - 1; x++) {
                for (var y = 0; y <= vm.specialDays.length - 1; y++) {
                    if (newSpecialSchedues[x].Date == vm.specialDays[y].Date) {
                        $('.alerts').empty();
                        common.showErrorMessage(Messages.error.errorCalendarDaysNoWorking);
                        newSpecialSchedues.splice(x, 1);
                        for (var x = 0; x <= newSpecialSchedues.length - 1; x++) {
                            for (var y = 0; y <= vm.specialDays.length - 1; y++) {
                                if (newSpecialSchedues[x].Date == vm.specialDays[y].Date) {
                                    $('.alerts').empty();
                                    common.showErrorMessage(Messages.error.errorCalendarDaysNoWorking);
                                    newSpecialSchedues.splice(x, 1);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            if (newSpecialSchedues.length > 0) {
                promises.push(calendarDataService.saveSpecialDays(newSpecialSchedues)
                    .success(function (data) {
                        common.showSuccessMessage(Messages.success.catalogItemUpdated);
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    })
                    .error(function (data) {
                        var count = vm.specialSchedues.length - 1;
                        for (var x = 0; x <= count; x++) {
                            if (vm.specialSchedues[x].Date == newSpecialSchedues[0].Date) {
                                vm.specialSchedues.splice(x, 1);
                            }
                        }
                    }));

                newSpecialSchedues.forEach(function (newSpecialSchedue, index) {
                    newSpecialSchedue.Quota.forEach(function (currentQuota, quotaIndex) {
                        quotas.push({
                            SpecialDayScheduleId: '',
                            DelegationId: vm.delegationId,
                            Date: newSpecialSchedue.Date,
                            Time: getTimeFromStringEditor(currentQuota.Time),
                            Capacity: currentQuota.Capacity
                        });
                    });

                    promises.push(calendarDataService.saveSpecialScheduleDays(quotas));
                });
            }
            return common.$q.all(promises);
        }

    }
})();