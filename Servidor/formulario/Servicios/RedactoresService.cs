using formulario.Entities;
using formulario.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace formulario.Servicios
{
    public class RedactoresService : IRedactoresService
    {
        private IRedactoresRepository redactorsRepository;
        public RedactoresService(IRedactoresRepository _redactorsRepository)
        {
            this.redactorsRepository = _redactorsRepository;
        }

        public Redactor Get(long id)
        {
            return redactorsRepository.Get(id);
        }

        public IQueryable<Redactor> Get()
        {
            return redactorsRepository.Get();
        }

        public Redactor Create(Redactor redactor)
        {
            return redactorsRepository.Create(redactor);
        }

        public void Put(Redactor redactor)
        {
            redactorsRepository.Put(redactor);
        }

        public Redactor Delete(long id)
        {
            return redactorsRepository.Delete(id);
        }
    }
}