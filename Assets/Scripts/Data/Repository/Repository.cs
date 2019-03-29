using Commands;
using Commands.Data;
using System;
using System.Collections.Generic;

namespace Data
{
    public class Repository
    {
        public  DataStorage<ShopItem> ShopItems;
        public  DataStorage<Product> Products;

        public event Action OnInitComplete = delegate { };

        private List<Command> _initStoragesCommands;
        private IDataBaseProxy _dbProxy;

        public Repository(IDataBaseProxy dbProxy)
        {
            _dbProxy = dbProxy;
            _initStoragesCommands = new List<Command>();
        }
        
        public  void Init()
        {
            ShopItems = CreateStorage<ShopItem>(Collections.SHOP);
            Products = CreateStorage<Product>(Collections.PRODUCTS);

            _dbProxy.OnInitialized += OnDbInitComplete;
            _dbProxy.Init();
        }


        /// <summary>
        /// register storage to init later
        /// </summary>
        /// <param name="dataStorage"></param>
        /// <typeparam name="T"></typeparam>
        DataStorage<T> CreateStorage<T>(string collectionName) where T : DataItem, new()
        {
            var dataStorage = new DataStorage<T>(collectionName, _dbProxy);
            var command = new InitStorageCommand<T>(dataStorage, _dbProxy);
            _initStoragesCommands.Add(command);

            return dataStorage;
        }

        void OnDbInitComplete()
        {
            _dbProxy.OnInitialized -= OnDbInitComplete;
            CommandSequence sequence = new CommandSequence(_initStoragesCommands.ToArray());
            sequence.OnComplete += () => OnInitComplete.Invoke();
            CommandManager.Execute(sequence);
        }
    }


    public class Collections
    {
        public const string SHOP = "shop";
        public const string PRODUCTS = "products";
    }


}