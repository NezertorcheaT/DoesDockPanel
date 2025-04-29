using System;
using System.Collections.Generic;

namespace MiddleSpawn
{
    public interface IOpenablesContainer : IEnumerable<(OpeningIndex ind, IOpenableObject openable)>
    {
        public IOpenablesContainer Parent { get; }
        void UseAt(OpeningIndex index, Action<IOpenablesContainer> updateContainer);
    }
}