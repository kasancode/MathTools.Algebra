namespace MathTools.Algebra.Functions
{
    public partial class Atan2 : Formula
    {
        public override Formula Derive(string variable)
            => (this.SubFormulae[0] * this.SubFormulae[1].Derive(variable)
            - this.SubFormulae[0].Derive(variable) * this.SubFormulae[1])
            / (Pow(this.SubFormulae[0], 2.0) + Pow(this.SubFormulae[1], 2.0));
    }
}
