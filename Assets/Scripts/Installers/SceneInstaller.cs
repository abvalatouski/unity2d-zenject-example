using System.Collections.Generic;

using UnityEngine;
using Zenject;

namespace Banner
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private ProviderType providerType;

        public override void InstallBindings()
        {
            switch (providerType)
            {
                case ProviderType.Enumerable:
                    UseEnumerable();
                    break;
                case ProviderType.Resources:
                    UseResources();
                    break;
                case ProviderType.Inspector:
                    UseInspector();
                    break;
                case ProviderType.Network:
                    UseNetwork();
                    break;
            }
        }

        private void UseEnumerable()
        {
            const string Path = "Textures";
            IEnumerable<Texture> textures = Resources.LoadAll<Texture>(Path);

            Container.Bind<IProvider<Texture>>()
                .To<EnumerableProvider<Texture>>()
                .FromNew()
                .AsSingle()
                .WithArguments(textures)
                .NonLazy();
        }

        private void UseResources()
        {
            const string Path = "Textures";

            Container.Bind<IProvider<Texture>>()
                .To<ResourcesProvider<Texture>>()
                .FromNew()
                .AsSingle()
                .WithArguments(Path)
                .NonLazy();
        }

        private void UseInspector()
        {
            var provider = FindObjectOfType<TextureInspectorProvider>();

            Container.Bind<IProvider<Texture>>()
                .To<TextureInspectorProvider>()
                .FromInstance(provider)
                .AsSingle()
                .NonLazy();
        }

        private void UseNetwork()
        {
            IEnumerable<string> urls = new string[]
            {
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/cabal.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/docker.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/elm.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/gitlab.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/haskell.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/json.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/ocaml.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/qsharp.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/rust.png",
                @"https://raw.githubusercontent.com/abvalatouski/unity2d-shader-example/master/Assets/Textures/xml.png",
            };

            Container.Bind<IProvider<Texture>>()
                .To<TextureNetworkProvider>()
                .FromNew()
                .AsSingle()
                .WithArguments(urls)
                .NonLazy();
        }
    }
}
