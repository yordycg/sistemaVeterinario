using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace sistemaVeterinario.Models;

public partial class Raza
{
    public int IdRaza { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una especie.")]
    public int IdEspecie { get; set; }

    public string NombreRaza { get; set; } = null!;

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();

    [ValidateNever]
    public virtual Especy IdEspecieNavigation { get; set; } = null!;
}
