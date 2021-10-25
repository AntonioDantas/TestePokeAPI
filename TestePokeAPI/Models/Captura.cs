using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestePokeAPI.Models
{
    /// <summary>
    /// Modelo para registro da captura do pokemon
    /// </summary>
    public class Captura
    {
        public int Id { get; set; }
        public int IdMestre { get; set; }
        public int IdPokemon { get; set; }
        public DateTime DataHora { get; set; }
    }
    /// <summary>
    /// Modelo para recuperação das informações de captura
    /// </summary>
    public class RequestCaptura
    {
        public Captura captura { get; set; }
        public RequestPokemon Pokemon { get; set; }
    }
}