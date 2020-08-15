using System;
using System.Diagnostics;


namespace CsharpTricks.MemoryManagement
{
    public class MemoryConsumption
    {
        public void GetMemoryUsage()
        {
            var procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            // dotnet add package System.Diagnostics.PerformanceCounter 
            using PerformanceCounter pc = new PerformanceCounter("Process", "Private Bytes", procName);
            Console.WriteLine(pc.NextValue());
        }
    }
}
