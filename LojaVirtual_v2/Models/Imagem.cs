using LojaVirtual_v2.Models.ProdutoAgregador;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtual_v2.Models
{
    public class Imagem
    {
        public int Id { get; set; }
        public string Caminho { get; set; }
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; }

    }
}