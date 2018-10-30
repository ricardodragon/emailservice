
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
using System.Net.Http;
using Newtonsoft.Json;

namespace emailservice.Service {
    public class EnviaEmail : IEnviaEmail{
        private readonly IConfiguration _configuration;
        private HttpClient client = new HttpClient();
        public readonly IConfiguration configuration;   
        public EnviaEmail(IConfiguration configuration){
            this.configuration = configuration;
        }
        
        public async Task<Thing> getThings(string id){
            var builder = new UriBuilder(configuration["things"]);                        
            return JsonConvert.DeserializeObject<Thing>(await client.GetStringAsync(builder.ToString()+id));
        }

        public async Task<string> enviaEmail(Alarm alarm){                           
            alarm.thing = await getThings(alarm.thingId.ToString());
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SPI", configuration["FromMail"]));
            message.To.Add(new MailboxAddress("Mahle", configuration["UrlGroupMailsList"]));            
            message.Subject = configuration["Assunto"];
            string cabecalho = configuration["cabecalho"];
            string rodape = configuration["rodape"];   
            message.Body = new TextPart("plain"){
                Text =  cabecalho + "\n\nEquipamento : " + alarm.thing.thingName + "\nNome do alarme : " + alarm.alarmName + 
                    "\nDescrição do alarme : "+ alarm.alarmDescription+"\nData do alarme : "+new DateTime(alarm.datetime).ToString("dd/MM/yyyy HH:mm")+
                    "\n\n"+ rodape                    
            };        
            using(var client = new SmtpClient()){
                client.Connect(configuration["SmtpServerHost"], Convert.ToInt32(configuration["Port"]), false);
                client.Authenticate(configuration["CredentialsEmail"], configuration["CredentialsUser"]);                    
                client.Send(message);
                client.Disconnect(true);
            }   
            return "Enviado";                 
        }
    }
}