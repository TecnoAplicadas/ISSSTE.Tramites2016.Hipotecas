$("#collapseOne").collapse("hide");

$("#menu-toggle").click(function(e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});


/*Agrega cupos a salones*/

$("#agregaCupo").click(function() {
    $("#salon1").removeClass("hidden");

    var atributos = $("#atributo").val();
    var valor = $("#valor").val();

    $("#cupos1").append("<tr><td>" + atributos + "</td><td>" + valor + "</td><td> <a href='#null' class='btn btn-danger  btn-sm eliminaFila'>Eliminar</a></td></tr>");
});


$("#cupos1").on("click", ".eliminaFila", function() {

    $(this).parent().parent().remove();
});