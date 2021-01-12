using LojaVirtual_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LojaVirtual_v2.Libraries.Email
{
    public class ContatoEmail
    {
        public static void EnviarContatoPorEmail(Contato contato)
        {
            /*
             *SMTP -> servidor que vai enviar a mensagem 
             */
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("gugostoso23@gmail.com","Narutolove12345");
            smtp.EnableSsl = true;

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
            mensagem.From = new MailAddress("gugostoso23@gmail.com"); //quem envia
            mensagem.To.Add("gugostoso23@gmail.com");
            mensagem.Subject = "Contato - LojaVirtual - E-mail: " + contato.Email;
            mensagem.Body = msg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via smtp
            smtp.Send(mensagem);
            
        }
    }
}
