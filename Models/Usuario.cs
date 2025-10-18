using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public int IdEstadoUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual EstadoUsuario IdEstadoUsuarioNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;
}
