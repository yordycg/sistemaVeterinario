using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Tratamiento
{
    public int IdTratamiento { get; set; }

    public int IdConsulta { get; set; }

    public string? Descripcion { get; set; }

    public string? Medicamento { get; set; }

    public virtual Consulta IdConsultaNavigation { get; set; } = null!;
}
