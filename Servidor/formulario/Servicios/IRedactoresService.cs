using formulario.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formulario.Servicios
{
    public interface IRedactoresService
    {
        Redactor Create(Redactor redactor);
        Redactor Get(long id);
        IQueryable<Redactor> Get();
        void Put(Redactor redactor);
        Redactor Delete(long id);
    }
}
