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
using formulario.Entities;

namespace formulario.Controllers
{
    [EnableCors(origins: "http://localhost:8080, http://localhost:3000", headers: "*", methods: "*")]

    public class RedactoresController : ApiController
    {
        private IRedactoresService redactorsService;

        public RedactoresController(IRedactoresService _redactorsService)
        {
            this.redactorsService = _redactorsService;
        }

        // GET: api/Redactores
        public IQueryable<Redactor> GetRedactores()
        {
            return redactorsService.Get();
        }

        // GET: api/Redactores/5
        [ResponseType(typeof(Redactor))]
        public IHttpActionResult GetRedactor(long id)
        {
            Redactor redactor = redactorsService.Get(id);
            if (redactor == null)
            {
                return NotFound();
            }

            return Ok(redactor);
        }

        // PUT: api/Redactores/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRedactor(long id, Redactor redactor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != redactor.Id)
            {
                return BadRequest();
            }

            try
            {
                redactorsService.Put(redactor);
            }
            catch (NoEncontradoException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Redactores
        [ResponseType(typeof(Redactor))]
        public IHttpActionResult PostRedactor(Redactor redactor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            redactor = redactorsService.Create(redactor);

            return CreatedAtRoute("DefaultApi", new { id = redactor.Id }, redactor);
        }

        // DELETE: api/Redactores/5
        [ResponseType(typeof(Redactor))]
        public IHttpActionResult DeleteRedactor(long id)
        {
            Redactor redactor;
            try
            {
                redactor = redactorsService.Delete(id);
            }
            catch (NoEncontradoException)
            {
                return NotFound();
            }

            return Ok(redactor);
        }
    }
}