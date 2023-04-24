namespace MathTools.Algebra.Functions
{
    public partial class Sinh : Formula
    {
        public override Formula Derive(string variable)
            => Cosh(this.SubFormulae[0]) * this.SubFormulae[0].Derive(variable);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Asinh asinh)
            {
                return asinh.SubFormulae[0];
            }
            return base.SpecificSimplify();
        }
    }
}
