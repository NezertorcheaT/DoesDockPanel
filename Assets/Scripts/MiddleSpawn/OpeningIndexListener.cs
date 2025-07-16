using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace MiddleSpawn
{
    public class OpeningIndexListener : ITickable
    {
        private const float ClickDelay = 0.2f;
        private static readonly TimeSpan DelayTimeSpan = TimeSpan.FromSeconds(ClickDelay);

        public event Action<OpeningIndex> OnSelected;
        private bool _delayed;

        [Inject]
        private OpeningIndexListener()
        {
        }

        public void Tick() => _ = Tick(0);

        private async UniTaskVoid Tick(float veryNeeded)
        {
            if (_delayed) return;
            var i = OpeningIndex.GetFromWindows();
            if (i.HasValue)
            {
                OnSelected?.Invoke(i.Value);
                _delayed = true;
                await UniTask.Delay(DelayTimeSpan);
                _delayed = false;
            }
        }
    }
}