using Futebox.Models;
using Newtonsoft.Json;
using Quartz;
using System.Threading.Tasks;

namespace Futebox.Services.Jobs
{
    public class HttpJob : IJob
    {
        HttpHandler _http = new HttpHandler().SetTimeoutMinutes(10);
        
        public Task Execute(IJobExecutionContext context)
        {
            return Execution(context);
        }

        private async Task Execution(IJobExecutionContext context)
        {
            var processo = context.MergedJobDataMap["processo"]?.ToString();
            var respostaDaApi = await _http.PostAsync($"{Settings.ApplicationHttpBaseUrl}/api/processo/{processo}/start", null);
            
            if (respostaDaApi.IsSuccessStatusCode)
            {
                var jsonRetornadoApi = respostaDaApi.Content?.ReadAsStringAsync()?.Result;
                var deserializado = JsonConvert.DeserializeObject<RobotResultApi>(jsonRetornadoApi);
                EyeLog.LogObject(deserializado);
            }
        }
    }
}
