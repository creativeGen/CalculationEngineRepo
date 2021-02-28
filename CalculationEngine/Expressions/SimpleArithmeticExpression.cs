using CalculationEngine.Models;
using CalculationEngine.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Expressions
{
    public class SimpleArithmeticExpression : IExpressions
    {
        public string Expression { get; set; }
        public char Operator { get; set; }
        //We are not using the expressions parameter passed as this will be removed from the method definition of interface IEXpressions
        //when we stop using the ChainOfExpressions class. Every class will have its own properties required to evaluiate the expressions
        //it is meant to calculate the results for.
        public async Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision)
        {
            try
            {
                return await Task.Run(() => ParseMDASExpression(Expression, Operator, precision)) ;
            }
            catch (Exception ex)
            {
                ExpressionResponse resp = new ExpressionResponse()
                {
                    result = null,
                    error = $"Error occurred while evaluating the expression - {ex.Message}"
                };
                return resp;
            }
        }

        private ExpressionResponse ParseMDASExpression(string expression, char arithmeticOperator, int? precision)
        {
            var strArray = expression.Split(arithmeticOperator);
            var result = ArithmeticOperations.EvaluateMDAS(strArray[0], arithmeticOperator, strArray[1]);
            if (result != null)
            {
                return new ExpressionResponse() { error = null, result = new List<string>() { result.ToString() } };
            }
            else
            {
                return new ExpressionResponse() { error = $"There was an error evaluating the expression - {expression}", result = null };
            }
        }

    }
}
