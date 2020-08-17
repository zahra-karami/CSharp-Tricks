using System;
using System.Linq;
using System.Timers;

namespace CsharpTricks.MemoryManagement
{

    /*
     * Managed memory leaks are caused by unused objects remaining alive by
     * virtue of unused or forgotten references.
    */
    public class MemoryLeakByUnusedObj
    {
        static Host _host = new Host();
        public static void CreateClients()
        {
            Client[] clients = Enumerable.Range(0, 1000)
                .Select(i => new Client(_host))
                .ToArray();
            // Do something with clients ...


            // this code avoid memory leak on this example
            Array.ForEach(clients, c => c.Dispose());
        }

        class Host
        {
            public event EventHandler Click;
        }

        /*
         * each client has another referee:
         * the _host object whose Click event now references each Client instance.
         * This may go unnoticed if the Click event does not fire
         * _or if the HostClicked method does not do anything to attract attention.
         *  this code can cause memory leak in application
         */
        class Client : IDisposable
        {
            private readonly Host _host;
            public Client(Host host)
            {
                _host = host;
                _host.Click += HostClicked;
            }

            void HostClicked(object sender, EventArgs e)
            {
                // doing something
            }

            public void Dispose() { _host.Click -= HostClicked; }
        }
    }




    /*
     * In the following example, the Foo class (when instantiated) calls
     * the tmr_Elapsed method once every second:
     * the instances of SampleTimer can never be garbage-collected! The problem is
     * the .NET Framework itself holds references to active timers so that it can fire their Elapsed events.
     */
    public class MemoryLeakForgottenTimer : IDisposable
    {
        readonly Timer _timer;
        MemoryLeakForgottenTimer()
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += tmr_Elapsed;
            _timer.Start();
        }

        void tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }




}
