
(function () {
    "use strict";

    var controllerId = "navigationController";

    angular
        .module(appName)
        .controller(controllerId, ["$rootScope", "$routeParams", "$location", "common", "appConfig", "localStorageService","homeDataService", navigationService]);

    function navigationService($rootScope, $routeParams, $location, common, appConfig, localStorageService, homeDataService) {

        //#region Constants

        var REQUEST_ID_TEMPLATE_PARAM = ":" + REQUEST_ID_PARAM  ;
        var MESSAGE_ID_TEMPLATE_PARAM = ":" + MESSAGE_ID_PARAM;

        var REQUEST_URL_LIST = [
            Routes.home.url,
            Routes.about.url,
            Routes.request.url,
            //Routes.requestData.url,
            //Routes.requestWork.url,
            //Routes.requestBeneficiaries.url,
            Routes.requestRelatives.url,

            Routes.requestFirstSuccess.url,
            Routes.requests.url,
            Routes.requestDetail.url,
            Routes.requestHistory.url,
            Routes.requestOpinionSuccess.url,
            Routes.requestOpinionFail.url,
            Routes.dates.url,
            Routes.date.url,
            Routes.noAccess.url
        ];

        //#endregion

        //#region Members

        var vm = this;
        vm.initEntitle = initEntitle;
        vm.entitleInformation = {};
        vm.homeUrl = Routes.home.url;
        vm.aboutUrl = Routes.about.url;
        vm.newRequestUrl = Routes.request.url.replace(REQUEST_ID_TEMPLATE_PARAM, null);
        vm.newMessagetUrl = Routes.request.url.replace(MESSAGE_ID_TEMPLATE_PARAM, null);


        vm.requestsUrl = Routes.requests.url;
        //vm.requestData = Routes.requestData.url;
        //vm.requestWork = Routes.requestWork.url;
        //vm.requestBeneficiaries = Routes.requestBeneficiaries.url;
        vm.requestRelatives = Routes.requestRelatives.url;

        vm.requestFirstSuccess = Routes.requestFirstSuccess;
        vm.requests = Routes.requests.url;
        vm.requestDetail = Routes.requestDetail.url;
        vm.requestHistory = Routes.requestHistory.url;
        vm.requestOpinionSuccess = Routes.requestOpinionSuccess.url;
        vm.requestOpinionFail = Routes.requestOpinionFail.url;
        vm.dates = Routes.dates.url;
        vm.date = Routes.date.url;
        vm.noAccess = Routes.noAccess.url;
        vm.isHomeSelected = false;
        vm.isAbouSelected = false;
        vm.isNewRequestSelected = false;
        vm.isRequestsSelected = false;
        vm.isOverrided = false;

        vm.selectHome = selectHome;
        vm.selectAbout = selectAbout;
        vm.selectNewRequest = selectNewRequest;
        vm.selectRequests = selectRequests;
        vm.navigateToLastRequestUrl = navigateToLastRequestUrl;
        vm.navigateToNextRequestUrl = navigateToNextRequestUrl;
        vm.navigateToRequestUrl = navigateToRequestUrl;
        //vm.navigateToRequestDataUrl = navigateToRequestDataUrl;
        //vm.navigateToRequestWorkUrl = navigateToRequestWorkUrl;
        //vm.navigateToRequestBeneficiariesUrl = navigateToRequestBeneficiariesUrl;
        vm.navigateToRequestRelativesUrl = navigateToRequestRelativesUrl;

        vm.navigateToDetailRequest = navigateToDetailRequest;
        vm.navigateToDateRequest = navigateToDateRequest;
        vm.navigateToDate = navigateToDate;
        vm.navigateToRequest = navigateToRequest;
        vm.init = init;
        vm.baseUrl = common.getBaseUrl();

        //#endregion

        //#region Fields

        var currentRequestUrl = null;
        var currentRequesId = null;

        //#endregion

        //#region Properties

        //#endregion

        //#region Initialization

        $rootScope.$on(appConfig.events.navigationMenuOverrided, function (event, data) {
            common.logger.log(Messages.info.navigationMenuOverrided, data, controllerId);
            vm.isOverrided = data.override;
        });

        $rootScope.$on(appConfig.events.changeRequestId, function (event, data) {
            common.logger.log(Messages.info.navigationMenuOverrided, data, controllerId);
            currentRequesId = data.requestId;
        });

        $rootScope.$on("$routeChangeSuccess",
            function (event, current, previous) {
                currentRequestUrl = current.originalPath;
                currentRequesId = $routeParams[REQUEST_ID_PARAM];

                if (currentRequestUrl == vm.requestDetail || currentRequestUrl == vm.dates || currentRequestUrl == vm.date)
                    deselectAllPages();
                else if (currentRequestUrl == vm.homeUrl)
                    selectHome();
                else if (currentRequestUrl == vm.aboutUrl)
                    selectAbout();
                else if (currentRequestUrl == vm.requestsUrl)
                    selectRequests();
                else
                    selectNewRequest();
            });

        //#endregion

        //#region Functions

        function init(noIssste, idpension) {
            if (localStorageService.get(common.config.ISSSTE_NUMBER_QUERY_PARAM) == null || (noIssste != null && localStorageService.get(common.config.ISSSTE_NUMBER_QUERY_PARAM) != noIssste)) {
                localStorageService.set(common.config.ISSSTE_NUMBER_QUERY_PARAM, noIssste);
                common.config.entitleInformation.NoISSSTE = noIssste;
            } else {
                common.config.entitleInformation.NoISSSTE = localStorage.get(common.config.ISSSTE_NUMBER_QUERY_PARAM);
            }
            if (localStorageService.get(common.config.ID_PENSION_QUERY_PARAM) == null || (idpension != null && localStorageService.get(common.config.ID_PENSION_QUERY_PARAM) != idpension)) {
                localStorageService.set(common.config.ID_PENSION_QUERY_PARAM, idpension);
                common.config.entitleInformation.PensionId = idpension;
            } else {
                common.config.entitleInformation.PensionId = localStorageService.get(common.config.ID_PENSION_QUERY_PARAM);
            }

            if (localStorageService.get(common.config.ID_CURP_PARAM) != null) {

                common.config.entitleInformation.CURP = localStorageService.get(common.config.ID_CURP_PARAM);
                common.config.entitleInformation.StateDescription = localStorageService.get(common.config.StateDescription);
            }

        }


        function selectHome() {
            deselectAllPages();

            vm.isHomeSelected = true;
            vm.isOverrided = true;
        }

        function selectAbout() {
            deselectAllPages();

            vm.isAbouSelected = true;
        }

        function selectNewRequest() {
            deselectAllPages();

            vm.isNewRequestSelected = true;
        }

        function selectRequests() {
            deselectAllPages();

            vm.isRequestsSelected = true;
        }

        function navigateToRequestUrl() {
            $location.path(Routes.request.url);
        }

        //function navigateToRequestDataUrl() {
        //    $location.path(Routes.requestData.url);
        //}

        //function navigateToRequestWorkUrl() {
        //    $location.path(Routes.requestWork.url);
        //}

        //function navigateToRequestBeneficiariesUrl() {
        //    $location.path(Routes.requestBeneficiaries.url);
        //}

        function navigateToRequestRelativesUrl() {
            $location.path(Routes.requestRelatives.url);
        }


        function navigateToDetailRequest(requestId) {
            $location.path(Routes.requestDetail.url.replace(REQUEST_ID_TEMPLATE_PARAM, requestId));
        }

        function navigateToMessage(messageId) {
            $location.path(Routes.messages.url.replace(MESSAGE_ID_TEMPLATE_PARAM, requestId));
        }
        function navigateToDateRequest(requestId) {
            $location.path(Routes.dates.url.replace(REQUEST_ID_TEMPLATE_PARAM, requestId));
        }

        function navigateToDate(requestId) {
            $location.path(Routes.date.url.replace(REQUEST_ID_TEMPLATE_PARAM, requestId));
        }

        function navigateToRequest() {
            $location.path(Routes.request);
        }

        function navigateToLastRequestUrl(m) {

            if (common.config.entitle.State != "F")
                m = 0;

            var resultUrl = "";

            var nextUrlIndex = null;

            for (var i = REQUEST_URL_LIST.length - 1; i >= 0; i--) {
                if (currentRequestUrl == REQUEST_URL_LIST[i])
                    nextUrlIndex = i - 1 - m;
                if (nextUrlIndex == 0)
                    nextUrlIndex = 2;
            }

            if (nextUrlIndex != null && nextUrlIndex >= 0) {
                resultUrl = REQUEST_URL_LIST[nextUrlIndex].replace(REQUEST_ID_TEMPLATE_PARAM, currentRequesId);

                $location.path(resultUrl);
            }
        }

        function navigateToNextRequestUrl(m) {
            var resultUrl = "";

            var nextUrlIndex = null;

            for (var i = 0; i < REQUEST_URL_LIST.length; i++) {
                if (currentRequestUrl == REQUEST_URL_LIST[i])
                    nextUrlIndex = i + 1 + m;
            }

            if (nextUrlIndex != null && nextUrlIndex < REQUEST_URL_LIST.length) {
                resultUrl = REQUEST_URL_LIST[nextUrlIndex].replace(REQUEST_ID_TEMPLATE_PARAM, currentRequesId);

                $location.path(resultUrl);
            }
        }


        //#endregion

        //#region Helper functions

        function deselectAllPages() {
            vm.isHomeSelected = false;
            vm.isAbouSelected = false;
            vm.isNewRequestSelected = false;
            vm.isRequestsSelected = false;

            vm.isOverrided = false;
        }

        function initEntitle() {
            $('[data-toggle="tooltip"]').tooltip();

            $('#home').addClass("active");
            $('#request').addClass("hidden");
            $('#infoGeneral').addClass("hidden");
           
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
          
                common.overrideNavigationMenu(false);
               


            });
                

        }

        function getEntitleInformation() {
            return homeDataService.getEntitleInformation(common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    common.config.entitle = data;


                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.getEntitleInformation);
                     $("html, body").animate({ scrollTop: 700 }, 2000);
                });
        }


        //#enregion
    }
})();