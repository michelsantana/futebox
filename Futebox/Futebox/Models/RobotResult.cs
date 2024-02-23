using System.Collections.Generic;
using System.Net;

namespace Futebox.Models
{
    public class RobotResultApi
    {
        public HttpStatusCode status { get; set; }
        public string metodo { get; set; }
        public List<string> stack { get; set; }
        private int index { get; set; }
        public RobotResultApi(string metodo = null)
        {
            status = HttpStatusCode.Unused;
            stack = new List<string>();
            index = 0;
            this.metodo = metodo;
        }
        public void Add(string mensagem)
        {
            if (metodo == null) stack.Add($"{index++}. {mensagem}");
            else stack.Add($"{index++}. {metodo} - {mensagem}");
        }

        public RobotResultApi Ok()
        {
            this.status = HttpStatusCode.OK;
            return this;
        }

        public bool IsOk()
        {
            return this.status == HttpStatusCode.OK;
        }

        public RobotResultApi Error()
        {
            this.status = HttpStatusCode.InternalServerError;
            return this;
        }

        public bool IsError()
        {
            return this.status == HttpStatusCode.InternalServerError;
        }

        public RobotResultApi Unauthorized()
        {
            this.status = HttpStatusCode.Unauthorized;
            return this;
        }

        public bool IsUnauthorized()
        {
            return this.status == HttpStatusCode.Unauthorized;
        }
    }

    public class NodeServiceResult
    {
        public bool status { get; set; }
        public string[] inputParams { get; set; }
        public string[] output { get; set; }
        public string[] steps { get; set; }
        public object error { get; set; }
    }
}
