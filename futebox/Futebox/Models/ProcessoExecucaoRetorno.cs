//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;

//namespace Futebox.Models
//{
//    public class ProcessoExecucaoRetorno
//    {
//        public HttpStatusCode status { get; set; }
//        public string metodo { get; set; }
//        public List<string> stack { get; set; }
//        private int index { get; set; }

//        public ProcessoExecucaoRetorno(string metodo = null)
//        {
//            status = HttpStatusCode.Unused;
//            stack = new List<string>();
//            index = 0;
//            this.metodo = metodo;
//        }

//        public void Add(string mensagem)
//        {
//            if (metodo == null) stack.Add($"{index++}. {mensagem}");
//            else stack.Add($"{index++}. {metodo} - {mensagem}");
//        }

//    }
//}
