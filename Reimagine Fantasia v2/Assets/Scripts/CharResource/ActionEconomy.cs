using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public Stress stress;
    public Conditions conditions;

    void Start()
    {
        turnButton.image.sprite = endTurn;
        turnSprite = actionPointImage;

        TurnEndUpdate();

        // Initialize action point buttons with empty action point image
        foreach (Button button in apButtons)
        {
            button.image.sprite = emptyActionPointImage;
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
                // Set the image to the current turn sprite and break the loop
                apButtons[i].image.sprite = turnSprite;
                actionPointCount++;
                break;
            }
        }
    }

    void DecreaseActionPoints()
    {
        // Find the rightmost action point button with the current turn sprite
        for (int i = apButtons.Length - 1; i >= 0; i--)
        {
            if (apButtons[i].image.sprite == turnSprite)
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
        int clickedIndex = Array.IndexOf(apButtons, clickedButton);

        // Check the sprite of the clicked button
        if (clickedButton.image.sprite == turnSprite)
        {
            // Set all buttons to the right of the clicked button to emptyActionPointImage
            for (int i = clickedIndex; i < apButtons.Length; i++)
            {
                apButtons[i].image.sprite = emptyActionPointImage;
            }

            clickedButton.image.sprite = emptyActionPointImage;
        }
        else if (clickedButton.image.sprite == emptyActionPointImage)
        {
            // Set all buttons to the left of the clicked button to the current turn sprite
            for (int i = 0; i <= clickedIndex; i++)
            {
                apButtons[i].image.sprite = turnSprite;
            }
        }
    }

    public void TurnManager()
    {
        TMP_Text turnButtonText = turnButton.GetComponentInChildren<TMP_Text>();
        if (turnButton.image.sprite == endTurn)
        {
            turnButton.image.sprite = startTurn;
            turnButtonText.text = "Start Turn";
            turnSprite = reactionPointImage;
            conditions.DecreaseConDuration();
            TurnStartUpdate();
        }
        else
        {
            turnButton.image.sprite = endTurn;
            turnButtonText.text = "End Turn";
            turnSprite = actionPointImage;
            TurnEndUpdate();
        }
    }

    void TurnStartUpdate()
    {
        for (int i = 0; i < apButtons.Length - stress.weakened; i++)
        {
            if (apButtons[i].image.sprite == actionPointImage || apButtons[i].image.sprite == reactionPointImage)
            {
                apButtons[i].image.sprite = turnSprite;
            }
        }
    }

    void TurnEndUpdate()
    {
        foreach (Button button in apButtons)
        {
            button.image.sprite = turnSprite;
        }
    }
}
