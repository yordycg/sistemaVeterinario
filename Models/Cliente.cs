using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Run { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Email { get; set; } = null!;

    public string? Direccion { get; set; }

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
