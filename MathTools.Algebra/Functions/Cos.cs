namespace MathTools.Algebra.Functions
{
    public partial class Cos : Formula
    {
        public override Formula Derive(string variable)
            => -Sin(this.SubFormulae[0]) * this.SubFormulae[0].Derive(variable);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Acos acos)
            {
                return acos.SubFormulae[0];
            }
            return base.SpecificSimplify();
        }
    }
}
