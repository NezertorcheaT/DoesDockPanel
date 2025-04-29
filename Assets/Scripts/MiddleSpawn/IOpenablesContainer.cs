using System.Collections.Generic;

namespace MiddleSpawn
{
    public interface IOpenablesContainer : IEnumerable<IOpenableObject>
    {
        public IOpenablesContainer Parent { get; }
        void OpenAt(OpeningIndex index);
    }
}