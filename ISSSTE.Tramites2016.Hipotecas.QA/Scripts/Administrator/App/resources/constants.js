var REQUEST_ID_PARAM = "requestId";
var ENTITLE_ID_PARAM = "entitleId";
var MESSAGE_ID_PARAM = "messageId";
var CATALOG_ITEM_KEY_PARAM = "catalogItemKey";
var JSON_CONTENT_TYPE = "application/json";
var FORM_CONTENT_TYPE = "application/x-www-form-urlencoded";
var BEARER_TOKEN_AUTHENTICATION_TEMPLATE = "Bearer {0}";

var Constants =
{
    accountRoutes: {
        logout: "account/logout?returnUrl={0}",
        softLogout: "account/logout?returnUrl={0}&soft=true",
        login: "account/login"
    },
    roles: {
        //chief: "Jefatura de Departamento de Pensiones y Seguridad e Higiene (Delegación)" + //Comento MFP 29-12-2016
        chief: "Administrador Hipotecas" + "",
        //operator: "Departamento de Pensiones y Seguridad e Higiene (Delegación)" // Comento MFP 29-12-2016
        operator: "Oficinas centrales Hipotecas"
    },
    newCatalogKeyId: "null",
    catalogs: {
        //childCareCenterTypes: "DocumentTypes",
        //childCareCenters: "ChildCareCenters",
        documentTypes: "DocumentTypes",
        delegations: "Delegations",
    },
    weekDays: {
        Domingo: { Name: "Domingo", Ordinal: 1 },
        Lunes: { Name: "Lunes", Ordinal: 2 },
        Martes: { Name: "Martes", Ordinal: 3 },
        Miercoles: { Name: "Miércoles", Ordinal: 4 },
        Jueves: { Name: "Jueves", Ordinal: 5 },
        Viernes: { Name: "Viernes", Ordinal: 6 },
        Sabado: { Name: "Sábado", Ordinal: 7 }
    }
};