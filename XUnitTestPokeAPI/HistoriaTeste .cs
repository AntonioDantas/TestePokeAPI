using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using TestePokeAPI.Controllers;
using TestePokeAPI.Models;
using Xunit;

namespace XUnitTestPokeAPI
{
    /// <summary>
    /// Teste simples para verificar o fluxo das informações
    /// Contudo é possível executar testes mais completos
    /// </summary>
    public class HistoriaTeste
    {
        /// <summary>
        /// Chamada da classe
        /// </summary>
        public HistoriaTeste()
        {
            Get_WhenCalled_ReturnsOkResult();
        }

        /// <summary>
        /// Classe de teste
        /// </summary>
        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            IHttpActionResult aleatorio = await new PokemonController().GetAleatorio();
            var lista = aleatorio as List<Pokemon>;
            System.Console.WriteLine("Recuperado lista de 10 pokemons aleatório");

            IHttpActionResult novomestre = new MestreController().PostMestre(new Mestre
            {
                Cpf = "36471042809",
                Idade = 30,
                Nome = "Antonio Dantas",
                Senha = "1234"
            });
            var mestre = novomestre as Mestre;
            System.Console.WriteLine("Cadastro de novo mestre");

            IHttpActionResult novacaptura = new CapturaController().PostCaptura(new Captura
            {
                IdMestre = mestre.Id,
                IdPokemon = lista[0].Id,
            });
            System.Console.WriteLine("Captura realizada");

            var capturas = await new CapturaController().GetCapturas(mestre.Id);
            System.Console.WriteLine($"Lista de {capturas.Count} capturas realizadas ");

            var okResult = (capturas.Count) > 0;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
        
    }
}
