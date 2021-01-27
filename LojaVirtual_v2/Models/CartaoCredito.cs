using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models
{
    public class CartaoCredito
    {
        public string NumeroCartao { get; set; }
        public string NomeNoCartao { get; set; }
        public string Vencimento { get; set; }
        public string CodigoSeguranca { get; set; }

    }
}
