using System;
using System.Collections.Generic;
using Commands;
using Commands.Data;
using Model;

namespace Data.Repository
{
    public abstract class Repository : TickableItem
    {
        public abstract void Init();
        public Dictionary<string, IDataStorage> Storages = new Dictionary<string, IDataStorage>();

        public event Action OnInitComplete = delegate { };

        private List<Command> _initStoragesCommands;
        protected IDataBaseProxy _dbProxy;

        public Repository(IDataBaseProxy dbProxy)
        {
            _dbProxy = dbProxy;
            _initStoragesCommands = new List<Command>();
        }

        /// <summary>
        /// register storage to init later
        /// </summary>
        /// <param name="dataStorage"></param>
        /// <typeparam name="T"></typeparam>
        protected DataStorage<T> CreateStorage<T>(string collectionName) where T : DataItem, new()
        {
            var dataStorage = new DataStorage<T>(collectionName, _dbProxy);
            var command = new InitStorageCommand<T>(dataStorage, _dbProxy);
            _initStoragesCommands.Add(command);

            Storages.Add(collectionName, dataStorage);
            return dataStorage;
        }

        protected void OnDbInitComplete()
        {
            _dbProxy.OnInitialized -= OnDbInitComplete;
            CommandSequence sequence = new CommandSequence(_initStoragesCommands.ToArray());
            sequence.OnComplete += () =>
            {
                OnInitComplete.Invoke();
            };
            CommandManager.Execute(sequence);
        }

        public T GetSetting<T>(string name)
        {
            return _dbProxy.GetConfigObject<T>(name);
        }

        
        

    }



}