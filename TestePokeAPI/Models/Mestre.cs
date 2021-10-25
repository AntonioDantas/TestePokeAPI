using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestePokeAPI.Models
{
    /// <summary>
    /// Modelo do Mestre Pokemon
    /// </summary>
    [Table("Mestre")]
    public class Mestre
    {
        /// <summary>
        /// Id Automático do BD
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Nome do Mestre Pokemon
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Idade do Mestre Pokemon
        /// </summary>
        public int Idade { get; set; }
        /// <summary>
        /// CPF do Mestre Pokemon
        /// </summary>
        public string Cpf { get; set; }
        /// <summary>
        /// Senha para geração do Token
        /// </summary>
        public string Senha { get; set; }
    }
}