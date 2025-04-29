using System;

namespace MiddleSpawn
{
    public interface IOpenableObject
    {
        FilePath CurrentPath { get; }
        void Open(Action<IOpenablesContainer> updateContainer);
    }
}