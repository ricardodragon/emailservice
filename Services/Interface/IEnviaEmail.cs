using System.Threading.Tasks;
using emailservice.Model;

namespace emailservice.Service.Interface {
    public interface IEnviaEmail {
        Task<string> enviaEmail(Alarm alarm);
    }
}