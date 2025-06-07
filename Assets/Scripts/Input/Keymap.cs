using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Input
{
    public class Keymap : IEnumerable<WindowsInput.Keys>
    {
        private ICollection<WindowsInput.Keys> _keyset;

        public Keymap(IEnumerable<WindowsInput.Keys> keyset)
        {
            _keyset = keyset.ToList();
        }

        public Keymap(string str)
        {
            var keyset = new List<WindowsInput.Keys>();
            foreach (var c in str.Split(','))
            {
                if (Enum.TryParse(c, out WindowsInput.Keys key))
                    keyset.Add(key);
            }

            _keyset = keyset;
        }

        public bool FromWindows() => _keyset.All(WindowsInput.GetKey);

        public IEnumerator<WindowsInput.Keys> GetEnumerator() => _keyset.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_keyset).GetEnumerator();

        public override string ToString()
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var key in _keyset)
            {
                sb.Append(key.ToString());
                if (i == _keyset.Count - 1)
                    sb.Append(',');
                i++;
            }

            return sb.ToString();
        }
    }
}