using System;
using System.Collections.Generic;

namespace PWAForTest.Models;

public partial class Sexo
{
    public int Iidsexo { get; set; }

    public string? Nombre { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<Autor> Autors { get; set; } = new List<Autor>();
}
