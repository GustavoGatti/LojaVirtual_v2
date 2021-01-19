using LojaVirtual_v2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Arquivo
{
    public class GerenciadorArquivo
    {
        public static string CadastrarImagemProduto(IFormFile file)
        {
            var NomeArquivo = Path.GetFileName(file.FileName);
            var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", NomeArquivo);

            using (var stream = new FileStream(Caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("/uploads/temp", NomeArquivo).Replace("\\", "/");
        }

        public static bool ExcluirImagemProduto(string path)
        {
            string caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path.Trim('/'));
            if (File.Exists(caminho))
            {
                //TODO - Deletar
                File.Delete(caminho);
                return true;
            }
            else
            {
                return false;
            }

        }

        public static List<Imagem> MoverImagensProduto(List<string> ListaCaminhoTemp, int id)
        {
            /*
             * Criar a pasta do produto
             */
            var CaminhoDefinitivoPastaProduto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", id.ToString());
            if (!Directory.Exists(CaminhoDefinitivoPastaProduto))
            {
                Directory.CreateDirectory(CaminhoDefinitivoPastaProduto);
            }

            /*
             * Mover a Imagem da Pasta Temp para a pasta definitiva
             */
            List<Imagem> ListaCaminhoDef = new List<Imagem>();
            foreach (var ImagemTemp in ListaCaminhoTemp)
            {
                if (!string.IsNullOrEmpty(ImagemTemp))
                {
                    // uploads / temp / imagem - name.jpg

                    var NomeArquivo = Path.GetFileName(ImagemTemp);
                    var CaminhoDef = Path.Combine("/uploads", id.ToString(), NomeArquivo).Replace("\\", "/");

                    if(CaminhoDef != ImagemTemp)
                    {
                        var CaminhoAbsolutoTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/temp", NomeArquivo);
                        var CaminhoAbsolutoDef = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", id.ToString(), NomeArquivo);

                        if (File.Exists(CaminhoAbsolutoTemp))
                        {

                            if (File.Exists(CaminhoAbsolutoDef))
                            {
                                File.Delete(CaminhoAbsolutoDef);
                            }

                            File.Copy(CaminhoAbsolutoTemp, CaminhoAbsolutoDef);

                            if (File.Exists(CaminhoAbsolutoDef))
                            {
                                File.Delete(CaminhoAbsolutoTemp);
                            }
                            ListaCaminhoDef.Add(new Imagem() { Caminho = Path.Combine("/uploads", id.ToString(), NomeArquivo).Replace("\\", "/"), ProdutoId = id });
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        ListaCaminhoDef.Add(new Imagem() { Caminho = Path.Combine("/uploads", id.ToString(), NomeArquivo).Replace("\\", "/"), ProdutoId = id });
                    }

                    

                }

            }
            return ListaCaminhoDef;

        }

        public static void ExcluirImagensProduto(List<Imagem> lists)
        {
            int ProdutoId = 0;
            foreach( var imagem in lists)
            {
                ExcluirImagemProduto(imagem.Caminho);
                ProdutoId = imagem.ProdutoId;
            }

            var PastaProduto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", ProdutoId.ToString());

            if (Directory.Exists(PastaProduto))
            {
                Directory.Delete(PastaProduto);
            }
        }
    }
}
