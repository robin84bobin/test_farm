using System;

namespace Data
{
    /// <summary>
    /// base class to store some data item
    /// </summary>
    public abstract class DataItem
    {
        public string Id = String.Empty;
        public string Type = String.Empty;

        internal virtual void Init() { }
    }
}