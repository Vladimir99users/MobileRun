using UnityEngine;
using Zenject;

namespace Assets.Scripts.Input
{
    public class InputInstaller : MonoInstaller
    {
        [SerializeField] private InputService inputServicePrefab;
        public override void InstallBindings()
        {
            BindingInputService();
        }

        private void BindingInputService()
        {
            var inputService = Container.InstantiatePrefabForComponent<InputService>(this.inputServicePrefab);
            inputService.Initialize();

            Container.Bind<IMovable>()
                .To<InputService>()
                .FromInstance(inputService)
                .AsSingle().NonLazy();
        }
    }
}
