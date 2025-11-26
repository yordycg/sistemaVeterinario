using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

    public string? Diagnostico { get; set; }

    [Required]
    public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public bool EsActivo { get; set; } = true;

    [NotMapped]
    public string MascotaYFecha => $"{IdMascotaNavigation?.Nombre} - {FechaConsulta.ToString("d")}";

    [ValidateNever]
    public virtual EstadoConsulta IdEstadoConsultaNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Mascota IdMascotaNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}
