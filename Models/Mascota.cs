using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sistemaVeterinario.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un dueño.")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una especie.")]
    public int IdEspecie { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una raza.")]
    public int IdRaza { get; set; }

    [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [StringLength(10)]
    public string? Sexo { get; set; }

    [Range(0, 50, ErrorMessage = "La edad debe ser un número entre 0 y 50.")]
    public int? Edad { get; set; }

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Especy IdEspecieNavigation { get; set; } = null!;

    public virtual Raza IdRazaNavigation { get; set; } = null!;
}
