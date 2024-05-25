using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LoadImageFromURL : MonoBehaviour
{
    public RawImage miniPortrait;
    public RawImage bigPortrait;

    public Sprite unknownPortrait;
    public TMP_InputField urlInputField;
    public Button uploadButton;
    public string portraitImage;

    private SaveSlot saveSlot;

    void Start()
    {
        saveSlot = GetComponent<SaveSlot>();
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
            //int selectedIndex = saveSlot.SelectedIndex;
            //saveSlot.saveSlots[selectedIndex].slotPortrait.texture = unknownPortrait.texture;
            
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
            miniPortrait.texture = texture;
            bigPortrait.texture = texture;

            int selectedIndex = saveSlot.SelectedIndex; // Assuming SelectedIndex is the index of the selected slot
            saveSlot.saveSlots[selectedIndex].slotPortrait.texture = texture;
        }
    }
}
