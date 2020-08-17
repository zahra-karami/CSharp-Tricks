using System;
using System.Diagnostics;


namespace CsharpTricks.MemoryManagement
{
    public class MemoryConsumption
    {
        public void GetCpuMemoryUsage()
        {
            var procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            // dotnet add package System.Diagnostics.PerformanceCounter 
            using PerformanceCounter pc = new PerformanceCounter("Process", "Private Bytes", procName);
            Console.WriteLine(pc.NextValue());
        }


        /*
         * in test-driven development, one possibility is to use unit tests to
         * assert that memory is reclaimed as expected. If such an assertion fails,
         * you then have to examine only the changes that you’ve made recently
         */

        public void GetGCMemoryUsage()
        {
            long memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine(memoryUsed);

        }

    }
}
