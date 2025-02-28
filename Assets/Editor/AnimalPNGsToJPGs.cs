using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimalPNGsToJPGs
{
    [MenuItem("HoloTools/Convert Animal PNG textures to JPG")]
    static void Main()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "Convert Animal PNG Textures To JPG",
            $"This operation includes reimports which might take some time.\n\n" +
            "Are you sure you want to begin?",
            "Yes", "Cancel"
        );

        if (!confirm)
        {
            Debug.Log("'Convert Animal PNG textures to JPG' canceled.");
            return;
        }

        FindTextureDirectories(UsedAssets.prefix);

        AssetDatabase.Refresh();
        EditorUtility.RequestScriptReload();
    }

    static void FindTextureDirectories(string targetDirectory)
    {
        string[] textureDirectories = Directory.GetDirectories(targetDirectory, "Texture*", SearchOption.AllDirectories);

        foreach (string textureDir in textureDirectories)
        {
            string textureDirectory = textureDir.Replace('\\', '/');
            FindPNGTextures(textureDirectory);
        }
    }

    static void FindPNGTextures(string targetDir)
    {
        string[] textures = Directory.GetFiles(targetDir, "*.png");

        foreach (string txt in textures)
        {
            string texture = txt.Replace('\\', '/');
            Debug.Log($"Texture: {texture}");

            string jpgPath = TextureToJPG(texture);
            if (jpgPath != "")
            {
                string parentDir = Path.GetDirectoryName(Path.GetDirectoryName(jpgPath));
                ChangeMaterialReferences(parentDir, texture, jpgPath);
            }
        }
    }

    static string TextureToJPG(string texturePath)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;

        if (texture == null)
        {
            Debug.LogError($"TextureToJPG - texture is null at {texturePath}");
            return "";
        }

        string directory = Path.GetDirectoryName(texturePath);
        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(texturePath);
        string jpgPath = Path.Combine(directory, filenameWithoutExtension + ".jpg");

        // Satisfy EncodeToJPG method's requirements
        textureImporter.isReadable = true;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        textureImporter.SaveAndReimport();

        byte[] jpgBytes = ImageConversion.EncodeToJPG(texture, 100);
        File.WriteAllBytes(jpgPath, jpgBytes);

        // Revert the changes
        textureImporter.isReadable = false;
        textureImporter.textureCompression = TextureImporterCompression.Compressed;
        textureImporter.SaveAndReimport();

        return jpgPath;
    }

    static void ChangeMaterialReferences(string targetDirectory, string old_file, string new_file)
    {
        targetDirectory = targetDirectory.Replace('\\', '/');
        old_file = old_file.Replace('\\', '/');
        new_file = new_file.Replace('\\', '/');
        string new_texture_name = Path.GetFileNameWithoutExtension(new_file);
        Debug.Log($"targetDirectory: {targetDirectory} old_file: {old_file} new_file: {new_file}");

        string[] dependentMaterialsPaths = Directory.GetFiles(targetDirectory, "*.mat", SearchOption.AllDirectories);

        foreach (string path in dependentMaterialsPaths)
        {
            string assetPath = path.Replace('\\', '/');
            Debug.Log($"Asset: {assetPath}");

            Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

            var propNames = material.GetTexturePropertyNames();

            foreach (string propName in propNames)
            {
                Debug.Log($"PropName: {propName}");
                Texture texture = material.GetTexture(propName);

                if (texture != null && texture.name == new_texture_name)
                {
                    Debug.Log($"Texture match - oldTextureName: {texture.name}");
                    Texture new_texture = AssetDatabase.LoadAssetAtPath<Texture>(new_file);
                    material.SetTexture(propName, new_texture);
                }
            }
        }
    }
}