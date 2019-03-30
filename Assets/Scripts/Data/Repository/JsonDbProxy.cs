using System;
using System.Collections.Generic;
using System.IO;
using InternalNewtonsoft.Json.Linq;
using UnityEngine;

namespace Data
{
    internal class JsonDbProxy:IDataBaseProxy
    {
        private string _path;
        private DateTime _lastReadTime;

        private string _dataJson;
        private string _rootNode;

        public JsonDbProxy(string path, string rootNode)
        {
            _rootNode = rootNode;
            _path =  path;
            /*if (IsFileExist(_path))
            {
                _lastReadTime = File.GetLastWriteTime(_path);
            }*/
        }

        public void Save<T>(string collection, T item, string id = "", Action<T> callback = null) where T : DataItem, new()
        {
            throw new NotImplementedException();
        }

        public event Action OnInitialized;

        public void Get<T>(string collection, Action<Dictionary<string, T>> callback, bool createIfNotExist = true) where T : DataItem, new()
        {
            TryRefreshData();
            
            //try
            //{
                JObject j = JObject.Parse(_dataJson);
                JToken jToken = j[_rootNode];
                GetData<T>(jToken, collection, callback);
            //}
            //catch (Exception e)s
            //{
                //Debug.LogError(this + ": OnGetData ("+collection+") error: " + e.Message);
            //}
        }
        
        private void GetData<T>(JToken j, string collection, Action<Dictionary<string, T>> callback) where T : DataItem, new()
        {
            if (string.IsNullOrEmpty(collection))
            {
                Debug.LogError(ToString() + " : no sourceName in storage had been set: " + collection);
                return ;
            }

            JToken jToken = j.SelectToken(collection);
            if (jToken == null)
            {
                Debug.LogError(ToString() + " : no source data was found by: " + collection);
                return ;
            }

            Dictionary<string, T> items = jToken.ToObject<Dictionary<string, T>>();
            /*T[] dataArray = jToken.ToObject<T[]>();
            
            var items = new Dictionary<string, T>();
            foreach (T newItem in dataArray)
            {
                if (items.ContainsKey(newItem.Id))
                {
                    Debug.LogError(string.Format("Map {0} already contains key {1}! Skiping...", typeof(T).Name,
                        newItem.Id));
                    continue;
                }
                items.Add(newItem.Id, newItem);
            }*/
            callback.Invoke(items);
        }

        public void Init()
        {
            TryRefreshData();
            if (OnInitialized != null)
                OnInitialized.Invoke();
        }

        private void TryRefreshData()
        {
            if (!IsFileExist(_path))
                File.CreateText(_path).Close();

            if (string.IsNullOrEmpty(_dataJson) || File.GetLastWriteTime(_path) > _lastReadTime)
            {
                _dataJson = File.ReadAllText(_path);
                _lastReadTime = DateTime.Now;
            }
        }

        public void SaveCollection<T>(string collection, Dictionary<string, T> items, Action callback = null)
            where T : DataItem, new()
        {
            TryRefreshData();

            JObject j = JObject.Parse(_dataJson);
            j[_rootNode][collection] = JToken.FromObject(items);

            StreamWriter writer = File.CreateText(_path);
            var sourceString = j.ToString();
            writer.Write(sourceString.ToCharArray());
            writer.Close();
        }

        public bool IsFileExist(string path_)
        {
            bool exists = File.Exists(path_);
            if (!exists) {
                Debug.LogWarning("FILE NOT FOUND: " + path_);
            }

            return exists;
        }

        public T GetConfigObject<T>(string name)
        {
            JObject j = JObject.Parse(_dataJson);
            JToken jToken = j[_rootNode];
            return jToken.ToObject<T>();
        }
    }
}