namespace MathTools.Algebra.Functions
{
    public partial class MaxMagnitude : Formula
    {
        public override Formula Derive(string variable)
        {
            return If(
                Abs(this.SubFormulae[0]) - Abs(this.SubFormulae[1]),
                this.SubFormulae[0].Derive(variable),
                this.SubFormulae[1].Derive(variable));
        }
    }
}
