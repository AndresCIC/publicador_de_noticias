using formulario.Entities;
using formulario.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace formulario.Repository
{
    public class RedactoresRepository : IRedactoresRepository
    {
        public Redactor Create(Redactor redactor)
        {
            return ApplicationDbContext.applicationDbContext.Redactores.Add(redactor);
        }

        public Redactor Get(long id)
        {
            return ApplicationDbContext.applicationDbContext.Redactores.Find(id);
        }

        public IQueryable<Redactor> Get()
        {
            IList<Redactor> lista = new List<Redactor>(ApplicationDbContext.applicationDbContext.Redactores);

            return lista.AsQueryable();
        }


        public void Put(Redactor redactor)
        {
            if (ApplicationDbContext.applicationDbContext.Redactores.Count(e => e.Id == redactor.Id) == 0)
            {
                throw new NoEncontradoException("No he encontrado la entidad");
            }
            ApplicationDbContext.applicationDbContext.Entry(redactor).State = EntityState.Modified;
        }

        public Redactor Delete(long id)
        {
            Redactor redactor = ApplicationDbContext.applicationDbContext.Redactores.Find(id);
            if (redactor == null)
            {
                throw new NoEncontradoException("No he encontrado la entidad");
            }

            ApplicationDbContext.applicationDbContext.Redactores.Remove(redactor);
            return redactor;
        }
    }
}