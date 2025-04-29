using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Input
{
    [UsedImplicitly]
    public class WindowsInputActions : ITickable
    {
        public class Eventer : IEquatable<Eventer>
        {
            public bool Equals(Eventer other) => Key == other.Key && Equals(Action, other.Action);
            public override bool Equals(object obj) => obj is Eventer other && Equals(other);
            public override int GetHashCode() => HashCode.Combine(Key, Action);

            public int Key { get; }
            public Action Action { get; }
            public bool Pressed { get; set; }

            public Eventer(int key, Action action)
            {
                Key = key;
                Action = action;
            }
        }

        public void Register(Eventer e) => _events.Add(e);
        public void Unregister(Eventer e) => _events.Add(e);

        private readonly List<Eventer> _events = new();

        public void Tick()
        {
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