﻿using LojaVirtual_v2.Models.ProdutoAgregador;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Models.ViewModels.Components
{
    public class ProdutoListagemViewModels
    {
        public IPagedList<Produto> lista { get; set; }
        public List<SelectListItem> ordenacao
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem("Alfabetica", "A"),
                    new SelectListItem("Menor preço", "ME"),
                    new SelectListItem("Maior preço", "MA"),
                };
            }
            private set { }
        }
    }
}
