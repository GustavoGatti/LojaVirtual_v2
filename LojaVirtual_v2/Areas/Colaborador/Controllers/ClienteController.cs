using LojaVirtual_v2.Libraries.Filtro;
using LojaVirtual_v2.Libraries.Lang;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.Constants;
using LojaVirtual_v2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            this._clienteRepository = clienteRepository;
        }

        public IActionResult Index(int?pagina, string pesquisa)
        {
            IPagedList<Cliente> cliente = this._clienteRepository.ObterTodosClientes(pagina, pesquisa);

            return View(cliente);
        }

        [ValidateHttpReferer]
        public IActionResult AtivarDesativar(int id)
        {
            Cliente cliente = _clienteRepository.ObterCliente(id);

            if(cliente.Situacao == SituacaoConstant.Ativo)
            {
                cliente.Situacao = SituacaoConstant.Desativado;
            }
            else
            { 
                cliente.Situacao = SituacaoConstant.Ativo;
            }
            this._clienteRepository.Atualizar(cliente);
            TempData["MSG_S"] = Mensagem.MSG_S002;
            return RedirectToAction(nameof(Index));
        }
    }
}
