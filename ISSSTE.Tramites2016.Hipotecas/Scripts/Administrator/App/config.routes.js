var Routes = {
    appointments: {
        url: "/appointments",
        templateUrl: "/Scripts/Administrator/App/Appointments/appointments.html",
        roles: [Constants.roles.chief, Constants.roles.operator]
    },
    search: {
        url: "/search",
        templateUrl: "/Scripts/Administrator/App/search/search.html",
        roles: [Constants.roles.chief, Constants.roles.operator]
    },
    requestDetail: {
        url: "/Administrator/Detail?NoIssste=:entitleId&Request=:requestId",
        roles: [Constants.roles.chief]
    },
    //requestDetail: {
    //    url: "/request/:requestId",
    //    templateUrl: "Scripts/Administrator/App/requests/requestDetail.html",
    //    roles: [Constants.roles.chief, Constants.roles.operator]
    //},
    calendar: {
        url: "/calendar",
        templateUrl: "/Scripts/Administrator/App/calendar/calendario.html",
        roles: [Constants.roles.chief, Constants.roles.operator]

    },
    dates: {
        url: "/dates",
        templateUrl: "/Scripts/Administrator/App/dates/dates.html",
        roles: [Constants.roles.chief, Constants.roles.operator]

    },
    messages: {
        url: "/messages",
        templateUrl: "/Scripts/Administrator/App/messages/messages.html",
        roles: [Constants.roles.chief, Constants.roles.Administrator]

    },
    messagesById: {
        url: "/messages/:messageId",
        templateUrl: "/Scripts/Administrator/App/messages/message.html",
        roles: [Constants.roles.chief]

    },
    reports: {
        url: '/reports',
        templateUrl: '/Scripts/Administrator/App/reports/Busqueda.html',
        roles: [Constants.roles.chief]
    },
    noAccess: {
        url: "/noacceso",
        templateUrl: "/Scripts/Administrator/App/error/noaccess.html"
    },
    support: {
        url: "/support",
        templateUrl: "/Scripts/Administrator/App/support/support.html",
        roles: [Constants.roles.chief, Constants.roles.operator]
    },
    //Catálogos
    catalogElements: {
        url: '/catalogs/{0}',
        templateUrl: '/Administrator/CatalogElements?catalogName={0}',
        roles: [Constants.roles.chief]
    },
    catalogItemDetail: {
        url: '/catalogs/{0}/:catalogItemKey',
        templateUrl: '/Administrator/CatalogItemDetail?catalogName={0}',
        roles: [Constants.roles.chief]
    },

    requestDetail_RevisionStatus:{
        url: '/ResultadoRevision/:NoIssste/:RequestId',
        //templateUrl: '/Scripts/Administrator/App/statusManager/statusManager.html',
        //templateUrl: '/Administrator/ResultadoRevision?NoIssste=1111&Request=3fb29394-ca7b-4b66-9eb3-b89d0722cd1d&Page=ResultadoRevision',
        templateUrl: '/Administrator/ResultadoRevision/EsAngular',
        //templateUrl: function (params) {
        //    debugger;
        //    return 'Administrator/ResultadoRevision?NoIssste='+params.NoIssste+'&Request='+params.requestId+'&Page=ResultadoRevision';
        //}, 
        roles: [Constants.roles.chief, Constants.roles.operator]
    },
}

