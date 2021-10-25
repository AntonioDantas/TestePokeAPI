using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TestePokeAPI.Models;
using TestePokeAPI.Services;

namespace TestePokeAPI.Controllers
{
    /// <summary>
    /// Controle das informações do pokemon
    /// </summary>
    public class PokemonController : ApiController
    {
        /// <summary>
        /// Dados de um pokemon específico
        /// </summary>
        /// <param name="id">Id do pokemon</param>
        /// <returns>Dados resumidos do pokemon</returns>
        [Authorize]
        [ResponseType(typeof(RequestPokemon))]
        public async Task<RequestPokemon> GetEspecifico(int id)
        {
            ApiClient client = new ApiClient();
            Pokemon pokemon = await client.GetResourceAsync<Pokemon>(id);
            if (pokemon == null)
            {
                return null;
            }
            return new RequestPokemon(pokemon,
                await client.GetResourceAsync<EvolutionChain>(id),
                await new ApiClient().GetImageAsBase64Url((pokemon.Sprites.FrontDefault == null) ? pokemon.Sprites.BackDefault : pokemon.Sprites.FrontDefault));

        }

        /// <summary>
        /// Retorna 10 pokemons aleatorios
        /// </summary>
        /// <returns>Lista com 10 pokemons</returns>
        [Authorize]
        [ResponseType(typeof(List<RequestPokemon>))]
        public async Task<IHttpActionResult> GetAleatorio()
        {
            Random randNum = new Random();
            var lista = new List<RequestPokemon>();
            ApiClient client = new ApiClient();
            while (lista.Count < 10)
            {
                int id = randNum.Next(248);
                if (lista.FindAll(x => x.Id == id).Count == 0)
                {
                    Pokemon pokemon = await client.GetResourceAsync<Pokemon>(id);
                    lista.Add(new RequestPokemon(pokemon,
                     await client.GetResourceAsync<EvolutionChain>(id),
                     await new ApiClient().GetImageAsBase64Url((pokemon.Sprites.FrontDefault == null) ? pokemon.Sprites.BackDefault : pokemon.Sprites.FrontDefault)));
                }
            }
            return Ok(lista);
        }
    }
}