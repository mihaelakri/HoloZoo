using UnityEditor;
using UnityEngine;
using System.IO;

// Inspired by this 
// https://github.com/CodeSmile-0000011110110111/de.codesmile.assetdatabase/blob/76e19c04c11a33b78b5b56a8801465479df937c4/Editor/Asset.File.cs#L649
// implementation to preserve Unity's internal references through using AssetDatabase methods
public class CopyUsedAnimalAssets
{
    static string resourcesRoot = "Assets/Resources/AnimalModels/";

    [MenuItem("Tools/Copy Used Animals to Resources")]
    public static void CopyAssetsToResources()
    {
        foreach (string assetPath in UsedAssets.assets)
        {
            if (!File.Exists(assetPath))
            {
                Debug.LogError($"Asset not found: {assetPath}");
                continue;
            }

            string targetDir = MakeTargetFolders(assetPath, false);

            // Get filename and target path
            string fileName = Path.GetFileName(assetPath);
            string targetPath = Path.Combine(targetDir, fileName);

            // Use AssetDatabase.CopyAsset to preserve metadata (GUIDs)

            if (fileName.Contains(".controller"))
            {
                string res = AssetDatabase.MoveAsset(assetPath, targetPath);
                if (res == "")
                    Debug.Log($"Moved: {assetPath} >> {targetPath}");
                else
                    Debug.LogError($"Failed to move: {assetPath}, error: {res}");
            }
            else
            {
                if (AssetDatabase.CopyAsset(assetPath, targetPath))
                    Debug.Log($"Copied: {assetPath} >> {targetPath}");
                else
                    Debug.LogError($"Failed to copy: {assetPath}");
            }
        }
        // Refresh Unity to detect new files
        AssetDatabase.Refresh();
    }

    static void CreateSubdirectories(string folderPath)
    {
        var folderNames = folderPath.Split('/');
        for (int i = 1; i < folderNames.Length; i++)
        {
            string parentPath = string.Join("/", folderNames[0..i]);
            string currentPath = string.Join("/", parentPath, folderNames[i]);
            if (!Directory.Exists(currentPath))
            {
                AssetDatabase.CreateFolder(parentPath, folderNames[i]);
                Debug.Log($"Directory {currentPath} created");
            }
        }
    }

    static string MakeTargetFolders(string assetPath, bool preserveStructure)
    {
        string relativeDir = Path.GetDirectoryName(assetPath)?.Replace("\\", "/").Replace(UsedAssets.prefix, "") ?? "";

        string targetDir = resourcesRoot;
        if (preserveStructure)
            targetDir = Path.Combine(resourcesRoot, relativeDir);

        CreateSubdirectories(targetDir);

        return targetDir;
    }
}
