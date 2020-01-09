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

        /// <summary>
        /// Opens a file dialog window to select cheems code file and loads it's contents into the "code" variable
        /// </summary>
        /// <returns>Returns true if succesfull</returns>
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
        /// <returns>Returns true if the code is correct</returns>
        public bool Analyse()
        {           
            Queue<Token> tokenqueue;
            for (int lineCounter = 0; lineCounter < code.Count; lineCounter++)
            {
                tokenqueue = DoLexicalAnalysis(code.ElementAt(lineCounter));            
                if( !DoSyntacticalAnalysis(tokenqueue) )
                {
                    return false;
                }
            }            
            return true;
        }

        /// <summary>
        /// Analyses lexems in a code line and returns them as a queue of tokens
        /// </summary>
        /// <param name="line">A line of code to analise</param>
        /// <returns>Returns a queue of tokens from "line"</returns>
        private Queue<Token> DoLexicalAnalysis(string line)
        {
            Queue<Token> tokens = new Queue<Token>();
            Token tempToken;
            string[] lexems = line.Split(' ');//, '"');
            
            for (int i = 0; i < lexems.Length; i++)
            {
                switch(lexems[i])
                {
                    case "imt":
                        tempToken = new Token(Token.Type.dataType, "int");
                        break;
                    case "float":
                        tempToken = new Token(Token.Type.dataType, "float");
                        break;
                    case "strimg":
                        tempToken = new Token(Token.Type.dataType, "string");
                        break;
                    case "boleam":
                        tempToken = new Token(Token.Type.dataType, "bool");
                        break;
                    case "if":
                        tempToken = new Token(Token.Type.ifCondition, "if");
                        break;
                    case "while":
                        tempToken = new Token(Token.Type.whileCycle, "while");
                        break;
                    case "for":
                        tempToken = new Token(Token.Type.forCycle, "for");
                        break;
                    case "=":
                        tempToken = new Token(Token.Type.assignment, "=");
                        break;
                    case "==":
                        tempToken = new Token(Token.Type.compare, "==");
                        break;
                    case ">":
                        tempToken = new Token(Token.Type.compare, ">");
                        break;
                    case "=>":
                        tempToken = new Token(Token.Type.compare, "=>");
                        break;
                    case "<":
                        tempToken = new Token(Token.Type.compare, "<");
                        break;
                    case "=<":
                        tempToken = new Token(Token.Type.compare, "=<");
                        break;
                    case "+":
                        tempToken = new Token(Token.Type.mathOperation, "+");
                        break;
                    case "-":
                        tempToken = new Token(Token.Type.compare, "-");
                        break;
                    case "*":
                        tempToken = new Token(Token.Type.compare, "*");
                        break;
                    case "/":
                        tempToken = new Token(Token.Type.compare, "/");
                        break;                    
                    default: // Default has to check if it's a constant i.e. a number or if it's a variable name
                        int intResult;
                        float floatResult;

                        if (lexems[i][0] == '"' && lexems[i][lexems[i].Length - 1] == '"') // lexem is an string constant
                        {
                            tempToken = new Token(Token.Type.constant, lexems[i].Trim('"'));
                        }
                        else if(int.TryParse(lexems[i], out intResult)) //lexem is an int constant
                        {
                            tempToken = new Token(Token.Type.constant, intResult.ToString());
                        }
                        else if (float.TryParse(lexems[i], out floatResult)) // lexem is an float constant
                        {
                            tempToken = new Token(Token.Type.constant, intResult.ToString());
                        }
                        else //lexem is an variable id
                        {
                            tempToken = new Token(Token.Type.id, lexems[i]);
                        }
                        
                        break;
                }
                tokens.Enqueue(tempToken);              
            }
            return tokens;
        }

        /// <summary>
        /// Does a syntax analysis on a queue of tokens
        /// </summary>
        /// <param name="tokens">Queue of tokens</param>
        /// <returns>True or false depending on if the tokens are in valid order</returns>
        private bool DoSyntacticalAnalysis(Queue<Token> tokens)
        {
            int lexemAmount = tokens.Count;
            int index = 1;
            Token firstToken = tokens.Dequeue();
            Token currentToken = null;

            //Helper variables to determine if the syntax is correct
            Token.Type expectedType = Token.Type.nothing;
            Token.Type expectedType2 = Token.Type.nothing;
            bool isAssignmentDone = false;

            switch (firstToken.type) // Type of the first token determins the syntax rules that will be used
            {
                case Token.Type.ifCondition:
                    expectedType = Token.Type.id;
                    expectedType2 = Token.Type.constant;
                    break;
                case Token.Type.id:
                    expectedType = Token.Type.assignment;
                    break;
                case Token.Type.dataType:
                    expectedType = Token.Type.id;
                    break;
            }

            //"currentToken" var has to be the same value as the "expectedType" var
            //If the condition is passed the "expectedType" var updates 
            while (index < lexemAmount)
            {
                currentToken = tokens.Dequeue();

                //Syntax rules for variable definition and value assignment
                if(firstToken.type == Token.Type.id || firstToken.type == Token.Type.dataType)
                {
                    if (currentToken.type == expectedType || currentToken.type == expectedType2)
                    {
                        if (currentToken.type == Token.Type.assignment)
                        {
                            isAssignmentDone = true;
                            expectedType = Token.Type.id;
                            expectedType2 = Token.Type.constant;
                        }
                        else if (currentToken.type == Token.Type.id)
                        {
                            if (isAssignmentDone)
                            {
                                expectedType = Token.Type.mathOperation;
                                expectedType2 = Token.Type.nothing;
                            }
                            else
                            {
                                expectedType = Token.Type.assignment;
                                expectedType2 = Token.Type.nothing;
                            }
                        }
                        else if (currentToken.type == Token.Type.mathOperation)
                        {
                            expectedType = Token.Type.id;
                            expectedType2 = Token.Type.constant;
                        }                                               
                    }
                    else
                    {
                        Console.WriteLine("Definition line failed");
                        return false; // failed some of the rules
                    }
                }

                //Syntax rules for an if statement
                if (firstToken.type == Token.Type.ifCondition) 
                {                    
                    if(currentToken.type == expectedType)
                    {
                        if(currentToken.type == Token.Type.id || currentToken.type == Token.Type.constant)
                        {
                            expectedType = Token.Type.compare;
                            expectedType2 = Token.Type.nothing;
                        }
                        if(currentToken.type == Token.Type.compare)
                        {
                            expectedType = Token.Type.id;
                            expectedType2 = Token.Type.constant;
                        }
                    }
                    else
                    {
                        Console.WriteLine("if Line failed");
                        return false;                        
                    }

                }

                //Syntax rules for a while cycle
                if (firstToken.type == Token.Type.whileCycle)
                {
                    if(currentToken.type == expectedType)
                    {
                        if(currentToken.type == Token.Type.id || currentToken.type == Token.Type.constant)
                        {
                            expectedType = Token.Type.compare;
                            expectedType2 = Token.Type.nothing;
                        }
                        else if (currentToken.type == Token.Type.compare)
                        {
                            expectedType = Token.Type.id;
                            expectedType2 = Token.Type.constant;
                        }
                    }
                    else
                    {
                        Console.WriteLine("while line failed");
                        return false;
                    }
                }

                index++;
            }

            //final syntax rules for variable initialization
            //(initialization line must end with an id or constant)
            if (firstToken.type == Token.Type.id || firstToken.type == Token.Type.dataType)
            {
                if(!(currentToken.type == Token.Type.id || currentToken.type == Token.Type.constant))
                {
                    return false;
                }
            }

            Console.WriteLine("Line passed");
            //Console.WriteLine(currentToken.content);

            return true;
        }

    }
}
