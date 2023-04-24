namespace MathTools.Algebra.Functions
{
    public partial class Sqrt : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / (2.0 * Sqrt(this.SubFormulae[0]));
    }
}
