using LojaVirtual_v2.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Email
{
    public class GerenciarEmail
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;

        public GerenciarEmail(SmtpClient smtp, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._smtp = smtp;
        }

        public void EnviarContatoPorEmail(Contato contato)
        {

            /*
             * MailMessage -> construir a mensagem
             */

            string msg = string.Format("<h2> Contato - Loja Virtual</h2>" +
                "<b> Nome: </b> {0} <br/>"+
                "<b> Email: </b> {1} <br/>" +
                "<b> Texto: </b> {2} <br/>" +
                "<br/> Email enviado automaticamente do site Loja Virtual.",
                contato.Nome,
                contato.Email,
                contato.Texto
             );

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:UserName")); //quem envia
            mensagem.To.Add("gugostoso23@gmail.com");
            mensagem.Subject = "Contato - LojaVirtual - E-mail: " + contato.Email;
            mensagem.Body = msg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via smtp
            _smtp.Send(mensagem);
        }

        public void EnviarSenhaParaColaboradorPorEmail(Colaborador colaborador)
        {
            string msg = string.Format("<h2> Colaborador - Loja Virtual</h2>" +
                "Sua senha é:" +
                "<h3>{0}</h3>", colaborador.Senha
            );

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:UserName")); //quem envia
            mensagem.To.Add(colaborador.Email);
            mensagem.Subject = "Colaborador - LojaVirtual - Senha do colaborador: " + colaborador.Nome;
            mensagem.Body = msg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via smtp
            _smtp.Send(mensagem);
        }
    }
}
