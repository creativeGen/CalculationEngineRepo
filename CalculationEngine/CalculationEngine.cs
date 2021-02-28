using CalculationEngine.Models;
using CalculationEngine.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class CalculationEngine
    {
        private ICalculateResultService _calculateResultService;
        public CalculationEngine(ICalculateResultService calculateResultService)
        {
            _calculateResultService = calculateResultService;
        }
        public async Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision = 0)
        {
            try
            {
                return await _calculateResultService.CalculateResult(expressions, precision);
            }
            catch(Exception ex)
            {
                return new ExpressionResponse()
                {
                    result = null,
                    error = $"An error occurred while calculating the expression - {ex.Message}"
                };
            }
        }

    }
}
