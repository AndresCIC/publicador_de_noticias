using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace formulario
{
    public class Noticia
    {
        public enum TipoDeNoticia { Cronologica, InteresHumano, Futuro, Espacial };

        public long Id { get; set; }
        public TipoDeNoticia Tipo { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public bool Publicado { get; set; }
        public DateTime FechaPublicacion { get; set; }

    }
}