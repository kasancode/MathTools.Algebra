namespace MathTools.Algebra.Functions
{
    public partial class Sin : Formula
    {
        public override Formula Derive(string variable)
            => Cos(this.SubFormulae[0]) * this.SubFormulae[0].Derive(variable);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Asin asin)
            {
                return asin.SubFormulae[0];
            }
            return base.SpecificSimplify();
        }
    }
}
