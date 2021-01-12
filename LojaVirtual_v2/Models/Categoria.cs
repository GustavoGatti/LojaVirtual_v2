using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models
{
    public class Categoria
    {
        public int ID { get; set; }
        public string Nome { get; set; }

        /*
         * Nome: Telefone sem fio
         * Slug: telefone-sem-fio
         * wwww.lojavirtual.com.br/categoria/5 (url normal)
         * wwww.lojavirtual.com.br/categoria/informatica (url amigavel)
         */

        public string Slug { get; set; }

        /*
         * Auto-relacionamento
         * Informatica
         *  -mouse
         *  -mouse sem fio
         *  -mouse gamer
         */
        public int?  CategoriaPaiId { get; set; }


    }
}
