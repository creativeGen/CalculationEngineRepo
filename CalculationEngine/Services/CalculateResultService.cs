using CalculationEngine.Expressions;
using CalculationEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculationEngine.Services
{
    public interface ICalculateResultService
    {
        Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision);
    }
    public class CalculateResultService : ICalculateResultService
    {
        IParseExpressions _parseExpressions;
        ICallMathsAPIService _callMathsAPIService;
        IExpressions _expressions;
        public CalculateResultService(IParseExpressions parseExpressions, ICallMathsAPIService callMathsAPIService, IExpressions expressions)
        {
            _parseExpressions = parseExpressions;
            _callMathsAPIService = callMathsAPIService;
            _expressions = expressions;
        }
        public async Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision)
        {
            /*when calculations are to be implemented in this calculation engine instead of using the Maths API,
            every expression in the chain will be either a SimpleArithmeticSingleExpression or ComplexArithmeticSingleExpression or we may create further
            classes to handle specific expressions based on the Parser output 
            so all we have to do is call ReturnExpressionResults for every expression in the chain of expressions and it should return us the object 
            of the specific class that will handle that expression
            For now, below are the categories
             -> SimpleArithmeticSingleExpression is an expression involving 2 numbers and only one operator like +, -, * or /
             -> ComplexArithmeticSingleExpression is an expression involving multiple numbers with multiple operators where BEDMAS will be applied
             -> ComplexArithmeticSingleExpression is also an expression like sqrt(4), sin(45 deg)^2 etc but we may create separate class for this

            This is how the implementation will look like -
            var expressionResponse = new ExpressionResponse();
            foreach(var expression in expressions)
            {
                var response = await ReturnExpressionResults(expression, precision);
                if(response!=null)
                {
                    expressionResponse.result.Add(response.result.FirstOrDefault());
                }
                else
                {
                    expressionResponse.result = null;
                    expressionResponse.error = $"An error occurred while evaluating this expression - {expression}";
                }
            }
            return expressionResponse; */

            //Until then we use the below to resolve the chain of expressions -
            if (expressions.Length == 1)
            {
                return await ReturnExpressionResults(expressions, precision);
            }
            else //This class ChainOfExpressions will not be required after we implement as described above.
                 //for now, create this class and call Maths API within.
            {
                _expressions = new ChainOfExpressions(_callMathsAPIService);
                return await _expressions.CalculateResult(expressions, precision);
            }
        }

        private async Task<ExpressionResponse> ReturnExpressionResults(string[] expressions, int? precision)
        {
            //First we Parse the expression to get the object that will evaluate the expression.
            var expressionType = _parseExpressions.Parse(expressions[0]);
            var resp = await expressionType.CalculateResult(expressions, precision);
            return resp;
        }
    }
}
