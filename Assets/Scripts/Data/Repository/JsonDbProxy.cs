using System;
using System.Collections.Generic;

namespace Data
{
    internal class JsonDbProxy:IDataBaseProxy
    {
        public JsonDbProxy()
        {
        }

        public event Action OnInitialized;

        public void Get<T>(string collection, Action<Dictionary<string, T>> callback, bool createIfNotExist = true) where T : DataItem, new()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(string collection, string id = "", Action<string> callback = null)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(string collection, T item, string id = "", Action<T> callback = null) where T : DataItem, new()
        {
            throw new NotImplementedException();
        }

        public void SaveCollection<T>(string collection, Dictionary<string, T> items, Action callback = null) where T : DataItem, new()
        {
            throw new NotImplementedException();
        }
    }
}