using System.Text;

namespace MathTools.Algebra.Functions
{
    public partial class MinMagnitude : Formula
    {
        public override Formula Derive(string variable)
        {
            return If(
                Abs(this.SubFormulae[1]) - Abs(this.SubFormulae[0]),
                this.SubFormulae[0].Derive(variable),
                this.SubFormulae[1].Derive(variable));
        }
    }
}
