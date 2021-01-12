using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Repositories.Contracts
{
    public interface INewletterRepository
    {
        void Cadastrar(NewsletterEmail newsletter);

        List<NewsletterEmail> ObterTodasNewsletter();
    }
}
