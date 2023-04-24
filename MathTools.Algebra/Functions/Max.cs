namespace MathTools.Algebra.Functions
{
    public partial class Max : Formula
    {
        public override Formula Derive(string variable)
            => If(
                this.SubFormulae[0] - this.SubFormulae[1],
                this.SubFormulae[0].Derive(variable),
                this.SubFormulae[1].Derive(variable));

    }
}
