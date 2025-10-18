using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Consulta
{
    public int IdConsulta { get; set; }

    public int IdMascota { get; set; }

    public int IdUsuario { get; set; }

    public int IdEstadoConsulta { get; set; }

    public DateOnly FechaConsulta { get; set; }

    public string? Motivo { get; set; }

    public string? Diagnostico { get; set; }

    public virtual EstadoConsulta IdEstadoConsultaNavigation { get; set; } = null!;

    public virtual Mascota IdMascotaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}
