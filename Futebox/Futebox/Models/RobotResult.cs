using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class RobotResult
    {
        public RobotResultCommand command { get; set; }
        public List<string> stack { get; set; }

        public RobotResult(RobotResultCommand forceStatus, Exception ex)
        {
            this.command = forceStatus;
            this.stack = new List<string>();
            this.stack.Add(ex.Message);
        }
        public RobotResult()
        {
            this.command = RobotResultCommand.BLANK;
            this.stack = new List<string>();
        }

        public void ReadLine(string command)
        {
            command = (command ?? "");
            if (command.StartsWith("!"))
            {
                if (command.StartsWith("!OK")) this.command = RobotResultCommand.OK;
                else if (command.StartsWith("!ERROR")) this.command = RobotResultCommand.ERROR;
                else if (command.StartsWith("!AUTHFAILED")) this.command = RobotResultCommand.AUTHFAILED;
                else if (command.StartsWith("!BLANK")) this.command = RobotResultCommand.BLANK;
                else if (command.StartsWith("!INVALID")) this.command = RobotResultCommand.INVALID;
                else this.command = RobotResultCommand.BLANK;
            }
            if (!string.IsNullOrEmpty(command.Trim()))
            {
                this.stack.Add(command);
                EyeLog.Log(command);
            }
        }
    }

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
        }
        public void Add(string mensagem)
        {
            if(metodo == null) stack.Add($"{index++}. {mensagem}");
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
}
