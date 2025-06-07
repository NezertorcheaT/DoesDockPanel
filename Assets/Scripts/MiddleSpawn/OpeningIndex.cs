using System;
using Input;

namespace MiddleSpawn
{
    public readonly struct OpeningIndex
    {
        public int Index => Math.Min(_index, 10);
        public readonly bool Back;
        private readonly int _index;

        public OpeningIndex(int? i = null)
        {
            Back = i is null;
            _index = i ?? 0;
        }

        public static OpeningIndex Return() => new(null);

        public static OpeningIndex? GetFromWindows()
        {
            if (WindowsInput.GetKey(WindowsInput.Keys.D1)) return new OpeningIndex(0);
            if (WindowsInput.GetKey(WindowsInput.Keys.D2)) return new OpeningIndex(1);
            if (WindowsInput.GetKey(WindowsInput.Keys.D3)) return new OpeningIndex(2);
            if (WindowsInput.GetKey(WindowsInput.Keys.D4)) return new OpeningIndex(3);
            if (WindowsInput.GetKey(WindowsInput.Keys.D5)) return new OpeningIndex(4);
            if (WindowsInput.GetKey(WindowsInput.Keys.D6)) return new OpeningIndex(5);
            if (WindowsInput.GetKey(WindowsInput.Keys.D7)) return new OpeningIndex(6);
            if (WindowsInput.GetKey(WindowsInput.Keys.D8)) return new OpeningIndex(7);
            if (WindowsInput.GetKey(WindowsInput.Keys.D9)) return new OpeningIndex(8);
            if (WindowsInput.GetKey(WindowsInput.Keys.D0)) return new OpeningIndex(9);
            if (WindowsInput.GetKey(WindowsInput.Keys.Back)) return Return();
            return null;
        }

        public WindowsInput.Keys AsKey()
        {
            if (Back) return WindowsInput.Keys.Back;
            return _index switch
            {
                0 => WindowsInput.Keys.D1,
                1 => WindowsInput.Keys.D2,
                2 => WindowsInput.Keys.D3,
                3 => WindowsInput.Keys.D4,
                4 => WindowsInput.Keys.D5,
                5 => WindowsInput.Keys.D6,
                6 => WindowsInput.Keys.D7,
                7 => WindowsInput.Keys.D8,
                8 => WindowsInput.Keys.D9,
                9 => WindowsInput.Keys.D0,
                _ => WindowsInput.Keys.Back
            };
        }
    }
}