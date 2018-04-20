﻿//Variable global con el nombre del módulo
var appName = "ISSSTE.Tramites2016.Hipotecas.Entitle.App";

(function () {
    "use strict";

    //Se crea la aplicación
    var app = angular.module(appName, [
        //Inyección de módulos angular
        "ngRoute", //módulo para ruteo de la aplicación
        "ngSanitize", //Corrige hallazgos HTML en el bindeo
        //Inyección de módulos de la aplicación
        "common",
        "ngFileUpload",
        "jcs-autoValidate",
        "ngFileUpload",
        "datetimepicker",
        "LocalStorageModule"
    ]);

    //Se obtiene una referncia al proveedor de rutas
    app.config([
        "$routeProvider", function ($routeProvider, routes) {
            $routeProviderReference = $routeProvider;
            //localStorageServiceProvider.setPrefix(appName);
        }
    ]);

    //código de arranque de la aplicación
    app.run(["$route", "$window", "$rootScope", "common", "defaultErrorMessageResolver", "localStorageService", startup]);

    //#region Constants

    var ISSSTE_NUMBER_QUERY_PARAM = "noissste";
    var ID_PENSION_QUERY_PARAM = "pensionId";

    var ID_REQUEST_QUERY_PARAM = "requestId";

    var ID_CURP_PARAM = "curp";

    //#endregion

    //#region Fields

    var $rootScopeReference;
    var $routeProviderReference;
    var $routeReference;
    var $windowReference;
    var commonReference;
    var localStorage;

    //#endregion

    //#region Información del usuario

    function startup($route, $window, $rootScope, common, defaultErrorMessageResolver, localStorageService) {
        //Asignación de objetos injectados para su utilización despues
        $rootScopeReference = $rootScope;
        $routeReference = $route;
        $windowReference = $window;
        commonReference = common;
        localStorage = localStorageService;

        //So configura el resolutor de errores
        defaultErrorMessageResolver.getErrorMessages().then(function (errorMessages) {
            errorMessages["required"] = Messages.validation.required;
            errorMessages["email"] = Messages.validation.email;
            errorMessages["number"] = Messages.validation.numbers;
            errorMessages["minlength"] = Messages.validation.minLenght;
            errorMessages["rfc"] = Messages.validation.rfc;
            errorMessages["curp"] = Messages.validation.curp;
        });

        var isssteNumber = Utils.getQueryStringValue(ISSSTE_NUMBER_QUERY_PARAM);
        var pensionId = Utils.getQueryStringValue(ID_PENSION_QUERY_PARAM);

        if (localStorage.get(commonReference.config.ISSSTE_NUMBER_QUERY_PARAM) == null || (isssteNumber != null && localStorage.get(commonReference.config.ISSSTE_NUMBER_QUERY_PARAM) != isssteNumber)) {
            localStorage.set(commonReference.config.ISSSTE_NUMBER_QUERY_PARAM, isssteNumber);
            commonReference.config.entitleInformation.NoISSSTE = isssteNumber;
        } else {
            commonReference.config.entitleInformation.NoISSSTE = localStorage.get(commonReference.config.ISSSTE_NUMBER_QUERY_PARAM);
        }
        if (localStorage.get(commonReference.config.ID_PENSION_QUERY_PARAM) == null || (pensionId != null && localStorage.get(commonReference.config.ID_PENSION_QUERY_PARAM) != pensionId)) {
            localStorage.set(commonReference.config.ID_PENSION_QUERY_PARAM, pensionId);
            commonReference.config.entitleInformation.PensionId = pensionId;
        } else {
            commonReference.config.entitleInformation.PensionId = localStorage.get(commonReference.config.ID_PENSION_QUERY_PARAM);
        }

        if (localStorage.get(common.config.ID_CURP_PARAM) != null) {

            commonReference.config.entitleInformation.CURP = localStorage.get(common.config.ID_CURP_PARAM);
        }


        //Se inzializan las rutas las rutas
        //routeConfigurator(); //Se forza un reprocesamiento de la ruta actual
        $routeReference.reload();
    }

    //#endregion

    //#region Rutas

    //function routeConfigurator(localStorageService) {
    //    var routes = getRoutes();

    //    for (var i = 0; i < routes.length; i++) {
    //        $routeProviderReference.when(routes[i].url, routes[i].config);
    //    }

    //    //Comento MFP 10-01-2017 para evitar redireccionamiento a la pagina de home
    //    //$routeProviderReference.otherwise({ redirectTo: Routes.home.url });

    //}

    function getRoutes() {
        return [
            {
            //    url: Routes.home.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.home.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.about.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.about.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.request.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.request.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestData.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestData.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            // {
            //     url: Routes.requestWork.url,
            //     config: {
            //         templateUrl: commonReference.getBaseUrl() + Routes.requestWork.templateUrl,
            //         resolve: {
            //             "check": ['$location', function ($location) {
            //                 validateUrlAccess($location);
            //             }]
            //         }
            //     }
            // },
            //{
            //    url: Routes.requestBeneficiaries.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestBeneficiaries.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestRelatives.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestRelatives.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestFirstSuccess.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestFirstSuccess.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requests.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requests.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestDetail.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestDetail.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestHistory.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestHistory.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestOpinionSuccess.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestOpinionSuccess.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.requestOpinionFail.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.requestOpinionFail.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.dates.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.dates.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.date.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.date.templateUrl,
            //        resolve: {
            //            "check": ['$location', function ($location) {
            //                validateUrlAccess($location);
            //            }]
            //        }
            //    }
            //},
            //{
            //    url: Routes.noAccess.url,
            //    config: {
            //        templateUrl: commonReference.getBaseUrl() + Routes.noAccess.templateUrl
            //    }
            }
        ];
    }

    //#endregion

    //#region UI

    //#endregion

    //#region Helper Functions

    function validateUrlAccess($location) {
        //if (commonReference.config.entitleInformation.NoISSSTE == undefined || commonReference.config.entitleInformation.NoISSSTE == "")
        if (localStorage.get(commonReference.config.ISSSTE_NUMBER_QUERY_PARAM) == undefined || localStorage.get(commonReference.config.ISSSTE_NUMBER_QUERY_PARAM) == "")
            $location.path(Routes.noAccess.url);
    }

    //#endregion

})();