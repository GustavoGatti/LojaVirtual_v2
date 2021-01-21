using LojaVirtual_v2.Libraries.Lang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models.ProdutoAgregador
{
    public class Produto
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public string Nome { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public decimal Valor { get; set; }

        [Display(Name = "Descrição")]
        
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Range(0, 10000, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [JsonIgnore]
        public int Quantidade { get; set; }

        //Frete
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Range(0.001, 30, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [JsonIgnore]
        public double Peso { get; set; }


        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Range(11, 105, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [JsonIgnore]
        public int Largura { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Range(2, 105, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [JsonIgnore]
        public int Altura { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Range(16, 105, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [JsonIgnore]
        public int Comprimento { get; set; }

        //EF - ORM - Biblioteca Unir - Banco de dados e POO - (ORM - Mapeamento de Objetos <-> Relacionamento)
        //Fluent Api - Attributes
        //Banco de dados - Relacionamento entre tabela
        [Display(Name = "Categoria")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        [JsonIgnore]
        //Poo - Associações entre objetos
        public virtual Categoria Categoria { get; set; }

        [JsonIgnore]
        public virtual ICollection<Imagem> Imagems { get; set; }


    }
}
