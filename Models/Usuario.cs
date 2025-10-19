using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace sistemaVeterinario.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un rol.")]
    public int IdRol { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un estado de usuario.")]
    public int IdEstadoUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(25, ErrorMessage = "El nombre no puede tener más de 25 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    [StringLength(100, ErrorMessage = "El email no puede tener más de 100 caracteres.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(10, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 10 caracteres.")]
    public string Password { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    [ValidateNever]
    public virtual EstadoUsuario IdEstadoUsuarioNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Role IdRolNavigation { get; set; } = null!;
}
