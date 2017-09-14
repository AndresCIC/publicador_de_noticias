using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace formulario.Entities
{
    public class Redactor
    {
        public enum Rol { Reportero, RedactorGrafico, Ilustrador, Publicista  };
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public Rol RolRedactor { get; set; }
        public bool Disable { get; set; }

    }
}