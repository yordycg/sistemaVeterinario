using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace sistemaVeterinario.Models;

public partial class Consulta
{
    public int IdConsulta { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una mascota.")]
    public int IdMascota { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un veterinario.")]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un estado.")]
    public int IdEstadoConsulta { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateOnly FechaConsulta { get; set; }

    [Required(ErrorMessage = "El motivo de la consulta es obligatorio.")]
    [StringLength(500, ErrorMessage = "El motivo no puede tener más de 500 caracteres.")]
    public string? Motivo { get; set; }

    [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
    [StringLength(1000, ErrorMessage = "El diagnóstico no puede tener más de 1000 caracteres.")]
    public string? Diagnostico { get; set; }

    public virtual EstadoConsulta IdEstadoConsultaNavigation { get; set; } = null!;

    public virtual Mascota IdMascotaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}
