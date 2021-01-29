using System;
using System.Collections.Generic;
using System.Linq;
using PagarMe;
using Newtonsoft.Json;
using PagarMe.Model;
using Microsoft.Extensions.Configuration;
using LojaVirtual_v2.Libraries.Login;
using LojaVirtual_v2.Models;
using LojaVirtual_v2.Libraries.Texto;
using LojaVirtual_v2.Models.ProdutoAgregador;

namespace LojaVirtual_v2.Libraries.Gerenciador.Pagamento.PagarMe
{
    public class GerenciarPagarMe
    {

        private IConfiguration _configuration;
        private LoginCliente _loginCliente;

        public GerenciarPagarMe(IConfiguration configuration, LoginCliente loginCliente)
        {
            _loginCliente = loginCliente;
            _configuration = configuration;
        }


        //gugostoso23@gmail.com
        //4khJa8rpP^!U
        public object GerarBoleto(decimal valor)
        {
            try
            {
                Cliente cliente = this._loginCliente.GetCliente();
                PagarMeService.DefaultApiKey = this._configuration.GetValue<string>("Pagamento:PagarMe:ApiKey");
                PagarMeService.DefaultEncryptionKey = this._configuration.GetValue<string>("Pagamento:PagarMe:EncryptionKey");

                Transaction transaction = new Transaction();

                //TODO - Calcular o valor total
                transaction.Amount = Convert.ToInt32(valor);
                transaction.PaymentMethod = PaymentMethod.Boleto;

                transaction.Customer = new Customer
                {
                    ExternalId = cliente.Id.ToString(),
                    Name = cliente.Nome,
                    Type = CustomerType.Individual,
                    Country = "br",
                    Email = cliente.Email,
                    Documents = new[]
                    {
                    new Document{
                      Type = DocumentType.Cpf,
                      Number =  Mascara.Remover(cliente.CPF)
                    },
                    new Document{
                      Type = DocumentType.Cnpj,
                      Number = "89388916000174"
                    }
                  },
                    PhoneNumbers = new string[]
                        {
                        "+55" + Mascara.Remover(cliente.Telefone),
                        },
                    Birthday = cliente.DataNasc.ToString("yyyy-MM-dd")
                };

                transaction.Save();

                return new
                {
                    BoletoURL = transaction.BoletoUrl,
                    BarCode = transaction.BoletoBarcode,
                    Expiracao = transaction.BoletoExpirationDate,
                };
            }
            catch (Exception e)
            {
                return new { Erro = e.Message};
            }
           
        }

        public object GerarPagCartaoCredito(CartaoCredito cartao, EnderecoEntrega enderecoEntrega, ValorPrazoFrete frete, List<ProdutoItem> produtos)
        {

            Cliente cliente = this._loginCliente.GetCliente();
            PagarMeService.DefaultApiKey = this._configuration.GetValue<string>("Pagamento:PagarMe:ApiKey");
            PagarMeService.DefaultEncryptionKey = this._configuration.GetValue<string>("Pagamento:PagarMe:EncryptionKey");

            Card card = new Card();
            card.Number = cartao.NumeroCartao;
            card.HolderName = cartao.NomeNoCartao;
            card.ExpirationDate = cartao.VencimentoMM + cartao.VencimentoYY;
            card.Cvv = cartao.CodigoSeguranca;
            card.Save();

            Transaction transaction = new Transaction();

            transaction.Amount = 2100;
            transaction.Card = new Card
            {
                Id = card.Id
            };

            transaction.Customer = new Customer
            {
                ExternalId = "#3311",
                Name = "Rick",
                Type = CustomerType.Individual,
                Country = "br",
                Email = "rick@morty.com",
                Documents = new[]
              {
                new Document{
                  Type = DocumentType.Cpf,
                  Number = Mascara.Remover(cliente.CPF)
                },
                new Document{
                  Type = DocumentType.Cnpj,
                  Number = "83134932000154"
                }
              },
                PhoneNumbers = new string[]
              {
                    "+55"+ Mascara.Remover(cliente.Telefone),
                   
              },
                Birthday = new DateTime(1991, 12, 12).ToString("yyyy-MM-dd")
            };

            transaction.Billing = new Billing
            {
                Name = cliente.Nome,
                Address = new Address()
                {
                    Country = "br",
                    State = cliente.Estado,
                    City = cliente.Cidade,
                    Neighborhood = cliente.Bairro,
                    Street = cliente.Endereco + " " + cliente.Complemento,
                    StreetNumber = cliente.Numero,
                    Zipcode = Mascara.Remover(cliente.CEP)
                }
            };

            var Today = DateTime.Now;
            //TODO - Converter corretamente o valor para a API do pagar.me
            transaction.Shipping = new Shipping
            {
                Name = enderecoEntrega.Nome,
                Fee = Convert.ToInt32(frete.Valor),
                DeliveryDate = Today.AddDays(this._configuration.GetValue<int>("Frete:DiasPreparo")).AddDays(frete.Prazo).ToString("yyyy-MM-dd"),
                Expedited = false,
                Address = new Address()
                {
                    Country = "br",
                    State = enderecoEntrega.Estado,
                    City = enderecoEntrega.Cidade,
                    Neighborhood = enderecoEntrega.Bairro,
                    Street = enderecoEntrega.Endereco + " " + enderecoEntrega.Complemento,
                    StreetNumber = enderecoEntrega.Numero,
                    Zipcode = enderecoEntrega.CEP
                }
            };

            Item[] itens = new Item[produtos.Count];
            //TODO - Converter corretamente o valor para a API do pagar.me
            for (var i = 0; i < produtos.Count; i++)
            {
                var item = produtos[i];
                var itemA = new Item()
                {
                    Id = item.Id.ToString(),
                    Title = item.Nome,
                    Quantity = item.QuantidadeProdutoCarrinho,
                    Tangible = true,
                    UnitPrice = Convert.ToInt32(item.Valor)
                };
                itens[i] = itemA;

            }

            transaction.Item = itens;

            transaction.Save();
            return null;
        }
    }
}
