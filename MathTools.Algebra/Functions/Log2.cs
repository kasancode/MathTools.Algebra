namespace MathTools.Algebra.Functions
{
    public partial class Log2 : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / (Math.Log(2.0) * this.SubFormulae[0]);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Pow { SubFormulae: [Constant { Value: 2.0 }, var fx] })
            {
                // log2(2^f(x)) -> f(x)
                return fx;
            }

            return base.SpecificSimplify();
        }
    }
}
