using LojaVirtual_v2.Libraries.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Models
{
    public class CartaoCredito
    {
        [Display(Name = "Número do Cartão")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string NumeroCartao { get; set; }

        [Display(Name = "Nome no cartão")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string NomeNoCartao { get; set; }

        [Display(Name = "Mês do Vencimento")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string VencimentoMM { get; set; }

        [Display(Name = "Ano do Vencimento")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string VencimentoYY { get; set; }

        [Display(Name = "Cód. segurança")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string CodigoSeguranca { get; set; }

    }
}
