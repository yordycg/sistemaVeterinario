using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class EstadoConsulta
{
    public int IdEstadoConsulta { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();
}
