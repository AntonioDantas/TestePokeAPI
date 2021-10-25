using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Description;
using TestePokeAPI.Models;
using TestePokeAPI.Services;

namespace TestePokeAPI.Controllers
{
    /// <summary>
    /// Controle do Mestre Pokemon
    /// </summary>
    public class MestreController : ApiController
    {
        private MestreDbContext db = new MestreDbContext();

        /// <summary>
        /// Retorna todos os mestres pokemon
        /// </summary>
        /// <returns>Lista de mestres</returns>
        // GET: api/Mestres      
        [Authorize]
        public IQueryable<Mestre> GetMestres()
        {
            return db.Mestres;
        }

        /// <summary>
        /// Retorna um mestre pokemon especifico
        /// </summary>
        /// <param name="id">Id do  mestre pokemon</param>
        /// <returns>objeto do mestre pokemon</returns>
        // GET: api/Mestres/5
        [Authorize]
        [ResponseType(typeof(Mestre))]
        public IHttpActionResult GetMestre(int id)
        {
            Mestre Mestre = db.Mestres.Find(id);
            if (Mestre == null)
            {
                return NotFound();
            }
            return Ok(Mestre);
        }

        /// <summary>
        /// Atualiza informações do mestre pokemon
        /// </summary>
        /// <param name="id">Id do mestre pokemon</param>
        /// <param name="Mestre">Novos dados do mestre pokemon</param>
        /// <returns>Status da requisição</returns>
        [Authorize]
        // PUT: api/Mestres/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMestre(int id, Mestre Mestre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != Mestre.Id)
            {
                return BadRequest();
            }
            var hash = new Hash(SHA512.Create());
            Mestre.Senha = hash.CriptografarSenha(Mestre.Senha);
            db.Entry(Mestre).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MestreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Cria um novo mestre pokemon
        /// </summary>
        /// <param name="Mestre">Dados do mestre pokemon</param>
        /// <returns>Dados do novo mestre pokemon</returns>
        // POST: api/Mestres
        [ResponseType(typeof(Mestre))]
        public IHttpActionResult PostMestre(Mestre Mestre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var hash = new Hash(SHA512.Create());
            Mestre.Senha  = hash.CriptografarSenha(Mestre.Senha);
            db.Mestres.Add(Mestre);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = Mestre.Id }, Mestre);
        }

        /// <summary>
        /// Exclui um mestre pokemon
        /// </summary>
        /// <param name="id">Id do  mestre pokemon</param>
        /// <returns>Status da requisicao</returns>
        [Authorize]
        // DELETE: api/Mestres/5
        [ResponseType(typeof(Mestre))]
        public IHttpActionResult DeleteMestre(int id)
        {
            Mestre Mestre = db.Mestres.Find(id);
            if (Mestre == null)
            {
                return NotFound();
            }
            db.Mestres.Remove(Mestre);
            db.SaveChanges();
            return Ok(Mestre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool MestreExists(int id)
        {
            return db.Mestres.Count(e => e.Id == id) > 0;
        }
    }
}
