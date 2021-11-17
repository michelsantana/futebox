using Futebox.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class RobotResult
    {
        public RobotResultCommand command { get; set; }
        public string commandLine { get; set; }
        public string[] args { get; set; }
        private object _arg { get; set; }

        public T ConvertArg<T>() where T : class
        {
            if (command == RobotResultCommand.RESULT)
            {
                if (_arg == null) _arg = JsonConvert.DeserializeObject<T>(args[0]);
                return (T)_arg;
            }
            return null;
        }

        public RobotResult(string command)
        {
            this.commandLine = command;
            string[] args = this.commandLine.Split(' ');
            if (args[0] == "!RESULT") this.command = RobotResultCommand.RESULT;
            else if (args[0] == "!ERROR") this.command = RobotResultCommand.ERROR;
            else if (args[0] == "!TRUE") this.command = RobotResultCommand.ISTRUE;
            else if (args[0] == "!FALSE") this.command = RobotResultCommand.ISFALSE;
            else this.command = RobotResultCommand.NONE;
            this.args = args.Skip(1).ToArray();
        }
    }
}
