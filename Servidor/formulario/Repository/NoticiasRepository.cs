using formulario.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace formulario.Repository
{
    public class NoticiasRepository : INoticiasRepository
    {
        public Noticia Create(Noticia noticia)
        {
            return ApplicationDbContext.applicationDbContext.Noticias.Add(noticia);
        }

        public Noticia Get(long id)
        {
            return ApplicationDbContext.applicationDbContext.Noticias.Find(id);
        }

        public IQueryable<Noticia> Get()
        {
            IList<Noticia> lista = new List<Noticia>(ApplicationDbContext.applicationDbContext.Noticias);

            return lista.AsQueryable();
        }


        public void Put(Noticia noticia)
        {
            if (ApplicationDbContext.applicationDbContext.Noticias.Count(e => e.Id == noticia.Id) == 0)
            {
                throw new NoEncontradoException("No he encontrado la entidad");
            }
            ApplicationDbContext.applicationDbContext.Entry(noticia).State = EntityState.Modified;
        }

        public Noticia Delete(long id)
        {
            Noticia noticia = ApplicationDbContext.applicationDbContext.Noticias.Find(id);
            if (noticia == null)
            {
                throw new NoEncontradoException("No he encontrado la entidad");
            }

            ApplicationDbContext.applicationDbContext.Noticias.Remove(noticia);
            return noticia;
        }
    }
}