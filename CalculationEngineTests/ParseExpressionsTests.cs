using NUnit.Framework;
using CalculationEngine.Services;
using CalculationEngine.Expressions;

namespace CalculationEngineTests
{
    public class ParseExpressionsTests
    {
        ICallMathsAPIService _callMathsAPIService;
        IParseExpressions _parseExpressions;

        [SetUp]
        public void Setup()
        {
            _callMathsAPIService = new CallMathsAPIService();
            _parseExpressions = new ParseExpressions(_callMathsAPIService);
        }
        [Test]
        public void ParseSimpleArithmeticOperation_ExpectSimpleArithmeticExpressionObject()
        {
            string[] simpleExpression = new string[] { "2 +  1" };
            var resp = _parseExpressions.Parse(simpleExpression[0]);
            Assert.IsInstanceOf<SimpleArithmeticExpression>(resp);
        }
        [Test]
        public void ParseComplexArithmeticOperation_ExpectComplexArithmeticExpressionObject()
        {
            string[] complexExpression = new string[] { "2 + (3 * 5.5)" };
            var resp = _parseExpressions.Parse(complexExpression[0]);
            Assert.IsInstanceOf<ComplexArithmeticExpression>(resp);
        }
        [Test]
        public void ParseComplexOperation_ExpectComplexArithmeticExpressionObject()
        {
            string[] multipleExpressions = new string[] { "sin(45 deg) ^ 2" };
            var resp = _parseExpressions.Parse(multipleExpressions[0]);
            Assert.IsInstanceOf<ComplexArithmeticExpression>(resp);
        }


    }
}
