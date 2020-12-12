namespace Befunge_Interpretor
{
    public class OnOutpuEventArgs
    {
        public string Output { get; set; }

        public OnOutpuEventArgs(string output)
        {
            this.Output = output;
        }
    }
}