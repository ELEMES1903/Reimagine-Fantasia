using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionEconomy : MonoBehaviour
{
    public Button[] apButtons; // Array of action point buttons
    public Button increaseButton; // Button to increase action points
    public Button decreaseButton; // Button to decrease action points

    public Sprite emptyActionPointImage; // Image for empty action point
    public Sprite actionPointImage; // Image for filled action point
    public Sprite reactionPointImage; // Image for reaction point
    private Sprite turnSprite;

    private int actionPointCount = 0; // Number of action points

    public Button turnButton;
    public Sprite endTurn;
    public Sprite startTurn;


    void Start()
    {
        turnButton.image.sprite = endTurn;
        turnSprite  = actionPointImage;
        
        TurnEndUpdate();

        // Initialize action point buttons with empty action point image
        foreach (Button button in apButtons)
        {
            button.image.sprite = actionPointImage;
            button.onClick.AddListener(() => AdjustActionPoints(button));
        }

        // Add listeners to increase and decrease buttons
        increaseButton.onClick.AddListener(IncreaseActionPoints);
        decreaseButton.onClick.AddListener(DecreaseActionPoints);

        turnButton.onClick.AddListener(TurnManager);
    }

    void IncreaseActionPoints()
    {
        // Find the leftmost action point button with empty action point image
        for (int i = 0; i < apButtons.Length; i++)
        {
            if (apButtons[i].image.sprite == emptyActionPointImage)
            {
                // Set the image to action point image and break the loop
                apButtons[i].image.sprite = actionPointImage;
                actionPointCount++;
                break;
            }
        }
    }

    void DecreaseActionPoints()
    {
        // Find the rightmost action point button with action point image
        for (int i = apButtons.Length - 1; i >= 0; i--)
        {
            if (apButtons[i].image.sprite == actionPointImage)
            {
                // Set the image to empty action point image and break the loop
                apButtons[i].image.sprite = emptyActionPointImage;
                actionPointCount--;
                break;
            }
        }
    }

    void AdjustActionPoints(Button clickedButton)
    {
        // Find the index of the clicked button
        int clickedIndex = System.Array.IndexOf(apButtons, clickedButton);

        // Check the sprite of the clicked button
        if (clickedButton.image.sprite == turnSprite)
        {
            // Set all buttons to the right of the clicked button to emptyActionPointImage
            for (int i = clickedIndex + 1; i < apButtons.Length; i++)
            {
                apButtons[i].image.sprite = emptyActionPointImage;
            }

            clickedButton.image.sprite = emptyActionPointImage;
        }
        else if (clickedButton.image.sprite == emptyActionPointImage)
        {
            // Set all buttons to the left of the clicked button to actionPointImage
            for (int i = clickedIndex - 1; i >= 0; i--)
            {   
                apButtons[i].image.sprite = turnSprite;
            }

            clickedButton.image.sprite = turnSprite;
        }
    }

    public void TurnManager(){

        if(turnButton.image.sprite == endTurn){

            turnButton.image.sprite = startTurn;
            turnSprite = reactionPointImage;
            TurnStartUpdate();

        } else {

            turnButton.image.sprite = endTurn;
            turnSprite = actionPointImage;
            TurnEndUpdate();
        }
    }
    void TurnStartUpdate(){

        foreach (Button button in apButtons)
        {
            if(button.image.sprite == actionPointImage){
                button.image.sprite = turnSprite;
            }
        }
    }

    void TurnEndUpdate(){
        foreach (Button button in apButtons)
        {
            button.image.sprite = turnSprite;
        }
    }
}
