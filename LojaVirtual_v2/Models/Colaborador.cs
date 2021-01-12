using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        /**
         * Tipo 
         * C- comum
         * g- gerente
         */
        public string Tipo { get; set; }
    }
}
