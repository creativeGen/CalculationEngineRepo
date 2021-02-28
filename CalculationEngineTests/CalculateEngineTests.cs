using NUnit.Framework;
using CalculationEngine.Services;
using CalculationEngine.Expressions;

namespace CalculationEngineTests
{
    public class CalculateEngineTests
    {
        ICallMathsAPIService _callMathsAPIService;
        IParseExpressions _parseExpressions;
        ICalculateResultService _calculateResultService;
        IExpressions _expression;
        CalculationEngine.CalculationEngine calcEngine;
        [SetUp]
        public void Setup()
        {
            _callMathsAPIService = new CallMathsAPIService();
            _parseExpressions = new ParseExpressions(_callMathsAPIService);
            _calculateResultService = new CalculateResultService(_parseExpressions, _callMathsAPIService, _expression);
            calcEngine = new CalculationEngine.CalculationEngine(_calculateResultService);
        }

        [Test]
        public void AddTwoNumbersExpectSuccess()
        {
            string[] singleExpressions = new string[] { "2 +  1" };
            var resp = calcEngine.CalculateResult(singleExpressions);
            Assert.AreEqual("3", resp.Result.result[0]);
        }
        [Test]
        public void SubtractTwoNumbersExpectSuccess()
        {
            string[] singleExpressions = new string[] { "22.2 - 11.1" };
            var resp = calcEngine.CalculateResult(singleExpressions);
            Assert.AreEqual("11.1", resp.Result.result[0]);
        }
        [Test]
        public void MultiplyTwoNumbersExpectSuccess()
        {
            string[] singleExpressions = new string[] { "4.5 * 5.5" };
            var resp = calcEngine.CalculateResult(singleExpressions);
            Assert.AreEqual("24.75", resp.Result.result[0]);
        }
        [Test]
        public void DivideTwoNumbersExpectSuccess()
        {
            string[] singleExpressions = new string[] { "20.25 / 4.5" };
            var resp = calcEngine.CalculateResult(singleExpressions);
            Assert.AreEqual("4.5", resp.Result.result[0]);
        }
        [Test]
        public void DivideByZeroExpectResultNULL()
        {
            string[] singleExpressions = new string[] { "20.25 / 0" };
            var resp = calcEngine.CalculateResult(singleExpressions);
            Assert.IsNull(resp.Result.result);
        }

        [Test]
        public void ChainMultipleExpressionExpectSuccess()
        {
            string[] multipleExpressions = new string[] { "a = 1.2 * (2 + 4.5)",
                                                          "a / 2",
                                                          "5.08 cm in inch",
                                                          "sin(45 deg) ^ 2",
                                                          "9 / 3 + 2i",
                                                          "b = [-1, 2; 3, 1]",
                                                          "det(b)" };
            var resp = calcEngine.CalculateResult(multipleExpressions);
            Assert.AreEqual("3.9", resp.Result.result[1]);
        }
        [Test]
        public void ChainMultipleExpressionBEDMASExpectSuccess()
        {
            string[] multipleExpressions = new string[] { "a = 2+3*sqrt(4)",
                                                          "a / 2",
                                                          "5.08 cm in inch",
                                                          "sin(45 deg) ^ 2",
                                                          "9 / 3 + 2i",
                                                          "b = [-1, 2; 3, 1]",
                                                          "det(b)" };
            var resp = calcEngine.CalculateResult(multipleExpressions);
            Assert.AreEqual("4", resp.Result.result[1]);
        }
        [Test]
        public void IncorrectExpressionExpectException()
        {
            string[] multipleExpressions = new string[] { "a +b + c" };         
            var resp = calcEngine.CalculateResult(multipleExpressions);
            Assert.IsNull(resp.Result.result);
        }

    }
}