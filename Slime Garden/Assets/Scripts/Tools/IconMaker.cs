using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

public class IconMaker : MonoBehaviour
{
    [SerializeField] RenderTexture renderText;
    [SerializeField] string pathName;
    [SerializeField] string identifierName;
    [SerializeField] string fileName;

    [DetailedInfoBox("Saves to:",  "C:/Users/Legod/AppData/LocalLow/JaredDevs/Pixel Slimes")]

    [Button("Save png")]
    public void SavePNG()
    {
        var tex = new Texture2D(renderText.width, renderText.height);
        RenderTexture.active = renderText;
        tex.ReadPixels(new Rect(0, 0, renderText.width, renderText.height), 0, 0);
        tex.Apply();

        var fullFileName = fileName + identifierName + ".png";
        var filepath = Path.Combine(Application.persistentDataPath, fullFileName);
        File.WriteAllBytes(filepath, tex.EncodeToPNG());
        Debug.Log("Saved icon to: " + filepath);
    }
}
