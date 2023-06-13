﻿using System;
using System.Collections.Generic;

namespace PWAForTest.Models;

public partial class Pagina
{
    public int Iidpagina { get; set; }

    public string? Mensaje { get; set; }

    public string? Accion { get; set; }

    public int? Bhabilitado { get; set; }

    public int? Bvisible { get; set; }

    public virtual ICollection<PaginaTipoUsuario> PaginaTipoUsuarios { get; set; } = new List<PaginaTipoUsuario>();
}
