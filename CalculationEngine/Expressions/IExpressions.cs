using CalculationEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Expressions
{
    public interface IExpressions
    {
        //The parameter string[] expressions will not be required after ChainOfExpressions class is removed
        //as we will set the properties like expression and other fields required to evaluate the expression 
        //while returning the respective class for the expression from ParseExpression.
        Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision);
    }
}
