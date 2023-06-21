﻿importScripts("/js/pouchDB.js")

var nombreCacheEstatico = "cacheEstatico1"
var nombreCacheDinamico = "cacheDinamico1"

var archivosEstaticos = [
    "/css/menu.css",
    "/PWAForTest.styles.css",
    "/lib/jquery/dist/jquery.min.js",
    "/lib/bootstrap/dist/js/bootstrap.bundle.min.js",
    "/js/menu.js",
    "/js/generic.js",
    "/img/loading.gif",
    "Persona/listarPersonas",
    "PaginaError/Index",
    "/js/sweetalert.js",
    "/js/pouchDB.js",
    "/"
]

self.addEventListener("install", event =>
{
    console.log("Evento install")
    event.waitUntil(
        caches.open(nombreCacheEstatico).then(cache => {
            return cache.addAll(archivosEstaticos)
        })
    )
})

self.addEventListener("activate", event =>
{
    console.log("Evento activate")
    event.waitUntil(self.clients.claim())
})

self.addEventListener("fetch", event =>
{
    if (event.request.method!="POST")
    {
        const respuesta = fetch(event.request).then(response => {
            caches.open(nombreCacheDinamico).then(cache => {
                cache.put(event.request, response)
            })
            return response.clone()
        }).catch(err => {
            return caches.match(event.request).then(res => {
                if (res) {
                    return res
                }
                else {
                    if (event.request.headers.get("accept").includes("text/html")) {
                        return caches.match("/PaginaError/Index")

                    }
                    else {
                        var response = new Response('<h1 class="text-danger">Para realizar esta accion necesita internet</h1>'
                            , {
                                headers: {
                                    "Content-Type": "test/html"
                                }
                            })
                        return response
                    }
                }

            })
            event.respondWith(respuesta)

        })
    }
    else
    {
        if (self.registration.sync) {
            var respuesta = fetch(event.request.clone()).then(response => {
                if (response) return response

            }).catch(err => {

                return event.request.clone().formData().then(formdata => {

                    var db = new PouchDB("BDBiblioteca")

                    var objeto = Object.fromEntries(formdata)

                    objeto._id = new Date().toISOString()
                    objeto.url = event.request.url
                    return db.put(objeto).then(res => {

                        self.registration.sync.register("insertData")

                        return new Response("2", {
                            headers: {
                                "Content-Type": "text/plain"
                            }
                        })
                    })
                })
            })

            event.respondWith(respuesta)
        }
        else
        {
            event.respondWith(fetch(event.request))
        }


    }
})

self.addEventListener("sync", event => {

    console.log("Entro")

    var db = new PouchDB("BDBiblioteca")

    var respuesta = db.allDocs({ include_docs: true }).then(data => {

        data.rows.forEach(fila => { 

            var doc = fila.doc;
            var frm = new FormData();

            for (var key in doc) {

                frm.append(key,doc[key])

            }
            console.log(doc)
            return fetch(doc.url, {
                method: "POST",
                body:frm             
            }).then(res => {
                db.remove(doc)
            })
        })
    })

    event.waitUntil(respuesta)
})