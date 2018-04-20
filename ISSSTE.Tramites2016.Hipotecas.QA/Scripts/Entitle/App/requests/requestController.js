//####################################################################
//      ## Fecha de creación: 18-03-2016
//      ## Fecha de última modificación: 30-03-2016
//      ## Responsable: Emanuel De la Isla Vértiz
//      ## Módulos asociados: Información general, Deudos, Beneficiarios, Historial Laboral.
//      ## Id Tickets asociados al cambio: R-013042
//####################################################################
(function () {
    "use strict";
    var controllerId = "requestController";

    angular
        .module(appName)
        .controller(controllerId, ["$routeParams", "common", "homeDataService", "requestDataService", requestController]);

    function requestController($routeParams, common, homeDataService, requestDataService) {
        //#region Controller Members
        var vm = this;
        vm.error = false;
        vm.disabledUploadDocuments = false;
        vm.documentsRequired = [];
        vm.formValid = false;
        vm.information = {};
        vm.requestInf = {};
        vm.requests = {};
        vm.pastrequests = {};
        vm.requestAll = {};
        vm.init = init;
        vm.initRequest = initRequest;
        vm.initData = initData;
        vm.nextRegistrar = nextRegistrar;
        vm.validateData = validateData;
        vm.validateBeneficiaries = validateBeneficiaries;
        vm.validateDebtors = validateDebtors;
        //vm.validateDebtorsByCURP = validateDebtorsByCURP;
        vm.validateCurp = validateCurp;
        vm.validateWorkHistory = validateWorkHistory;
        vm.isFormValid = isFormValid;
        vm.initFinalData = initFinalData;
        vm.getRequests = getRequests;
        vm.getPastRequests = getPastRequests;
        vm.initRequests = initRequests;
        vm.initDetail = initDetail;
        vm.getAllSavedEntitleInformation = getAllSavedEntitleInformation;
        vm.getRequestAll = getRequestAll;
        vm.getCurrentRequests = getCurrentRequests;
        vm.countRequest = {};
        vm.countErrors = {};
        vm.countWarningsHistorial = {};
        vm.initDocuments = initDocuments;
        // vm.validDebtorPension = validDebtorPension;
        // vm.validBeneficiariePension = validBeneficiariePension;
        //vm.validLaboralPension = validLaboralPension;

        vm.inValidDocuments = false;
        vm.Property = [];
        vm.Urban = [];
        vm.Owner = [];
        vm.lada;
        vm.ApplicationStatus;
        vm.requestInf.ApplicationMessages = {};
        vm.pTiempoCotizado = null;
        vm.pTiempoLicenciasliquidar = null;
        vm.pTotalTiempoAcumulado = null;
        vm.telefono;
        vm.toggleEnableNewDebtor = toggleEnableNewDebtor;
        vm.informationDebtor = {};
        vm.informationDebtor2 = {};
        vm.toggleAddEnableNewDebtor = toggleAddEnableNewDebtor;
        vm.validDebtors = validDebtors;
        vm.saveDocuments = saveDocuments;

        /* NUEVOS CONTROLES PARA AGREGAR ITEMS */
        vm.addItem = addItem;
        vm.deleteDebtors = deleteDebtors;
        vm.toggleChecked = toggleChecked; // VALIDA LA INFORMACIÓN DEL DEUDO
        vm.viewAddEnableNewDebtor = viewAddEnableNewDebtor;


        vm.listaDocumentosSolicitud = {};
        vm.getDocumentsByRelationship = getDocumentsByRelationship;
        // vm.getModalidadPension = getModalidadPension;
        vm.modalidadPension = {};

        vm.searchRequestDebtorsByCURP = searchRequestDebtorsByCURP;
        vm.requestDebtors = null;
        vm.getRequestDebtors = getRequestDebtors;

        vm.getRequestDebtorsByRequestId = getRequestDebtorsByRequestId;
        //vm.getOptions = getTypeProperty;
        vm.urbancenter = GetUrbanCenter;
        vm.inValidDocument = inValidDocument;
        vm.showMessages = showMessages;
        vm.countMessages = 0;

        function showMessages(message) {
            //if (vm.countMessages < 1) {
            //    common.showErrorMessage(message);
            //}
            //vm.countMessages++;
        }


        function inValidDocument() {
            //common.displayLoadingScreen();
            vm.documentsRequired = [];
            if (!vm.disabledUploadDocuments) {
                vm.inValidDocuments = false;
                var contador = 0;

                vm.documentsRequired.forEach(function (index) {

                    var value = document.getElementById("document" + index).value;
                    vm.documentsRequired.push({ value: true });
                    if (value == "") {
                        vm.documentsRequired[index].value = false
                        event.preventDefault();
                        vm.inValidDocuments = true;
                    }
                });

                //if (!vm.inValidDocuments) {
                //    //common.showErrorMessage('Te falta subir algún documento.', "¡Atención!");                    
                //    //$("html, body").animate({ scrollTop: 1 }, 1000);
                //    //common.hideLoadingScreen();
                //}
            }

        }



        //#endregion
        //#region Constants

        //#endregion

        //#region Fields


        //#endregion

        //#region Properties


        //#endregion        

        //#region Public Functions



        function saveDocuments(requestId) {
            var promises = [];
            vm.successfullFiles = 0;

            vm.documentsTemplate.forEach(function (actualDocument, index) {
                if (actualDocument.File != null) {
                    var documentPromise = requestDataService.uploadDocument(common.config.entitleInformation.CURP, actualDocument.File, requestId, actualDocument.DocumentTypeId)
                   .then(function () {
                       vm.successfullFiles++;
                       common.hideLoadingScreen();
                   })
                   .catch(function (data) {
                       common.showErrorMessage(data, Messages.error.fileUpload);
                       common.hideLoadingScreen();
                       $("html, body").animate({ scrollTop: 1 }, 2000);
                   });
                    promises.push(documentPromise);
                }
            });

            return common.$q.all(promises);
        }

        function isFormValid(form, telephone, email) {
            if (vm.information.Entitle.Telephone != null) {
                vm.information.Entitle.Telephone = vm.lada + vm.information.Entitle.Telephone;
            }
            //telephone.$valid.removeClass('has-success has-feedback');
            if (telephone.$valid) {
                $('#divCurp').removeClass("has-success has-feedback ");
                $('#spantel').removeClass("form-text-error");
                $('#inpttel').removeClass("form-control-error");

            }

            if (email.$valid) {
                $('#divCurp').removeClass("has-success has-feedback ");
                $('#spanemail').removeClass("form-text-error");
                $('#inptemail').removeClass("form-control-error");
            }
            return telephone.$valid && email.$valid;
            //else
            //    return form.$valid;
        }


        function isFormValidCurp() {
            return form.$valid;
        }


        function init() {
            completeControllerInit();
            getDocumentsForInfo();
            initRequest();
            if (vm.countErrors == 0) {
                common.showErrorMessage('Te falta subir algún documento.', "¡Atención!");
                $("html, body").animate({ scrollTop: 1 }, 1000);
                common.hideLoadingScreen();
            }

            var url = window.location.toString().split('?');
            var paramsPart = url[1];
            var params = paramsPart.split('&');

            var paramValues = new Array(3);
            var x = 0;

            for (x = 0; x < params.length; x++) {
                var param = params[x].split('=');
                paramValues[x] = param[1];
            }

            if (paramValues[2] != null) {
                if (paramValues[2] == '1') {
                    common.showErrorMessage('Algún documento ingresado es mayor al tamaño permitido.', "¡Atención!");
                    $("html, body").animate({ scrollTop: 1000 }, 2000);
                }

                if (paramValues[2] == '2') {
                    common.showErrorMessage('Algún documento ingresado no es del formato correcto.', "¡Atención!");
                    $("html, body").animate({ scrollTop: 1000 }, 2000);
                }

                if (paramValues[2] == '3') {
                    common.showErrorMessage('Te falta subir algún documento.', "¡Atención!");
                    $("html, body").animate({ scrollTop: 1000 }, 2000);
                }

              
            }


            
            // initData();
        }

        function initRequest() {
            $('#request').removeClass("hidden");
            $('#request').addClass("active");

            $('#infoGeneral').addClass("hidden");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hidden");
            $('#appoiments').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");

            vm.information = {};
            common.config.information = {};
            vm.requestInf = {};
            common.config.requestInf = {};
            var dataPromise = null;
            dataPromise = getAllEntitleInformation();
            // var promise = getCurrentRequests();
            dataPromise.success(function (data, status, headers, config) {
                vm.information = data;
                //  promise.success(function () {
            //    getTypeProperty(vm.information.Entitle.CURP);
                // completeControllerInit();
                common.overrideNavigationMenu(false);
                if (vm.information.ErrorMesage != null) {
                    common.showErrorMessage(vm.information.ErrorMesage);
                    vm.countErrors = 1;

                    //   }
                    //}).error(function () {

                    //});
                }
                common.hideLoadingScreen();
            })
                .error(function (data, status, headers, config) {
                    //completeControllerInit();
                    //common.overrideNavigationMenu(true);     
                    common.hideLoadingScreen();
                    common.showErrorMessage(Messages.error.getAllEntitleInformation);
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });

        }

        function initData() {
            $('#request').removeClass("hidden");
            $('#request').addClass("inactive");
            $('#infoGeneral').removeClass("hidden");
            $('#infoGeneral').addClass("active");


            $('#finish').addClass("hidden");
            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hidden");
            $('#appoiments').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");

            //vm.init();
            vm.information = common.config.information;


            vm.requestInf = common.config.requestInf;
            completeControllerInit();
            common.overrideNavigationMenu(false);

            if (vm.information.ErrorMesage != null) {
                common.showErrorMessage(vm.information.ErrorMesage);
                $("html, body").animate({ scrollTop: 1 }, 2000);

            }


            if (vm.information.InconsistenciesLHMessages != null) {
                common.showWarningMessage(vm.information.InconsistenciesLHMessages);
                vm.countWarningsHistorial = 1;
                vm.information.Validation.IsGeneralDataCorrect = false;
                vm.information.Validation.IsWorkHistoryCorrect = false;
            }

            if (vm.information.Entitle.State == "F") {
                vm.information.Entitle.Age = null;
            }

            vm.pTiempoCotizado = vm.information.totalContributionTime;
            vm.pTiempoLicenciasliquidar = vm.information.totalLicensesTime;
            vm.pTotalTiempoAcumulado = vm.information.totalTime;


        }

        function initFinalData() {
            $('#request').removeClass("hidden");
            $('#request').addClass("inactive");
            $('#infoGeneral').removeClass("hidden");
            $('#infoGeneral').addClass("inactive");
            $('#finish').removeClass("hidden");
            $('#finish').addClass("active");

            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#requests').addClass("hidden");
            $('#detailRequest').addClass("hidden");
            $('#appoiments').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");



            //vm.init();
            vm.information = common.config.information;
            vm.requestInf = common.config.requestInf;
            if (vm.information.MessageInfoRO != null) {
                if (vm.information.MessageInfoRO != "") {

                    vm.information.MessageInfoRO = common.config.information.MessageInfoRO;
                    common.showInfoMessage(vm.information.MessageInfoRO);
                }
            }
            //if (vm.information.Validation.IsBeneficiaries === true && vm.information.Validation.IsCurpCorrect === true &&
            //    vm.information.Validation.IsDebtorsCorrect === true && vm.information.Validation.IsGeneralDataCorrect === true &&
            //    vm.information.Validation.IsWorkHistoryCorrect === true)
            if (vm.information.Entitle.State == "F" && vm.information.Entitle.DirectType == "P") {
                if (vm.information.Validation.IsDebtorsCorrect === true) {
                    vm.ApplicationStatus = "Registro finalizado";
                    vm.error = false;
                }
                else {
                    vm.ApplicationStatus = "Solicitud con inconsistencias";
                    if (vm.information.InconsistenciesLHMessages != null) {
                        if (vm.information.InconsistenciesLHMessages != "") {
                            vm.requestInf.ApplicationMessages = vm.information.InconsistenciesLHMessages + '\n';
                        }
                    }
                    if (vm.information.ErrorMesage != null)
                        vm.requestInf.ApplicationMessages += vm.information.ErrorMesage;
                    vm.error = true;
                }
                $('#wizardfinal').addClass("debtor");
                $('#wizardfinal').removeClass("Nodebtor");
            }
            if (vm.information.Entitle.State == "F" && vm.information.Entitle.DirectType == "T") {
                if (vm.information.Validation.IsCurpCorrect === true &&
              vm.information.Validation.IsDebtorsCorrect === true && vm.information.Validation.IsWorkHistoryCorrect) {
                    vm.ApplicationStatus = "Registro finalizado";
                    vm.error = false;
                }
                else {
                    vm.ApplicationStatus = "Solicitud con inconsistencias";
                    if (vm.information.InconsistenciesLHMessages != null) {
                        if (vm.information.InconsistenciesLHMessages != "") {
                            vm.requestInf.ApplicationMessages = vm.information.InconsistenciesLHMessages + '\n';
                        }
                    }
                    if (vm.information.ErrorMesage != null)
                        vm.requestInf.ApplicationMessages += vm.information.ErrorMesage;
                    vm.error = true;
                }
                $('#wizardfinal').removeClass("Nodebtor");
                $('#wizardfinal').addClass("debtor");

            }
            else if (vm.information.Entitle.State != "F") {
                if (vm.information.Validation.IsCurpCorrect === true &&
                   vm.information.Validation.IsWorkHistoryCorrect === true) {
                    vm.ApplicationStatus = "Registro finalizado";
                    vm.error = false;
                }
                else {
                    vm.ApplicationStatus = "Solicitud con inconsistencias";
                    if (vm.information.InconsistenciesLHMessages != null) {
                        if (vm.information.InconsistenciesLHMessages != "") {
                            vm.requestInf.ApplicationMessages = vm.information.InconsistenciesLHMessages + '\n';
                        }
                    }
                    if (vm.information.ErrorMesage != null)
                        vm.requestInf.ApplicationMessages += vm.information.ErrorMesage;
                    vm.error = true;
                }
                $('#wizardfinal').addClass("Nodebtor");
                $('#wizardfinal').removeClass("debtor");
            }

            getDocumentsByRelationship();
            completeControllerInit();
            common.overrideNavigationMenu(false);


        }

        /* FUNCIÓN PARA EXTRAER LA LISTA DE DOCUMENTOS */
        vm.itemsKeyRelationship = [];
        vm.newListKeyDebtors = [];
        function getDocumentsByRelationship() {
            var data = null;
            var pension = common.config.information.PensionId;


            if (pension == 1 || pension == 2 || pension == 3 || pension == 6 || pension == 7 || pension == 8) {
                return requestDataService.getDocumentsByRelationship(pension, 0)
                .success(function (data, status, headers, config) {
                    vm.itemsKeyRelationship.push(0);
                    vm.listaDocumentosSolicitud = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
            } else {
                for (var x = 0; x <= common.config.information.Debtors.length - 1; x++) {
                    vm.itemsKeyRelationship.push(common.config.information.Debtors[x].KeyRelationship);
                }
                vm.itemsKeyRelationship = eliminateDuplicates(vm.itemsKeyRelationship);
                return requestDataService.getDocumentsByRelationship(pension, vm.itemsKeyRelationship)
                .success(function (data, status, headers, config) {
                    vm.listaDocumentosSolicitud = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
            }
        }

        function eliminateDuplicates(arr) {
            var i, len = arr.length, out = [], obj = {};
            for (i = 0; i < len; i++) {
                obj[arr[i]] = 0;
            }
            for (i in obj) {
                out.push(i);
            } return out;
        }

        function nextRegistrar(navigate) {
            common.config.information = vm.information;
            navigate();
        };

        function validateData(navigate) {
            common.config.information = vm.information;
            if (vm.information.Entitle.State == "F") {
                navigate(2);
            }
            else {
                navigate(0);
            }
            common.config.information.Validation.IsGeneralDataCorrect = true;
        };

        function validateBeneficiaries(navigate) {
            common.config.information = vm.information;
            navigate();
        };


        function initDocuments(requestId) {
            vm.requestInfo.Documents.forEach(function (actualDocument, index) {
                actualDocument.DownloadUrl = requestsDataService.getRequestDocumentDownloadUrl(requestId, actualDocument.DocumentTypeId);
            });
        }


        // Función para almacenar la solicitud del Deudo.
        function saveRequestDebtor() {
            var accion = requestDataService.saveRequest(vm.requestInf, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    common.config.requestInf = data;
                    vm.requestInf = data;
                    common.config.information.Folio = data.Folio;
                    common.config.information.RequestId = data.RequestId;
                    vm.information.RequestId = data.RequestId;
                    vm.information.Folio = data.Folio;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.hideLoadingScreen();
                    common.showErrorMessage(Messages.error.saveRequest);
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
            alert(common.showErrorMessage(Messages.error.saveRequest));
        }


        // Función para el botón "Agregar deudo a la lista - validar_deudos.html"
        vm.items = [];
        vm.checked = [];
        function addItem() {
            vm.items.push({
                CURP: vm.nuevocurp
            });
            vm.addNewDebtor = false;

            return requestDataService.getCurpRequests(vm.nuevocurp)
            .success(function (data, status, headers, config) {
                vm.countCurpRequest = 0;
                if (data == 0) {
                    vm.curpR2 = true;
                    vm.curpR = true;
                } else {
                    vm.curpR2 = false;
                    vm.curpR = false;
                    document.getElementById("DebtorComplete2").style.display = "Block";
                    document.getElementById("MoreDebtor").style.display = "none";
                    document.getElementById("NoDebtorsFound").style.display = "none";

                }
                common.hideLoadingScreen();
            })
         .error(function (data, status, headers, config) {
             common.showErrorMessage(Messages.error.validateCurp);
             common.hideLoadingScreen();
             $("html, body").animate({ scrollTop: 1 }, 2000);
         });
        }

        // Función del botón "Validar información del Deudo - validar_deudos.html"
        function toggleChecked(index) {
            var deudo1 = vm.informationDebtor[0].CURP;
            var deudoX = vm.items[0].CURP;
            var getPosition = -1;
            var isDebtors = false;
            if (deudo1 != deudoX) {
                if (vm.checked.length == 0) { // Valida si no existen CURPS validadas
                    for (var x = 0; x <= common.config.information.Debtors.length - 1; x++) {
                        if (vm.items[0].CURP == common.config.information.Debtors[x].CURP) {
                            if (common.config.information.Debtors[x].Age < 18 && (common.config.information.Debtors[x].KeyRelationship == 70 || common.config.information.Debtors[x].KeyRelationship == 80)) {
                                vm.checked.push({ // AGREGA LA INFORMACIÓN A LA VISTA
                                    Age: common.config.information.Debtors[x].Age,
                                    Birthplace: common.config.information.Debtors[x].Birthplace,
                                    CURP: common.config.information.Debtors[x].CURP,
                                    DebtorId: common.config.information.Debtors[x].DebtorId,
                                    MaternalLastName: common.config.information.Debtors[x].MaternalLastName,
                                    Name: common.config.information.Debtors[x].Name,
                                    PaternalLastName: common.config.information.Debtors[x].PaternalLastName,
                                    Relationship: common.config.information.Debtors[x].Relationship,
                                    Request: common.config.information.Debtors[x].Request,
                                    RequestId: common.config.information.Debtors[x].RequestId,
                                    KeyRelationship: common.config.information.Debtors[x].KeyRelationship
                                });
                                document.getElementById("MoreDebtor").style.display = "none";
                                document.getElementById("moreOldDebtor").style.display = "none";
                                isDebtors = true;
                            } else if (common.config.information.Debtors[x].KeyRelationship == 70 || common.config.information.Debtors[x].KeyRelationship == 80 && (common.config.information.Debtors[x].Age >= 18 && common.config.information.Debtors[x].Age <= 25)) {
                                document.getElementById("moreOldDebtor").style.display = "block";
                                document.getElementById("MoreDebtor").style.display = "none";
                                isDebtors = true;
                            } else if (common.config.information.Debtors[x].KeyRelationship != 70 || common.config.information.Debtors[x].KeyRelationship != 80) {
                                vm.checked.push({ // AGREGA LA INFORMACIÓN A LA VISTA
                                    Age: common.config.information.Debtors[x].Age,
                                    Birthplace: common.config.information.Debtors[x].Birthplace,
                                    CURP: common.config.information.Debtors[x].CURP,
                                    DebtorId: common.config.information.Debtors[x].DebtorId,
                                    MaternalLastName: common.config.information.Debtors[x].MaternalLastName,
                                    Name: common.config.information.Debtors[x].Name,
                                    PaternalLastName: common.config.information.Debtors[x].PaternalLastName,
                                    Relationship: common.config.information.Debtors[x].Relationship,
                                    Request: common.config.information.Debtors[x].Request,
                                    RequestId: common.config.information.Debtors[x].RequestId,
                                    KeyRelationship: common.config.information.Debtors[x].KeyRelationship
                                });
                                document.getElementById("MoreDebtor").style.display = "none";
                                document.getElementById("moreOldDebtor").style.display = "none";
                                isDebtors = true;
                            } else {
                                document.getElementById("moreOldDebtor").style.display = "block";
                                document.getElementById("MoreDebtor").style.display = "none";
                                isDebtors = true;
                            }
                        }
                    }
                } else if (vm.checked.length > 0) { // SI EXISTEN CURPS VALIDADAS
                    for (var y = 0; y <= vm.checked.length - 1; y++) {
                        if (deudoX == vm.checked[y].CURP) {
                            getPosition = y;
                            document.getElementById("MoreDebtor").style.display = "block";
                            document.getElementById("moreOldDebtor").style.display = "none";
                            isDebtors = true;
                        } else {
                            isDebtors = false;
                        }
                    }
                    if (getPosition == -1) {
                        for (var y = 0; y <= common.config.information.Debtors.length - 1; y++) {
                            if (vm.items[0].CURP == common.config.information.Debtors[y].CURP) {
                                if (common.config.information.Debtors[y].Age < 18 && (common.config.information.Debtors[y].KeyRelationship == 70 || common.config.information.Debtors[y].KeyRelationship == 80)) {
                                    vm.checked.push({ // AGREGA LA INFORMACIÓN A LA VISTA
                                        Age: common.config.information.Debtors[y].Age,
                                        Birthplace: common.config.information.Debtors[y].Birthplace,
                                        CURP: common.config.information.Debtors[y].CURP,
                                        DebtorId: common.config.information.Debtors[y].DebtorId,
                                        MaternalLastName: common.config.information.Debtors[y].MaternalLastName,
                                        Name: common.config.information.Debtors[y].Name,
                                        PaternalLastName: common.config.information.Debtors[y].PaternalLastName,
                                        Relationship: common.config.information.Debtors[y].Relationship,
                                        Request: common.config.information.Debtors[y].Request,
                                        RequestId: common.config.information.Debtors[y].RequestId,
                                        KeyRelationship: common.config.information.Debtors[y].KeyRelationship
                                    });
                                    document.getElementById("MoreDebtor").style.display = "none";
                                    document.getElementById("moreOldDebtor").style.display = "none";
                                    isDebtors = true;
                                } else if (common.config.information.Debtors[y].KeyRelationship == 70 || common.config.information.Debtors[y].KeyRelationship == 80 && (common.config.information.Debtors[y].Age >= 18 && common.config.information.Debtors[y].Age <= 25)) {
                                    document.getElementById("moreOldDebtor").style.display = "block";
                                    document.getElementById("MoreDebtor").style.display = "none";
                                    isDebtors = true;
                                } else if (common.config.information.Debtors[y].KeyRelationship != 70 || common.config.information.Debtors[y].KeyRelationship != 80) {
                                    vm.checked.push({ // AGREGA LA INFORMACIÓN A LA VISTA
                                        Age: common.config.information.Debtors[y].Age,
                                        Birthplace: common.config.information.Debtors[y].Birthplace,
                                        CURP: common.config.information.Debtors[y].CURP,
                                        DebtorId: common.config.information.Debtors[y].DebtorId,
                                        MaternalLastName: common.config.information.Debtors[y].MaternalLastName,
                                        Name: common.config.information.Debtors[y].Name,
                                        PaternalLastName: common.config.information.Debtors[y].PaternalLastName,
                                        Relationship: common.config.information.Debtors[y].Relationship,
                                        Request: common.config.information.Debtors[y].Request,
                                        RequestId: common.config.information.Debtors[y].RequestId,
                                        KeyRelationship: common.config.information.Debtors[y].KeyRelationship
                                    });
                                    document.getElementById("MoreDebtor").style.display = "none";
                                    document.getElementById("moreOldDebtor").style.display = "none";
                                    isDebtors = true;
                                } else {
                                    document.getElementById("moreOldDebtor").style.display = "block";
                                    document.getElementById("MoreDebtor").style.display = "none";
                                    isDebtors = true;
                                }
                            }
                        }
                    }
                }
                vm.items.splice(index, 1);
            } else {
                isDebtors = true;
                document.getElementById("MoreDebtor").style.display = "Block";
                document.getElementById("moreOldDebtor").style.display = "none";
                deleteDebtors(index);
            }
            vm.addNewDebtor = false;
            vm.viewAddEnableNewDebtor = true;

            if (isDebtors == false) {
                document.getElementById("NoDebtorsFound").style.display = "Block";
                document.getElementById("MoreDebtor").style.display = "none";
            }
            else {
                document.getElementById("NoDebtorsFound").style.display = "none";
            }
        }



        // Función del botón "Eliminar - validar_deudos.html"
        function deleteDebtors(index) {
            vm.items.splice(index, 1);
            vm.addNewDebtor = false;
            vm.viewAddEnableNewDebtor = true;
            document.getElementById("DebtorComplete2").style.display = "none";
            document.getElementById("NoDebtorsFound").style.display = "none";
            vm.curpR = true;
        }

        function convertToArrayOfObjects(data) {
            var keys = data.shift(),
                i = 0, k = 0,
                obj = null,
                output = [];

            for (i = 0; i < data.length; i++) {
                obj = {};

                for (k = 0; k < keys.length; k++) {
                    obj[keys[k]] = data[i][k];
                }

                output.push(obj);
            }

            return output;
        };

        // MODIFICADA
        function toggleAddEnableNewDebtor() {

            vm.addNewDebtor = !vm.addNewDebtor;
            vm.viewAddEnableNewDebtor = !vm.viewAddEnableNewDebtor;
            vm.enableAddEnableNewDebtor = false;
            vm.nuevocurp = "";
        };

        function viewInformationDebtor(valid) {
            vm.viewInformationDebtor = valid;
        }


        // Función para mostrar y/u ocultar el DIV "vm.viewAddEnableNewDebtor"
        function viewAddEnableNewDebtor() {
            vm.viewAddEnableNewDebtor = !vm.viewAddEnableNewDebtor;
        }

        function validDebtors() {
            if (vm.newDebtor.CURP != null || vm.newDebtor.CURP != "") {
                var getPosition = -1;
                var data = null;
                var curp = vm.newDebtor.CURP;
                if (vm.informationDebtor[0].CURP != vm.newDebtor.CURP) {
                    var lista = common.config.information.Debtors;
                    for (var x = 0; x <= lista.length - 1; x++) {
                        if (curp == lista[x].CURP) {
                            getPosition = x;
                            data = [
                                ["Age", "Birthplace", "CURP", "DebtorId", "MaternalLastName", "Name", "PaternalLastName", "Relationship", "Request", "RequestId"],
                                [lista[x].Age, lista[x].Birthplace, lista[x].CURP, lista[x].DebtorId, lista[x].MaternalLastName, lista[x].Name, lista[x].PaternalLastName, lista[x].Relationship, lista[x].Request, lista[x].RequestId]
                            ];
                            //var out = convertToArrayOfObjects(data);
                            //vm.informationDebtor2 = out;
                        }
                    }
                    document.getElementById("MoreDebtor").style.display = 'none'; // Oculta el mensaje de error
                } else {
                    document.getElementById("MoreDebtor").style.display = 'block'; // Muestra el mensaje de error
                }
                if (getPosition >= 0 && data != null) {
                    var out = convertToArrayOfObjects(data);
                    viewInformationDebtor(true);
                    vm.informationDebtor2 = out;
                    vm.viewDebtors = true;
                    document.getElementById("NoDebtorsFound").style.display = 'none'; // Oculta el mensaje de error
                } else {
                    document.getElementById("NoDebtorsFound").style.display = 'block'; // Muestra el mensaje de error
                    vm.viewDebtors = false;
                }
            } else {
                alert("Ingresar CURP del deudo");
            }
        }
        function validateDebtors(navigate) {

            var listFinally = null;
            listFinally = vm.informationDebtor.concat(vm.checked);
            common.config.information.Debtors = listFinally;

            //common.config.information = vm.information;
            //vm.requestInf.RequestId = null;
            vm.requestInf.EntitleId = vm.newDebtorData.CURP;
            vm.requestInf.Date = null;
            vm.requestInf.Folio = " ";
            vm.requestInf.Description = " ";
            vm.requestInf.IsAssigned = false;
            vm.requestInf.IsComplete = false;
            vm.requestInf.PensionId = common.config.information.PensionId;
            vm.requestInf.UserId = " ";
            vm.requestInf.AppointmentCount = 0;
            vm.requestInf.Validation = common.config.information.Validation;
            vm.requestInf.TimeContribution = common.config.information.timeContributions;
            vm.requestInf.StatusId = null;
            vm.requestInf.Validation.IsDebtorsCorrect = true;
            common.config
            var dataPromise = null;
            common.displayLoadingScreen();
            var dataPromiseCurp = validateCurpDebtor(vm.requestInf.EntitleId);
            dataPromiseCurp.success(function () {
                dataPromise = saveRequest(vm.requestInf);
                dataPromise.success(function (data1, status, headers, config) {
                    var dataPro = null;
                    dataPro = saveAllEntitle();
                    dataPro.success(function (data, status, headers, config) {
                        common.hideLoadingScreen();
                        // if (vm.information.Entitle.State == "F" && vm.information.Entitle.DirectType == "P") {
                        //    navigate(0);
                        //}
                        //else {
                        common.config.information.Validation.IsDebtorsCorrect = true;
                        navigate(0);
                        //}
                    }).error(function (data, status, headers, config) {
                        common.showErrorMessage(Messages.error.errorSave);
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 1 }, 2000);
                    });
                }).error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorSave);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
            });
            //  }

            //navigate(0);
        };
        function toggleEnableNewDebtor(valid) {
            vm.enableNewDebtor = valid;
        }


        function validateWorkHistory(navigate) {
            if (vm.countWarningsHistorial < 0) {
                common.config.information.Validation.IsWorkHistoryCorrect = true;
            }

            vm.requestInf.EntitleId = vm.information.Entitle.CURP;
            vm.requestInf.Date = null;
            vm.requestInf.Folio = "";
            vm.requestInf.Description = " ";
            vm.requestInf.IsAssigned = false;
            vm.requestInf.IsComplete = false;
            //CAP vm.requestInf.PensionId = common.config.entitleInformation.PensionId;
            vm.requestInf.PensionId = common.config.information.PensionId;
            vm.requestInf.UserId = " ";
            vm.requestInf.AppointmentCount = 0;
            vm.requestInf.Validation = common.config.information.Validation;
            vm.requestInf.TimeContribution = common.config.information.timeContributions;
            vm.requestInf.StatusId = null;
            var dataPromise = null;
            common.displayLoadingScreen();
            var dataPromiseCurp = validateCurp();
            dataPromiseCurp.success(function () {
                dataPromise = saveRequest(vm.requestInf);
                dataPromise.success(function (data1, status, headers, config) {
                    var dataPro = null;
                    dataPro = saveAllEntitle();
                    dataPro.success(function (data, status, headers, config) {
                        common.hideLoadingScreen();
                        navigate(2);
                    }).error(function (data, status, headers, config) {
                        common.showErrorMessage(Messages.error.errorSave);
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 1 }, 2000);
                    });
                }).error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorSave);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
            });
            //  navigate(2);
        };


        function initRequests() {
            $('#home').addClass("active");
            $('#requests').removeClass("hidden");
            $('#requests').addClass("active");

            $('#request').addClass("hidden");
            $('#infoGeneral').addClass("hidden");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#detailRequest').addClass("hidden");
            $('#appoiments').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");

            var dataPromise = null;
            dataPromise = getAllEntitleInformation();
            dataPromise.success(function (data, status, headers, config) {
                vm.information = data;
                common.displayLoadingScreen();
                var promise = null;
                promise = getRequests();
                promise.success(function () {
                    var promisreq = getPastRequests();
                    common.hideLoadingScreen();
                    common.displayLoadingScreen();
                    promisreq.success(function () {
                        common.hideLoadingScreen();
                       // getTypeProperty(vm.information.Entitle.CURP);
                        // completeControllerInit();
                        common.overrideNavigationMenu(false);
                    })
                        .error(function () {
                            common.showErrorMessage(Messages.error.errorRequests);
                            common.hideLoadingScreen();
                            $("html, body").animate({ scrollTop: 1 }, 2000);

                        });
                }).error(function () {
                    common.showErrorMessage(Messages.error.errorRequests);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
            });
        };

        function initDetail() {
            $('#requests').removeClass("hidden");
            $('#requests').addClass("inactive");
            $('#detailRequest').removeClass("hidden");
            $('#detailRequest').addClass("active");

            $('#infoGeneral').addClass("hidden");
            $('#historyLaboral').addClass("hidden");
            $('#debtors').addClass("hidden");
            $('#finish').addClass("hidden");
            $('#appoiments').addClass("hidden");
            $('#scheduleAppointment').addClass("hidden");

            vm.information = {};
            common.config.information = {};
            vm.information = common.config.information;
            vm.requestInf = {};
            vm.requestAll = {};
            common.config.requestInf = {};
            var requestId = $routeParams["requestId"];

            var promise = getAllSavedEntitleInformation(requestId);
            promise.success(function () {
                if (vm.information.Debtors.length > 0) {
                    var prom = getRequestDebtorsByRequestId(requestId);
                    prom.success(function () {
                        completeControllerInit();
                    })
                    .error(function () {
                    });
                } else {
                    var promise2 = getRequestAll(requestId);
                    promise2.success(function () {
                        completeControllerInit();
                        common.overrideNavigationMenu(false);
                    }).error(function () {
                        completeControllerInit();
                        common.overrideNavigationMenu(false);
                    });
                }
                common.hideLoadingScreen();
            })
                .error(function () {
                    completeControllerInit();
                    common.overrideNavigationMenu(false);
                    common.hideLoadingScreen();
                });


        }

        function getOptions(arg) {
            return arg ? 'Si' : 'No';
        }


        function searchRequestDebtorsByCURP() {
            getRequestDebtors(vm.newDebtorData.CURP);
        }
        function getRequestDebtors(curp) {
            var dataPromise = null;
            var lista = {};
            var existe = 0;
            dataPromise = getAllEntitleInformation();
            dataPromise.success(function (data, status, headers, config) {
                lista = data;
                if (data.Debtors != null) {
                    for (var x = 0; x <= lista.Debtors.length - 1; x++) {
                        if (curp == lista.Debtors[x].CURP) {
                            var existe = 1;
                        }
                    }
                    if (existe > 0) {
                        document.getElementById("noDebtorsList").style.display = 'none';
                        common.displayLoadingScreen();
                        return requestDataService.getRequestDebtors(curp)
                        .success(function (data, status, headers, config) {
                            vm.requestDebtors = data;
                            common.hideLoadingScreen();
                        })
                        .error(function (data, status, headers, config) {
                            vm.requestDebtors = null;
                        });
                    } else {
                        document.getElementById("noDebtorsList").style.display = 'block';
                    }
                }
            }).error(function (data, status, headers, config) {
                lista = null;
            });
        }
        function getRequestDebtorsByRequestId(requestId) {
            common.displayLoadingScreen();
            return requestDataService.getRequestDebtorsByRequestId(requestId)
            .success(function (data, status, headers, config) {
                vm.requestAll = data;
                common.hideLoadingScreen();
            })
            .error(function (data, status, headers, config) {
                vm.requestAll = null;
                common.hideLoadingScreen();
            });

        }
        //#endregion

        //#region Helper Functions

        function getAllEntitleInformation() {
            return homeDataService.getAllEntitleInformation(common.config.entitleInformation.NoISSSTE, common.config.entitleInformation.PensionId)
                .success(function (data, status, headers, config) {
                    common.config.information = data;
                    if (data.Entitle.Telephone != null) {
                        if ((data.Entitle.Telephone.substring(0, 2) == '55') || (data.Entitle.Telephone.substring(0, 2) == '33') || (data.Entitle.Telephone.substring(0, 2) == '81')) {
                            vm.lada = data.Entitle.Telephone.substring(0, 2);
                            vm.telefono = data.Entitle.Telephone.substring(2);
                        }
                        else {
                            vm.lada = data.Entitle.Telephone.substring(0, 3);
                            vm.telefono = data.Entitle.Telephone.substring(3);
                        }
                    }

                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.getAllEntitleInformation);
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function saveEntitle() {
            return homeDataService.saveEntitle(vm.information.Entitle)
                .success(function (data, status, headers, config) {
                    common.hideLoadingScreen();
                    //common.config.information = data;
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.saveEntitle);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function saveAllEntitle() {
            vm.information.telephone = vm.lada + vm.information.Entitle.Telephone;
            return homeDataService.saveAllEntitle(vm.information, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    vm.information = data;
                    common.config.information = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.saveRequestData);
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function saveRequest() {

            return requestDataService.saveRequest(vm.requestInf, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    common.config.requestInf = data;
                    vm.requestInf = data;
                    common.config.information.Folio = data.Folio;
                    common.config.information.RequestId = data.RequestId;
                    vm.information.RequestId = data.RequestId;
                    vm.information.Folio = data.Folio;
                    vm.information.validateData = common.config.information.Validation;
                    vm.requestInf.TimeContribution = common.config.information.timeContributions;
                    // vm.information.validateWorkHistory = data.validateWorkHistory;
                    vm.information.validateCurp = data.validateCurp;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.saveRequest);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function validateCurp() {
            if (common.config.entitle.State == "F") {
                vm.information.Validation.IsCurpCorrect = true;
            }
            else {
                return homeDataService.validateCurp(vm.information.Entitle.CURP, common.config.entitleInformation.NoISSSTE)
                    .success(function (data, status, headers, config) {
                        common.config.information.Validation.IsCurpCorrect = data.statusOperBit;
                        vm.information.Validation.IsCurpCorrect = data.statusOperBit;
                        common.hideLoadingScreen();
                    })
                    .error(function (data, status, headers, config) {
                        common.showErrorMessage(Messages.error.validateCurp);
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 1 }, 2000);
                    });

            }
        }

        function validateCurpDebtor(curpDebtor) {
            return homeDataService.validateCurp(curpDebtor, common.config.entitleInformation.NoISSSTE)
                .success(function (data, status, headers, config) {
                    common.config.information.Validation.IsCurpCorrect = data.statusOperBit;
                    vm.information.Validation.IsCurpCorrect = data.statusOperBit;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.validateCurp);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getRequests() {
            return requestDataService.getRequests(common.config.entitleInformation.CURP)
                .success(function (data, status, headers, config) {
                    vm.requests = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getPastRequests() {
            return requestDataService.getPastRequests(common.config.entitleInformation.CURP)
                .success(function (data, status, headers, config) {
                    vm.pastrequests = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.errorInformation);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }


        function getAllSavedEntitleInformation(requestId) {
            return homeDataService.getAllSavedEntitleInformation(common.config.entitleInformation.NoISSSTE, requestId)
                .success(function (data, status, headers, config) {
                    vm.information = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.validateCurp);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getRequestAll(requestId) {
            return requestDataService.getRequestAll(requestId, common.config.entitle.State)
                .success(function (data, status, headers, config) {
                    vm.requestAll = data;
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    common.showErrorMessage(Messages.error.validateCurp);
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }

        function getCurrentRequests() {


            if (common.config.entitle.State == "F") {
                return requestDataService.getCurrentRequests(common.config.entitleInformation.CURP)
                    .success(function (data, status, headers, config) {
                        vm.countRequest = 0;
                        common.hideLoadingScreen();
                    })
                    .error(function (data, status, headers, config) {
                        common.showErrorMessage(Messages.error.validateCurp);
                        common.hideLoadingScreen();
                        $("html, body").animate({ scrollTop: 1 }, 2000);
                    });
            }
            else {
                return requestDataService.getCurrentRequests(common.config.entitleInformation.CURP)
                 .success(function (data, status, headers, config) {
                     vm.countRequest = data;
                     common.hideLoadingScreen();
                 })
                 .error(function (data, status, headers, config) {
                     common.showErrorMessage(Messages.error.validateCurp);
                     common.hideLoadingScreen();
                     $("html, body").animate({ scrollTop: 1 }, 2000);
                 });
            }


        }

        function getDocumentsForInfo() {
            return requestDataService.getDocumentsForInfo()
                .success(function (data, status, headers, config) {
                    //vm.validationDocumentsAfiliatedKid = data[0];
                    //vm.validationDocumentsNonAfiliatedKid = data[1];    
                    vm.complementaryScanDocuments = data[0];
                    // vm.complementaryNonScanDocuments = data[3];
                    common.hideLoadingScreen();
                })
                .error(function (data, status, headers, config) {
                    vm.error = false;
                    common.showErrorMessage(data, "Error al obtener los documentos.");
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }


        function completeControllerInit() {
            common.logger.log(Messages.info.contollerLoaded, null, controllerId);
            common.overrideNavigationMenu(true);
            common.activateController([], controllerId);
        }


        function completeControllerSuccess() {
            completeControllerInit();
            common.overrideNavigationMenu(false);
        }

        function completeControllerFail() {
            completeControllerInit();
            common.overrideNavigationMenu(true);
        }


        function GetUrbanCenter() {
            return requestDataService.GetUrbanCenter(id)
                .success(function (data, status, headers, config) {
                    vm.Property = data.PropertTypeCat;
                    vm.Urban = data.TypeOwnerCat;
                    vm.Owner = data.UrbanCenterCat;
                    common.hideLoadingScreen();
                    // vm.complementaryNonScanDocuments = data[3];
                })
                .error(function (data, status, headers, config) {
                    vm.error = false;
                    common.showErrorMessage(data, "Error al obtener el catálogo de Propiedades.");
                    common.hideLoadingScreen();
                    $("html, body").animate({ scrollTop: 1 }, 2000);
                });
        }



        //#endregion
    };
})();