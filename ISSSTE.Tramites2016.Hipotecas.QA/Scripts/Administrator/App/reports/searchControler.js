(function () {
    "use strict";

    var controllerId = "searchControler";

    angular
        .module(appName)
        .controller(controllerId, ["common", "RepoDataController", "webApiService", searchControler]);


    function searchControler(common, RepoDataController, webApiService) {
        //#region Controller Members

        var vm = this;

        vm.banderaSoloDelegacion = false;
        vm.alertaFechas = true;
        vm.delegationsList = [];
        vm.statusList = [];
        vm.tipoBeneficioList = [];
        vm.requests = [];
        vm.numIssste = null;
        vm.nameEntiti = null;
        vm.selectedStatus = null;
        vm.selectedGenero = null;
        vm.selectedDelegation = null;
        vm.selectedTipoPension = null;

        vm.pageSizes = [
            10, 20, 30, 40, 50
        ];
        vm.selectedPageSize = vm.pageSizes[0];
        vm.pages = [1];
        vm.totalPages = 1;
        vm.selectedPage = 1;
        vm.delegationId = -1;
        vm.selectedStatus = null;
        vm.init = init;
        vm.initSearch = initSearch;
        vm.searchRequests = searchRequests;
        vm.selectStatus = selectStatus;
        vm.TotalesPorSolicitudExitoso = 0;
        vm.TotalesPorSolicitudIncorrecto = 0;
        vm.TotalesPorCitaCorrecto = 0;
        vm.TotalesPorCitaIncorrecto = 0;
        vm.TotalesPorBeneficioJubilacion = 0;
        vm.TotalesPorBeneficioEdadYTiempoServicio = 0;
        vm.TotalesPorBeneficioCesantia = 0;
        vm.TotalesPorBeneficioMuerteTrabajador = 0;
        vm.TotalesPorBeneficioMuertePensionado = 0;



        vm.TotalesDelegacionExitosas = 0;
        vm.TotalesDelegacionIncorrectas = 0;
        vm.TotalesDelegacionAgendada = 0;
        vm.TotalesDelegacionCancelada = 0;

        vm.selecTipoPension = selecTipoPension;
        vm.selectGenero = selectGenero;
        vm.selectDelegation = selectDelegation;
        vm.isPageSelected = isPageSelected;
        vm.isFirstPage = isFirstPage;
        vm.isLasPage = isLasPage;
        vm.changeSelectedPage = changeSelectedPage;
        vm.changeToPreviousPage = changeToPreviousPage;
        vm.changeToNextPage = changeToNextPage;
        vm.usersoperator = {};
        vm.getUsersOperator = getUsersOperator;
        //vm.delegationChange = delegationChange;
        vm.userselected = {};

        var date = new Date();
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();
        vm.final = year + "/" + monthIndex + "/" + day;
        vm.inicio = year + "/" + monthIndex + "/" + (day - 1);
        vm.assigUser = assigUser;
        vm.assig = assig;
        // vm.myCtrl=myCtrl;
        vm.assignService = assignService;
        var MAX_PAGES = 11;
        var paginationHelper = new PaginationHelper(MAX_PAGES);
        //#endregion
        //#region Functions

        function init() {

            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initSearch() {
            var delegationListPromise = getDelegationList();
            var statusListPromise = getStatusList();
            var requestsPromise = getRepo();
            var impresion = impresion;
            var usersPromise = getUsersOperator();
            var tipoBeneficioListPromise = getTipoPensionList();
            vm.requestassig = null;
            vm.userselected = null;
            common.$q.all([delegationListPromise, statusListPromise, tipoBeneficioListPromise, requestsPromise, usersPromise])
                .finally(function () {
                    init();

                    UI.initStatusDropdown();
                    // UI.initDelegationDropdown();
                });
        }



        //Estatus 
        function selectStatus(status) {
            vm.selectedStatus = status;

            vm.searchRequests();
        }
        //Genero
        function selectGenero(genero) {

            if (genero == 1) {
                vm.selectedGenero = "H";
            } else if (genero == 2) {
                vm.selectedGenero = "M";
            } else {
                vm.selectedGenero = null;
            }

            vm.searchRequests();
        }

        //Delegacion
        function selectDelegation(delegacion) {
            if (delegacion != null) {
                vm.banderaSoloDelegacion = true;
            }
            else {
                vm.banderaSoloDelegacion = false;
            }
            vm.selectedDelegation = delegacion;

            vm.searchRequests();
        }
        //TipoPension
        function selecTipoPension(tipoPension) {
            vm.selectedTipoPension = tipoPension;
            vm.searchRequests();
        }


        function searchRequests() {
            if (vm.inicio > vm.final) {
                vm.alertaFechas = 0;
            }
            else {
                vm.alertaFechas = 1;
                common.displayLoadingScreen();
                getRepo()
                    .finally((function () {
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    }));
            }
        }

        function assig(requestId) {
            vm.requestassig = {};
            vm.requestassig = requestId;
        }

        function assigUser() {
            common.displayLoadingScreen();
            var promise = assignService(vm.requestassig, vm.userselected);
            promise.then(function () {
                common.hideLoadingScreen();
                vm.requestassig = null;
                initSearch();
            }).catch(function () {
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
            });
        }

        function isPageSelected(page) {
            return page == vm.selectedPage;
        }

        function isFirstPage() {
            return vm.selectedPage == 1;
        }

        function isLasPage() {
            return vm.selectedPage == vm.totalPages;
        }

        function changeSelectedPage(page) {
            if (!isPageSelected(page)) {
                vm.selectedPage = page;

                vm.searchRequests();
            }
        }

        function changeToPreviousPage() {
            if (!vm.isFirstPage())
                changeSelectedPage(vm.selectedPage - 1);
        }

        function changeToNextPage() {
            if (!isLasPage())
                changeSelectedPage(vm.selectedPage + 1);
        }

        //#endregion

        //#regio  Helper Methods
        /*function myCtrl() {
            var blob = new Blob([document.getElementById('exportable').innerHTM], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };
        }*/

        //filtro de inicio
        function getRepo() {

            return webApiService.makeRetryRequest(1, function () {

                if (vm.nameEntiti == '') {

                    vm.nameEntiti = null;
                }
                return RepoDataController.getRepo(vm.selectedPageSize, vm.selectedPage, vm.selectedGenero,
                    vm.selectedDelegation, vm.selectedTipoPension, vm.selectedStatus, vm.nameEntiti, vm.numIssste, vm.inicio, vm.final, vm.banderaSoloDelegacion);
                /*return RepoDataController.getRepo(vm.selectedPageSize, vm.selectedPage, vm.numIssste, vm.nameEntiti,
                     vm.selectedGenero, vm.selectedDelegation != null ? vm.selectedDelegation.delegacion : null, vm.selectedTipoPension != null ? vm.selectedTipoPension.tipoPension : null, vm.selectedStatus != null ? vm.selectedStatus.StatusId : null);
            */
            })
                .then(function (data) {
                    vm.selectedPage = data.CurrentPage;
                    vm.totalPages = data.TotalPages;
                    vm.pages = paginationHelper.getPages(vm.selectedPage, vm.totalPages);
                    //Totales 
                    vm.TotalesPorSolicitudExitoso = data.TotalesPorSolicitudExitoso;
                    vm.TotalesPorSolicitudIncorrecto = data.TotalesPorSolicitudIncorrecto;
                    vm.TotalesPorCitaCorrecto = data.TotalesPorCitaCorrecto;
                    vm.TotalesPorCitaIncorrecto = data.TotalesPorCitaIncorrecto;
                    vm.TotalesPorBeneficioJubilacion = data.TotalesPorBeneficioJubilacion;
                    vm.TotalesPorBeneficioEdadYTiempoServicio = data.TotalesPorBeneficioEdadYTiempoServicio;
                    vm.TotalesPorBeneficioCesantia = data.TotalesPorBeneficioCesantia;
                    vm.TotalesPorBeneficioMuerteTrabajador = data.TotalesPorBeneficioMuerteTrabajador;
                    vm.TotalesPorBeneficioMuertePensionado = data.TotalesPorBeneficioMuertePensionado;
                    if (vm.banderaSoloDelegacion == true) {
                        vm.TotalesDelegacionExitosas = data.TotalesDelegacionExitosas;
                        vm.TotalesDelegacionIncorrectas = data.TotalesDelegacionIncorrectas;
                        vm.TotalesDelegacionAgendada = data.TotalesDelegacionAgendada;
                        vm.TotalesDelegacionCancelada = data.TotalesDelegacionCancelada;
                        if (vm.TotalesDelegacionCancelada == undefined)
                            vm.TotalesDelegacionCancelada = 0;
                        if (vm.TotalesDelegacionAgendada == undefined)
                            vm.TotalesDelegacionAgendada = 0;


                    }
                    //filas 
                    vm.requests = data.Requests;
                    vm.impresion = data.ParaImpresion;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requests);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });

        }

        function getStatusList() {
            return webApiService.makeRetryRequest(1, function () {
                return RepoDataController.getStatusList();
            })
                .then(function (data) {
                    vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getDelegationList() {
            return webApiService.makeRetryRequest(1, function () {
                return RepoDataController.getAllDelegationList();
            })
                .then(function (data) {
                    vm.delegationsList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getTipoPensionList() {
            return webApiService.makeRetryRequest(1, function () {
                return RepoDataController.getTipoPensionList();
            })
                .then(function (data) {
                    vm.tipoBeneficioList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        function getUsersOperator() {
            return webApiService.makeRetryRequest(1, function () {
                return RepoDataController.usersOperator();
            })
                .then(function (data) {
                    vm.usersoperator = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function assignService(requestId, user) {
            return webApiService.makeRetryRequest(1, function () {
                return RepoDataController.assign(requestId, user);
            })
                .then(function (data) {
                    //vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        //#endregion
    }
})();