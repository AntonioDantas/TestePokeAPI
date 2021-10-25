using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TestePokeAPI.Models;

namespace TestePokeAPI.Controllers
{
    /// <summary>
    /// Controle para captura de pokemons
    /// </summary>
    public class CapturaController : ApiController
    {
        private MestreDbContext db = new MestreDbContext();

        /// <summary>
        /// Registra a captura de um novo pokemon
        /// </summary>
        /// <param name="captura">Modelo de captura</param>
        /// <returns>Objeto registrado</returns>
        [Authorize]
        [ResponseType(typeof(Captura))]
        public IHttpActionResult PostCaptura(Captura captura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            captura.DataHora = DateTime.Now;
            db.Capturas.Add(captura);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = captura.Id }, captura);
        }

        /// <summary>
        /// Retorna todas as capturas realizadas
        /// </summary>
        /// <param name="IdMestre">Id do mestre pokemon</param>
        /// <returns>Lista de objetos dos detalhes de capturas</returns>
        [Authorize]
        [ResponseType(typeof(List<RequestCaptura>))]
        public async Task<List<RequestCaptura>> GetCapturas(int IdMestre)
        {
            var retorno = new List<RequestCaptura>();
            var capturas = db.Capturas.Where(x => x.IdMestre == IdMestre).ToList();
            if (capturas == null)
            {
                return retorno;
            }
            foreach (var c in capturas)
            {
                retorno.Add(new RequestCaptura
                {
                    captura = c,
                    Pokemon = await new PokemonController().GetEspecifico(c.IdPokemon),
                });
            }
            return retorno;
        }
    }
}
