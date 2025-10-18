using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    public int IdCliente { get; set; }

    public int IdEspecie { get; set; }

    public int IdRaza { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Sexo { get; set; }

    public int? Edad { get; set; }

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Especy IdEspecieNavigation { get; set; } = null!;

    public virtual Raza IdRazaNavigation { get; set; } = null!;
}
