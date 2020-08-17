using System;
using System.Collections.Concurrent;
using System.IO;


namespace CsharpTricks.MemoryManagement
{
    public class Finalizer : IDisposable
    {
        public readonly FileStream FileS;

        public Finalizer(string filePath)
        {
            FileS = new FileStream(filePath, FileMode.Open);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //Prevent finalizer from running
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Call Dispose() on other objects owned by this instance.
                // You can reference other finalizable objects here.
            }

            ((IDisposable)FileS)?.Dispose();
        }

        ~Finalizer()
        {
            Dispose(false);

        }
    }
    
    public class EnsureFinalizerWorks
    {
        static readonly ConcurrentQueue<EnsureFinalizerWorks> _failedDeletions = new ConcurrentQueue<EnsureFinalizerWorks>();
        public readonly string FilePath;
        public Exception DeletionError { get; private set; }
        public EnsureFinalizerWorks(string filePath) { FilePath = filePath; }
        ~EnsureFinalizerWorks()
        {
            try { File.Delete(FilePath); }
            catch (Exception ex)
            {
                DeletionError = ex;
                _failedDeletions.Enqueue(this); // Resurrection

                /*
                 * Enqueuing the object to the static _failedDeletions collection gives the object
                another referee, ensuring that it remains alive until the object is eventually
                dequeued.*
                */
            }
        }
    }

    public class TrySeveralTimesRunFinalizer
    {
        public readonly string FilePath;
        int _deleteAttempt;
        public TrySeveralTimesRunFinalizer(string filePath) { FilePath = filePath; }
        ~TrySeveralTimesRunFinalizer()
        {
            try { File.Delete(FilePath); }
            catch
            {
                if (_deleteAttempt++ < 3)
                    GC.ReRegisterForFinalize(this); // reregister the object for next garbage collection
            }

            /*
             * In this example we try to delete a temporary file in a finalizer.
             * But if the deletion fails, we reregister the object so as to try again in the next garbage collection:
             */
        }
    }

}
