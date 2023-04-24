using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTools.Algebra
{
    public class FormulaException : Exception
    {
        public FormulaException() { }
        public FormulaException(string message) : base(message) { }
    }
}
