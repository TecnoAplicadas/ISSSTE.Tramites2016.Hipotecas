(function () {
    "use strict";

    var controllerId = "navigationController";

    angular
        .module(appName)
        .controller(controllerId, ["$rootScope", "$routeParams", "$location", "common", "appConfig", navigationController]);

    function navigationController($rootScope, $routeParams, $location, common, appConfig) {

        //#region Constants

        var REQUEST_ID_TEMPLATE_PARAM = ":" + REQUEST_ID_PARAM;
        var ENTITLE_ID_TEMPLATE_PARAM = ":" + ENTITLE_ID_PARAM;
        var CATALOG_ITEM_KEY_TEMPLATE_PARAM = ":" + "catalogItemKey";
        var MESSAGE_ID_TEMPLATE_PARAM = ":" + MESSAGE_ID_PARAM;

        //#endregion

        //#region Members

        var vm = this;
        vm.searchUrl = Routes.search.url;
        vm.requestDetailUrl = Routes.requestDetail.url;
        vm.messageDetailUrl = Routes.messagesById.url;
        vm.MessageUrl = Routes.messages.url;
        vm.calendarUrl = Routes.calendar.url;
        vm.messagesUrl = Routes.messages.url;
        vm.reportsUrl = Routes.reports.url;
        vm.datesUrl = Routes.dates.url;
        vm.RequestDetailRevisionResult = Routes.requestDetail_RevisionStatus.url;
        //Support

        vm.supportUrl = Routes.support.url;

        //Catálogos
        vm.canUserAccessCatalogs = canUserAccessCatalogs;
        vm.isAnyCatalogSelected = isAnyCatalogSelected;
        vm.isCatalogSelected = isCatalogSelected;
        vm.catalogs = Constants.catalogs;
        vm.getCatalogUrl = getCatalogUrl;
        vm.getCatalogItemDetailUrl = getCatalogItemDetailUrl;
        vm.getNewCatalogItemDetail = getNewCatalogItemDetail;
        vm.getSupportUrl = getSupportUrl;
        var selectedCatalog = null;

        //Support
        vm.canUserAccessSupport = canUserAccessSupport;

        vm.isSearchSelected = false;
        vm.isreportSelected = false;
        vm.isPitCatalogSelected = false;
        vm.isCalendarSelected = false;
        vm.isDatesSelected = false;
        vm.isMessagesSelected = false;
        vm.isSupportSelected = false;

        //  vm.pitsCatalog = Constants.catalogs.pits.toLowerCase();

        vm.getRequestDetailUrl = getRequestDetailUrl;
        vm.getCalendarUrl = getCalendarUrl;
        vm.getreportes = getreportes;
        vm.getMessagesUrl = getMessagesUrl;
        vm.getmessagesByIdUrl = getmessagesByIdUrl;

        vm.isAnyCatalogSelected = isAnyCatalogSelected;
        vm.selectSearch = selectSearch;
        vm.selectReport = selectReport;
        vm.selectPitCatalog = selectPitCatalog;
        vm.selectCalendar = selectCalendar;
        vm.selectDates = selectDates;
        vm.selectMessages = selectMessages;
        vm.navigateToRequestUrl = navigateToRequestUrl;
        vm.canUseCalendar = canUseCalendar;
        vm.canUseMessages = canUseMessages;
        vm.navigateSupport = navigateSupport;

        //#endregion

        //#region Initialization
        
        $rootScope.$on("$routeChangeSuccess",
            function (event, current, previous) {
                var currentRequestUrl = current.originalPath;

                if (currentRequestUrl == vm.searchUrl || currentRequestUrl == vm.requestDetailUrl)
                    selectSearch();
                if (currentRequestUrl == vm.calendarUrl)
                    selectCalendar();
                if (currentRequestUrl == vm.datesUrl)
                    selectDates();
                if (currentRequestUrl == vm.supportUrl)
                    selectSupport();
                if (currentRequestUrl == vm.messagesUrl)
                    selectMessages();
                if (currentRequestUrl == vm.reportsUrl)
                    selectReport();
                else {
                    R.forEach(function (actualCatalog) {
                        if (currentRequestUrl == vm.getCatalogUrl(actualCatalog) || currentRequestUrl == vm.getRequestDetailUrl(actualCatalog))
                            selectCatalog(actualCatalog);
                    }, R.values(vm.catalogs));
                }

            });

        //#endregion

        //#region Functions

        function navigateToRequestUrl() {
            $location.path(Routes.search.url);
        }

        function getRequestDetailUrl(requestId, entitleId) {
            var url = common.getBaseUrl();
            var complete = url + vm.requestDetailUrl.replace(REQUEST_ID_TEMPLATE_PARAM, requestId)
                                      .replace(ENTITLE_ID_TEMPLATE_PARAM, entitleId);
            return complete;    

        }

        //function getRequestDetailUrl(requestId) {
        //    return vm.requestDetailUrl.replace(REQUEST_ID_TEMPLATE_PARAM, requestId);
        //}

        function getCalendarUrl() {
            return Routes.calendar.url;
        }
        function getreportes() {
            return Routes.reports.url;
        }

        function getMessagesUrl() {
            return Routes.messages.url;
        }
        function getmessagesByIdUrl(messageId) {
            return vm.messageDetailUrl.replace(MESSAGE_ID_TEMPLATE_PARAM, messageId);
            //   return Routes.messagesById.url;
        }

        function getCatalogUrl(catalogName) {
            return Routes.catalogElements.url.format(catalogName.toLowerCase());
        }

        function getCatalogItemDetailUrl(catalogName, catalogItemKey) {
            return Routes.catalogItemDetail.url.format(catalogName.toLowerCase()).replace(CATALOG_ITEM_KEY_TEMPLATE_PARAM, catalogItemKey);
        }

        function isAnyCatalogSelected() {
            return vm.isPitCatalogSelected;
        }

        function selectSearch() {
            deselectAllPages();

            vm.isSearchSelected = true;
        }
        function selectReport() {
            deselectAllPages();

            vm.isreportSelected = true;
        }
        function navigateSupport() {
            $location.path(Routes.support.url);
        }



        function selectPitCatalog() {
            deselectAllPages();
            vm.isPitCatalogSelected = true;
        }

        function selectCalendar() {
            deselectAllPages();

            vm.isCalendarSelected = true;
        }

        function selectDates() {
            deselectAllPages();

            vm.isDatesSelected = true;
        }

        function selectMessages() {
            deselectAllPages();

            vm.isMessagesSelected = true;
        }
        function canUseCalendar() {
            var roles = [Constants.roles.chief];
            return common.doesUserHasNecessaryRoles(roles);

        }
        //support
        function getSupportUrl() {
            return Routes.support.url;
        }

        function canUserAccessSupport() {
            return common.doesUserHasNecessaryRoles(Routes.support.roles);
        }

        function canUseMessages() {
            var roles = [Constants.roles.chief];
            return common.doesUserHasNecessaryRoles(roles);

        }
        //Catálogos
        function canUserAccessCatalogs() {
            return common.doesUserHasNecessaryRoles(Routes.catalogElements.roles);
        }
        function isAnyCatalogSelected() {
            return selectedCatalog;
        }

        function isCatalogSelected(catalog) {
            return selectedCatalog == catalog;
        }

        function selectCatalog(catalog) {
            deselectAllPages();

            selectedCatalog = catalog;
        }
        function getNewCatalogItemDetail(catalogName) {
            return Routes.catalogItemDetail.url.format(catalogName.toLowerCase()).replace(CATALOG_ITEM_KEY_TEMPLATE_PARAM, Constants.newCatalogKeyId);
        }


        function canUseReport() {
            var roles = [Constants.roles.chief];
            return true;

        }


        //#endregion

        //#region Helper functions

        function getCatalogItemDetailTemplateUrl(catalogName) {
            return Routes.catalogItemDetail.url.format(catalogName.toLowerCase());
        }

        function deselectAllPages() {
            vm.isSearchSelected = false;
            vm.isPitCatalogSelected = false;
            vm.isCalendarSelected = false;
            vm.isDatesSelected = false;
            vm.isMessagesSelected = false;
            vm.isSearchSelected = false;
            vm.isreportSelected = false;
            selectedCatalog = null;
            vm.isSupportSelected = false;
        }

        function selectCalendar() {
            deselectAllPages();
            vm.isCalendarSelected = true;
        }
        //suport

        function selectSupport() {
            deselectAllPages();

            vm.isSupportSelected = true;
        }
        //#enregion
    }
})();