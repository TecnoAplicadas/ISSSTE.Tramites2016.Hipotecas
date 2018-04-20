//####################################################################
//      ## Fecha de creación: 18-03-2016
//      ## Fecha de última modificación: 30-03-2016
//      ## Responsable: Emanuel De la Isla Vértiz
//      ## Módulos asociados: Información general, Deudos, Beneficiarios, Historial Laboral.
//      ## Id Tickets asociados al cambio: R-013042
//####################################################################
(function () {
    "use strict";
    var controllerId = "homeController";
    angular
        .module(appName)
        .controller(controllerId, ["common", "homeDataService", "requestDataService", "localStorageService", homeController]);

    function homeController(common, homeDataService, requestDataService, localStorageService) {
        //#region Controller Members
        var show = false;
        var vm = this;
        vm.entitleInformation = {};
        vm.recentNotifications = [];
        vm.init = init;
        vm.initEntitle = initEntitle;
        vm.isFormValid = isFormValid;
        vm.updateEntitleInformation = updateEntitleInformation;
        vm.show = show;
        vm.getNotifications = getNotifications;
        vm.entitleInformation.Telephone;
        vm.telefono;
        vm.lada;
        vm.getAlerts = getAlerts;
        //#endregion

        //#region Constants

        //#endregion

        //#region Fields

        var isInfoUpdated = true;

        //#endregion

        //#region Properties


        //#endregion        

        //#region Public Functions

        function init() {
            //common.displayLoadingScreen();
            completeControllerInit();
            common.overrideNavigationMenu(false);
            vm.show = false;
            common.hideLoadingScreen();
        }

        function initEntitle() {
            $('[data-toggle="tooltip"]').tooltip();

            $('#home').addClass("active");
            $('#request').addClass("hidden");
            $('#infoGeneral').addClass("hidden");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hidden");
            $('#appoiments').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");
            //$('#').removeClass("hidden");
            //$('#').removeClass("hidden");

            var keys = localStorageService.keys();
            vm.show = false;
            var dataPromise = null;
            dataPromise = getEntitleInformation();

            dataPromise.success(function (data, status, headers, config) {
                vm.entitleInformation = data;
                common.config.entitleInformation.CURP = vm.entitleInformation.CURP;
                common.config.entitleInformation.StateDescription = vm.entitleInformation.StateDescription;



                localStorageService.set(common.config.ID_CURP_PARAM, common.config.entitleInformation.CURP, common.config.entitleInformation.StateDescription);
                var promise = getNotifications();
                completeControllerInit();
                common.overrideNavigationMenu(false);
                vm.show = false;
                promise.success(function () {
                    completeControllerInit();
                    common.overrideNavigationMenu(false);

                    vm.show = false;

                })
                    .error(function () {
                        completeControllerInit();
                        common.overrideNavigationMenu(true);
                        vm.show = true;
                        common.showErrorMessage(Messages.error.errorInformation);
                        common.showErrorMessage(Messages.error.getEntitleInformationCURP);
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    });

            })
                .error(function (data, status, headers, config) {
                    completeControllerInit();
                    common.overrideNavigationMenu(false);
                    vm.show = true;
                    //CAP common.showErrorMessage(Messages.error.errorInformation);
                    common.showErrorMessage(data.Message);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });

        }

        function isFormValid(form) {

            return form.$valid;
        }


        function updateEntitleInformation() {


            var promise;

            promise = homeDataService.updateEntitleInformation(common.config.entitleInformation.CURP, vm.lada + vm.telefono, vm.information.Entitle.Email);


            promise
                .success(function (data, status, headers, config) {
                    common.showSuccessMessage(Messages.success.informationUpdated);
             
                    isInfoUpdated = true;
                    if ((vm.entitledData.Telephone.substring(0, 2) == "55") || (vm.entitledData.Telephone.substring(0, 2) == "33") || (vm.entitledData.Telephone.substring(0, 2) == "81")) {
                        vm.lada = vm.entitledData.Telephone.substring(0, 2);
                        vm.entitledData.Telephone = vm.entitledData.Telephone.substring(2);
                    }
                    else {
                        vm.lada = vm.entitledData.Telephone.substring(0, 3);
                        vm.entitledData.Telephone = vm.entitledData.Telephone.substring(3);
                    }
     
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(data, Messages.error.informationUpdated);
                   
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                })

                .finally(function () {

                    //common.hideLoadingScreen();
                    //common.hideLoadingScreen();
                    //var old = $location.hash();
                    //$location.hash('top-div');
                    //$anchorScroll();
                    //$location.hash(old);
                    //$anchorScroll('top-div');
                });
            /* } else {
                 common.showErrorMessage("No has llenado campos requeridos. Por favor verifica.", "¡Error!");
                 $("html, body").animate({ scrollTop: 1 }, 2000);
 
             }*/

        }


        //#endregion

        //#region Helper Functions

        function getEntitleInformation() {
            return homeDataService.getEntitleInformation(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    common.config.entitle = data;
                    vm.entitleInformation.Telephone = data.Telephone;
                    if ((vm.entitleInformation.Telephone + "").length > 5) {
                        //var tel = vm.entitleInformation.Telephone+"".substring(2);

                        //vm.entitleInformation.Telephone.slice(2);
                        if (((vm.entitleInformation.Telephone + "").substring(0, 2) == '55') || ((vm.entitleInformation.Telephone + "").substring(0, 2) == '33') || ((vm.entitleInformation.Telephone + "").substring(0, 2) == '81')) {
                            vm.lada = (vm.entitleInformation.Telephone + "").substring(0, 2);
                            //  vm.information.Entitle.Telephone = vm.entitleInformation.Telephone.substring(2);
                            vm.entitleInformation.Telephone = (vm.entitleInformation.Telephone + "").substring(2);
                        }
                        else {
                            vm.lada = (vm.entitleInformation.Telephone + "").substring(0, 3);
                            //  vm.information.Entitle.Telephone = vm.entitleInformation.Telephone.substring(2);
                            vm.entitleInformation.Telephone = (vm.entitleInformation.Telephone + "").substring(3);
                        }
                    }
                    common.hideLoadingScreen();

                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.getEntitleInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getAlerts() {
            var promise;
            promise = homeDataService.getEntitledAlerts(common.config.entitleInformation.NoISSSTE);

            promise
                .success(function (data, status, headers, config) {
                    vm.alerts = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.hideLoadingScreen();
                    common.showErrorMessage(data, Messages.error.informationUpdated);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                    
                });
        }


        function getNotifications() {
            return requestDataService.getNotifications(common.config.entitleInformation.CURP)
                .success(function (data, status, headers, config) {
                    vm.recentNotifications = data;
                    common.hideLoadingScreen()
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        function completeControllerInit() {
            common.logger.log(Messages.info.contollerLoaded, null, controllerId);
            common.overrideNavigationMenu(true);
            common.activateController([], controllerId);
        }
        //#endregion

        function closeWorking() {
            common.hideLoadingScreen();
            
        }
    };
})();