namespace MathTools.Algebra.Functions
{
    public partial class Exp : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) * Exp(this.SubFormulae[0]);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Log log)
            {
                // Exp(Log(f(x)) -> f(x)
                return log.SubFormulae[0];
            }

            return base.SpecificSimplify();
        }
    }
}
