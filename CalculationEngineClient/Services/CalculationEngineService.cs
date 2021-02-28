using CalculationEngine.Models;
using CalculationEngine.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngineClient.Services
{
    public interface ICalculationEngineService
    {
        Task<ExpressionResponse> CalculateResult(string[] strExpressionsArr, bool applyBEDMAS);
    }
    public class CalculationEngineService : ICalculationEngineService
    {
        ICalculateResultService _calculateResultService;
        public CalculationEngineService(ICalculateResultService calculateResultService)
        {
            _calculateResultService = calculateResultService;
        }
        public async Task<ExpressionResponse> CalculateResult(string[] strExpressionsArr, bool applyBEDMAS)
        {
            try
            {
                var response = await _calculateResultService.CalculateResult(strExpressionsArr, 0);
                //I am not factoring if BEDMAS should be applied when calling the new calculation engine as I strongly believe we have to apply BEDMAS
                //by default otherwise the results will be all wrong. So would like to challenge the user story for factoring in using BEDMAS as that should
                //be the default implementation and client shouldn't have a choice otherwise.
                if (response!= null && response.result != null)
                {
                    return response;
                }
                else if (!applyBEDMAS) //there is no point in calling A cool Calculation company's calculation methods 
                                       //if BEDMAS is to be applied as they dont implement it
                {
                    //A cool Calculation company's calculation methods should be used as our calculation engine failed to produce the result.
                    //For now we return the response with result as "Result received from A cool Calculation Company".
                    return new ExpressionResponse()
                    {
                        result = new List<string>() { "Result received from A cool Calculation Company" },
                        error = null
                    };
                }
                return response;
            }
            catch(Exception ex)
            {
                return new ExpressionResponse()
                {
                    result = null,
                    error = $"An error occurred while evaluating the expression - {ex.Message}"
                };
            }
        }

    }
}
