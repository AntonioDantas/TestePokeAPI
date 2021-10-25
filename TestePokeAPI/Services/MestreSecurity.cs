using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using TestePokeAPI.Models;

namespace TestePokeAPI.Services
{
    public class MestreSecurity
    {
        public static bool Login(string Cpf, string Senha)
        {
            using (MestreDbContext entities = new MestreDbContext())
            {
                var hash = new Hash(SHA512.Create());
                string hashSenha = hash.CriptografarSenha(Senha);
                return entities.Mestres.Any(user =>
               user.Cpf.Equals(Cpf, StringComparison.OrdinalIgnoreCase)
               && user.Senha == hashSenha);
            }
        }
    }
}