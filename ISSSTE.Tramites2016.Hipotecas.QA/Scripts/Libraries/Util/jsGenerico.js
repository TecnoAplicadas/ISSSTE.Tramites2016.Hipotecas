


function jsGetdata(url, modelo, divRemplazo) {
    var tipo = "html";
    if (divRemplazo == null) {
        tipo = "json";
    }

    return $.ajax({
        type: "POST",
        url: url,
        data: modelo,
        async: false,
        cache: false,
        dataType: tipo
    });
}


function getBtnVerDetalle(dataValues, jsVerDetalle) {

    var btnVer = "<div class='pull-right'>"
                + "<a style='cursor: pointer'>"
                    + "<button type='button'  onclick='" + jsVerDetalle.name + "(this)' " + dataValues + "' class='btn btn-primary btn-sm'>"
                    + "Ver detalle"
                    + "</button>"
                + "</a>"
             + "</div>";
    return btnVer;

}

$.makeTable = function (dataObject, columnId, jsVerDetalle) {

    var table = $("<table  class='table'>");
    var tblHeader = "<tr>";

    for (var k in dataObject[0]) {
        if (k != columnId)
            tblHeader += "<th>" + k + "</th>";
    }

    tblHeader += "</tr>";
    $(tblHeader).appendTo(table);
    $.each(dataObject, function (index, value) {
        var TableRow = "<tr>";
        var DataValues = "";
        $.each(value, function (key, val) {

            if (key != columnId) {
                TableRow += "<td>" + val + "</td>";
            }

            DataValues += " data-" + key + "='" + val + "'";

        });

        if (jsVerDetalle != undefined || jsVerDetalle != null) TableRow += "<td>" + getBtnVerDetalle(DataValues, jsVerDetalle) + "</td>";

        TableRow += "</tr>";
        $(table).append(TableRow);
    });
    return ($(table));
};




$.makeBodyTable = function (dataObject, columnId, jsVerDetalle) {

    var tableBody = "";

    if (dataObject != undefined || dataObject != null) {
        $.each(dataObject, function(index, value) {
            var tableRows = "<tr>";
            var dataValues = "";
            $.each(value, function(key, val) {

                if (key != columnId) {
                    tableRows += "<td>" + val + "</td>";
                }

                dataValues += " data-" + key + "='" + val + "'";
            });

            if (jsVerDetalle != undefined || jsVerDetalle != null) tableRows += "<td>" + getBtnVerDetalle(dataValues, jsVerDetalle) + "</td>";

            tableRows += "</tr>";

            tableBody += tableRows;
        });
    } else {
        console.log("No se construyo el cuerpo de la tabla: La lista no contiene elementos");
    }

    return tableBody;
};


function jsGenerarCuerpoTabla(model, divRemplazo, columnId, jsVerDetalleSolicitud) {

    columnId = columnId == undefined || columnId == null ? "" : columnId;

    var mydata = eval(model);
    var table = $.makeBodyTable(mydata, columnId, jsVerDetalleSolicitud);
    $("#" + divRemplazo).empty();
    $(table).appendTo("#" + divRemplazo);

}




