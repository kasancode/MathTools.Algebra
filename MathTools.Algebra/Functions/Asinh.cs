namespace MathTools.Algebra.Functions
{
    public partial class Asinh : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / Sqrt(Pow(this.SubFormulae[0], 2.0) + 1.0);
    }
}
