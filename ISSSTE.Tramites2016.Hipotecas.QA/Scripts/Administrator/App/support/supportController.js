(function () {
    'use strict';

    var controllerId = 'supportController';

    angular
        .module(appName)
        .controller(controllerId, ['common', 'supportDataService', 'webApiService', supportController]);


    function supportController(common, supportDataService, webApiService) {
        //#region Controller Members

        var vm = this;

        vm.curp = null;
        vm.rfc = null;
        vm.isssteNumber = null;

        vm.init = init;
        vm.beginQuoteWithCurp = beginQuoteWithCurp;
        vm.beginQuoteWithRfc = beginQuoteWithRfc;
        vm.beginQuoteWithIsssteNumber = beginQuoteWithIsssteNumber;

        //#endregion

        //#region Functions

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function beginQuoteWithCurp() {
            common.displayLoadingScreen();

            return webApiService.makeRetryRequest(1, function () {
                return supportDataService.getEntitleIsssteNumberByCurp(vm.curp.toUpperCase());
            })
                .then(function (data) {
                    if (data)
                        openEntitleApplicationPage(data);
                    else
                        openNotEntitleApplicationPage(vm.curp);
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.entitleIsssteNumber);
                     $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .finally(function () {
                    common.hideLoadingScreen();
                     $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function beginQuoteWithRfc() {
            common.displayLoadingScreen();

            return webApiService.makeRetryRequest(1, function () {
                return supportDataService.getEntitleIsssteNumberByRfc(vm.rfc.toUpperCase());
            })
                .then(function (data) {
                    if (data) {
                        openEntitleApplicationPage(data);
                    }
                    else {
                        common.showErrorMessage(Messages.error.entitleNotFoundHelp, Messages.error.entitleNotFound);
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    }
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.entitleIsssteNumber);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .finally(function () {
                    common.hideLoadingScreen();
                });
        }

        function beginQuoteWithIsssteNumber() {
            common.displayLoadingScreen();

            return webApiService.makeRetryRequest(1, function () {
                return supportDataService.validateIsssteNumber(vm.isssteNumber);
            })
                .then(function (data) {
                    if (data) {
                        openEntitleApplicationPage(data);
                    }
                    else {
                        common.showErrorMessage(Messages.error.entitleNotFoundHelp, Messages.error.entitleNotFound);
                        $("html, body").animate({ scrollTop: 700 }, 2000);
                    }})
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.entitleIsssteNumber);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .finally(function () {
                    common.hideLoadingScreen();
                });
        }

        //#endregion

        //#regio  Helper Methods

        function openEntitleApplicationPage(isssteNumber) {
            var applicationUrl = supportDataService.getEntitleApplicationUrl(isssteNumber);

            openApplicationPage(applicationUrl);
        }

        function openNotEntitleApplicationPage(curp) {
            var applicationUrl = supportDataService.getNotEntitleApplicationUrl(curp);

            openApplicationPage(applicationUrl);
        }

        function openApplicationPage(applicationUrl) {
            window.open(applicationUrl, '_blank', '');

            common.showSuccessMessage(null, Messages.success.entitleApplicationLaunched);
            $("html, body").animate({ scrollTop: 700 }, 2000);
        }

        //#endregion
    }
})();