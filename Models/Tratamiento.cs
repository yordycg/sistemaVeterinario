using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace sistemaVeterinario.Models;

public partial class Tratamiento
{
    public int IdTratamiento { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una consulta.")]
    public int IdConsulta { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El medicamento es obligatorio.")]
    [StringLength(100, ErrorMessage = "El medicamento no puede tener más de 100 caracteres.")]
    public string? Medicamento { get; set; }

    [ValidateNever]
    public virtual Consulta IdConsultaNavigation { get; set; } = null!;
}
