using UnityEngine;
using Zenject;

namespace Assets.Scripts.UI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIWindow uiWindow;
        public override void InstallBindings()
        {
            BindingUIWindow();
        }

        private void BindingUIWindow()
        {
            Container.Bind<UIWindow>()
                .FromInstance(uiWindow)
                .AsSingle()
                .NonLazy();
        }
    }
}
