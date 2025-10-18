using System;
using System.Collections.Generic;

namespace sistemaVeterinario.Models;

public partial class EstadoUsuario
{
    public int IdEstadoUsuario { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
