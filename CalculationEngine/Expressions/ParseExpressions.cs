using CalculationEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculationEngine.Expressions
{
    public interface IParseExpressions
    {
        IExpressions Parse(string expression);
    }
    public class ParseExpressions : IParseExpressions
    {
        ICallMathsAPIService _callMathsAPIService;
        public ParseExpressions(ICallMathsAPIService callMathsAPIService)
        {
            _callMathsAPIService = callMathsAPIService;
        }
        public IExpressions Parse(string expression)
        {
            //see if the expression is like 2+3, 4.5*5.5, 33*3, 44/2, 2+(4*5) etc
            if (ParseForMDAS(expression))
            {
                //Parse further to see if this expression has multiple numbers and operators +,-,* or /
                return CheckForSimpleArithmeticExpression(expression);
            }
            else
            {
                //Parse for other expressions. 
                //For now, return the ComplexArithmeticExpression object as this will handle other complex operations
                return new ComplexArithmeticExpression(_callMathsAPIService);
            }
        }

        private IExpressions CheckForSimpleArithmeticExpression(string expression)
        {
            char[] operators = { '+', '-', '*', '/' };
            var formattedStr = RemoveWhitespaces(expression);

            List<char> expressionOperators = new List<char>();

            for (int i = 0; i < formattedStr.Length; i++)
            {
                if (operators.Contains(formattedStr[i]))
                {
                    expressionOperators.Add(formattedStr[i]);
                }
            }
            if (expressionOperators.Count == 1)
            {
                //looks like a simple arithmetic expression involving 2 numbers and one operator
                return new SimpleArithmeticExpression() { Expression = formattedStr, Operator = expressionOperators[0] };
            }
            else
            {
                //Although a MDAS expression, this is not straight forward +, -, *, / arithmetic operation as it has multiple numbers and operators
                //Will have to implement BEDMAS for this expression
                //So a separate class to handle complex arithmetic operations is required.
                //as implementing the logic for such complex operations is out of scope for the MVP/POC,
                //calling the MathsAPI to get the results.
                //Passing the dependency ICallMathsAPIService will not be required once we replace all the calculation methods in our Calculation engine. 
                return new ComplexArithmeticExpression(_callMathsAPIService);
            }
        }

        private static bool ParseForMDAS(string expression)
        {
            //This regex only matches if the expression is like 2+3, 4.5*5.5, 33*3, 44/2, 2+(4*5) but not sin(45 deg)^2, a=2+3 etc
            //Thanks to this article for help with this regex - https://stackoverflow.com/questions/9443291/regular-expression-for-validating-arithmetic-expression
            var regex = new Regex(@"(?x)
                ^
                (?> (?<p> \( )* (?>-?\d+(?:\.\d+)?) (?<-p> \) )* )
                (?>(?:
                    [-+*/]
                    (?> (?<p> \( )* (?>-?\d+(?:\.\d+)?) (?<-p> \) )* )
                )*)
                (?(p)(?!))
                $
            ");
            string formattedStr = RemoveWhitespaces(expression);
            if (!regex.IsMatch(formattedStr))
            {
                return false;
            }

            return true;
        }

        private static string RemoveWhitespaces(string expression)
        {
            return new string(expression
                                    .Where(c => !Char.IsWhiteSpace(c))
                                    .ToArray());
        }
    }
}
