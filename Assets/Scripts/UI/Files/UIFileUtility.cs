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
            AdvancedLinkUI advancedLinkPrefab,
            bool insideFolder = true
        )
        {
            foreach (var _ in GetFilesForContainer(container, files, linkPrefab, folderPrefab, advancedLinkPrefab,
                         insideFolder))
            {
            }
        }

        public static IEnumerable<FileUI> GetFilesForContainer(
            Transform container,
            IEnumerable<FileObject> files,
            LinkUI linkPrefab,
            FolderUI folderPrefab,
            AdvancedLinkUI advancedLinkPrefab,
            bool insideFolder = true
        )
        {
            foreach (var file in files)
            {
                FileUI i = null;
                if (file is Link link)
                {
                    var linkUI = Object.Instantiate(linkPrefab, Vector3.zero, Quaternion.identity, container);
                    linkUI.Initialize(link);
                    i = linkUI;
                }

                if (file is Folder folder)
                {
                    var folderUI = Object.Instantiate(folderPrefab, Vector3.zero, Quaternion.identity, container);
                    folderUI.Initialize(folder, !insideFolder);
                    i = folderUI;
                }

                if (file is AdvancedLink advancedLink)
                {
                    var advancedLinkUI = Object.Instantiate(
                        advancedLinkPrefab,
                        Vector3.zero,
                        Quaternion.identity,
                        container
                    );
                    advancedLinkUI.Initialize(advancedLink);
                    i = advancedLinkUI;
                }

                yield return i;
            }
        }
    }
}