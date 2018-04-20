(function () {
    'use strict';

    var controllerId = 'messagesController';
    angular
        .module(appName)
        .controller(controllerId, ['common', "$location", "$routeParams", 'messagesDataService', 'webApiService', messagesController]);

    function messagesController(common, $location, $routeParams, messagesDataService, webApiService) {
        //#region Controller Members

        var vm = this;

        vm.query = "";

        vm.pageSizes = [
            10, 20, 30, 40, 50
        ];
        vm.selectedPageSize = vm.pageSizes[0];
        vm.pages = [1];
        vm.totalPages = 1;
        vm.selectedPage = 1;

        vm.init = init;
        vm.initSearch = initSearch;

        vm.isPageSelected = isPageSelected;
        vm.isFirstPage = isFirstPage;
        vm.isLasPage = isLasPage;
        vm.changeSelectedPage = changeSelectedPage;
        vm.changeToPreviousPage = changeToPreviousPage;
        vm.changeToNextPage = changeToNextPage;
        vm.savemessagesContr = savemessagesContr;

        vm.savemessages = savemessages;
        vm.getmessages = getmessages;
        vm.getmessagesById = getmessagesById;
        vm.Savesucces;


        vm.messages = {};
        vm.searchmessages = searchmessages;
        vm.initSave = initSave;
        vm.isFormValid = isFormValid;
        vm.messagesExists = false;

        //#endregion

        //#region Constants

        var MAX_PAGES = 11;

        //#enregion

        //#region Fields

        var paginationHelper = new PaginationHelper(MAX_PAGES);

        //#endregion

        //#region Functions

        function isFormValid() {
            if (vm.messagesSave[0].Description != undefined) {
                if ((vm.messagesSave[0].Description != '' && vm.messagesSave[0].Description != null && vm.messagesSave[0].Description != undefined)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initSearch() {
            var search = getmessages();
            common.$q.all([search])
                .finally(function () {
                    init();
                });
        }

        function initSave() {
            vm.messagesExists = false;

            vm.savemessages = {};
            //vm.savemessages.messagesId = 0;
            // var messagesId = $routeParams["messagesId"];
            var messagesId = $routeParams[MESSAGE_ID_PARAM];
            var messagesPromise = getmessagesById(messagesId);

            common.$q.all([messagesPromise])
                .finally(function () {
                    init();
                    if (messagesId == '0') {
                        vm.savemessages = {};
                        vm.savemessages.messagesId = 0;
                    }

                });
        }

        function savemessagesContr(url) {

            if (!isFormValid()) { return; }
            var promisesave = savemessages();
            common.$q.all([promisesave])
                .finally(function () {
                    common.hideLoadingScreen();
                    $location.path(url);
                    UI.initStatusDropdown();
                     $("html, body").animate({ scrollTop: 700 }, 2000);
                });


        }

        function changeDescription(message, messageId) {
            return webApiService.makeRetryRequest(1, function () {
                return datesDataService.changeDescription(message, messageId);
            })
                .then(function (data) {
                    // vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        function searchmessages() {
            common.displayLoadingScreen();

            getmessages()
                .finally((function () {
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }))
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
                vm.searchmessages();
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







        function getmessages() {
            return webApiService.makeRetryRequest(1, function () {
                return messagesDataService.getmessages(vm.selectedPageSize, vm.selectedPage, vm.query);
            })
                .then(function (data) {
                    vm.selectedPage = data.CurrentPage;
                    vm.totalPages = data.TotalPages;

                    vm.pages = paginationHelper.getPages(vm.selectedPage, vm.totalPages);

                    vm.messages = data.Messages;

                    if (vm.Savesucces > 0) {
                        vm.Savesucces = Savesucces;
                    }
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requests);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getmessagesById(id) {
            return webApiService.makeRetryRequest(1, function () {
                return messagesDataService.getmessagesById(id);
            })
                .then(function (data) {
                    if (data != null) {
                        vm.messagesSave = data;
                    }
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requests);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function savemessages() {
            return webApiService.makeRetryRequest(1, function () {

                var messageDescription = [];
                vm.messagesSave.forEach(function (message, index) {
                    messageDescription.push(message);
                });


                return messagesDataService.changeDescription(messageDescription);
            })
                .then(function (data) {
                    var Savesucces = "Se han guardado correctamente los cambios";
                    vm.Savesucces = 1;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.catalogList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        //#endregion
    }
})();