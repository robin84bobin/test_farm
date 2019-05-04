using Data;
using Data.Catalog;
using Data.User;
using Model;
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;

namespace DI
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private Config _config;
        
        public override void InstallBindings()
        {
            Container.Bind<IDataBaseProxy>().To<JsonDbProxy>().WithArguments(_config.CatalogPath, _config.CatalogRoot).WhenInjectedInto<CatalogRepository>();
            Container.Bind<IDataBaseProxy>().To<JsonDbProxy>().WithArguments(_config.UserRepositoryPath, _config.CatalogRoot).WhenInjectedInto<UserRepository>();
            Container.Bind<CatalogRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserRepository>().AsSingle();

            Container.Bind<Farm>().AsSingle();
            Container.Bind<ShopInventory>().AsSingle();
            Container.Bind<ProductInventory>().AsSingle();

            Container.DeclareSignal<DataInitComplete>();
        }
    }

    public class DataInitComplete
    {
    }
}