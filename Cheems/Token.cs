using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheems
{
    class Token
    {
        public enum Types
        {
            id, integer, floatingPoint, strng, 
        }

        public int type { get; set; }
        public string content { get; set; }

        public Token(int type, string content)
        {
            this.type = type;
            this.content = content;
        }

    }
}
