using NUnit.Framework;
using CalculationEngineClient.Services;
using CalculationEngine.Services;
using CalculationEngine.Expressions;

namespace CalculationEngineClientTests
{
    public class Tests
    {
        private ICalculationEngineService calculationEngineService;
        ICallMathsAPIService _callMathsAPIService;
        IParseExpressions _parseExpressions;
        ICalculateResultService _calculateResultService;
        IExpressions _expression;

        [SetUp]
        public void Setup()
        {
            _callMathsAPIService = new CallMathsAPIService();
            _parseExpressions = new ParseExpressions(_callMathsAPIService);
            _calculateResultService = new CalculateResultService(_parseExpressions, _callMathsAPIService, _expression);
            calculationEngineService = new CalculationEngineService(_calculateResultService);
        }

        [Test]
        public void ErroredOperationSentToCoolCalcComp_BEDMAS_NotApplied()
        {
            var expression = new string[] { "25.5/0" };

            var resp = calculationEngineService.CalculateResult(expression, false);

            Assert.AreEqual("Result received from A cool Calculation Company", resp.Result.result[0]);
        }
        [Test]
        public void ErroredOperationNOTSentToCoolCalcComp_BEDMAS_Applied()
        {
            var expression = new string[] { "25.5/0" };

            var resp = calculationEngineService.CalculateResult(expression, true);

            Assert.IsNull(resp.Result.result);
        }
    }
}