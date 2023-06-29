﻿window.onload = function () {
    listarLibro();
    startListening(function (texto) {
        if (texto == "Open.") {
            document.getElementById("btnNuevo").click()
        }
        else if (texto == "Close.") {
            document.getElementById("btnCerrar").click()
        }
    })
}

function listarLibro() {

    fetchGet("Autor/listarAutor", "json", function (rpta) {
        llenarCombo(rpta,"cboAutor","iidautor","nombreautor")
    })
    pintar({
        url: "Libro/listarLibros",
        propiedades: ["base64", "titulo"],
        cabeceras: ["Foto", "Libro"],
        //titlePopup: "Tipo Libro",
        rowClickRecuperar: true,
        type:"popup",
        propiedadId: "iidlibro",
        columnaimg: ["base64"],
        callbackrecuperar: function (id) {
            setI("lblTitulo", "Editar Libro")
            LimpiarDatos("frmLibro")
            document.getElementById("videoFoto").srcObject = null
            document.getElementById("videoFoto").poster = ""
            recuperarGenerico("Libro/recuperarLibro/?iidLibro=" + id, "frmLibro", function (data) {
                document.getElementById("videoFoto").poster=data.base64
            })
        }
    })
}

function Nuevo() {
    LimpiarDatos("frmLibro")
    document.getElementById("videoFoto").srcObject = null
    document.getElementById("videoFoto").poster = ""
    setI("lblTitulo", "Agregar Libro")
}

function GuardarDatos() {
    var frmLibro = document.getElementById("frmLibro")
    var frm = new FormData(frmLibro)
    var fotoTomada = obtenerImagenVideo("videoFoto")
    frm.append("base64", fotoTomada)

    var errores = ValidarDatos("frmLibro")

    if (errores != "") {
        Error(errores)
        vibrarTelefono(2000);
        return;
    }

    Confirmacion("Confirmacion", "¿Desea guardar cambios?", function () {
        fetchPost("Libro/guardarDatos", "text", frm, function (rpta) {
            if (rpta == 1) {
                Exito("Se guardo correctamente")
                listarLibro()
                docuemnt.getElementById("btnCerrar").click()
            } else {
                Error()
            }
        })
    })

}