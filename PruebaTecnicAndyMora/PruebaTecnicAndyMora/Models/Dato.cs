using System;
using System.Collections.Generic;

namespace PruebaTecnicAndyMora.Models;

public partial class Dato
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
