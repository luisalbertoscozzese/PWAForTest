window.onload = function () {
    listarPersonas();
    activarNotificaciones();
}

function activarNotificaciones(){

    if (window.Notification) {

        if (Notification.permission != "granted") {

            Notification.requestPermission(function (rpta) {

                if (rpta == "granted") {

                    new Notification("Mi Primera Notificacion", {
                        body: "Contenido de notificacion",
                        icon:"/img/icon-192.png"
                        });
                }
            })
        }
    }
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