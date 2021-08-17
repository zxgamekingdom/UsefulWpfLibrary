using System;
using System.Collections.ObjectModel;

namespace UsefulWpfLibrary.Logic
{
    public class DisposableObservableCollection<T> : IDisposable
    {
        public ObservableCollection<T> Collection { get; } = new();
        public bool IsDisposeChildren { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && IsDisposeChildren)
            {
                foreach (T x1 in Collection)
                    if (x1 is IDisposable disposable)
                        disposable.Dispose();
                Collection.Clear();
            }
        }
    }
}
