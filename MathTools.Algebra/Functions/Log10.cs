namespace MathTools.Algebra.Functions
{
    public partial class Log10 : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / (Math.Log(10.0) * this.SubFormulae[0]);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Pow { SubFormulae: [Constant { Value: 10.0}, var fx] })
            {
                // log10(10^f(x)) -> f(x)
                return fx;
            }

            return base.SpecificSimplify();
        }
    }
}
