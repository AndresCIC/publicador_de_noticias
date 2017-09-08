using formulario.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace formulario.Servicios
{
    public class NoticiasService : INoticiasService
    {
        private INoticiasRepository noticiasRepository;
        public NoticiasService(INoticiasRepository _noticiasRepository)
        {
            this.noticiasRepository = _noticiasRepository;
        }

        public Noticia Get(long id)
        {
            return noticiasRepository.Get(id);
        }

        public IQueryable<Noticia> Get()
        {
            return noticiasRepository.Get();
        }

        public Noticia Create(Noticia noticia)
        {
            return noticiasRepository.Create(noticia);
        }

        public void Put(Noticia noticia)
        {
            noticiasRepository.Put(noticia);
        }

        public Noticia Delete(long id)
        {
            return noticiasRepository.Delete(id);
        }
    }
}