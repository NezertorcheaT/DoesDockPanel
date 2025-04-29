using Input;

namespace MiddleSpawn
{
    public readonly struct OpeningIndex
    {
        private readonly bool _back;
        private readonly int _index;

        public OpeningIndex(int? i = null)
        {
            _back = !i.HasValue;
            _index = i ?? 0;
        }

        public static OpeningIndex? GetFromWindows()
        {
            if (WindowsInput.GetKey(WindowsInput.Keys.D0)) return new OpeningIndex(0);
            if (WindowsInput.GetKey(WindowsInput.Keys.D1)) return new OpeningIndex(1);
            if (WindowsInput.GetKey(WindowsInput.Keys.D2)) return new OpeningIndex(2);
            if (WindowsInput.GetKey(WindowsInput.Keys.D3)) return new OpeningIndex(3);
            if (WindowsInput.GetKey(WindowsInput.Keys.D4)) return new OpeningIndex(4);
            if (WindowsInput.GetKey(WindowsInput.Keys.D5)) return new OpeningIndex(5);
            if (WindowsInput.GetKey(WindowsInput.Keys.D6)) return new OpeningIndex(6);
            if (WindowsInput.GetKey(WindowsInput.Keys.D7)) return new OpeningIndex(7);
            if (WindowsInput.GetKey(WindowsInput.Keys.D8)) return new OpeningIndex(8);
            if (WindowsInput.GetKey(WindowsInput.Keys.D9)) return new OpeningIndex(9);
            if (WindowsInput.GetKey(WindowsInput.Keys.Back)) return new OpeningIndex();
            return null;
        }

        public WindowsInput.Keys AsKey()
        {
            if (_back) return WindowsInput.Keys.Back;
            return _index switch
            {
                1 => WindowsInput.Keys.D1,
                2 => WindowsInput.Keys.D2,
                3 => WindowsInput.Keys.D3,
                4 => WindowsInput.Keys.D4,
                5 => WindowsInput.Keys.D5,
                6 => WindowsInput.Keys.D6,
                7 => WindowsInput.Keys.D7,
                8 => WindowsInput.Keys.D8,
                9 => WindowsInput.Keys.D9,
                0 => WindowsInput.Keys.D0,
                _ => WindowsInput.Keys.Back
            };
        }
    }
}