using System;

namespace Data
{
    public class DataItem
    {
        public string Id = String.Empty;
        public string Type = String.Empty;

        internal virtual void Init() { }
    }
}