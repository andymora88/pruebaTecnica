using System;
using System.Collections.Generic;

namespace PruebaTecnicAndyMora.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Cedula { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Dato> Datos { get; } = new List<Dato>();
}
