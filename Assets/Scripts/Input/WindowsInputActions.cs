using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace Input
{
    public class WindowsInputActions : ITickable
    {
        private class Eventer
        {
            public int Key { get; }
            public Action Action { get; }
            public bool Pressed { get; set; } = false;

            public Eventer(int key, Action action)
            {
                Key = key;
                Action = action;
            }
        }

        private List<Eventer> _events;

        public void Tick()
        {
            _events ??= new List<Eventer>();
            foreach (var eventer in _events)
            {
                var key = WindowsInput.GetKey(eventer.Key);
                if (key && !eventer.Pressed)
                {
                    eventer.Pressed = true;
                    eventer.Action?.Invoke();
                    return;
                }

                if (!key && eventer.Pressed)
                    eventer.Pressed = false;
            }
        }
    }
}