﻿using LojaVirtual_v2.Models;
using LojaVirtual_v2.Models.ProdutoAgregador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Gerenciador.Frete
{
    public class CalcularPacote
    {
        public List<Pacote> CalcularPacotesDeProdutos(List<ProdutoItem> produtos)
        {
            List<Pacote> pacotes = new List<Pacote>();
            Pacote pacote = new Pacote();
            foreach (var item in produtos)
            {
                for (int i = 0; i < item.QuantidadeProdutoCarrinho; i++)
                {
                    var peso = pacote.Peso + item.Peso;
                    var comprimento = (pacote.Comprimento > item.Comprimento) ? pacote.Comprimento : item.Comprimento;
                    var largura = (pacote.Largura > item.Largura) ? pacote.Largura : item.Largura;
                    var altura = pacote.Altura + item.Altura;
                    var dimensao = comprimento + largura + altura;

                    //TODO - Criar novo pacote caso: peso > 30kg, dimensao > 200cm
                    if (peso > 30 || dimensao > 200)
                    {
                        pacotes.Add(pacote);
                        pacote = new Pacote();
                    }

                    pacote.Peso = pacote.Peso + item.Peso;
                    pacote.Comprimento = (pacote.Comprimento > item.Comprimento) ? pacote.Comprimento : item.Comprimento;
                    pacote.Largura = (pacote.Largura > item.Largura) ? pacote.Largura : item.Largura;
                    pacote.Altura = altura = pacote.Altura + item.Altura;

                }

            }
            pacotes.Add(pacote);
            return pacotes;
        }
    }
}
