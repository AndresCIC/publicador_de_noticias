using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formulario.Repository
{
    public interface INoticiasRepository
    {
        Noticia Create(Noticia noticia);
        Noticia Get(long id);
        IQueryable<Noticia> Get();
        void Put(Noticia noticia);
        Noticia Delete(long id);
    }
}
