using System;
using System.Collections.Generic;
using Files;
using UI.Files;
using UnityEngine;

namespace CustomHelper
{
    public static partial class Helper
    {
        public static void FillContainerWithFiles(
            Transform container,
            IEnumerable<FileObject> files,
            LinkUI linkPrefab,
            IFolderUI folderPrefab,
            Func<IFolderUI, Transform, Folder, bool, IFolderUI> folderFactory,
            Func<LinkUI, Transform, Link, LinkUI> linkFactory,
            bool insideFolder = false
        )
        {
            foreach (var _ in GetFilesForContainer(container, files, linkPrefab, folderPrefab, folderFactory,
                         linkFactory, insideFolder))
            {
            }
        }

        public static IEnumerable<FileUI> GetFilesForContainer(
            Transform container,
            IEnumerable<FileObject> files,
            LinkUI linkPrefab,
            IFolderUI folderPrefab,
            Func<IFolderUI, Transform, Folder, bool, IFolderUI> folderFactory,
            Func<LinkUI, Transform, Link, LinkUI> linkFactory,
            bool insideFolder = true
        )
        {
            foreach (var file in files)
            {
                var i = file switch
                {
                    Link link => linkFactory(linkPrefab, container, link),
                    Folder folder => folderFactory(folderPrefab, container, folder, insideFolder) as FileUI,
                    _ => null
                };
                if (i is null) continue;
                yield return i;
            }
        }
    }
}