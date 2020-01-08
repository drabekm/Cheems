using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheems
{
    class Token
    {
        public enum Type
        {
            dataType, constant, id, whileCycle, forCycle, ifCondition,
            assignment, compare, mathOperation, nothing 
        }

        public Type type { get; set; }
        public string content { get; set; }

        public Token(Type type, string content)
        {
            this.type = type;
            this.content = content;
        }

        public override string ToString()
        {
            return "<" + type + "> " + content;
        }

    }
}
