using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models.ProdutoAgregador
{
    public class ProdutoItem: Produto
    {
        /*
         * Quantidade de produtos no carrinho
         */
        public int QuantidadeProdutoCarrinho { get; set; }

    }
}
