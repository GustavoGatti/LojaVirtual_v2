using LojaVirtual_v2.Libraries.Arquivo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ImagemController : Controller
    {
        [HttpPost]
        public IActionResult Armazenar(IFormFile file)
        {
            //TODO - Lógica
            var Caminho = GerenciadorArquivo.CadastrarImagemProduto(file);
            if(Caminho.Length > 0)
            {
                return Ok(new { caminho = Caminho});//Converte para json -> Java script object Notation 
            }
            else
            {
                return new StatusCodeResult(500);
            }
        }

        public IActionResult Deletar(string caminho)
        {
            if (GerenciadorArquivo.ExcluirImagemProduto(caminho))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
