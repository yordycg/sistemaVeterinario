using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace sistemaVeterinario.Models;

public partial class Role
{
    public int IdRol { get; set; }

    [Required]
    [StringLength(25)]
    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
