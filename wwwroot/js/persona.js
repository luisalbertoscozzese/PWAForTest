﻿window.onload = function () {
    listarPersonas();
}

function listarPersonas() {
    pintar({
        url: "Persona/listarPersonas",
        propiedades: ["nombreCompleto", "correo"],
        cabeceras:["Nombre Completo","Correo"]
    }, {
        url: "Persona/listarPersonas",
        formulario: [
            [
                {
                    class: "col-md-6",
                    label: "Nombre Completo",
                    name: "NombreCompleto",
                    type:"text"
                    }
                ]
        ]
    })
}