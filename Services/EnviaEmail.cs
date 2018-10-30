
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.Extensions.Configuration;
using emailservice.Model;
using emailservice.Service.Interface;
using MailKit.Security;

namespace emailservice.Service {
    public class EnviaEmail : IEnviaEmail{
        private readonly IConfiguration _configuration;
        public readonly IConfiguration configuration;   
        public EnviaEmail(IConfiguration configuration){
            this.configuration = configuration;
        }
        
        public async Task<string> enviaEmail(Alarm alarm){                                                
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SPI", configuration["FromMail"]));
            message.To.Add(new MailboxAddress("Mahle", configuration["UrlGroupMailsList"]));
            message.Subject = configuration["Assunto"];
            message.Body = new TextPart("plain"){
                Text = "Descrição do alarme : " + alarm.alarmDescription + " Nome do alarme : " + alarm.alarmName + " Data do alarme : " + alarm.datetime + 
                    " Equipamento : " + alarm.thing.thingName
            };        
            using(var client = new SmtpClient()){
                client.Connect(configuration["SmtpServerHost"], Convert.ToInt32(configuration["Port"]), false);
                //client.Authenticate(configuration["CredentialsEmail"], configuration["CredentialsUser"]);                    
                client.Send(message);
                client.Disconnect(true);
            }   
            return "Enviado";                 
        }
    }
}