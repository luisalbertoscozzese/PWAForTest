﻿window.onload = function () {
    listarTipoLibro();
}

var bc=new BroadcastChannel("TipoLibro")

function listarTipoLibro() {
    pintar({
        url: "TipoLibro/listarTipoLibro",
        propiedades: ["base64","nombre"],
        cabeceras: ["Foto","Tipo Libro"],
        titlePopup: "Tipo Libro",
        rowClickRecuperar: true,
        propiedadId: "idtipolibro",
        columnaimg:["base64"]
    }, {
        url: "TipoLibro/listarTipoLibro",
        formulario: [
            [
                {
                    class: "col-md-6",
                    label: "Nombre Tipo Libro",
                    name: "nombreTipoLibrobusqueda",
                    type: "text"
                }
            ]
        ]
    }, {
        type: "popup",
        urlguardar: "TipoLibro/guardarTipoLibro",
        urlrecuperar: "TipoLibro/recuperarTipoLibro",
        parametrorecuperar:"id",
        formulario: [
            [
                {
                    class: "d-none",
                    label: "Id Tipo Libro",
                    name: "idtipolibro",
                    type: "text"
                },
                {
                    class: "col-md-6",
                    label: "Nombre Tipo Libro",
                    name: "nombre",
                    type: "text",
                    classControl:"ob max-20"
                },
                {
                    class: "col-md-6",
                    label: "Descripcion Tipo Libro",
                    name: "descripcion",
                    type: "textarea",
                    classControl: "ob max-300"
                },
                {
                    class: "col-md-6",
                    label: "Suba una foto",
                    name: "fotoEnviar",
                    type: "file",
                    preview: true,
                    imgwidth: 100,
                    imgheight: 100,
                    namefoto:"base64"
                }
            ]
        ],
        callbackGuardar: function () {
            var frm = new FormData();
            var contenido="Se guardo los cambios del tipo libro "+ getN("nombre")
            frm.append("parametroPorContenido", "Registro Satisfactorio_"+ contenido +"_Contenido de la alerta_/img/icon-512.png")
            fetchPostSinLoading("Notificacion/enviarNotificaciones", "text", frm, () => { })

            if (getN("idtipolibro") != "") {
                bc.postMessage(getN("idtipolibro") + "_" + getN("nombre"))
            }


        }
       }
    )
}