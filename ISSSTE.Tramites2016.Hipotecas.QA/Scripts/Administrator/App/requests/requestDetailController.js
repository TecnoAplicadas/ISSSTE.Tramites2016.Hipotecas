(function() {
    "use strict";

    var controllerId = "requestDetailController";
    angular
        .module(appName)
        .controller(controllerId, ["$routeParams", "$location", "common", "requestsDataService", "webApiService", requestDetailController]);


    function requestDetailController($routeParams, $location, common, requestsDataService, webApiService) {
        //#region Controller Members

        var vm = this;

        vm.error = false;
        vm.requestInfo = null;
        vm.nextStatus = [];
        vm.selectedNextStatus = null;
        vm.nextStatusObservations = null;
        vm.botonPresionado = null;
        vm.init = init;
        vm.initRequest = initRequest;
        vm.getRequestEntitleInformation = getRequestEntitleInformation;
        vm.getOpinionRequest = getOpinionRequest;
        vm.getMessagesOpinion = getMessagesOpinion;
        vm.saveStatusRequest = saveStatusRequest;
        vm.updateDocument = updateDocument;//MFP
        vm.isDocumentUpdate = null;
        vm.requestAll = {};
        vm.information = {};
        vm.messages = {};
        vm.StatusId = {};
        vm.opinionSelected = {};
        vm.statusSelected = {};
        vm.saveRequest = saveRequest;
        vm.setSelectedNextStatus = setSelectedNextStatus;
        vm.getMessagesOpinionParameter = getMessagesOpinionParameter;
        vm.setDiag = setDiag;
        vm.loadSeguimiento = vm.loadSeguimiento;
        vm.saveTempObservations = saveTempObservations;
       
       
        vm.comments = [];

        vm.initDocuments = initDocuments;


        //---------
        vm.completeRequestReview = completeRequestReview;
        //-----------
        //UpdateStatus

        function initDocuments(observations) {
            observations.forEach(function (observation) {
                vm.comments.push(observation);
            });
        }

        function saveTempObservations() {
            var url = window.location.href;
            if(url.indexOf("Documentos") > 0) {
                requestsDataService.saveTempObservations(vm.comments)
                .then(function (data) {
                }, function (data) {
                    console.log(data);
                });
            }
        }


        function completeRequestReview() {

            debugger;
            if (vm.selectedNextStatus != null) {
                status = vm.selectedNextStatus.StatusId;
            }
            if (status != "") {
                if (vm.selectedNextStatus.StatusId <= 50200 || vm.selectedNextStatus.StatusId >= 50301) {
                    
                        var error = validatePrincipalReject();
                        if (error != '') {
                            common.showErrorMessage(Messages.error.principalReject);
                            $("html, body").animate({ scrollTop: 700 }, 2000);
                        } else {
                            updateRequest();
                        }
                    

                } else {

                    if (vm.selectedRoom != null && vm.selectedRoom.RoomName != undefined) {
                        if (vm.selectedRoom.EntryDate != undefined) {
                            if (isRequestReviewed() && validateChildType() && validateKidRoom()) {
                                var error = validatePrincipalReject();
                                if (error != '') {
                                    common.showErrorMessage(Messages.error.principalReject);
                                    $("html, body").animate({ scrollTop: 700 }, 2000);
                                } else {
                                    updateRequest();
                                }
                            }
                        } else {
                            common.showErrorMessage("El campo fecha de ingreso es obligatorio");
                            $("html, body").animate({ scrollTop: 700 }, 2000);
                        }
                    } else {
                        common.showErrorMessage("Debes de seleccionar una sala");
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    }
                }
            } else {
                if (isRequestReviewed() && validateChildType() && validateKidRoom()) {
                    var error = validatePrincipalReject();
                    if (error != '') {
                        common.showErrorMessage(Messages.error.principalReject);
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    }
                }
            }
        }
        //---------------------


        function validatePrincipalReject() {
            var error = '';
            if (vm.selectedNextStatus.StatusId == Constants.requestStatuses.rejectedEntry ||
                vm.selectedNextStatus.StatusId == Constants.requestStatuses.postponedEntry) {
                if (vm.nextStatusObservations == null || vm.nextStatusObservations == '') {
                    error = Messages.error.principalReject;
                }
            }

            return error;
        }



        //#endregion

        //#region Functions

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initRequest() {
            common.displayLoadingScreen();
            vm.requestAll = {};
            vm.information = {};
            vm.messages = {};
            vm.statusSelected = 1;
            var initPromises = [];

            var requestId = $routeParams[REQUEST_ID_PARAM];

            if (requestId) {
                initPromises.push(getRequestEntitleInformation(requestId));
                initPromises.push(getOpinionRequest(requestId));
                initPromises.push(getMessagesOpinionParameter(vm.statusSelected));
            }

            common.$q.all(initPromises)
                .finally(function() {
                    init();
                    vm.statusSelected = null;
                    vm.opinionSelected = null;
                    common.hideLoadingScreen();
                }).catch(function () {
                    common.hideLoadingScreen();
                     $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function setDiag(messa) {
            vm.opinionSelected = messa;
        }

        function saveRequest(navigate) {

            if (vm.statusSelected == null || vm.opinionSelected == null) {
                common.showErrorMessage("Error", Messages.error.requestDetailselectStatusAndOpinion);
                $("html, body").animate({ scrollTop: 700 }, 2000);
            } else {
                var requestId = $routeParams[REQUEST_ID_PARAM];
                var happy = false;
                if (vm.statusSelected === 1)
                    happy = true;
                else
                    happy = false;
                var promise = this.saveStatusRequest(vm.requestAll.Request.RequestId, vm.statusSelected, happy, vm.opinionSelected);

                promise.then(function() {
                    navigate();
                }).catch(function() {
                    common.showErrorMessage("Error", Messages.error.errorSave);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }).finally(function() {

                });
            }

        }


        function setSelectedNextStatus(status) {
            common.displayLoadingScreen();
            vm.selectedNextStatus = status;
            //vm.statusSelected = status;
            var initPromises = [];
            initPromises.push(getMessagesOpinionParameter(vm.statusSelected));

            common.$q.all(initPromises)
                .finally(function() {
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });

        }

//#endregion

        //#region Helper Functions

        function getRequestEntitleInformation(requestId) {
            return webApiService.makeRetryRequest(1, function() {
                    return requestsDataService.getRequestEntitleInformation(requestId);
                })
                .then(function(data) {
                    vm.information = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                    vm.error = true;
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getOpinionRequest(requestId) {
            return webApiService.makeRetryRequest(1, function() {
                    return requestsDataService.getOpinionRequest(requestId);
                })
                .then(function(data) {
                    vm.requestAll = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                    vm.error = true;
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getMessagesOpinion(requestId) {
            return webApiService.makeRetryRequest(1, function() {
                    return requestsDataService.getMessagesOpinion(requestId);
                })
                .then(function(data) {
                    vm.messages = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                    vm.error = true;
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        
        
        function updateDocument(requestId, DocumentId, isValid, observations) {
            return webApiService.makeRetryRequest(1, function () {
                return requestsDataService.updateDocument(requestId, DocumentId, isValid, observations);
            })
                .then(function (data) {
                    vm.isDocumentUpdate = true;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                    vm.error = true;
                    vm.isDocumentUpdate = false;
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function saveStatusRequest(requestId, idstatus, happy, opinion) {
            return webApiService.makeRetryRequest(1, function() {
                    return requestsDataService.saveStatusRequest(requestId, idstatus, happy, opinion);
                })
                .then(function(data) {
                    vm.save = data;

                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                    vm.error = true;
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getMessagesOpinionParameter(id) {
            return webApiService.makeRetryRequest(1, function() {
                    return requestsDataService.getMessagesOpinionParameter(id);
                })
                .then(function(data) {
                    vm.messages = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                    vm.error = true;
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        function loadSeguimiento() {
            console.log("loadSeguimiento()");
        }

        function setDocumentApprovedState(botonAprobar) {
            botonAprobar = true;
            vm.botonPresionado = botonAprobar;


        }

//#endregion
    }
    function completeRequestReview() {
        if (vm.selectedNextStatus != null) {
            status = vm.selectedNextStatus.StatusId;
        }
        if (status != "") {
            if (vm.selectedNextStatus.StatusId <= 50200 || vm.selectedNextStatus.StatusId >= 50301) {
                if (isRequestReviewed() && validateChildType() && validateKidRoom()) {
                    var error = validatePrincipalReject();
                    if (error != '') {
                        common.showErrorMessage(Messages.error.principalReject);
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    } else {
                        updateRequest();
                    }
                }

            } else {

                if (vm.selectedRoom != null && vm.selectedRoom.RoomName != undefined) {
                    if (vm.selectedRoom.EntryDate != undefined) {
                        if (isRequestReviewed() && validateChildType() && validateKidRoom()) {
                            var error = validatePrincipalReject();
                            if (error != '') {
                                common.showErrorMessage(Messages.error.principalReject);
                                $("html, body").animate({ scrollTop: 700 }, 2000);
                            } else {
                                updateRequest();
                            }
                        }
                    } else {
                        common.showErrorMessage("El campo fecha de ingreso es obligatorio");
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    }
                } else {
                    common.showErrorMessage("Debes de seleccionar una sala");
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }
            }
        } else {
            if (isRequestReviewed() && validateChildType() && validateKidRoom()) {
                var error = validatePrincipalReject();
                if (error != '') {
                    common.showErrorMessage(Messages.error.principalReject);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }
            }
        }
    }



})
();