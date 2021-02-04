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
            return valor.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("R$", "").Replace(",", "").Replace(" ", "");
        }
        /*
         * PagarMe recebe o valor no seguinte formato: 3310, que representa R$ 33,10 
         */
        // 33.1 PagarMe- 3310
        public static int ConverterValorPagarMe(decimal valor)
        {
            string novoValor = valor.ToString("C");
            novoValor = Remover(novoValor);

            int ValorConvertido = int.Parse(novoValor);
            return ValorConvertido;
        }
    }
}
