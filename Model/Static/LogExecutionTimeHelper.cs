using System.Diagnostics;

namespace Model.Static
{
    public class LogExecutionTimeHelper
    {
        Stopwatch _stopwatch;
        public LogExecutionTimeHelper()
        {
            _stopwatch = new Stopwatch();
        }
        // public void WriteLog(string stepName)
        // {
        //     Console.WriteLine($"{stepName} hoàn thành trong {_stopwatch.ElapsedMilliseconds} ms");
        //     _stopwatch.Restart();
        // }
    }
}