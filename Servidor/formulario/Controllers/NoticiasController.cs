using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using formulario;
using formulario.Models;
using formulario.Servicios;
using System.Web.Http.Cors;

namespace formulario.Controllers
{
    [EnableCors(origins: "http://localhost:8080, http://localhost:3000", headers: "*", methods: "*")]

    public class NoticiasController : ApiController
    {
        private INoticiasService noticiasService;

        public NoticiasController(INoticiasService _noticiasService)
        {
            this.noticiasService = _noticiasService;
        }

        // GET: api/Noticias
        public IQueryable<Noticia> GetNoticias()
        {
            return noticiasService.Get();
        }

        // GET: api/Noticias/5
        [ResponseType(typeof(Noticia))]
        public IHttpActionResult GetNoticia(long id)
        {
            Noticia noticia = noticiasService.Get(id);
            if (noticia == null)
            {
                return NotFound();
            }

            return Ok(noticia);
        }

        // PUT: api/Noticias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNoticia(long id, Noticia noticia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != noticia.Id)
            {
                return BadRequest();
            }

            try
            {
                noticiasService.Put(noticia);
            }
            catch (NoEncontradoException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Noticias
        [ResponseType(typeof(Noticia))]
        public IHttpActionResult PostNoticia(Noticia noticia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            noticia = noticiasService.Create(noticia);

            return CreatedAtRoute("DefaultApi", new { id = noticia.Id }, noticia);
        }

        // DELETE: api/Noticias/5
        [ResponseType(typeof(Noticia))]
        public IHttpActionResult DeleteNoticia(long id)
        {
            Noticia noticia;
            try
            {
                noticia = noticiasService.Delete(id);
            }
            catch (NoEncontradoException)
            {
                return NotFound();
            }

            return Ok(noticia);
        }
    }
}