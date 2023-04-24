namespace MathTools.Algebra.Functions
{
    public partial class Tan : Formula
    {
        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable) / Pow(Cos(this.SubFormulae[0]), 2.0);

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Atan atan)
            {
                return atan.SubFormulae[0];
            }
            return base.SpecificSimplify();
        }
    }
}
