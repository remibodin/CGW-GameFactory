namespace Cgw.Assets
{
    public abstract class Asset
    {
        public delegate void UpdatedHandler(Asset p_asset);
        public event UpdatedHandler OnUpdated;
    }
}