using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class Poker : MonoBehaviour
{
    //declare variables
    //Dealing Cards

    private List<string> deck = new List<string>();
    private List<string> player1Hand = new List<string>();
    private List<string> player2Hand = new List<string>();

    public Image player1Card1;
    public Image player1Card2;
    public Image player2Card1;
    public Image player2Card2;
    //Deak Button
    public Button dealButton; 
    private Dictionary<string, Sprite> cardSprites = new Dictionary<string, Sprite>();


    //Placing Chips

    //starting chips for player 1 and 2
    private int player1Chips = 1000;
    private int player2Chips = 1000;


    // Small and Big Blinds
    
    private int smallBlind = 10;
    private int bigBlind = 20;

    //images for the chips in Ui for player 1 and 2
    public Image player1ChipStack; 
    public Image player2ChipStack; 
    public TMP_Text player1ChipText;
    public TMP_Text player2ChipText;
    //keeping track of chips and text
    private Dictionary<int, Sprite> chipSprites = new Dictionary<int, Sprite>();
    //placeBet Button
      public Button placeBetsButton;
   
    void Start()
    {
        //dealing functions
        LoadAllCardSprites();
        InitializeDeck();
        ShuffleDeck();
        //Activate DealButton
        dealButton.gameObject.SetActive(true);
        // Add event listener 
        dealButton.onClick.AddListener(DealAndDisplayCards);
        placeBetsButton.onClick.AddListener(PlaceInitialBets);

        //change visibility of cheaps to false
        player1ChipStack.gameObject.SetActive(false);
        player2ChipStack.gameObject.SetActive(false);

        // card visibility on start
        player1Card1.gameObject.SetActive(false);
        player1Card2.gameObject.SetActive(false);
        player2Card1.gameObject.SetActive(false);
        player2Card2.gameObject.SetActive(false);
        
        
    }

    void InitializeDeck()
    {
        //keep track of all cards in game
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        //iterate through the deck of cards
        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                string cardName = rank+"_of_"+suit;
                deck.Add(cardName);
            }
        }
    }

    //Ensure card randomness 
    void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (deck[k], deck[n]) = (deck[n], deck[k]);
        }
    }

    //Deal cards to both platers
    void DealAndDisplayCards()
    {
        if (deck.Count < 4)
        {
            Debug.LogError("Not enough cards left to deal!");
            return;
        }

        player1Hand.Clear();
        player2Hand.Clear();

        player1Hand.Add(deck[0]);
        player1Hand.Add(deck[1]);
        player2Hand.Add(deck[2]);
        player2Hand.Add(deck[3]);

        deck.RemoveRange(0, 4);

        player1Card1.sprite = GetCardSprite(player1Hand[0]);
        player1Card2.sprite = GetCardSprite(player1Hand[1]);
        player2Card1.sprite = GetCardSprite(player2Hand[0]);
        player2Card2.sprite = GetCardSprite(player2Hand[1]);

        Debug.Log("New cards dealt!");

        //change visibility of cards
        player1Card1.gameObject.SetActive(true);
        player1Card2.gameObject.SetActive(true);
        player2Card1.gameObject.SetActive(true);
        player2Card2.gameObject.SetActive(true);

        //deactivate deal button
        dealButton.gameObject.SetActive(false);

        //set the place bets button to active
         placeBetsButton.gameObject.SetActive(true);


        //chip placing
        //LoadChipSprites();
        //PlaceInitialBets();
    }

    Sprite GetCardSprite(string cardName)
    {
        string lowerCaseName = cardName.ToLower();
        if (cardSprites.ContainsKey(lowerCaseName))
        {
            return cardSprites[lowerCaseName];
        }
        else
        {
            Debug.LogError("Card sprite not found: " + lowerCaseName);
            return null;
        }
    }
    void LoadAllCardSprites()
{
    Sprite[] sprites = Resources.LoadAll<Sprite>("Cards/");
    if (sprites.Length == 0)
    {
        Debug.LogError("No sprites found in Resources/Cards/. Check if images are inside the correct folder.");
        return;
    }
   

    foreach (Sprite sprite in sprites)
    {
        cardSprites[sprite.name.ToLower()] = sprite;
        Debug.Log("Loaded card: " + sprite.name); // Log loaded sprites
    }
}

//placing chips code after dealing
void PlaceInitialBets()
    {
        player1Chips -= smallBlind;
        player2Chips -= bigBlind;

        // Show the chips
        player1ChipStack.gameObject.SetActive(true);
        player2ChipStack.gameObject.SetActive(true);

        // Update chip count UI
        player1ChipText.text = "Chips: " + player1Chips;
        player2ChipText.text = "Chips: " + player2Chips;

        // Disable the Place Bets button after clicking
        placeBetsButton.gameObject.SetActive(false);
    }
//display the chips
    void ShowChips(Image chipImage, int chipValue)
    {
        string chipPath = "Chips/" + chipValue; // Assumes chips are stored in Resources/Chips/
        Sprite chipSprite = Resources.Load<Sprite>(chipPath);

        if (chipSprite == null)
        {
            Debug.LogError("Chip sprite not found for: " + chipValue);
            return;
        }

        chipImage.sprite = chipSprite;
        chipImage.gameObject.SetActive(true);
    }

//  void LoadChipSprites()
//     {
//         Sprite[] sprites = Resources.LoadAll<Sprite>("Chips/");
//         foreach (Sprite sprite in sprites)
//         {
//             string spriteName = sprite.name.Replace("chip_", ""); // Extract value (e.g., "10" from "chip_10")
//             int chipValue;
//             if (int.TryParse(spriteName, out chipValue))
//             {
//                 chipSprites[chipValue] = sprite;
//             }
//         }
//     }

//     void PlaceInitialBets()
//     {
//         player1Chips -= smallBlind;
//         player2Chips -= bigBlind;

//         UpdateChipDisplay(player1ChipStack, smallBlind);
//         UpdateChipDisplay(player2ChipStack, bigBlind);

//         player1ChipText.text = "Chips: " + player1Chips;
//         player2ChipText.text = "Chips: " + player2Chips;
//     }

//     void UpdateChipDisplay(Image chipStack, int betAmount)
//     {
//         if (chipSprites.ContainsKey(betAmount))
//         {
//             chipStack.sprite = chipSprites[betAmount];
//         }
//         else
//         {
//             Debug.LogError("Chip sprite not found for: " + betAmount);
//         }
    

    //activate deal at endof round
    void StartNewRound()
{
    dealButton.interactable = true;  // Reactivate the button
}

    
    }



