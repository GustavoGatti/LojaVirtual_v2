using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual_v2.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private IConfiguration _conf;
        private LojaVirtualContext _banco;
        private List<Categoria> ListaCategoriaRecursiva = new List<Categoria>();
        private List<Categoria> categorias;

        public CategoriaRepository(LojaVirtualContext banco, IConfiguration conf)
        {
            this._conf = conf;
            this._banco = banco;
        }

        public void Atualizar(Categoria categoria)
        {
            this._banco.Categorias.Update(categoria);
            this._banco.SaveChanges();
        }

        public void Cadastrar(Categoria categoria)
        {
            this._banco.Add(categoria);
            this._banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Categoria categoria = ObterCategoria(Id);
            this._banco.Categorias.Remove(categoria);
            this._banco.SaveChanges();
        }

        public Categoria ObterCategoria(int Id)
        {
            return this._banco.Categorias.Find(Id);
        }
    
        public IEnumerable<Categoria> ObterCategoriasRecursivamente(Categoria categoriaPai)
        {
            if(this.categorias == null)
            {
                this.categorias = ObterTodasCategorias().ToList();
            }
          

            if (!ListaCategoriaRecursiva.Exists(x => x.ID == categoriaPai.ID))
            {
                ListaCategoriaRecursiva.Add(categoriaPai);
            }

            var ListaCategoriaFilho = categorias.Where(x => x.CategoriaPaiId == categoriaPai.ID);
            if (ListaCategoriaFilho.Count() > 0)
            {
                ListaCategoriaRecursiva.AddRange(ListaCategoriaFilho.ToList());
                foreach (var categoria in ListaCategoriaFilho)
                {
                    ObterCategoriasRecursivamente(categoria);
                }
            }
            return ListaCategoriaRecursiva;
        }

        public IPagedList<Categoria> ObterTodasCategorias(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return this._banco.Categorias.Include(a => a.CategoriaPai).ToPagedList<Categoria>(NumeroPagina, this._conf.GetValue<int>("RegistroPorPagina"));
        }

        public IEnumerable<Categoria> ObterTodasCategorias()
        {
            return this._banco.Categorias;
        }

        public Categoria ObterTodasCategorias(string slug)
        {
            return this._banco.Categorias.Where(x => x.Slug == slug).FirstOrDefault();
        }
    }
}
