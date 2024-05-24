using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LoadImageFromURL : MonoBehaviour
{
    public RawImage targetRawImage; // Reference to the UI RawImage component
    public TMP_InputField urlInputField; // Reference to the UI InputField for typing the URL
    public Button uploadButton; // Reference to the UI Button to trigger the upload

    public string portraitImage;

    void Start()
    {
        // Add listener to the button to trigger the image loading
        uploadButton.onClick.AddListener(delegate {SaveImage(urlInputField.text);} );
    }

    public void SaveImage(string imageURL)
    {
        portraitImage = imageURL;
        StartCoroutine(LoadImage(portraitImage));
    }
    public IEnumerator LoadImage(string imageURL)
    {
        if (string.IsNullOrEmpty(imageURL))
        {
            Debug.LogError("URL is empty or null");
            yield break;
        }

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load image: " + request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            targetRawImage.texture = texture;
        }
    }
}
