using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BetManager : MonoBehaviour
 {
    //Declarations
   public Button dealButton;
    public Button callButton;
    public Button raiseButton;
    public Button foldButton;
    public Button betButton;

    public TMP_Text player1ChipText;
    public TMP_Text player2ChipText;
    public TMP_Text potText;

    private int player1Chips = 1000;
    private int player2Chips = 1000;
    private int pot = 0;
    private int currentBet = 0;
    private int raiseAmount = 50;

// check if deal and bets are placed
    private bool hasDealtCards = false;
    private bool hasPlacedBet = false;

    void Start()
    {
        dealButton.onClick.AddListener(DealCards);
        betButton.onClick.AddListener(() => PlaceInitialBet(100));
        callButton.onClick.AddListener(PlayerCall);
        raiseButton.onClick.AddListener(PlayerRaise);
        foldButton.onClick.AddListener(PlayerFold);

        DisableBettingButtons();
        UpdateChipUI();
    }

//NEED TO BE CHANGED TO AVOID DUPLICATING IN POKER.cs
    void DealCards()
    {
        Debug.Log("Cards Dealt!");
        hasDealtCards = true;

        //disable and enable buttons
        dealButton.interactable = false; 
        betButton.interactable = true; 
    }

    void PlaceInitialBet(int betAmount)
    {
        if (!hasDealtCards)
        {
            //To be changed to a text box warning
            Debug.LogWarning("Bet Cannot be Placed.");
            return;
        }
        // TO BE DISPLAYED AS TEXT
        Debug.Log($"Initial Bet of {betAmount} chips.");
        player1Chips -= betAmount;
        pot += betAmount;
        currentBet = betAmount;
        hasPlacedBet = true;

        EnableBettingButtons();
        UpdateChipUI();
    }
// call , raise and fold functionalities
    void PlayerCall()
    {
        if (!Proceed("Call")) return;

        Debug.Log("Player Calls.");
        player1Chips -= currentBet;
        pot += currentBet;
        UpdateChipUI();
    }

    void PlayerRaise()
    {
        if (!Proceed("Raise")) return;

        Debug.Log("Player Raises.");
        player1Chips -= (currentBet + raiseAmount);
        pot += (currentBet + raiseAmount);
        currentBet += raiseAmount;
        hasPlacedBet = true; // A raise is a new bet
        UpdateChipUI();
    }

    void PlayerFold()
    {
        if (!Proceed("Fold")) return;

        Debug.Log("Player Folds.");
        ResetRound();
    }

    bool Proceed(string action)
    {
        if (!hasDealtCards)
        {
            Debug.LogWarning($"Cannot {action}! Cards have not been dealt.");
            return false;
        }
        if (!hasPlacedBet)
        {
            Debug.LogWarning($"Cannot {action}! No bet has been placed.");
            return false;
        }
        return true;
    }

    void ResetRound()
    {
        hasDealtCards = false;
        hasPlacedBet = false;
        pot = 0;
        currentBet = 0;

        dealButton.interactable = true;
        DisableBettingButtons();
        UpdateChipUI();
    }

    void UpdateChipUI()
    {
        player1ChipText.text = "Chips: " + player1Chips;
        player2ChipText.text = "Chips: " + player2Chips;
        potText.text = "Pot: " + pot;
    }
// when to enable and siable buttons
    void EnableBettingButtons()
    {
        callButton.interactable = true;
        raiseButton.interactable = true;
        foldButton.interactable = true;
    }

    void DisableBettingButtons()
    {
        callButton.interactable = false;
        raiseButton.interactable = false;
        foldButton.interactable = false;
        betButton.interactable = false;
    }
}
