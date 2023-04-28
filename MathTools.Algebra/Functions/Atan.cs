namespace MathTools.Algebra.Functions
{
    public partial class Atan : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / (1.0 + Pow(this.SubFormulae[0], 2.0));
    }
}
