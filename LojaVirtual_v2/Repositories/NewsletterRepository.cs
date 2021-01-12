using LojaVirtual_v2.Database;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories
{
    public class NewsletterRepository : INewletterRepository
    {
        private LojaVirtualContext _banco;

        public NewsletterRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }

        public void Cadastrar(NewsletterEmail newsletter)
        {
            this._banco.NewsletterEmail.Add(newsletter);
            this._banco.SaveChanges();
        }

        public List<NewsletterEmail> ObterTodasNewsletter()
        {
            return this._banco.NewsletterEmail.ToList();
        }
    }
}
