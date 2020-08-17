using System;
using System.Runtime;

namespace CsharpTricks.MemoryManagement
{
    public class TuningGarbageCollection
    {

        public void SetToDefault()
        {
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
        }

        public void SetToLowLatency()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            /*
             * this config instructs the CLR to favor quicker (but more frequent) collections.
             * This is useful if your application needs to respond very quickly to real-time events
             */
        }

        public void SuspendGC()
        {
            long totalsize = 100_000_000;

            // temporarily suspend GC until total amount
            GC.TryStartNoGCRegion(totalsize);

            //
            // doing something
            //

            // resume the default functionality
            GC.EndNoGCRegion();
        }

    }


    public class GarbageCollectionForce
    {
        /*
         * To ensure the collection of objects for which collection is delayed by finalizers,
         * you can take the additional step of calling WaitForPendingFinalizers and recollecting
         * we get the best performance by allowing the GC to decide when to collect
         * but in some cases like when an application goes to sleep for a while: a good example is a Windows Service
         * this scenario can free memory after completing the activity
         */

        public void CallGarbageCollectionExplicitly()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
