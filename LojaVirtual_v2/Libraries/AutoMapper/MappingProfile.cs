using AutoMapper;
using LojaVirtual_v2.Models.ProdutoAgregador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.AutoMapper
{
    public class MappingProfile: Profile
    {

        public MappingProfile()
        {
            CreateMap<Produto, ProdutoItem>();
        }
    }
}
