using System;

namespace Cgw.Assets
{
    public abstract class Asset : IDisposable
    {
        public delegate void UpdatedHandler(Asset p_asset);
        public event UpdatedHandler OnUpdated;

        public void Updated(Asset p_newAsset)
        {
            OnUpdated?.Invoke(p_newAsset);
        }

        public virtual void Dispose() { }
    }
}