using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject OtherPlayerPrefab;
    [SerializeField] private GameObject JokerPrefab;
    [SerializeField] private GameObject TarotPrefab;
    [SerializeField] private GameObject otherPlayersVisuals;
    private RectTransform rect;

    [Header("Spawn Settings")]
    public List<Card> cards;
    private System.Random rnd = new System.Random();
    public Dictionary<string, string> fullDeckDict = new Dictionary<string, string>();
    public Dictionary<string, string> deckDict = new Dictionary<string, string>();
    public List<string> fullDeckList = new List<string>();
    public List<string> deck = new List<string>();
    private Button TestBtn;
    bool isCrossing = false;
    private GameObject cardClone;
    private GameObject PlayerNameText;
    [SerializeField] private RoomBrowser room;
    [SerializeField] private GameObject Gameplayinfo;
    [SerializeField] private GameObject OtherPlayersCardholders;
    [SerializeField] private GameObject inGameJokerCardholder;
    [SerializeField] private GameObject inGameTarotCardholder;
    [SerializeField] private GameObject inGameJokerCardDescription;
    [SerializeField] private GameObject inGameTarotCardDescription;
    [SerializeField] private GameObject StealBtn;
    [SerializeField] private GameObject CardVisuals;
    [SerializeField] private bool tweenCardReturn = true;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public int PlayerID;
    private Multiplayer multiplayer;

    private bool roundStart = true;

    int CheckForFELSO()
    {
        Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        int hand = 0;
        foreach (string joker in Player.GetComponent<Player>().currentJokerCards)
        {
            if (joker.Split(" ")[1] == "FELSŐ") hand++;
        }
        return hand;
    }

    int CheckForALSO()
    {
        Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        int discard = 0;
        foreach (string joker in Player.GetComponent<Player>().currentJokerCards)
        {
            if (joker.Split(" ")[1] == "ALSÓ") discard++;
        }
        return discard;
    }

    int CheckForPIROS()
    {
        Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        int size = 0;
        foreach (string joker in Player.GetComponent<Player>().currentJokerCards)
        {
            if (joker.Split(" ")[0] == "Piros") size = 1;
        }
        return size;
    }

    void Start()
    {   
        StealBtn = GameObject.Find("STEAL BUTTON");
        room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;
        Gameplayinfo = GameObject.Find("GameplayInfo");

        fullDeckList = new List<string>(Gameplayinfo.GetComponent<GameplayInfo>().pokerCardImageNames);

        foreach (string card in Gameplayinfo.GetComponent<GameplayInfo>().pokerCardImageNames)
        {
            fullDeckDict[$"{card};{rnd.Next(10000000)}"] = $"REGULAR";
        }
        //fullDeckList = new List<string>(Gameplayinfo.GetComponent<GameplayInfo>().pokerCardImageNames);
        deck = fullDeckList;
        deckDict = new Dictionary<string, string>(fullDeckDict);        
    }

    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }


    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0,selectedCard.selectionOffset,0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    IEnumerator CardOrder()
    {   

        cards = GetComponentsInChildren<Card>().ToList();

        int count = 1;
        if (Gameplayinfo.GetComponent<GameplayInfo>().orderByRank)
        {
            foreach (string rank in Gameplayinfo.GetComponent<GameplayInfo>().ranks)
            {
                foreach (string suit in Gameplayinfo.GetComponent<GameplayInfo>().suit)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].suit == suit &&  cards[i].rank == rank)
                        {  
                            cards[i].transform.parent.SetSiblingIndex(count);
                            cards[i].cardVisual.UpdateIndex(transform.childCount);
                            count++;
                        }
                    }   
                }
            }
        }
        else
        {
            foreach (string suit in Gameplayinfo.GetComponent<GameplayInfo>().suit)
            {
                foreach (string rank in Gameplayinfo.GetComponent<GameplayInfo>().ranks)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].cardVisual != null && cards[i].suit == suit &&  cards[i].rank == rank)
                        {  
                            cards[i].transform.parent.SetSiblingIndex(count);
                            cards[i].cardVisual.UpdateIndex(transform.childCount);
                            count++;
                        }
                    }   
                }
            }
        }
        yield return new WaitForSeconds(0);
    }

    public void AddCards(bool random = true, string suitInput = null, string rankInput = null, string editionInput = "REGULAR", string idInput = "0")
    {   
        int cardsInHandCount = Player.GetComponent<Player>().cardsInHandCount;
        int handSize = Player.GetComponent<Player>().handSize;

        int objectCount = 0;

        if (random)
        {   
            try
            {
                for (int i = 0; cardsInHandCount < handSize; i++)
                {   
                    objectCount = 0;

                    
                    //Deckssss
                    if (deck.Count == 0) break;
                    objectCount++; //1
                    
                    int randomCard = rnd.Next(deckDict.Count); 
                    objectCount++; //2

                    string[] suitRank = deckDict.ElementAt(randomCard).Key.Split(';')[0].Split(' ');
                    objectCount++; //3

                    string edition = deckDict.ElementAt(randomCard).Value;
                    objectCount++; //4

                    string suit = suitRank[0];
                    objectCount++; //5

                    string rank = suitRank[1];
                    objectCount++; //6

                    cardClone = Instantiate(slotPrefab, transform);
                    objectCount++; //7

                    cardClone.tag = "Slot";
                    objectCount++; //8

                    GameObject cardObject = cardClone.transform.GetChild(0).gameObject;
                    objectCount++; //9

                    Card cardComponent = cardObject.GetComponent<Card>();
                    objectCount++; //10

                    //Player.GetComponent<Player>().currentCardsInHand.Add($"{suit} {rank}");
                    cardComponent.canvas = cardObject.GetComponentInParent<Canvas>();
                    objectCount++; //11

                    cardComponent.suit = suit;
                    objectCount++; //12

                    cardComponent.rank = rank;
                    objectCount++; //13

                    cardComponent.cardID = deckDict.ElementAt(randomCard).Key.Split(';')[1];
                    objectCount++; //14

                    cardComponent.edition = deckDict.ElementAt(randomCard).Value;
                    objectCount++; //15

                    cardComponent.visualHandler = null;
                    objectCount++; //16

                    cardComponent.cardSlot = cardComponent.transform.parent.gameObject;
                    objectCount++; //17

                    cardComponent.cardVisualObj = Instantiate(cardComponent.cardVisualPrefab, cardComponent.visualHandler ? cardComponent.visualHandler.transform : cardComponent.canvas.transform);
                    objectCount++; //18

                    cardComponent.cardVisual = cardComponent.cardVisualObj.GetComponent<CardVisual>();
                    objectCount++; //19
                    //cardComponent.cardVisualObj.transform.SetParent(GameObject.Find("CardVisuals").transform);
                    cardComponent.cardVisualObj.transform.SetParent(CardVisuals.transform);
                    objectCount++; //20
                    //cardVisual.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    cardComponent.cardVisual.Initialize(cardComponent);
                    objectCount++; //21
                    cardComponent.cardVisualObj.GetComponent<CardVisual>().suit = suit;
                    objectCount++; //22
                    cardComponent.cardVisualObj.GetComponent<CardVisual>().rank = rank;
                    objectCount++; //23
                    ShaderCode shaderCode = cardComponent.cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>();
                    objectCount++; //24
                    shaderCode.image = cardComponent.cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                    objectCount++; //25
                    shaderCode.m = new Material(shaderCode.image.material);
                    objectCount++; //26
                    shaderCode.image.material = shaderCode.m;
                    objectCount++; //27
                    for (int j = 0; j < shaderCode.image.material.enabledKeywords.Length; j++)
                    {
                        shaderCode.image.material.DisableKeyword(shaderCode.image.material.enabledKeywords[j]);
                    }
                    objectCount++; //28
                    shaderCode.image.material.EnableKeyword($"_EDITION_{cardComponent.edition}");
                    objectCount++; //29
                    Player.GetComponent<Player>().cardsInHand.Add(cardClone);
                    objectCount++; //30
                    if (cardComponent.edition != "NEGATIVE") Player.GetComponent<Player>().cardsInHandCount++;
                    objectCount++; //31
                    Player.GetComponent<Player>().remainingDeck -= 1;
                    objectCount++; //32
                    cardsInHandCount = Player.GetComponent<Player>().cardsInHandCount;
                    objectCount++; //33
                    handSize = Player.GetComponent<Player>().handSize;
                    objectCount++; //34
                    deckDict.Remove(deckDict.ElementAt(randomCard).Key);
                    objectCount++; //35
                    //Debug.Log(objectCount);
                }
            } 
            catch (Exception e)
            {   
                StatusPopup.Instance.TriggerStatus($"Error RANDOM AT {objectCount}: {e}");
            }
        }
        else
        {   
            try
            {
                cardClone = Instantiate(slotPrefab, transform);
                cardClone.tag = "Slot";
                GameObject cardObject = cardClone.transform.GetChild(0).gameObject;
                Card cardComponent = cardObject.GetComponent<Card>();
                cardComponent.canvas = cardObject.GetComponentInParent<Canvas>();
                cardComponent.suit = suitInput;
                cardComponent.rank = rankInput;
                cardComponent.edition = editionInput;
                cardComponent.cardID = idInput;
                cardComponent.visualHandler = FindObjectOfType<VisualCardsHandler>();
                cardComponent.cardSlot = cardComponent.transform.parent.gameObject;
                cardComponent.cardVisualObj = Instantiate(cardComponent.cardVisualPrefab, cardComponent.visualHandler ? cardComponent.visualHandler.transform : cardComponent.canvas.transform);
                cardComponent.cardVisual = cardComponent.cardVisualObj.GetComponent<CardVisual>();
                if (transform.parent.name == "OTHER PLAYERS CARDHOLDERS")
                {   
                    cardClone.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    cardComponent.cardVisual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    string playerName = transform.name.Split(" ")[1];
                    GameObject.Find($"PlayerCardVisualGroup {playerName}").transform.SetParent(GameObject.Find("OtherPlayersCardVisuals").transform);
                    cardComponent.cardVisualObj.transform.SetParent(GameObject.Find($"PlayerCardVisualGroup {playerName}").transform);
                }
                else
                {
                    cardComponent.cardVisualObj.transform.SetParent(CardVisuals.transform);
                }
                ShaderCode shaderCode = cardComponent.cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>();
                shaderCode.image = cardComponent.cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                shaderCode.m = new Material(shaderCode.image.material);
                shaderCode.image.material = shaderCode.m;
                for (int j = 0; j < shaderCode.image.material.enabledKeywords.Length; j++)
                {
                    shaderCode.image.material.DisableKeyword(shaderCode.image.material.enabledKeywords[j]);
                }
                shaderCode.image.material.EnableKeyword($"_EDITION_{cardComponent.edition}");
                cardComponent.cardVisual.Initialize(cardComponent);
                cardComponent.cardVisualObj.GetComponent<CardVisual>().suit = suitInput;
                cardComponent.cardVisualObj.GetComponent<CardVisual>().rank = rankInput;

                //cardsInHandCount = Player.GetComponent<Player>().cardsInHandCount;
                //handSize = Player.GetComponent<Player>().handSize;

            } catch (Exception e)
            {   
                StatusPopup.Instance.TriggerStatus($"Error OTHERPLAYERS: {e}");
            }
        }
        
        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>().ToList();

        foreach (Card card in cards)
        {
            card.PointerEnterEvent.AddListener(CardPointerEnter);
            card.PointerExitEvent.AddListener(CardPointerExit);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);
            card.name = card.suit + " " + card.rank;
        }
        
        //Gameplayinfo.GetComponent<GameplayInfo>().initiatedOrdering = true;
        StartCoroutine(CardOrder());
        //yield return new WaitForSeconds(0);
        
    }

    public void OnDisable()
    {
        if (transform.parent.name == "OTHER PLAYERS CARDHOLDERS")
        {
            foreach (Transform cardslot in transform)
            {   
                foreach(Transform card in cardslot)
                {
                    Destroy(card.gameObject.GetComponent<Card>().cardVisualObj);
                    Destroy(card.gameObject.GetComponent<Card>().cardSlot);
                    Destroy(card.gameObject);
                } 
            }
        }
    }

    void AddOtherPlayerCardGroups()
    {   try
        {
            if (transform.parent.name != "OTHER PLAYERS CARDHOLDERS")
            {   

                foreach (User user in multiplayer.GetUsers())
                {   
                    GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                    GameObject PlayerNameText = new GameObject($"PlayerNameText {user.Name}");
                    GameObject PlayerCardVisualGroup = new GameObject($"PlayerCardVisualGroup {user.Name}");
                    PlayerCardVisualGroup.transform.SetParent(otherPlayersVisuals.transform);
                    PlayerNameText.AddComponent<TextMeshProUGUI>();
                    PlayerNameText.GetComponent<TMP_Text>().text = $"<color=red>{user.Name}</color> | <color=blue>SCORE: {currentPlayer.GetComponent<Player>().score}</color> | <color=green>HANDS: {currentPlayer.GetComponent<Player>().hands}</color> | <color=red>DISCARDS: {currentPlayer.GetComponent<Player>().discards}</color> | TOTAL POINTS: {currentPlayer.GetComponent<Player>().points} | MONEY: {currentPlayer.GetComponent<Player>().money}";
                    PlayerNameText.GetComponent<TMP_Text>().fontSize = 40f;
                    PlayerNameText.GetComponent<TMP_Text>().color = Color.black;
                    PlayerNameText.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                    PlayerNameText.transform.SetParent(OtherPlayersCardholders.transform);
                    PlayerNameText.transform.localScale = new Vector3(1f, 1f, 1f);
                    if (user.Name == multiplayer.Me.Name) continue;
                    GameObject otherPlayer = Instantiate(OtherPlayerPrefab);
                    otherPlayer.name = $"PlayingCardGroup {user.Name}";
                    otherPlayer.transform.SetParent(OtherPlayersCardholders.transform);
                    otherPlayer.transform.localScale = new Vector3(0.5f, 1f, 1f);
                }
                Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
            //PlayerID = Player.GetComponent<PlayerScript>().playerID;
                deck = fullDeckList.ToList();
                deckDict = new Dictionary<string, string>(fullDeckDict);

                Player.GetComponent<Player>().fullDeck = deck.Count;
                Player.GetComponent<Player>().remainingDeck = deck.Count;

                Player.GetComponent<Player>().discards = Player.GetComponent<Player>().maxDiscards;
                Player.GetComponent<Player>().hands = Player.GetComponent<Player>().maxHands;
                Player.GetComponent<Player>().hands += CheckForFELSO();
                Player.GetComponent<Player>().discards += CheckForALSO();
                Player.GetComponent<Player>().handSize = 8 + CheckForPIROS();

                if (Player.GetComponent<Player>().wasWinner) Player.GetComponent<Player>().hands--;
                else if (Player.GetComponent<Player>().wasLast) Player.GetComponent<Player>().discards++;

                Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();

                string selectedJokerDeck = GameObject.Find("GameHandler").GetComponent<GameHandler>().selectedJokerDeck;
                string selectedTarotDeck = GameObject.Find("GameHandler").GetComponent<GameHandler>().selectedTarotDeck;

                foreach (Transform child in inGameJokerCardholder.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (Transform child in inGameTarotCardholder.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (string joker in Player.GetComponent<Player>().currentJokerCards)
                {
                    string suit = joker.Split(" ")[0];
                    string rank = joker.Split(" ")[1];
                    GameObject ownedJokerClone = Instantiate(JokerPrefab, inGameJokerCardholder.transform);
                    ownedJokerClone.transform.localScale = new Vector3(0.7f, 0.77f, 0.7f);
                    ownedJokerClone.transform.name = $"{suit} {rank}";
                    ownedJokerClone.GetComponent<JokerCard>().suit = suit;
                    ownedJokerClone.GetComponent<JokerCard>().rank = rank;
                    ownedJokerClone.GetComponent<JokerCard>().cardName = Gameplayinfo.GetComponent<GameplayInfo>().jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[0];
                    ownedJokerClone.GetComponent<JokerCard>().cardDescription = Gameplayinfo.GetComponent<GameplayInfo>().jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[1];
                    ownedJokerClone.GetComponent<JokerCard>().price = Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardPricesSimple[ownedJokerClone.GetComponent<JokerCard>().rank];
                    ownedJokerClone.GetComponent<JokerCard>().description = Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[ownedJokerClone.GetComponent<JokerCard>().rank];
                    ownedJokerClone.GetComponent<JokerCard>().isOwned = true;
                    Sprite[] cards = Resources.LoadAll<Sprite>(selectedJokerDeck);
                    ownedJokerClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[Gameplayinfo.GetComponent<GameplayInfo>()._jokerImageNamesDict[$"{suit} {rank}"]];
                }

                foreach (string tarot in Player.GetComponent<Player>().currentTarotCards)
                {
                    GameObject ownedTarotClone = Instantiate(TarotPrefab, inGameTarotCardholder.transform);
                    ownedTarotClone.transform.localScale = new Vector3(0.7f, 0.77f, 0.7f);
                    ownedTarotClone.transform.name = tarot;
                    ownedTarotClone.GetComponent<TarotCard>().nameOfCard = tarot;
                    ownedTarotClone.GetComponent<TarotCard>().cardName = Gameplayinfo.GetComponent<GameplayInfo>().tarotDeckCardNamesDescDict[selectedTarotDeck][tarot].Split(';')[0];
                    ownedTarotClone.GetComponent<TarotCard>().cardDescription = Gameplayinfo.GetComponent<GameplayInfo>().tarotDeckCardNamesDescDict[selectedTarotDeck][tarot].Split(';')[1];
                    ownedTarotClone.GetComponent<TarotCard>().price = Gameplayinfo.GetComponent<GameplayInfo>()._tarotCardPricesSimple[tarot];
                    ownedTarotClone.GetComponent<TarotCard>().description = Gameplayinfo.GetComponent<GameplayInfo>()._tarotCardDescriptionSimple[tarot];
                    ownedTarotClone.GetComponent<TarotCard>().isOwned = true;
                    Sprite[] cards = Resources.LoadAll<Sprite>(selectedTarotDeck);
                    ownedTarotClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[Gameplayinfo.GetComponent<GameplayInfo>()._tarotImageNamesDict[tarot]];
                }
            } 
            else
            {
                string playerName = transform.name.Split(" ")[1];
                Player = GameObject.Find($"Playerinfo ({playerName})");
            }

            roundStart = false;
        }
        catch (Exception e)
        {   
            StatusPopup.Instance.TriggerStatus($"Error: {e}");
        }
    }

    public void RoundStart()
    {
        roundStart = true;
        Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        Gameplayinfo.GetComponent<GameplayInfo>().selectedJokerCount = 0;
        Gameplayinfo.GetComponent<GameplayInfo>().selectedOwnedJokerCount = 0;
        foreach (Transform child in OtherPlayersCardholders.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {   
        
        if (roundStart) AddOtherPlayerCardGroups();

        GameObject Gamehandler = GameObject.Find("GameHandler");

        bool otherPlayerPlaysOrDiscards = false;

        foreach (User user in Gamehandler.GetComponent<GameHandler>().users)
        {
            GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
            if ((currentPlayer.GetComponent<Player>().isPlaying || currentPlayer.GetComponent<Player>().isDiscarding) && transform.parent.name == "OTHER PLAYERS CARDHOLDERS")
            {   
                foreach (Transform child in GameObject.Find($"PlayingCardGroup {user.Name}").transform)
                {   
                    GameObject card = child.GetChild(0).gameObject;
                    if (card.GetComponent<Card>().selected && user.Name == transform.name.Split(" ")[1])
                    {
                        card.GetComponent<Card>().selected = false;
                        card.transform.localPosition = Vector3.zero;
                        Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCards = 0;
                        Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCardObj = null;
                        Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCardUsername = "";
                    }
                }
                otherPlayerPlaysOrDiscards = true;
                break;
            } 
            
        }

        if (otherPlayerPlaysOrDiscards && transform.parent.name == "OTHER PLAYERS CARDHOLDERS") StealBtn.GetComponent<Button>().interactable = false;
        else if (transform.parent.name == "OTHER PLAYERS CARDHOLDERS" && GameObject.Find("GameHandler").GetComponent<GameHandler>().magicianActivated && Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCards == 1)
        {
            StealBtn.GetComponent<Button>().interactable = true;
        }
        else if (transform.parent.name == "OTHER PLAYERS CARDHOLDERS")
        {
            StealBtn.GetComponent<Button>().interactable = false;
        }
        
        if (transform.parent.name == "OTHER PLAYERS CARDHOLDERS" && !Gamehandler.GetComponent<GameHandler>().roundEnd)
        {
            string playerName = transform.name.Split(" ")[1];
            Player = GameObject.Find($"Playerinfo ({playerName})");
            PlayerNameText = GameObject.Find($"PlayerNameText {playerName}");

            if (Player.GetComponent<Player>().playerID == 1)
            {
                PlayerNameText.transform.SetSiblingIndex(0);
                transform.SetSiblingIndex(1);   
            }
            else if (Player.GetComponent<Player>().playerID == 2)
            {
                PlayerNameText.transform.SetSiblingIndex(2);
                transform.SetSiblingIndex(3);   
            }
            else if (Player.GetComponent<Player>().playerID == 3)
            {
                PlayerNameText.transform.SetSiblingIndex(4);
                transform.SetSiblingIndex(5);   
            }
            else if (Player.GetComponent<Player>().playerID == 4)
            {
                PlayerNameText.transform.SetSiblingIndex(6);
                transform.SetSiblingIndex(7);   
            }
            else if (Player.GetComponent<Player>().playerID == 5)
            {
                PlayerNameText.transform.SetSiblingIndex(8);
                transform.SetSiblingIndex(9);   
            }
            else if (Player.GetComponent<Player>().playerID == 6)
            {
                PlayerNameText.transform.SetSiblingIndex(10);
                transform.SetSiblingIndex(11);   
            }
        }

        else
        {
            Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        }

        if (transform.parent.name != "OTHER PLAYERS CARDHOLDERS") transform.SetSiblingIndex(0);

        if (Player.GetComponent<Player>().cardsInHandCount < Player.GetComponent<Player>().handSize && Player.GetComponent<Player>().hands != 0 && transform.parent.name != "OTHER PLAYERS CARDHOLDERS" && deck.Count > 0)
        {   
            AddCards();          
        }
        else if (transform.childCount < Player.GetComponent<Player>().currentCardsInHand.Count && transform.parent.name == "OTHER PLAYERS CARDHOLDERS" && Player.GetComponent<Player>().hands != 0 && Player.GetComponent<Player>().Username != multiplayer.Me.Name)
        {
            
            // foreach (Transform child in transform)
            // {   
            //     Destroy(child.gameObject.GetComponent<Card>().cardVisualObj);
            //     Destroy(child.gameObject.GetComponent<Card>().cardSlot);
            //     Destroy(child);
            // }

            foreach (Transform child in transform)
            {
                Destroy(child.GetChild(0).gameObject);
                Destroy(child.GetChild(0).gameObject.GetComponent<Card>().cardVisualObj);
                Destroy(child.GetChild(0).gameObject.GetComponent<Card>().cardSlot);
            }

            foreach (string card in Player.GetComponent<Player>().currentCardsInHand)
            {   
                string suit = card.Split(" ")[0];
                string rank = card.Split(" ")[1];
                AddCards(false, suit, rank);  
            }
        } 
        
        if (Gameplayinfo.GetComponent<GameplayInfo>().initiatedOrdering == true)
        {
            StartCoroutine(CardOrder());
            Gameplayinfo.GetComponent<GameplayInfo>().initiatedOrdering = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Card card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);
        
        //Updated Visual Indexes
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }

}
