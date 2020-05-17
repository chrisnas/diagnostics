using Microsoft.Diagnostics.Repl;
using Microsoft.Diagnostics.Runtime;
using System;
using System.Text;
using ParallelStacks.Runtime;

namespace Microsoft.Diagnostics.Tools.Dump
{
    static class Constants
    {
        public const string PStacksDescription = "Aggregate the threads callstacks a la Visual Studio 'Parallel Stacks'";
        public const string DisplayThreadIDsCountLimitHelp = "Max number of thread IDs to display at each group level (4 by default)";
        public static string PStacksExampleForHelp = 
        "________________________________________________" + Environment.NewLine +
        "~~~~ 8f8c" + Environment.NewLine +
        "    1 (dynamicClass).IL_STUB_PInvoke(IntPtr, Byte*, Int32, Int32 ByRef, IntPtr)" + Environment.NewLine +
        "    ..." + Environment.NewLine +
        "    1 System.Console.ReadLine()" + Environment.NewLine +
        "    1 NetCoreConsoleApp.Program.Main(String[])" + Environment.NewLine +
        Environment.NewLine +
        "________________________________________________" + Environment.NewLine +
        "           ~~~~ 7034" + Environment.NewLine +
        "              1 System.Threading.Monitor.Wait(Object, Int32, Boolean)" + Environment.NewLine +
        "              ..." + Environment.NewLine +
        "              1 System.Threading.Tasks.Task.Wait()" + Environment.NewLine +
        "              1 NetCoreConsoleApp.Program+c.b__1_4(Object)" + Environment.NewLine +
        "           ~~~~ 9c6c,4020" + Environment.NewLine +
        "              2 System.Threading.Monitor.Wait(Object, Int32, Boolean)" + Environment.NewLine +
        "              ..." + Environment.NewLine +
        "                   2 NetCoreConsoleApp.Program+c__DisplayClass1_0.b__7()" + Environment.NewLine +
        "              3 System.Threading.Tasks.Task.InnerInvoke()" + Environment.NewLine +
        "         4 System.Threading.Tasks.Task+c.cctor>b__278_1(Object)" + Environment.NewLine +
        "         ..." + Environment.NewLine +
        "         4 System.Threading.Tasks.Task.ExecuteEntryUnsafe()" + Environment.NewLine +
        "         4 System.Threading.Tasks.Task.ExecuteWorkItem()" + Environment.NewLine +
        "    7 System.Threading.ThreadPoolWorkQueue.Dispatch()" + Environment.NewLine +
        "    7 System.Threading._ThreadPoolWaitCallback.PerformWaitCallback()" + Environment.NewLine +
        Environment.NewLine +
        "==> 8 threads with 2 roots"
        ;
    }

    [Command(Name = "pstacks", Help = Constants.PStacksDescription)]
    public class PStacks : CommandBase
    {
        public ClrRuntime Runtime { get; set; }

        [Argument(Help = Constants.DisplayThreadIDsCountLimitHelp)]
        public int? DisplayThreadIDsCountLimit { get; set; } = null;

        public override void Invoke()
        {
            var buffer = new StringBuilder(42 * 1000);
            var maxThreadIdsToDisplay = (DisplayThreadIDsCountLimit.HasValue) ? DisplayThreadIDsCountLimit.Value : 4;
            var visitor = new TextRenderer(buffer, maxThreadIdsToDisplay);
            var ps = ParallelStack.Build(Runtime);

            WriteLine("");
            foreach (var stack in ps.Stacks)
            {
                WriteLine("________________________________________________");
                stack.Render(visitor);
                WriteLine(buffer.ToString());
                WriteLine("");
                WriteLine("");
                WriteLine("");
            }

            WriteLine($"==> {ps.ThreadIds.Count} threads with {ps.Stacks.Count} roots{Environment.NewLine}");
        }

        [HelpInvoke]
        public void InvokeHelp()
        {
            WriteLine($"pstacks [{Constants.DisplayThreadIDsCountLimitHelp}]");
            WriteLine("");
            WriteLine(Constants.PStacksDescription);
            WriteLine(Constants.PStacksExampleForHelp);
        }
    }
}
