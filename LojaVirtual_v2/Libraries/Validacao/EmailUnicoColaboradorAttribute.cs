using LojaVirtual_v2.Models;
using LojaVirtual_v2.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Validacao
{
    public class EmailUnicoColaboradorAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //TODO - Obter o repository do colaborador, fazer verificação

            string Email = (value as string).Trim();
            IColaboradorRepository _colaborador = (IColaboradorRepository)validationContext.GetService(typeof(IColaboradorRepository));
            List<Colaborador> colaboradores = _colaborador.ObterColaboradorPorEmail(Email);

            Colaborador objetoColaborador = (Colaborador)validationContext.ObjectInstance;

        
            if (colaboradores.Count > 1)
            {
                return new ValidationResult("Email já existente");
            } else if (colaboradores.Count == 1 && objetoColaborador.Id != colaboradores[0].Id)
            {
                return new ValidationResult("Email já existente");
            }
            return ValidationResult.Success;
        }
    }
}
