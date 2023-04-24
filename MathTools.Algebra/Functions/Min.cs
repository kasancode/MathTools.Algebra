namespace MathTools.Algebra.Functions
{
    public partial class Min : Formula
    {
        public override Formula Derive(string variable)
            => If(
                this.SubFormulae[1] - this.SubFormulae[0],
                this.SubFormulae[0].Derive(variable),
                this.SubFormulae[1].Derive(variable));
    }
}
