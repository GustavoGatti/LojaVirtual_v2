using LojaVirtual_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Controllers
{
    public class ProdutoController : Controller
    {
         


        public ActionResult Visualizar()
        {
            Produto produto = GetProduto();
            //return new ContentResult() {Content = "Gustavo o rei do mundo", ContentType="text/html" };
            return View(produto);
        }

        private Produto GetProduto()
        {
            return new Produto()
            {
                Id=1,
                Nome = "Cadeira Gamer",
                Descricao = "Verde",
                Valor = 200.000M
            };
        }
    }
}
