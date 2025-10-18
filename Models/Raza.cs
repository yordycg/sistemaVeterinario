using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Raza
{
    public int IdRaza { get; set; }

    public string NombreRaza { get; set; } = null!;

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
