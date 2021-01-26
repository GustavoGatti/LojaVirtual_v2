using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSCorreios;

namespace LojaVirtual_v2.Libraries.Gerenciador.Frete
{
    public class WSCorreiosCalcularFrete
    {
        private IConfiguration _configuration;
        private CalcPrecoPrazoWSSoap _servico;

        public WSCorreiosCalcularFrete(IConfiguration configuration, CalcPrecoPrazoWSSoap servico)
        {
            _configuration = configuration;
            this._servico = servico;
        }

        public async Task<ValorPrazoFrete> CalcularFrete(string cepDestino, string tipoFrete, List<Pacote> pacotes)
        {

            List<ValorPrazoFrete> valorPrazoFretes = new List<ValorPrazoFrete>();
            foreach (var pacote in pacotes)
            {
                var resultado = await CalcularValorPrazoFrete(cepDestino, tipoFrete, pacote);
                if (resultado != null)
                {
                    valorPrazoFretes.Add(resultado);
                }

            }
            if (valorPrazoFretes.Count > 0)
            {
                ValorPrazoFrete ValorDosFretes = valorPrazoFretes.GroupBy(a => tipoFrete).Select(
                   list => new ValorPrazoFrete
                   {
                       TipoFrete = list.First().TipoFrete,
                       CodigoTipoFrete = list.First().CodigoTipoFrete,
                       Prazo = list.Max(c => c.Prazo),
                       Valor = list.Sum(c => c.Valor)
                   }).ToList().First();
                return ValorDosFretes;
            }

            return null;
        }

        public async Task<ValorPrazoFrete> CalcularValorPrazoFrete(string cepDestino, string tipoFrete, Pacote pacote)
        {
            var cepOrigem = _configuration.GetValue<String>("Frete:CepOrigem");
            var maoPropria = _configuration.GetValue<String>("Frete:MaoPropria");
            var avisoRecebimento = _configuration.GetValue<String>("Frete:AvisoRecebimento");
            var diametro = Math.Max(Math.Max(pacote.Comprimento, pacote.Largura), pacote.Altura);

            //

            cResultado resultado = await _servico.CalcPrecoPrazoAsync("", "", tipoFrete, cepOrigem, cepDestino, pacote.Peso.ToString(), 1, pacote.Comprimento, pacote.Altura, pacote.Largura, diametro, maoPropria, 0, avisoRecebimento);

            if (resultado.Servicos[0].Erro == "0")
            {
                //Deu certo - implementar um resultado - valor e prazo de entrega, tipo frete

                return new ValorPrazoFrete()
                {
                    TipoFrete = TipoFreteConstante.GetNames(tipoFrete),
                    CodigoTipoFrete = tipoFrete,
                    Prazo = int.Parse(resultado.Servicos[0].PrazoEntrega),
                    Valor = double.Parse(resultado.Servicos[0].Valor.Replace(".", ""))
                };

            }
            else if (resultado.Servicos[0].Erro == "008" || resultado.Servicos[0].Erro == "-888")
            {
                //Exempo de codigo para n possuir entrega na região, ex: SEDEX10
                return null;
            }
            else
            {
                //Deu ruim
                throw new Exception("Erro: " + resultado.Servicos[0].MsgErro);
            }
        }

    }
}
