using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Saving.Settings;
using VContainer;

namespace MiddleSpawn
{
    [UsedImplicitly]
    public class MainContainer : IOpenablesContainer
    {
        private readonly DirectoryObject _implementation;

        [Inject]
        private MainContainer(ConfigEntry entry)
        {
            _implementation = new DirectoryObject(entry.Current.LinksPath, this);
        }

        public IOpenablesContainer Parent => this;

        public IEnumerator<(OpeningIndex, IOpenableObject)> GetEnumerator() =>
            _implementation.EvaluateInners(this).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void UseAt(OpeningIndex index, Action<IOpenablesContainer> updateContainer) =>
            _implementation.UseAt(index, updateContainer);
    }
}