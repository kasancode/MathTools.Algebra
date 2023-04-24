namespace MathTools.Algebra.Functions
{
    public partial class Tanh : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / Pow(Cosh(this.SubFormulae[0]), 2.0);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Atanh atanh)
            {
                return atanh.SubFormulae[0];
            }
            return base.SpecificSimplify();
        }
    }
}
