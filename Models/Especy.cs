using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Especy
{
    public int IdEspecie { get; set; }

    public string NombreEspecie { get; set; } = null!;

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
