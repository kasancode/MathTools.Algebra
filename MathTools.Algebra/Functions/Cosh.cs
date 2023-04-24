namespace MathTools.Algebra.Functions
{
    public partial class Cosh : Formula
    {
        public override Formula Derive(string variable)
            => -Sinh(this.SubFormulae[0]) * this.SubFormulae[0].Derive(variable);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Acosh acosh)
            {
                return acosh.SubFormulae[0];
            }
            return base.SpecificSimplify();
        }
    }
}
