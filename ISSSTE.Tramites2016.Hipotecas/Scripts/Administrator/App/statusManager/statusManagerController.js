(function () {
    'use strict';

    var controllerId = 'statusManagerController';

    angular
        .module(appName)
        .controller(controllerId, ['$routeParams', 'common', 'statusManagerDataService', function ($routeParams, common, statusManagerDataService) {
            //#region Controller Members

            var vm = this;

            vm.init = init;
            vm.availableNextStatus = [];
            vm.requestId ='';
            vm.NoIssste ='';
            vm.selectedNextStatus = {};

            vm.changeRequestStatus = changeRequestStatus;
            vm.setSelectedNextStatus = setSelectedNextStatus;
            vm.getCompleteUrl= getCompleteUrl;
            //#endregion

            //#region Functions
            function getCompleteUrl(partialUrl)
            {
                window.location.href= partialUrl.format(vm.NoIssste,vm.requestId);
            }

            function changeRequestStatus(successUrl)
            {
                try
                {
                    common.displayLoadingScreen();
                    if (angular.equals(vm.selectedNextStatus ,{}))
                    {
                        common.showWarningMessage("Debes seleccionar un estatus para poder guardar tu revisión.","Aviso")
                    }
                    var nextStatusPromise = statusManagerDataService.changeRequestStatus(vm.selectedNextStatus.StatusId, vm.requestId);
                    nextStatusPromise.success(function (data) {
                        debugger;
                        if (data == '') {
                            common.showSuccessMessage("El estatus de la solicitud ha sido exitosamente modificada");
                            $("html, body").animate({ scrollTop: 700 }, 2000);
                        }
                        else {
                            common.showWarningMessage(data);
                        }
                        
                        init();
                    });
                    nextStatusPromise.error(function (data) {
                        common.showErrorMessage(data, "Aviso");
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    });
                    common.$q.all([nextStatusPromise])
                        .finally(function ()  {  
                            common.hideLoadingScreen();
                        });
                }
                catch(ex)
                {
                    common.showErrorMessage(ex, "DEBUG_MODE");
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }
            }
            function setSelectedNextStatus(status) {
                vm.selectedNextStatus = status;
            }
            function init() {
                try
                {
                    common.displayLoadingScreen();
                    vm.requestId = $routeParams['RequestId'];
                    vm.NoIssste = $routeParams['NoIssste'];
                    common.logger.log(Messages.info.controllerLoaded, null, controllerId);
                    common.activateController([], controllerId);
                    var nextStatusPromise = statusManagerDataService.getAvailableNextStatus(vm.requestId);
                    nextStatusPromise.success(function (data) {
                        vm.availableNextStatus = data;
                    });
                    nextStatusPromise.error(function (data) {
                        common.showErrorMessage(data, "Aviso");
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    });
                    common.$q.all([nextStatusPromise])
                        .finally(function () {
                            common.hideLoadingScreen();
                        });
                }
                catch(ex)
                {
                    common.showErrorMessage(ex, "DEBUG_MODE");
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }
            }


            //#endregion
        }
        ]);
})();