namespace MathTools.Algebra.Functions
{
    public partial class Log : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / this.SubFormulae[0];

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Exp exp)
            {
                // log(exp(f(x))) -> f(x)
                return exp.SubFormulae[0];
            }

            return base.SpecificSimplify();
        }
    }
}
