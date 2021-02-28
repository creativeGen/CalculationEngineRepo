using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationEngine.Operations
{
    /// <summary>
    /// This static class will evaluate different types of operations.
    /// </summary>
    public static class ArithmeticOperations
    {
        public  static decimal? EvaluateMDAS(string num1, char arithmeticOperator, string num2)
        {
            try
            {
                decimal deci1, deci2;
                if (decimal.TryParse(num1, out deci1) && decimal.TryParse(num2, out deci2))
                {
                    switch (arithmeticOperator)
                    {
                        case '+': return deci1 + deci2;
                        case '-': return deci1 - deci2;
                        case '*': return deci1 * deci2;
                        case '/': return deci1 / deci2;
                        default: return null;
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
