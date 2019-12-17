using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Cheems
{
    class Interpreter
    {
        private LinkedList<string> code;

        public Interpreter()
        {
            code = new LinkedList<string>();
        }

        /// <summary>
        /// Resets the interpreter for new code execution
        /// </summary>
        public void Reset()
        {
            code = new LinkedList<string>();
        }

        public bool GetCode()
        {
            string filePath;
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Cheems code files (*.chms)|*.chms";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;                    
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {                      
                        filePath = openFileDialog.FileName;

                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            string line = "";
                            while((line = reader.ReadLine()) != null)
                            {
                                code.AddLast(line);
                            }                            
                        }
                    }
                }
                return true;
            }
            catch
            {
                Console.WriteLine("Error ocurred while reading from a code file");
                return false;
            }


        }

        /// <summary>
        /// Analyses the supplied code and checks if it's valid.
        /// </summary>
        /// <returns></returns>
        public bool Analyse()
        {
            /* Iterates line after line. 
             * DoLexycalLine get's tokens from a supplied line and returns them in a queue.
             * That stack of tokens is then given to a DoSyntactical function that does more analysis duh
             */

            Queue<Token> tokenqueue;
            for (int lineCounter = 0; lineCounter < code.Count; lineCounter++)
            {
                tokenqueue = DoLexicalLine(code.ElementAt(lineCounter));
                DoSyntacticalLine(tokenqueue);
            }
            

            return true;
        }

        /// <summary>
        /// Analyses lexems in a code line and returns them as a queue of tokens
        /// </summary>
        /// <param name="line">A line of code to analise</param>
        /// <returns>Queue of tokens</returns>
        private Queue<Token> DoLexicalLine(string line)
        {
            Queue<Token> tokens = new Queue<Token>();
            string[] lexems = line.Split(' ', '"');
            Token token;
            for (int i = 0; i < lexems.Length; i++)
            {
                switch(lexems[i])
                {
                    case "imt":
                        token = new Token( (int)Token.Types.integer, "imt");
                        break;
                    case "float":
                        token = new Token((int)Token.Types.floatingPoint, "float");
                        break;
                    case "strimg":
                        token = new Token((int)Token.Types.strng, "strimg");
                        break;
                    default:
                        token = new Token((int)Token.Types.id, lexems[i]);
                        break;
                }
                tokens.Enqueue(token);              
            }
            return tokens;
        }

        /// <summary>
        /// Does a syntax analysis on a queue of tokens
        /// </summary>
        /// <param name="tokens">Queue of tokens</param>
        /// <returns>True or false depending on if the tokens are in valid order</returns>
        private bool DoSyntacticalLine(Queue<Token> tokens)
        {
            int lexemAmount = tokens.Count;
            int index = 0;
            Token currentToken = tokens.Dequeue();
            while (index < lexemAmount - 1)
            {
                //TODO: remove this shit
                Console.WriteLine(currentToken.content);
                currentToken = tokens.Dequeue();
                index++;
            }
            Console.WriteLine(currentToken.content);

            return true;
        }

    }
}
