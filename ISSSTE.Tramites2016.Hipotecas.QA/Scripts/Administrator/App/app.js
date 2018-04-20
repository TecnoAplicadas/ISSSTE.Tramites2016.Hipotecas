//Variable global con el nombre del módulo
var appName = "ISSSTE.Tramites2016.Hipotecas.Administrator.App";

(function () {
    "use strict";

    //Se crea la aplicación
    var app = angular.module(appName, [
        //Inyección de módulos angular
        "ngRoute", //módulo para ruteo de la aplicación
        "ngSanitize", //Corrige hallazgos HTML en el bindeo
        //Inyección de módulos de la aplicación
        "common",
        "LocalStorageModule",
        "jcs-autoValidate",
        "ngFileUpload",
        "datetimepicker"
    ]);

    //Se obtiene una referncia al proveedor de rutas
    app.config([
        "$routeProvider", function ($routeProvider, routes) {
            $routeProviderReference = $routeProvider;
        }
    ]);

    //código de arranque de la aplicación
    app.run(["$route", "$window", "$rootScope", "common", "authenticationService", "webApiService", "defaultErrorMessageResolver", startup]);

    //#region Constants

    //#endregion

    //#region Fields

    var $rootScopeReference;
    var $routeProviderReference;
    var $routeReference;
    var $windowReference;
    var commonReference;
    var authenticationServiceReference;
    var webApiServiceReference;

    //#endregion

    //#region Información del usuario

    function startup($route, $window, $rootScope, common, authenticationService, webApiService, defaultErrorMessageResolver) {
        //Asignación de objetos injectados para su utilización despues
        $rootScopeReference = $rootScope;
        $routeReference = $route;
        $windowReference = $window;
        commonReference = common;
        authenticationServiceReference = authenticationService;
        webApiServiceReference = webApiService;

        //So configura el resolutor de errores
        defaultErrorMessageResolver.getErrorMessages().then(function (errorMessages) {
            errorMessages["required"] = Messages.validation.required;
            errorMessages["email"] = Messages.validation.email;
            errorMessages["number"] = Messages.validation.numbers;
            errorMessages["minlength"] = Messages.validation.minLenght;
            errorMessages["rfc"] = Messages.validation.rfc;
            errorMessages["curp"] = Messages.validation.curp;
        });

        debugger;
        //Si no se encuentra en alguna de las páginas de login o logout...
        if ($windowReference.location.pathname.toLocaleLowerCase().indexOf("account", 0) == -1) {
            
            //Se obtienen los roles del usuario logueado
            getUserRoles()
            .finally(function () {
                //Se inzializan las rutas las rutas
                routeConfigurator();

                //Se forza un reprocesamiento de la ruta actual para que se apliquen las validaciones y se obtenga el html correspondiente
                $routeReference.reload();
            });

            //MFP 03/01/2017 :

                //.finally(function () {
                //    //Se inzializan las rutas las rutas
                //    routeConfigurator();

                //    //Se forza un reprocesamiento de la ruta actual para que se apliquen las validaciones y se obtenga el html correspondiente
                //    $routeReference.reload();
                //});
        }
    }

    //#endregion

    //#region Rutas

    function routeConfigurator() {
        var routes = getRoutes();

        for (var i = 0; i < routes.length; i++) {
            $routeProviderReference.when(routes[i].url, routes[i].config);
        }

        //MFP -> Para no colocar uan ruta por default
        //$routeProviderReference.otherwise({ redirectTo: Routes.search.url });
    }

    function getRoutes() {
        var routes = [
              {
                  url: Routes.appointments.url,
                config: {
                    templateUrl: commonReference.getBaseUrl() + Routes.appointments.templateUrl,
                    resolve: {
                        "check": ['$location', function ($location) {
                            validateUrlAccess($location, Routes.appointments.roles);
                        }]
                    }
                }
            },
            {
                url: Routes.search.url,
                config: {
                    templateUrl: commonReference.getBaseUrl() + Routes.search.templateUrl,
                    resolve: {
                        "check": ['$location', function ($location) {
                            validateUrlAccess($location, Routes.search.roles);
                        }]
                    }
                }
            },
             {
                 url: Routes.support.url,
                 config: {
                     templateUrl: commonReference.getBaseUrl() + Routes.support.templateUrl,
                     resolve: {
                         "check": ['$location', function ($location) {
                             validateUrlAccess($location, Routes.support.roles);
                         }]
                     }
                 }
             },
            {
                url: Routes.requestDetail.url,
                config: {
                    templateUrl: commonReference.getBaseUrl() + Routes.requestDetail.templateUrl,
                    resolve: {
                        "check": ['$location', function ($location) {
                            validateUrlAccess($location, Routes.requestDetail.roles);
                        }]
                    }
                }
            },
              {
                  url: Routes.dates.url,
                  config: {
                      templateUrl: commonReference.getBaseUrl() + Routes.dates.templateUrl,
                      resolve: {
                          "check": ['$location', function ($location) {
                              validateUrlAccess($location, Routes.dates.roles);
                          }]
                      }
                  }
              },
              {
                  url: Routes.calendar.url,
                  config: {
                      templateUrl: commonReference.getBaseUrl() + Routes.calendar.templateUrl,
                      resolve: {
                          "check": ['$location', function ($location) {
                              validateUrlAccess($location, Routes.calendar.roles);
                          }]
                      }
                  }
              },
               {
                   url: Routes.messages.url,
                   config: {
                       templateUrl: commonReference.getBaseUrl() + Routes.messages.templateUrl,
                       resolve: {
                           "check": ['$location', function ($location) {
                               validateUrlAccess($location, Routes.messages.roles);
                           }]
                       }
                   }
               },
                {
                    url: Routes.messagesById.url,
                    config: {
                        templateUrl: commonReference.getBaseUrl() + Routes.messagesById.templateUrl,
                        resolve: {
                            "check": ['$location', function ($location) {
                                validateUrlAccess($location, Routes.messages.roles);
                            }]
                        }
                    }
                },
                 {
                     url: Routes.reports.url,
                     config: {
                         templateUrl: commonReference.getBaseUrl() + Routes.reports.templateUrl,
                         resolve: {
                             "check": ['$location', function ($location) {
                                 validateUrlAccess($location, Routes.reports.roles);
                             }]
                         }
                     }
                 },
                 {
                     url: Routes.requestDetail_RevisionStatus.url,
                     config: {
                         templateUrl: commonReference.getBaseUrl() + Routes.requestDetail_RevisionStatus.templateUrl,
                         resolve: {
                             "check": ['$location', function ($location) {
                                 validateUrlAccess($location, Routes.requestDetail_RevisionStatus.roles);
                             }]
                         }
                     }
                 },
            {
                url: Routes.noAccess.url,
                config: {
                    templateUrl: commonReference.getBaseUrl() + Routes.noAccess.templateUrl
                }
            }
        ];
        //Catálogos 
        R.forEach(function (actualCatalog) {
            var detailTemplateUrl = Routes.catalogItemDetail.templateUrl.format(actualCatalog);

            routes.push({
                url: Routes.catalogElements.url.format(actualCatalog.toLowerCase()),
                config: {
                    templateUrl: commonReference.getBaseUrl() + Routes.catalogElements.templateUrl.format(actualCatalog),
                    resolve: {
                        "check": ['$location', function ($location) {
                            validateUrlAccess($location, Routes.catalogElements.roles);
                        }]
                    }
                }
            },
                {
                    url: Routes.catalogItemDetail.url.format(actualCatalog.toLowerCase()),
                    config: {
                        templateUrl: commonReference.getBaseUrl() + detailTemplateUrl,
                        resolve: {
                            "check": ['$location', function ($location) {
                                validateUrlAccess($location, Routes.catalogItemDetail.roles);
                            }]
                        }
                    }
                })
        }, R.values(Constants.catalogs));


        return routes;
    }

    //#endregion

    //#region UI

    //#endregion

    //#region Helper Functions

    function getUserRoles() {
        
        return webApiServiceReference.makeRetryRequest(1, function () {
            return authenticationServiceReference.getUserRoles();
        })
            .then(function (data) {
                commonReference.config.userRoles = data;
            })
            .catch(function (reason) {
                commonReference.showErrorMessage(reason, Messages.error.userRoles);
            });
    }

    function validateUrlAccess($location, necessaryRoles) {
        //Si caduco la sesión (token) y no se puede renovar, se enviara a la pantalla de logout
        webApiServiceReference.makeRetryRequest(1, function () {
            return authenticationServiceReference.validateToken();
        });

        if (!commonReference.doesUserHasNecessaryRoles(necessaryRoles))
            $windowReference.location.href = Constants.accountRoutes.login;
    }

    //#endregion

})();
