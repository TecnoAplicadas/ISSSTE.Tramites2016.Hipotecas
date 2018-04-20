//################################################
//     ##Fecha de creación: 18/03/15
//     ##Fecha de última modificación: 29/03/16
//     ##Responsable: Emanuel De la Isla Vértiz
//     ##Módulos asociados: acerca del trámite
//     ##Id Tickets asociados al cambio: R-013039
//################################################

(function () {
    'use strict';

    var controllerId = 'requisitosController';

    angular
        .module(appName)
        .controller(controllerId, ['common', 'homeDataService', requisitosController]);


    function requisitosController(common, homeDataService) {




        var vm = this;

        vm.validationDocumentsAfiliatedKid = [];
        vm.validationDocumentsNonAfiliatedKid = [];
        vm.complementaryScanDocuments = [];
        vm.complementaryNonScanDocuments = [];

        vm.init = init;
        vm.getDocumentsForInfo = getDocumentsForInfo;

        function init() {
            var dataPromise = getDocumentsForInfo();

            dataPromise
                .finally(function () {
                    common.logger.log("controller loaded", null, controllerId);
                    common.activateController([], controllerId);
                    common.hideLoadingScreen();
                });
        }

        function getDocumentsForInfo() {
            return homeDataService.getDocumentsForInfo()
                .success(function (data, status, headers, config) {
                    //vm.validationDocumentsAfiliatedKid = data[0];
                    //vm.validationDocumentsNonAfiliatedKid = data[1];    
                    vm.complementaryScanDocuments = data[0];
                    // vm.complementaryNonScanDocuments = data[3];
                })
                .error(function (data, status, headers, config) {
                    vm.error = false;
                    common.showErrorMessage(data, "Error al obtener los documentos.");
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }
    }
})();
