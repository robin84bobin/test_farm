using System;
using System.Collections.Generic;

namespace Data
{
    public interface IDataBaseProxy
    {
        /// <summary>
        /// initialize connection to db etc.
        /// </summary>
        void Init();

        /// <summary>
        /// get data list of current type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"> data table/collection name in database</param>
        /// <param name="callback"> returns data list </param>
        /// <param name="createIfNotExist"></param>
        void Get<T>(string collection, Action<Dictionary<string, T>> callback, bool createIfNotExist = true) where T : DataItem, new();

        /// <summary>
        /// save data items to database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        /// <param name="callback"></param>
        void SaveCollection<T>(string collection, Dictionary<string, T> items, Action callback = null) where T : DataItem, new();

        /// <summary>
        /// save data item to database
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        void Save<T>(string collection, T item, string id = "", Action<T> callback = null) where T : DataItem, new();

        /// <summary>
        /// remove data item from database
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        void Remove<T>(string collection, string id = "", Action<string> callback = null);

        event Action OnInitialized;

    }
}