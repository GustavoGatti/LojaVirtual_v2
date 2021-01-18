using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private LojaVirtualContext _banco;
 

        public ImagemRepository(LojaVirtualContext banco)
        {
            this._banco = banco;
        }

        public void Cadastrar(Imagem imagem)
        {
            this._banco.Imagems.Add(imagem);
            this._banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            Imagem imagem = this._banco.Imagems.Find(id);
            this._banco.Remove(imagem);
            this._banco.SaveChanges();
        }

        public void ExcluirImagemDoProduto(int ProdutoId)
        {
            List<Imagem> imagens = this._banco.Imagems.Where(x => x.ProdutoId == ProdutoId).ToList();
            foreach(Imagem imagem in imagens)
            {
                this._banco.Imagems.Remove(imagem);
            }
            this._banco.SaveChanges();
        }

        public void CadastrarImagens(List<Imagem> CaminhoImagens, int id)
        {
            if(CaminhoImagens != null && CaminhoImagens.Count > 0)
            {
                foreach (var CaminhoDef in CaminhoImagens)
                {
                    Cadastrar(CaminhoDef);
                }
            }
            
        }
    }
}
