using CalculationEngine.Models;
using CalculationEngine.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Expressions
{
    public class ComplexArithmeticExpression : IExpressions
    {
        ICallMathsAPIService _callMathsAPIService;
        public ComplexArithmeticExpression(ICallMathsAPIService callMathsAPIService)
        {
            _callMathsAPIService = callMathsAPIService;
        }
        public async Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision)
        {
            return await _callMathsAPIService.CalculateResult(expressions, precision);
        }
    }
}
