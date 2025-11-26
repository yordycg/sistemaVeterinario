using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace sistemaVeterinario.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un dueño.")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una raza.")]
    public int IdRaza { get; set; }

    [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [StringLength(10)]
    public string? Sexo { get; set; }

    [Range(0, 50, ErrorMessage = "La edad debe ser un número entre 0 y 50.")]
    public int? Edad { get; set; }

    [Required]
    public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public bool EsActivo { get; set; } = true;

    [NotMapped]
    public string NombreDueño => IdClienteNavigation != null ? $"{Nombre} - {IdClienteNavigation.Run}" : Nombre;

    [ValidateNever]
    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    [ValidateNever]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Raza IdRazaNavigation { get; set; } = null!;
}
