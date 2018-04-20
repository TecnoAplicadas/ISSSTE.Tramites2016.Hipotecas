(function () {
    "use strict";

    var controllerId = "appointmentsController";

    angular
        .module(appName)
        .controller(controllerId, ["common", "appointmentsDataService", "webApiService", appointmentsController]);


    function appointmentsController(common, appointmentsDataService, webApiService) {
        //#region Controller Members

        var vm = this;

        vm.statusList = [];
        vm.requests = [];
        vm.query = "";
        vm.selectedStatus = null;
        vm.pageSizes = [
            10, 20, 30, 40, 50
        ];
        vm.selectedPageSize = vm.pageSizes[0];
        vm.pages = [1];
        vm.totalPages = 1;
        vm.selectedPage = 1;

        vm.init = init;
        vm.initSearch = initSearch;
        vm.searchRequests = searchRequests;
        vm.selectStatus = selectStatus;
        vm.isPageSelected = isPageSelected;
        vm.isFirstPage = isFirstPage;
        vm.isLasPage = isLasPage;
        vm.changeSelectedPage = changeSelectedPage;
        vm.changeToPreviousPage = changeToPreviousPage;
        vm.changeToNextPage = changeToNextPage;
        vm.changueSta = changueSta;
        vm.date = "";
        vm.time = "";
        vm.chanStatus = chanStatus;
        vm.clear = clear;

        //#endregion

        //#region Constants

        var MAX_PAGES = 11;

        //#enregion

        //#region Fields

        var paginationHelper = new PaginationHelper(MAX_PAGES);

        //#endregion

        //#region Functions

        function clear() {
            vm.date = "";
            vm.time = "";
            vm.query = "";
        }

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initSearch() {
            var statusListPromise = getStatusList();
            var requestsPromise = getRequests();

            common.$q.all([statusListPromise, requestsPromise])
                .finally(function () {
                    init();

                    UI.initStatusDropdown();
                });
        }

        function chanStatus(appId, isattend, requestId) {
            common.displayLoadingScreen();
            var promise = changueSta(appId, isattend, requestId);
            promise.then(function () {
                searchRequests();
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
            }).error(function () {
                common.hideLoadingScreen();
                $("html, body").animate({ scrollTop: 700 }, 2000);
            });

        }

        function selectStatus(status) {
            vm.selectedStatus = status;

            vm.searchRequests();
        }

        function getStatusList() {
            return webApiService.makeRetryRequest(1, function () {
                return appointmentsDataService.getStatusList();
            })
                .then(function (data) {
                    vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                });
        }

        function searchRequests() {
            common.displayLoadingScreen();

            getRequests()
                .finally((function () {
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                }));
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

        function getRequests() {
            return webApiService.makeRetryRequest(1, function () {
                return appointmentsDataService.getRequests(vm.selectedPageSize, vm.selectedPage, vm.query, vm.date, vm.time, vm.selectedStatus != null ? vm.selectedStatus.StatusId : null);
            })
                .then(function (data) {
                    vm.selectedPage = data.CurrentPage;
                    vm.totalPages = data.TotalPages;

                    vm.pages = paginationHelper.getPages(vm.selectedPage, vm.totalPages);

                    vm.requests = data.ResultList;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requests);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function changueSta(appId, isattend, requestId) {
            return webApiService.makeRetryRequest(1, function () {
                return appointmentsDataService.changueSta(appId, isattend, requestId);
            })
                .then(function (data) {
                    // vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        //#endregion
    }
})();