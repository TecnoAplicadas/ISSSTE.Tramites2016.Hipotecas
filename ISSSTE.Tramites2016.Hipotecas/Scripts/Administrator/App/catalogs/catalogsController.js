(function () {
    'use strict';

    var controllerId = 'catalogsController';
    angular
        .module(appName)
        .controller(controllerId, ['$routeParams', '$location', 'common', 'catalogsDataService', 'webApiService', catalogsController]);


    function catalogsController($routeParams, $location, common, catalogsDataService, webApiService) {
        //#region Controller Members

        var vm = this;
        vm.isNew = false;
        vm.error = false;
        vm.catalogName = null;
        vm.catalogItems = [];
        vm.catalogItemKey = null;
        vm.catalogItem = null;
        vm.booleanArray = [
            true,
            false
        ];

        vm.init = init;
        vm.initCatalog = initCatalog;
        vm.initCatalogItemDetail = initCatalogItemDetail;
        vm.addOrUpdateCatalogItem = addOrUpdateCatalogItem;
        vm.deleteCatalogItem = deleteCatalogItem;
        vm.getBooleanComboBoxLabel = getBooleanComboBoxLabel;
        vm.getDependentCatalogValues = getDependentCatalogValues;
        vm.getDependentCatalogLabel = getDependentCatalogLabel;
        vm.validateInput = validateInput;

        //#endregion

        //#region Fields

        var dependentCatalogs = [];

        //#endregion

        //#region Functions


        function validateInput(e, cadena) {
            if (e.keyCode === 16 || e.keyCode === 20) {
                return cadena
            }
            else {
                if (!soloLetras(e)) {
                    return cadena.substring(0, cadena.length - 1)
                }
            }
            return cadena
        }

        function soloLetras(e) {
            var key = e.keyCode || e.which;
            var tecla = String.fromCharCode(key).toLowerCase();
            var letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
  

            if (letras.indexOf(tecla) == -1) {
                return false;
            }
            return true;
        }

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initCatalog(catalogName, dependentPropertiesNames) {
            var initPromise = [];

            vm.catalogName = catalogName;

            initPromise.push(getCatalog());

            if (dependentPropertiesNames && dependentPropertiesNames.length > 0)
                initPromise.push(getDependentCatalogs(dependentPropertiesNames, false));

            common.$q.all(initPromise)
                .finally(function () {
                    init();
                })
        }

        function initCatalogItemDetail(catalogName, dependentPropertiesNames) {
            var initPromise = [];

            vm.catalogName = catalogName;
            vm.catalogItemKey = $routeParams[CATALOG_ITEM_KEY_PARAM];

            if (vm.catalogItemKey != Constants.newCatalogKeyId)
                initPromise.push(getCatalogItemDetail());
            else
                vm.isNew = true;

            if (dependentPropertiesNames && dependentPropertiesNames.length > 0)
                initPromise.push(getDependentCatalogs(dependentPropertiesNames, true));

            common.$q.all(initPromise)
                .finally(function () {
                    init();
                })
        }

        function addOrUpdateCatalogItem() {
            common.displayLoadingScreen();

            return webApiService.makeRetryRequest(1, function () {
                return catalogsDataService.addOrUpdateCatalogItem(vm.catalogName, vm.catalogItem);
            })
                .then(function (data) {
                    vm.catalogItem = data;
                    vm.isNew = false;

                    common.showSuccessMessage("", Messages.success.catalogItemUpdated)
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                })
                .finally(function () {
                    common.hideLoadingScreen();
                });
        }

        function deleteCatalogItem(navigationUrl) {
            common.displayLoadingScreen();

            return webApiService.makeRetryRequest(1, function () {
                return catalogsDataService.deleteCatalogItemDetail(vm.catalogName, vm.catalogItem);
            })
                .then(function (data) {
                    $location.path(navigationUrl);
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.deleteItem);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getBooleanComboBoxLabel(value) {
            if (value == true)
                return Messages.info.yes;
            else
                return Messages.info.no;
        }

        function getDependentCatalogValues(property) {
            var result = null;

            if (dependentCatalogs[property] != null) {
                result = R.map(function (actualElement) {
                    return actualElement.Id;
                }, dependentCatalogs[property])
            }

            return result;
        }

        function getDependentCatalogLabel(elementId, property) {
            var label = '';

            if (dependentCatalogs[property] != null) {
                var element = R.find(function (actualElement) {
                    return actualElement.Id == elementId;
                }, dependentCatalogs[property]);

                if (element)
                    label = element.Name;
            }

            return label;
        }

        //#endregion

        //#regio  Helper Methods

        function getCatalog() {
            return webApiService.makeRetryRequest(1, function () {
                return catalogsDataService.getCatalogItems(vm.catalogName);
            })
                .then(function (data) {
                    vm.catalogItems = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.catalogList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getCatalogItemDetail() {
            return webApiService.makeRetryRequest(1, function () {
                return catalogsDataService.getCatalogItemDetail(vm.catalogName, vm.catalogItemKey);
            })
                .then(function (data) {
                    vm.catalogItem = data;
                })
                .catch(function (reason) {
                    vm.error = true;
                    common.showErrorMessage(reason, Messages.error.catalogItemDetail);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getDependentCatalogs(dependentPropertyNames, addFirstElement) {
            return webApiService.makeRetryRequest(1, function () {
                return catalogsDataService.getDependentCatalogs(vm.catalogName, dependentPropertyNames);
            })
                .then(function (data) {
                    dependentCatalogs = data;

                    if (addFirstElement) {
                        R.forEach(function (actualPropertyData) {
                            actualPropertyData.splice(0, 0, { "Id": null, "Name": "Selecciona un elemento" });

                        }, R.values(dependentCatalogs))
                    }
                })
                .catch(function (reason) {
                    vm.error = true;
                    common.showErrorMessage(reason, Messages.error.dependentCatalogs);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        //#endregion
    }
})
();