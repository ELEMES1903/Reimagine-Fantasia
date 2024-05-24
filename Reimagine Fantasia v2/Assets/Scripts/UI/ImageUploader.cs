using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class ImageUploader : MonoBehaviour
{
    public Button uploadButton;
    public Image portraitImage;

    void Start()
    {
        uploadButton.onClick.AddListener(OpenFileBrowser);
    }

    void OpenFileBrowser()
    {
        string path = OpenFileDialog();
        if (!string.IsNullOrEmpty(path))
        {
            StartCoroutine(LoadImage(path));
        }
    }

    string OpenFileDialog()
    {
        // This will open a file browser dialog to select an image
        // This example works on Windows
        // You can use a FileBrowser package from the Asset Store for cross-platform support

        #if UNITY_EDITOR
                string path = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");
        #else
                string path = null;
                // Implement cross-platform file browser here if necessary
        #endif
        return path;
    }

    IEnumerator LoadImage(string path)
    {
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            portraitImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        yield return null;
    }
}
