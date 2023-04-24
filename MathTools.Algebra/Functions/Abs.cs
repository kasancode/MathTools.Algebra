namespace MathTools.Algebra.Functions
{
    public partial class Abs : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0] / Abs(this.SubFormulae[0]) * this.SubFormulae[0].Derive(variable);
    }
}
