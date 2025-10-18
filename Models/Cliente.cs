using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sistemaVeterinario.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El RUN es obligatorio.")]
    [StringLength(12, ErrorMessage = "El RUN no puede tener más de 12 caracteres.")]
    public string Run { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    public string Email { get; set; } = null!;

    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string? Direccion { get; set; }

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
