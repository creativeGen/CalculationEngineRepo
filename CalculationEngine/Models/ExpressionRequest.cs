using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationEngine.Models
{
    public class ExpressionRequest
    {
        public string[] expr { get; set; }
        public int precision { get; set; }
    }
}
