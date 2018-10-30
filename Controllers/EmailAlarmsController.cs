using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using emailservice.Model;
using emailservice.Service.Interface;

namespace emailservice.Controllers{

    [Route("api/[controller]")]
    public class EmailAlarmsController : Controller{
        public IConfiguration configuration;
        public IEnviaEmail iEnviaEmail;
        public EmailAlarmsController(IConfiguration configuration, IEnviaEmail iEnviaEmail){            
            this.configuration = configuration;
            this.iEnviaEmail = iEnviaEmail;
        }
        
        [HttpPost()]
        public async Task<IActionResult> Get([FromBody] Alarm alarm){
           Console.WriteLine(alarm.thingId); 
           await iEnviaEmail.enviaEmail(alarm);           
           return Ok("Email enviado");
        }       
    }
}
