using Microsoft.Diagnostics.Repl;
using Microsoft.Diagnostics.Runtime;

namespace Microsoft.Diagnostics.Tools.Dump
{
    [Command(Name = "clrversion", Help = "Display CLR version details.")]
    public class ClrVersion : CommandBase
    {
        public ClrRuntime Runtime { get; set; }

        public override void Invoke()
        {
            WriteLine($"CLR Version: {Runtime.ClrInfo.Version}");
        }
    }
}
