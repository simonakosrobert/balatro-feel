using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Player : AttributesSync
{
    [SynchronizableField] public bool Ready = false;
    [SynchronizableField] public bool resultsReady = false;
    [SynchronizableField] public bool shopReady = false;
    [SynchronizableField] public string Username = "";
    [SynchronizableField] public int playerID = 0;
    [SynchronizableField] public bool StartGame = false;
    [SynchronizableField] public bool isCurrent = false;
    [SynchronizableField] public bool isPlaying = false;
    [SynchronizableField] public bool isDiscarding = false;
    [SynchronizableField] public int currentPlayerID = 1;
    [SynchronizableField] public int discards = 3;
    [SynchronizableField] public int maxDiscards = 3;
    [SynchronizableField] public int hands = 4;
    [SynchronizableField] public int maxHands = 4;

    [SynchronizableField] public int gameMaxPlayers = 4;
    [SynchronizableField] public int TimeLimit = 30;
    [SynchronizableField] public int gameRounds = 4;
    [SynchronizableField] public int gameMaxJokers = 3;
    [SynchronizableField] public int gameMaxTarots = 2;
    [SynchronizableField] public int gameMoneyMult = 1;
    [SynchronizableField] public int gameJokerChance = 75;

    [SynchronizableField] public int handSize = 8;
    [SynchronizableField] public int fullDeck = 52;
    [SynchronizableField] public int remainingDeck = 52;
    [SynchronizableField] public float score = 0;
    [SynchronizableField] public float allRoundTotalScore = 0;
    [SynchronizableField] public float allRoundTotalWins = 0;
    [SynchronizableField] public float points = 0;
    [SynchronizableField] public float money = 0;
    [SynchronizableField] public List<string> currentCardsInHand = new List<string>();
    [SynchronizableField] public List<string> currentJokerCards = new List<string>();
    [SynchronizableField] public List<string> currentTarotCards = new List<string>();
    public Dictionary<string, int> handScoresDict = new Dictionary<string, int>();
    
    public List<GameObject> cardsInHand = new List<GameObject>();
    public List<string> jokerDeck = new List<string>();
    public List<string> tarotDeck = new List<string>();
    public List<string> countyDeck = new List<string>();
    [SerializeField] public int cardsInHandCount;
    private GameObject Gameplayinfo;
    public Alteruna.Avatar avatar;

    public bool wasWinner = false;
    public bool wasLast = false;

    void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        Gameplayinfo = GameObject.Find("GameplayInfo");
        jokerDeck = Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardImageNamesList;
        tarotDeck = Gameplayinfo.GetComponent<GameplayInfo>()._tarotCardImageNamesList;
        countyDeck = Gameplayinfo.GetComponent<GameplayInfo>()._countyCardImageNamesList;
        handScoresDict = new Dictionary<string, int>(Gameplayinfo.GetComponent<GameplayInfo>().handScores);
    }

    void Update()
    {
        if (cardsInHand.Count != currentCardsInHand.Count && avatar.IsMe) BroadcastCard();
    }

    void BroadcastCard()
    {   
        if (!avatar.IsMe) return;
        List<string> cardList = new List<string>();
        List<string> jokerList = new List<string>();
        foreach (GameObject cardslot in cardsInHand)
        {
            GameObject card = cardslot.transform.GetChild(0).gameObject;
            cardList.Add($"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank}");
        }

        foreach (string joker in currentJokerCards)
        {
            jokerList.Add(joker);
        }

        BroadcastRemoteMethod("AddCardsTest", cardList, jokerList);
    }

    [SynchronizableMethod]
    void AddCardsTest(List<string> suitRankList, List<string> jokerList)
    {    
        currentCardsInHand.Clear();
        currentJokerCards.Clear();
        foreach (string suitrank in suitRankList)
        {
            currentCardsInHand.Add(suitrank);
        }

        foreach (string suitrank in jokerList)
        {
            currentJokerCards.Add(suitrank);
        }
    }



}
