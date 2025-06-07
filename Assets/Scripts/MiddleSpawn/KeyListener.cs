using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace MiddleSpawn
{
    public class KeyListener : ITickable
    {
        public event Action<OpeningIndex> OnSelected;
        public const float ClickDelay = 0.2f;
        private bool _delayed;

        [Inject]
        private KeyListener()
        {
        }

        public async void Tick()
        {
            if (_delayed) return;
            var i = OpeningIndex.GetFromWindows();
            if (i.HasValue)
            {
                OnSelected?.Invoke(i.Value);
                _delayed = true;
                await UniTask.Delay(TimeSpan.FromSeconds(ClickDelay));
                _delayed = false;
            }
        }
    }
}