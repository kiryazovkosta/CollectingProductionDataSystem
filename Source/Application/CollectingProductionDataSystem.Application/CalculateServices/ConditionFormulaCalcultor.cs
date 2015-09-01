using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Application.CalculateServices
{
    public class ConditionFormulaCalcultor : IConditionFormulaCalcultor
    {
        private static readonly IList<char> arithmeticOperations = new List<char>() { '+', '-', '*', '/' };

        private static readonly IList<char> brackets = new List<char> { '(', ')' };

        private static readonly IList<string> functions = new List<string> { "ln", "pow", "sqrt" };

        private string TrimInput(string input)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != ' ')
                {
                    result.Append(input[i]);
                }
            }

            return result.ToString();
        }

        private List<string> SeparateTokens(string input)
        {
            List<string> result = new List<string>();
            var number = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '-' && (i == 0 || input[i] == ',' || input[i] == '('))
                {
                    number.Append('-');
                }
                else if(char.IsDigit(input[i]) || input[i] == '.')
                {
                    number.Append(input[i]);
                }
                else if (!char.IsDigit(input[i]) && input[i] != '.' && number.Length != 0)
                {
                    result.Add(number.ToString());
                    number.Clear();
                    i--;
                }
                else if (brackets.Contains(input[i]))
                {
                    result.Add(input[i].ToString());
                }
                else if (arithmeticOperations.Contains(input[i]))
                {
                    result.Add(input[i].ToString());
                }
                else if(input[i] == ',')
                {
                    result.Add(",");
                }
                else if (i + 1 < input.Length && input.Substring(i, 2).ToLower() == "ln")
                {
                    result.Add("ln");
                    i++;
                }
                else if (i + 2 < input.Length && input.Substring(i, 3).ToLower() == "pow")
                {
                    result.Add("pow");
                    i += 2;
                }
                else if (i + 3 < input.Length && input.Substring(i, 4).ToLower() == "sqrt")
                {
                    result.Add("sqrt");
                    i += 3;
                }
                else
                {
                    throw new ArgumentException("Invalid expression");
                }
            }

            if (number.Length != 0)
            {
                result.Add(number.ToString());
            }

            return result;
        }

        public static int Precedence(string arithmeticOperator)
        {
            if (arithmeticOperator == "+" || arithmeticOperator == "-")
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public Queue<string> CovertToReversePolishNotation(IList<string> tokens)
        {
            var stack = new Stack<string>();
            var queue = new Queue<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                var currentToken = tokens[i];
                double number;

                if (double.TryParse(currentToken, out number))
                {
                    queue.Enqueue(currentToken);
                }
                else if(functions.Contains(currentToken))
                {
                    stack.Push(currentToken);
                }
                else if (currentToken == ",")
                {
                    if (!stack.Contains("(") || stack.Count == 0)
                    {
                        throw new ArgumentException("Invalid brackets or function separator");
                    }

                    while(stack.Peek() != "(")
                    {
                        queue.Enqueue(stack.Pop());
                    }
                }
                else if (arithmeticOperations.Contains(currentToken[0]))
                {
                    while (stack.Count != 0 && arithmeticOperations.Contains(stack.Peek()[0]) && Precedence(currentToken) <= Precedence(stack.Peek()))
                    {
                        queue.Enqueue(stack.Pop()); 
                    }

                    stack.Push(currentToken);
                }
                else if (currentToken == "(")
                {
                    stack.Push("(");
                }
                else if (currentToken == ")")
                {
                    if (!stack.Contains("(") || stack.Count == 0)
                    {
                        throw new ArgumentException("Invalid brackets position");
                    }

                    while (stack.Peek() != "(")
                    {
                        queue.Enqueue(stack.Pop());
                    }

                    stack.Pop();

                    if (stack.Count != 0 && functions.Contains(stack.Peek()))
                    {
                        queue.Enqueue(stack.Pop());  
                    }
                }
            }

            while(stack.Count != 0)
            {
                if (brackets.Contains(stack.Peek()[0]))
                {
                    throw new Exception("Invalid brackets position");
                }
                queue.Enqueue(stack.Pop());
            }

            return queue;
        }

        public decimal GetResultFromRpn(Queue<string> queue)
        {
            var stack = new Stack<double>();
            while (queue.Count != 0)
            {
                var currentToken = queue.Dequeue();

                double number;
                if (double.TryParse(currentToken, out number))
                {
                    stack.Push(number);
                }
                else if (arithmeticOperations.Contains(currentToken[0]) || functions.Contains(currentToken))
                {
                    if (currentToken == "+")
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double firstValue = stack.Pop();
                        double secondValue = stack.Pop();

                        stack.Push(firstValue + secondValue);
                    }
                    else if (currentToken == "-")
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double firstValue = stack.Pop();
                        double secondValue = stack.Pop();

                        stack.Push(secondValue - firstValue);   
                    }
                    else if (currentToken == "*")
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double firstValue = stack.Pop();
                        double secondValue = stack.Pop();

                        stack.Push(secondValue * firstValue);
                    }
                    else if (currentToken == "/")
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double firstValue = stack.Pop();
                        double secondValue = stack.Pop();

                        stack.Push(secondValue / firstValue);
                    }
                    else if (currentToken == "pow")
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double firstValue = stack.Pop();
                        double secondValue = stack.Pop();

                        stack.Push(Math.Pow(secondValue, firstValue));
                    }
                    else if (currentToken == "sqrt")
                    {
                        if (stack.Count < 1)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double value = stack.Pop();

                        stack.Push(Math.Sqrt(value));
                    }
                    else if (currentToken == "ln")
                    {
                        if (stack.Count < 1)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double value = stack.Pop();

                        stack.Push(Math.Log(value));
                    }
                }
            }

            if (stack.Count == 1)
            {
                return Convert.ToDecimal(stack.Pop());
            }
            else
            {
                throw new ArgumentException("Invalid expression");
            }
        }

        public static void SetInvariantCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        public decimal Calc(string expression)
        {
            SetInvariantCulture();
            string trimmedInput = TrimInput(expression);
            var saparatedTokens = SeparateTokens(trimmedInput);
            var rpn = CovertToReversePolishNotation(saparatedTokens);
            var result = GetResultFromRpn(rpn);
            return result;
        }
    }
}
