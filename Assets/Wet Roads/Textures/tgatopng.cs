using UnityEngine;
using System.IO;

public class TGAtoPNGConverter : MonoBehaviour
{
    public Texture2D tgaTexture;

    void Start()
    {
        if (tgaTexture != null)
        {
            ConvertToPNG(tgaTexture);
        }
    }

    void ConvertToPNG(Texture2D texture)
    {
        byte[] pngData = texture.EncodeToPNG(); // Convert to PNG format
        string path = Application.dataPath + "/ConvertedTextures/" + texture.name + ".png";

        if (!Directory.Exists(Application.dataPath + "/ConvertedTextures"))
        {
            Directory.CreateDirectory(Application.dataPath + "/ConvertedTextures");
        }

        File.WriteAllBytes(path, pngData);
        Debug.Log("PNG saved at: " + path);
    }
}
