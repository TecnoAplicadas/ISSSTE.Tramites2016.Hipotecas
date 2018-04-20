(function () {
    "use strict";

    var controllerId = "searchController";

    angular
        .module(appName)
        .controller(controllerId, ["common", "searchDataService", "webApiService", searchController]);


    function searchController(common, searchDataService, webApiService) {
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
        vm.usersoperator = {};
        vm.getUsersOperator = getUsersOperator;

        vm.userselected = {};
        vm.assigUser = assigUser;
        vm.assig = assig;
        vm.assignService = assignService;
        vm.orderAsc = orderAsc;
        vm.orderDesc = orderDesc;
        //#endregion

        //#region Constants

        var MAX_PAGES = 11;

        //#enregion

        //#region Fields

        var paginationHelper = new PaginationHelper(MAX_PAGES);

        //#endregion

        //#region Functions



        function init() {

            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initSearch() {
            common.displayLoadingScreen();
            vm.ordenarAsc = false;
            var statusListPromise = getStatusList();
            var requestsPromise = getRequests();
           // var usersPromise = getUsersOperator();
            vm.requestassig = null;
            vm.userselected = null;

            common.$q.all([statusListPromise, requestsPromise])
                .finally(function () {
                    init();

                    UI.initStatusDropdown();
                    common.hideLoadingScreen();
                });
        }

        function selectStatus(status) {
            vm.selectedStatus = status;

            vm.searchRequests();
        }

        function searchRequests() {
            common.displayLoadingScreen();

            getRequests()
                .finally((function () {
                    common.hideLoadingScreen();
                }));
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
        function orderAsc() {
            vm.ordenarAsc = true;
            getRequests();
        }
        function orderDesc() {
            vm.ordenarAsc = false;
            getRequests();
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
                return searchDataService.getRequests(vm.selectedPageSize, vm.selectedPage, vm.query, vm.selectedStatus != null ? vm.selectedStatus.StatusId : null, vm.ordenarAsc);
            })
                .then(function (data) {
                    vm.selectedPage = data.CurrentPage;
                    vm.totalPages = data.TotalPages;

                    vm.pages = paginationHelper.getPages(vm.selectedPage, vm.totalPages);

                    vm.requests = data.Requests;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requests);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }

        function getStatusList() {
            return webApiService.makeRetryRequest(1, function () {
                return searchDataService.getStatusList();
            })
                .then(function (data) {
                    vm.statusList = data;
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                    $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        function getUsersOperator() {
            return webApiService.makeRetryRequest(1, function () {
                return searchDataService.usersOperator();
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
                return searchDataService.assign(requestId, user);
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