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
            FolderUI folderPrefab,
            Func<FolderUI, Transform, Folder, bool, FolderUI> folderFactory,
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
            FolderUI folderPrefab,
            Func<FolderUI, Transform, Folder, bool, FolderUI> folderFactory,
            Func<LinkUI, Transform, Link, LinkUI> linkFactory,
            bool insideFolder = true
        )
        {
            foreach (var file in files)
            {
                FileUI i = file switch
                {
                    Link link => linkFactory(linkPrefab, container, link),
                    Folder folder => folderFactory(folderPrefab, container, folder, insideFolder),
                    _ => null
                };
                if (i is null) continue;
                yield return i;
            }
        }
    }
}