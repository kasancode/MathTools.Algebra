namespace MathTools.Algebra.Functions
{
    public partial class Acosh : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / Sqrt(this.SubFormulae[0] - 1.0) / Sqrt(this.SubFormulae[0] + 1.0);
    }
}
