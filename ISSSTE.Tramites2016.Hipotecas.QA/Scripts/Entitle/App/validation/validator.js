$.validator.addMethod('filesize', function (value, element, param) {
    return this.optional(element) || (element.files[0].size <= param)
});

$(function () {
    $("#formIndex").validate({
        errorElement: "small",
        rules: {
            Lada: {
                //required: true,
                number: true
            },
            Telephone: {
                required: true,
                number: true
            },
            Email: {
                required: true,
                minlength: 2,
                email: true
            }
            ,
            MobilePhone: {
                //required: true,
                minlength: 10,
                number: true
            },
        },
        messages: {
            Lada: {
                //required: "Este campo es obligatorio.",
                number: "Introduce un número válido.",
                minlength: "Introduce al menos 2 dígitos."
                
            },
            Telephone: {
                required: "Este campo es obligatorio.",
                number: "Introduce un número válido.",
                minlength: "Introduce al menos 8 dígitos."
            },
            Email: {
                required: "Este campo es obligatorio.",
                email :"Introduce un correo válido.",
              minlength: "Introduce al menos 2 dígitos."
            },
            MobilePhone: {
                number: "Introduce un número válido.",
                minlength: "Introduce al menos 10 dígitos."
            }

        },
        highlight: function (element, errorClass, validClass) {
            
            $(element).prev().find(".required")
                      .addClass("error")
                      .removeClass(validClass);
            $(element).addClass("error")
                      .removeClass(validClass);
            $(".alert-danger").show();
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).prev().find(".required")
                      .removeClass("error")
                      .addClass(validClass);
            $(element).removeClass("error")
                      .addClass(validClass);
        }
    });

    $("#formRegister").validate({
        errorElement: "small",
        rules: {
            "Item2.IsConjugalCredit":{
                required: true
            },
            "WritingProperty": {
                required: true
            }

        },
        messages: {
        "Item2.IsConjugalCredit": {
                required: "Selecciona una opción."
        },
        "WritingProperty": {
            required: "Este campo es obligatorio."        
        }
    },
    highlight: function (element, errorClass, validClass) {

        $(element).prev().find(".required")
                    .addClass("error")
                    .removeClass(validClass);
        $(element).addClass("error")
                    .removeClass(validClass);

    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).prev().find(".required")
                    .removeClass("error")
                    .addClass(validClass);
        $(element).removeClass("error")
                    .addClass(validClass);
    }
    });



    $("#formDocs").validate({
        errorElement: "small",
        rules: {
            "12": {
                required: true,
                filesize: 5242880
            },
            "13": {
                required: true,
                filesize: 5242880
            },
            "21": {
                required: true,
                filesize: 5242880
            },
            "22": {
                required: true,
                filesize: 5242880
            },
            "23": {
                required: true,
                filesize: 5242880
            },
            "24": {
                required: true,
                filesize: 5242880
            },
            "25": {
                required: true,
                filesize: 5242880
            },
            "30": {
                required: true,
                filesize: 5242880
            },
            "14": {
                filesize: 5242880
            },
            "15": {
                filesize: 5242880
            },
            "16": {
                filesize: 5242880
            },
            "17": {
                filesize: 5242880
            },
            "18": {
                filesize: 5242880
            },
            "19": {
                filesize: 5242880
            },
            "26": {
                filesize: 5242880
            },
            "27": {
                filesize: 5242880
            },
            "28": {
                filesize: 5242880
            }
        },
        messages: {
            "12": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
},
            "13": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "21": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "22": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "23": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "24": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "25": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "30": {
                required: "Debes subir el documento requerido.",
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "14": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "15": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "16": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "17": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "18": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "19": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "26": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "27": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            },
            "28": {
                filesize: "El tamaño del archivo debe ser menor a 5 MB."
            }
        },
        highlight: function (element, errorClass, validClass) {
            
            $(element).prev().find(".required")
                        .addClass("error")
                        .removeClass(validClass);
            $(element).addClass("error")
                        .removeClass(validClass);
            
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).prev().find(".required")
                        .removeClass("error")
                        .addClass(validClass);
            $(element).removeClass("error")
                        .addClass(validClass);
        },
        submitHandler: function (form) {
            form.submit();
            Mostrar();
        }
    });

});







