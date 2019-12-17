using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheems
{
    
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Interpreter cheemsnator = new Interpreter();
            cheemsnator.Reset();

            if(cheemsnator.GetCode())
            {
                cheemsnator.Analyse();
            }
            Console.ReadKey();
            
        }
    }
}
