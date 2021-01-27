using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Texto
{
    public class Mascara
    {
        public static string Remover(string valor)
        {
            return valor.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "");
        }
    }
}
