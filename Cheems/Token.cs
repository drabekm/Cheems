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

        public enum DataType
        {
            integer, str, floatPoint, boolean, operatoR, comperator, noDataType
        }

        public Token(Type type, DataType dataType, string content)
        {
            this.type = type;
            this.dataType = dataType;
            this.content = content;
        }

        public Type type { get; }
        public DataType dataType { get; }
        public string content { get; set; }

        

        public override string ToString()
        {
            return "<" + type + "> " + content;
        }

    }
}
