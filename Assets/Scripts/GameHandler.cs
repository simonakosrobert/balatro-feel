using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Alteruna;
using System;
using Unity.VisualScripting;
using UnityEngine.Diagnostics;
public class GameHandler : AttributesSync
{
    [Header("Objects and prefabs")]
    [SerializeField] private GameObject Gameplayinfo;
    [SerializeField] private GameplayInfo gameplayInfoComponent;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject PointText;
    [SerializeField] private GameObject JokerPrefab;
    [SerializeField] private GameObject tarotPrefab;
    [SerializeField] private GameObject countyPrefab;
    [SerializeField] private GameObject priestessEffectPrefab;
    [SerializeField] private GameObject CardVisuals;
    [SerializeField] private GameObject OtherPlayersCardVisuals;
    [SerializeField] private GameObject yourTurnSymbol;
    [SerializeField] private GameObject timerText;
    [SerializeField] private GameObject timerCircle;
    [SerializeField] public GameObject touchBlock;
    [SerializeField] private GameObject TrippyBG;
    [SerializeField] private GameObject ingameJokerHolder;
    [SerializeField] private GameObject ingameTarotHolder;
    [SerializeField] private GameObject playerResultBar1;
    [SerializeField] private GameObject playerResultBar1StartPos;
    [SerializeField] private GameObject playerResultBar1FinalPos;
    [SerializeField] private GameObject playerResultBar2;
    [SerializeField] private GameObject playerResultBar2StartPos;
    [SerializeField] private GameObject playerResultBar2FinalPos;
    [SerializeField] private GameObject playerResultBar3;
    [SerializeField] private GameObject playerResultBar3StartPos;
    [SerializeField] private GameObject playerResultBar3FinalPos;
    [SerializeField] private GameObject playerResultBar4;
    [SerializeField] private GameObject playerResultBar4StartPos;
    [SerializeField] private GameObject playerResultBar4FinalPos;
    [SerializeField] private GameObject playerResultBar5;
    [SerializeField] private GameObject playerResultBar5StartPos;
    [SerializeField] private GameObject playerResultBar5FinalPos;
    [SerializeField] private GameObject playerResultBar6;
    [SerializeField] private GameObject playerResultBar6StartPos;
    [SerializeField] private GameObject playerResultBar6FinalPos;
    [Header("Texts")]
    [SerializeField] private GameObject HandCheckText = null;
    [SerializeField] private GameObject HandScoreText = null;
    [SerializeField] private GameObject scoreGatheredText = null;
    [SerializeField] private GameObject TotalScoreText = null;
    [SerializeField] private GameObject HandsText = null;
    [SerializeField] private GameObject ScoreText = null;
    [SerializeField] private GameObject DiscardsText = null;
    [SerializeField] private GameObject DeckSizeText = null;
    [SerializeField] private GameObject CurrentPlayerText = null;
    [SerializeField] private GameObject CurrentPlayerTextTop = null;
    [SerializeField] private GameObject RoundText = null;
    [SerializeField] private GameObject shopRoundText = null;
    [SerializeField] private GameObject resultsReadyPlayersCount = null;
    [SerializeField] private GameObject shopReadyPlayersCount = null;
    [SerializeField] private GameObject otherPlayerPlayedHandText;

    [Header("Buttons")]
    [SerializeField] private GameObject DiscardBtn;
    [SerializeField] private GameObject PlayBtn;
    [SerializeField] private GameObject SuitBtn;
    [SerializeField] private GameObject RankBtn;
    [SerializeField] private GameObject OtherPlayersBtn;
    [SerializeField] private GameObject ShopReadyBtn;
    [SerializeField] private GameObject ResultsReadyBtn;
    [SerializeField] private GameObject RerollBtn;
    [SerializeField] private GameObject ExitBtn;
    [SerializeField] private GameObject DeckBtn;
    [SerializeField] private GameObject HandScoreBtn;
    [SerializeField] private GameObject StealBtn;

    [Header("Card Groups")]
    [SerializeField] private Transform PlayingCardGroup;
    [SerializeField] private GameObject PlayedHandGroup;

    [Header("Panels")]
    [SerializeField] private GameObject PlayerStatsPanel;
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject HandScorePanel;
    [SerializeField] private GameObject DeckPanel;
    [SerializeField] private GameObject LeftBGPanel;

    [Header("Hover Parameters")]
    [SerializeField] private float hoverPunchAngle = 35;
    [SerializeField] private float hoverTransition = .15f;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public Player playerComponent;
    [HideInInspector] public int PlayerID;
    [HideInInspector] public int currentPlayerID;
    
    private float score = 0;
    private float mult = 0;
    private float cardscore = 0;
    //private float totalScore = 0;
    public bool Playing = false;
    public bool Discarding = false;
    private bool foundRoom = false;
    private bool inResults = false;
    private bool inResultsInitialized = false;
    private bool inShop = false;
    private bool inShopInitialized = false;
    private bool vibrated = false;
    private bool otherPlayerPlayedHand = false;
    private bool timeSet = false;
    private float otherPlayerPlayedHandTimer = 0;
    public List<User> users = new List<User>();
    List<string> scores = new List<string>();
    [SerializeField] List<string> endGamePointsList = new List<string>();
    public bool roundEnd;
    bool roundEndInitialized = false;
    bool scoresShowing = false;
    int rerollCost = 2;
    int noHandsLeftCounter = 0;

    [NonSerialized] public string selectedJokerDeck = "hun_historical_figures";
    [NonSerialized] public string selectedTarotDeck = "mystical";
    [NonSerialized] public string selectedCountyDeck = "varmegye";
    int tokMoney = 0;
    int zoldScore = 0;
    int firstScore = 0;
    int secondScore = 0;
    int thirdScore = 0;
    int fourthScore = 0;
    int fifthScore = 0;
    int firstMoney = 0;
    int secondMoney = 0;
    int thirdMoney = 0;
    int fourthMoney = 0;
    int fifthMoney = 0;
    int sixthMoney = 0;
    [SerializeField] float timeLeft = 30f;
    [Range(0, 100)]
    
    public bool magicianActivated = false;
    private System.Random rnd = new System.Random();

    [SerializeField] private RoomBrowser room;

    private Multiplayer multiplayer;
    //private bool foundRoom = false;
    public IEnumerator FlipCardsEnum(Transform PlayingCardHolder)
	{   

        PlayerStatsPanel.SetActive(false);
        DeckPanel.SetActive(false);
        HandScorePanel.SetActive(false);
        CardVisuals.SetActive(true);

		foreach (Transform child in PlayingCardHolder)
		{
			GameObject cardObject = child.GetChild(0).gameObject;
            cardObject.GetComponent<Card>().isFlipped = true;
            ShaderCode shaderCode = cardObject.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>();
			shaderCode.image = cardObject.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
			shaderCode.m = new Material(shaderCode.image.material);
			shaderCode.image.material = shaderCode.m;
			for (int j = 0; j < shaderCode.image.material.enabledKeywords.Length; j++)
			{
				shaderCode.image.material.DisableKeyword(shaderCode.image.material.enabledKeywords[j]);
			}
			shaderCode.image.material.EnableKeyword($"_EDITION_REGULAR");
			cardObject.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("pokercardback");
			yield return new WaitForSeconds(0.2f);
		}
		yield return null;
	}

	public void FlipCardsBroadcast()
	{   
        if (users.Count == 1) BroadcastRemoteMethod("FlipCards");
		else InvokeRemoteMethod("FlipCards");
	}

    [SynchronizableMethod]
    public void NoHandsLeft()
    {
        noHandsLeftCounter++;
    }

	[SynchronizableMethod]
	public void FlipCards()
	{
		StartCoroutine(FlipCardsEnum(GameObject.Find("PlayingCardGroup").transform));
        GameObject priestessEffect = Instantiate(priestessEffectPrefab, GameObject.Find("Game Canvas").transform);
        priestessEffect.transform.SetSiblingIndex(0);
	}

    public IEnumerator MagicianCardEnum(string suitrank)
	{   

        PlayerStatsPanel.SetActive(false);
        DeckPanel.SetActive(false);
        HandScorePanel.SetActive(false);
        CardVisuals.SetActive(true);

        foreach (GameObject card in playerComponent.cardsInHand)
        {
            if (card.transform.GetChild(0).gameObject.GetComponent<Card>().selected)
            {
                card.transform.GetChild(0).gameObject.GetComponent<Card>().selected = false;
                card.transform.GetChild(0).gameObject.transform.localPosition = Vector3.zero;
            }
        }

        gameplayInfoComponent.selectedCards.Clear();
        gameplayInfoComponent.selectedHandCount = 0;

		foreach (Transform child in PlayingCardGroup)
		{
			GameObject card = child.GetChild(0).gameObject;
            Card cardComponent = card.GetComponent<Card>();

            if ($"{cardComponent.suit} {cardComponent.rank}" == suitrank)
            {
                cardComponent.cardSlot = card.transform.parent.gameObject;
                cardComponent.isDiscarded = true;
                card.transform.localPosition += cardComponent.cardVisual.transform.up * -600;
                playerComponent.currentCardsInHand.Remove($"{cardComponent.suit} {cardComponent.rank}");
                yield return new WaitForSeconds(0.05f);
                playerComponent.cardsInHand.Remove(cardComponent.cardSlot);
                if (cardComponent.edition != "NEGATIVE") playerComponent.cardsInHandCount--;
                PlayingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckList.Remove($"{cardComponent.suit} {cardComponent.rank}");
                PlayingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict.Remove($"{cardComponent.suit} {cardComponent.rank};{cardComponent.cardID}");
                playerComponent.fullDeck -= 1;
                Destroy(card);
                Destroy(cardComponent.cardVisualObj);
                Destroy(cardComponent.cardSlot);
                break;
            }
		}
		yield return null;
	}

	public void MagicianBroadcast()
	{   
        string suit = gameplayInfoComponent.selectedMagicianCardObj.GetComponent<Card>().suit;
        string rank = gameplayInfoComponent.selectedMagicianCardObj.GetComponent<Card>().rank;
        string username = gameplayInfoComponent.selectedMagicianCardUsername;
        string suitrank = $"{suit} {rank}";
        //gameHandler.MagicianCard(gameplayInfoComponent.selectedMagicianCardUsername, $"{suit} {rank}");

        PlayerStatsPanel.SetActive(false);
        DeckPanel.SetActive(false);
        HandScorePanel.SetActive(false);
        CardVisuals.SetActive(true);

        int randomID = rnd.Next(10000000);
        //GameObject playingCardGroup = GameObject.Find("PlayingCardGroup");
        PlayingCardGroup.GetComponent<HorizontalCardHolder>().AddCards(false, suit, rank, "REGULAR", randomID.ToString());
        PlayingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckList.Add($"{suit} {rank}");
        PlayingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict[$"{suit} {rank};{randomID}"] = "REGULAR";
        playerComponent.cardsInHandCount += 1;  
        playerComponent.fullDeck += 1;
        BroadcastRemoteMethod("MagicianCard", username, suitrank);
        magicianActivated = false;
        gameplayInfoComponent.selectedMagicianCards = 0;
        gameplayInfoComponent.selectedMagicianCardObj = null;         
        gameplayInfoComponent.selectedMagicianCardUsername = ""; 
	}

	[SynchronizableMethod]
	public void MagicianCard(string username, string suitrank)
	{   
        if (username != playerComponent.Username) return;
		StartCoroutine(MagicianCardEnum(suitrank));
        GameObject priestessEffect = Instantiate(priestessEffectPrefab, GameObject.Find("Game Canvas").transform);
        priestessEffect.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>(selectedTarotDeck)[1];
        priestessEffect.transform.SetSiblingIndex(0);
	}

    [SynchronizableMethod]
    public void SendPlayedHand(string username, int score, string hand)
    {   
        otherPlayerPlayedHandTimer = 0;
        otherPlayerPlayedHand = true;
        otherPlayerPlayedHandText.GetComponent<TMP_Text>().text = $"{username} kijátszotta:\n{hand} - {score} pont";
        otherPlayerPlayedHandText.SetActive(true);

        GameObject userGameObject = GameObject.Find($"PlayingCardGroup {username}");

        if (userGameObject == null) return;

        foreach (Transform child in userGameObject.transform)
        {
            Destroy(child.GetChild(0).gameObject);
            Destroy(child.GetChild(0).gameObject.GetComponent<Card>().cardVisualObj);
            Destroy(child.GetChild(0).gameObject.GetComponent<Card>().cardSlot);
        }

        //otherPlayerPlayedHandText.SetActive(false);
    }

    [SynchronizableMethod]
    public void SendDiscard(string username)
    {   
        otherPlayerPlayedHandTimer = 0;
        otherPlayerPlayedHand = true;
        otherPlayerPlayedHandText.GetComponent<TMP_Text>().text = $"{username} lapokat dobott el!";
        otherPlayerPlayedHandText.SetActive(true);
        GameObject userGameObject = GameObject.Find($"PlayingCardGroup {username}");

        if (userGameObject == null) return;

        foreach (Transform child in userGameObject.transform)
        {
            Destroy(child.GetChild(0).gameObject);
            Destroy(child.GetChild(0).gameObject.GetComponent<Card>().cardVisualObj);
            Destroy(child.GetChild(0).gameObject.GetComponent<Card>().cardSlot);
        }
    }

    int CheckForALSO()
    {
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        int discard = 0;
        foreach (string joker in playerComponent.currentJokerCards)
        {
            if (joker.Split(" ")[1] == "ALSÓ") discard++;
        }
        return discard;
    }
    
    IEnumerator DiscardCards(List<GameObject> selectedCards, bool outOfTime)
    {    
        touchBlock.SetActive(true);
        yourTurnSymbol.SetActive(false);
        int playedCardCount = 0;
        int discardedCardCount = selectedCards.Count;

        if (outOfTime)
        {   
            foreach (GameObject card in selectedCards)
            {   
                card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * 50;
                yield return new WaitForSeconds(0.5f);
            }
        }

        foreach (GameObject card in selectedCards)
        {   
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.cardSlot = card.transform.parent.gameObject;
            cardComponent.isDiscarded = true;
            if (cardComponent.edition != "NEGATIVE") playedCardCount++;
            card.transform.localPosition += cardComponent.cardVisual.transform.up * -600;
            playerComponent.currentCardsInHand.Remove($"{cardComponent.suit} {cardComponent.rank}");
            yield return new WaitForSeconds(0.05f);
            playerComponent.cardsInHand.Remove(cardComponent.cardSlot);
            Destroy(card);
            Destroy(cardComponent.cardVisualObj);
            Destroy(cardComponent.cardSlot);
            yield return new WaitForSeconds(0.05f);
        }
        
        BroadcastRemoteMethod("SendDiscard", playerComponent.Username);

        gameplayInfoComponent.selectedCards.Clear();
        gameplayInfoComponent.selectedHandCount = 0;
        playerComponent.cardsInHandCount -= playedCardCount;        
        
        Discarding = false;
        playerComponent.isDiscarding = false;
        vibrated = false;

        if (playerComponent.cardsInHandCount == 0) playerComponent.hands = 0;

        int nextID = playerComponent.playerID + 1;
        if (nextID > users.Count) nextID = 1;

        playerComponent.isCurrent = false;
        timeSet = false;

        bool nextFound = false;
        int endCount = 0;

        touchBlock.SetActive(false);

        while (!nextFound)
        {
            foreach (User user in users)
            {
                GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                Player currentPlayerComponent = currentPlayer.GetComponent<Player>();
                if (currentPlayerComponent.playerID == nextID && currentPlayerComponent.hands == 0) 
                {
                    nextID++;
                    if (nextID > users.Count) nextID = 1;
                    endCount++;
                }
                if (currentPlayerComponent.playerID == nextID)
                {
                    currentPlayerComponent.isCurrent = true;
                    nextFound = true;
                } 
            }

            if (endCount == users.Count)
            {   

                break;
            }
        }
    }

    IEnumerator CountUpToTarget(float currentDisplayScore, float targetScore, TMP_Text field, bool gatheredScore = false)
    {
        while (Mathf.Round(currentDisplayScore) < Mathf.Round(targetScore))
        {
            currentDisplayScore += Time.deltaTime * (targetScore - currentDisplayScore) * 8;
            currentDisplayScore = Mathf.Clamp(currentDisplayScore, 0f, targetScore);
            field.text = Mathf.Round(currentDisplayScore).ToString();
            if (gatheredScore)
            {
                float currentColor = 0;

                currentColor = currentDisplayScore / 10;
                if (currentColor > 255) currentColor = 255;
                field.color = new Color32(255, (byte)(255 - currentColor), (byte)(255 - currentColor), 255);
                float fontsize = 100 + currentDisplayScore / 70;
                field.fontSize = fontsize < 150 ? fontsize : 150;
                TrippyBG.GetComponent<Image>().color = new Color32((byte)currentColor, 60, 47, 100);

                //field.transform.gameObject.transform.DOShakePosition(0.3f, 1, vibrato: (int)Mathf.Round(currentDisplayScore / 5));
                //field.transform.gameObject.transform.DOPunchRotation(Vector3.forward * (currentDisplayScore / 300), .05f, 20, 0).SetId(3);
            }
            yield return null;
        }
    }

    IEnumerator CountDownToTarget(float currentDisplayScore, float targetScore, TMP_Text field, float scoreGathered, float initFontSize)
    {
        while (Mathf.Floor(currentDisplayScore) > Mathf.Floor(targetScore))
        {   
            
            currentDisplayScore -= Time.deltaTime * currentDisplayScore * 8;
            currentDisplayScore = Mathf.Clamp(currentDisplayScore, 0f, currentDisplayScore);
            field.text = Mathf.Floor(currentDisplayScore).ToString();
            float fontsize = initFontSize - (initFontSize/scoreGathered * (scoreGathered - currentDisplayScore));
            field.fontSize = fontsize < 150 ? fontsize : 150;

            float currentColor = 0;
            currentColor = currentDisplayScore / 10;
            if (currentColor > 255) currentColor = 255;
            TrippyBG.GetComponent<Image>().color = new Color32((byte)currentColor, 60, 47, 100);
            yield return null;
        }
    }

    IEnumerator PlayCards(List<GameObject> selectedCards, bool outOfTime)
    {   
        touchBlock.SetActive(true);
        Player.GetComponent<Player>().isPlaying = true;
        Playing = true;
        vibrated = false;
        yourTurnSymbol.SetActive(false);

        float shakeIntensity = 0.025f;

        //GameObject.Find("PLAYER").GetComponent<CameraShake>().cameraShake(shakeIntensity);

        //int playedCardCount = selectedCards.Count;
        int playedCardCount = 0;
        if (outOfTime)
        {   
            foreach (GameObject card in selectedCards)
            {   
                card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * 50;
                yield return new WaitForSeconds(0.5f);
            }
        }

        GameObject jokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
        if (jokerDescription == null) jokerDescription = GameObject.Find("INGAME OWNED JOKER CARD DESCRIPTION");
        jokerDescription.GetComponent<TMP_Text>().text = "";

        foreach (string joker in playerComponent.currentJokerCards)
        {   
            GameObject currentJoker = GameObject.Find(joker);
            JokerCard jokerCard = currentJoker.GetComponent<JokerCard>();

            if (jokerCard.selected)
            {
                //jokerCard.transform.localPosition -= transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(jokerCard.gameObject, -0.5f * jokerCard.transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(jokerCard.transform.GetChild(0).gameObject, 0.7f * jokerCard.transform.localScale.x, touchBlock, 10f, false));
                //jokerCard.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                jokerCard.moved = false;
                jokerCard.selected = false;
                gameplayInfoComponent.selectedOwnedJokerCount--;
                jokerCard.wasSelected = false;
            }
        }

        GameObject tarotDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
        if (tarotDescription == null) tarotDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
        tarotDescription.GetComponent<TMP_Text>().text = "";

        foreach (string tarot in playerComponent.currentTarotCards)
        {   
            GameObject currentTarot = GameObject.Find(tarot);
            TarotCard tarotCard = currentTarot.GetComponent<TarotCard>();

            if (tarotCard.selected)
            {
                //tarotCard.transform.localPosition -= transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(tarotCard.gameObject, -0.5f * tarotCard.transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(tarotCard.transform.GetChild(0).gameObject, 0.7f * tarotCard.transform.localScale.x, touchBlock, 10f, false));
                //tarotCard.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                tarotCard.moved = false;
                tarotCard.selected = false;
                gameplayInfoComponent.selectedOwnedJokerCount--;
                tarotCard.wasSelected = false;
            }
        }

        float elapsedTime = 0;

        float movementLength = -3;

        Vector3 PlayingCardGroupOrig = PlayingCardGroup.position;
        Vector3 PlayingCardGroupStartingPos  = PlayingCardGroup.position;
        Vector3 PlayingCardGroupFinalPos = PlayingCardGroup.position + (PlayingCardGroup.up * movementLength);

        Vector3 suitBtnOrig = SuitBtn.transform.position;
        Vector3 suitBtnStartingPos = SuitBtn.transform.position;
        Vector3 suitBtnFinalPos = SuitBtn.transform.position + (RankBtn.transform.up * movementLength);

        Vector3 rankBtnOrig = RankBtn.transform.position;
        Vector3 rankBtnStartingPos = RankBtn.transform.position;
        Vector3 rankBtnFinalPos = RankBtn.transform.position + (RankBtn.transform.up * movementLength);

        Vector3 deckSizeOrig = DeckSizeText.transform.position;
        Vector3 deckSizeStartingPos = DeckSizeText.transform.position;
        Vector3 deckSizeFinalPos = DeckSizeText.transform.position + (DeckSizeText.transform.up * movementLength);

        Vector3 otherPlayersBtnOrig = OtherPlayersBtn.transform.position;
        Vector3 otherPlayersBtnStartingPos = OtherPlayersBtn.transform.position;
        Vector3 otherPlayersBtnFinalPos = OtherPlayersBtn.transform.position + (OtherPlayersBtn.transform.right * -movementLength * 2);

        Vector3 handScoreBtnOrig = HandScoreBtn.transform.position;
        Vector3 handScoreBtnStartingPos = HandScoreBtn.transform.position;
        Vector3 handScoreBtnFinalPos = HandScoreBtn.transform.position + (HandScoreBtn.transform.right * -movementLength * 2);

        Vector3 deckBtnOrig = DeckBtn.transform.position;
        Vector3 deckBtnStartingPos = DeckBtn.transform.position;
        Vector3 deckBtnFinalPos = DeckBtn.transform.position + (DeckBtn.transform.right * -movementLength * 2);

        elapsedTime = 0;
        while (PlayingCardGroup.position != PlayingCardGroupFinalPos)
        {	
            float stepAmount = Mathf.Pow(elapsedTime * 5, 3f);
            PlayingCardGroup.position = Vector3.Lerp(PlayingCardGroupStartingPos, PlayingCardGroupFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            SuitBtn.transform.position = Vector3.Lerp(suitBtnStartingPos, suitBtnFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            RankBtn.transform.position = Vector3.Lerp(rankBtnStartingPos, rankBtnFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            DeckSizeText.transform.position = Vector3.Lerp(deckSizeStartingPos, deckSizeFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            OtherPlayersBtn.transform.position = Vector3.Lerp(otherPlayersBtnStartingPos, otherPlayersBtnFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            HandScoreBtn.transform.position = Vector3.Lerp(handScoreBtnStartingPos, handScoreBtnFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            DeckBtn.transform.position = Vector3.Lerp(deckBtnStartingPos, deckBtnFinalPos, Mathf.MoveTowards(0f, 2f, stepAmount));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        foreach (GameObject card in selectedCards)
        {   
            Card cardComponent = card.GetComponent<Card>();
            card.transform.localPosition = Vector3.zero;
            cardComponent.cardSlot = card.transform.parent.gameObject;
            cardComponent.isPlayed = true;
            card.transform.localPosition += cardComponent.cardVisual.transform.up * -50;
            cardComponent.cardVisual.transform.Find("ShakeParent").transform.Find("TiltParent").transform.rotation = new Quaternion(0, 0, 0, Quaternion.kEpsilon);
            card.transform.parent.SetParent(PlayedHandGroup.transform);
            card.transform.localPosition = new Vector3 (card.transform.localPosition.x, 0, card.transform.localPosition.z);
            if (cardComponent.edition != "NEGATIVE") playedCardCount += 1;
            if (cardComponent.isFlipped)
            {   
                cardComponent.isFlipped = false;
                ShaderCode shaderCode = cardComponent.cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>();

                shaderCode.image = cardComponent.cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                shaderCode.m = new Material(shaderCode.image.material);
                shaderCode.image.material = shaderCode.m;
                for (int j = 0; j < shaderCode.image.material.enabledKeywords.Length; j++)
                {
                    shaderCode.image.material.DisableKeyword(shaderCode.image.material.enabledKeywords[j]);
                }
                shaderCode.image.material.EnableKeyword($"_EDITION_{cardComponent.edition}");

                Sprite[] cards = Resources.LoadAll<Sprite>("pokercards");

                shaderCode.image.sprite = cards[gameplayInfoComponent.pokerCardsDict[$"{cardComponent.suit} {cardComponent.rank}"]];
            }
            yield return new WaitForSeconds(0.1f);
        }

        CheckHand(PlayingCardGroup, PlayedHandGroup.transform);

        int scoringCardCount = 0;

        foreach (GameObject card in selectedCards)
        {   
            if (card.GetComponent<Card>().isScoring)
            {
                scoringCardCount++;
                card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * 50;
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject card in selectedCards)
        {   
            Card cardComponent = card.GetComponent<Card>();
            if (cardComponent.isScoring)
            {
                //DOTween.Kill(2, true);
                cardComponent.cardVisual.transform.Find("ShakeParent").DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(2);
                GameObject TextObject = Instantiate(PointText, cardComponent.cardVisual.transform);
                cardscore = gameplayInfoComponent.cardScore[cardComponent.rank];

                if (cardComponent.edition == "POLYCHROME")
                {
                    mult += 4;
                }
                if (cardComponent.edition == "FOIL")
                {
                    cardscore += 25;
                }

                foreach (string joker in playerComponent.currentJokerCards)
                {   
                    int number = 1;
                    if (joker.Split(" ")[1] == "VIII" && Int32.TryParse(cardComponent.rank, out number) && number % 2 == 0)
                    {
                        cardscore += 20;
                        GameObject currentJoker = GameObject.Find(joker);
                        currentJoker.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                        cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), currentJoker.transform, 0.5f, currentJoker.name);
                        shakeIntensity += 0.003f;
                    }
                    else if (joker.Split(" ")[1] == "IX" && ((Int32.TryParse(cardComponent.rank, out number) && number % 2 == 1) || cardComponent.rank == "A"))
                    {
                        mult += 2;
                        GameObject currentJoker = GameObject.Find(joker);
                        currentJoker.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                        cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), currentJoker.transform, 0.5f, currentJoker.name);
                        shakeIntensity += 0.003f;
                    }
                    if (joker.Split(" ")[1] == "X" && scoringCardCount == 5)
                    {
                        cardscore += 12;
                        GameObject currentJoker = GameObject.Find(joker);
                        currentJoker.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                        cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), currentJoker.transform, 0.5f,currentJoker.name);
                        shakeIntensity += 0.003f;
                    }
                    if(joker.Split(" ")[0] == "Makk")
                    {
                        cardscore += 5;
                    }
                }

                TextObject.GetComponent<TMP_Text>().text = "+" + cardscore.ToString();
                TextObject.transform.localPosition = new Vector3(0, 200, 0);
                score += cardscore;
                cameraShake.cameraShake(shakeIntensity + (score * mult / 500000));
                shakeIntensity += 0.005f;
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                //HandScoreText.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), HandScoreText.transform, 0.5f, HandScoreText.name);
                yield return new WaitForSeconds(0.2f);
                Destroy(TextObject);
                cardComponent.isScoring = false;
            }
        }

        yield return new WaitForSeconds(0.3f);

        foreach (string joker in playerComponent.currentJokerCards)
        {   
            int wasLastBonusDiscard = 0;
            if (playerComponent.wasLast)
            {
                wasLastBonusDiscard++;
            }

            int maxDiscardCount = playerComponent.maxDiscards + CheckForALSO() + wasLastBonusDiscard;

            if (joker.Split(" ")[1] == "VII" && playerComponent.discards == maxDiscardCount)
            {
                mult += 2;
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                //HandScoreText.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), HandScoreText.transform, 0.5f, HandScoreText.name);
                GameObject currentJoker = GameObject.Find(joker);
                currentJoker.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), currentJoker.transform, 0.5f, currentJoker.name);
                shakeIntensity += 0.005f;
                cameraShake.cameraShake(shakeIntensity + (score * mult / 500000));
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            if (joker.Split(" ")[1] == "KIRÁLY")
            {
                mult += 3;
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                //HandScoreText.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), HandScoreText.transform, 0.5f, HandScoreText.name);
                GameObject currentJoker = GameObject.Find(joker);
                currentJoker.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), currentJoker.transform, 0.5f,currentJoker.name);
                shakeIntensity += 0.005f;
                cameraShake.cameraShake(shakeIntensity + (score * mult / 500000));
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            if (joker.Split(" ")[1] == "ÁSZ")
            {
                mult += 5;
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                //HandScoreText.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), HandScoreText.transform, 0.5f, HandScoreText.name);
                GameObject currentJoker = GameObject.Find(joker);
                currentJoker.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 0).SetId(3);
                cameraShake.shakeObject(shakeIntensity + (score * mult / 500000), currentJoker.transform, 0.5f, currentJoker.name);
                shakeIntensity += 0.005f;
                cameraShake.cameraShake(shakeIntensity + (score * mult / 500000));
                yield return new WaitForSeconds(0.5f);
                continue;
            }

        }      

        scoreGatheredText.SetActive(true);
        scoreGatheredText.GetComponent<TMP_Text>().fontSize = 100;

        StartCoroutine(CountUpToTarget(0, Mathf.Round(score * mult), scoreGatheredText.GetComponent<TMP_Text>(), true));

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(CountDownToTarget(Mathf.Round(score * mult), 0, scoreGatheredText.GetComponent<TMP_Text>(), Mathf.Round(score * mult), scoreGatheredText.GetComponent<TMP_Text>().fontSize));
        StartCoroutine(CountUpToTarget(Player.GetComponent<Player>().score, Mathf.Round(Player.GetComponent<Player>().score + (score * mult)), TotalScoreText.GetComponent<TMP_Text>()));

        Player.GetComponent<Player>().score = Mathf.Round(Player.GetComponent<Player>().score + (score * mult));

        //TotalScoreText.GetComponent<TMP_Text>().text = totalScore.ToString();

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject card in selectedCards.ToList())
        {   
            Card cardComponent = card.GetComponent<Card>();
            cardComponent.cardSlot = card.transform.parent.gameObject;
            cardComponent.isDiscarded = true;
            card.transform.localPosition += cardComponent.cardVisual.transform.up * 600;
            Player.GetComponent<Player>().currentCardsInHand.Remove($"{cardComponent.suit} {cardComponent.rank}");
            yield return new WaitForSeconds(0.05f);
            Player.GetComponent<Player>().cardsInHand.Remove(cardComponent.cardSlot);
            Destroy(card);
            Destroy(cardComponent.cardVisualObj);
            Destroy(cardComponent.cardSlot);
            yield return new WaitForSeconds(0.05f);
        }

        elapsedTime = 0;
        while (PlayingCardGroup.position != PlayingCardGroupOrig)
        {	
            float stepAmount = Mathf.Pow(elapsedTime * 5, 3f);
            PlayingCardGroup.position = Vector3.Lerp(PlayingCardGroup.position, PlayingCardGroupOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            SuitBtn.transform.position = Vector3.Lerp(SuitBtn.transform.position, suitBtnOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            RankBtn.transform.position = Vector3.Lerp(RankBtn.transform.position, rankBtnOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            DeckSizeText.transform.position = Vector3.Lerp(DeckSizeText.transform.position, deckSizeOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            OtherPlayersBtn.transform.position = Vector3.Lerp(OtherPlayersBtn.transform.position, otherPlayersBtnOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            HandScoreBtn.transform.position = Vector3.Lerp(HandScoreBtn.transform.position, handScoreBtnOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            DeckBtn.transform.position = Vector3.Lerp(DeckBtn.transform.position, deckBtnOrig, Mathf.MoveTowards(0f, 2f, stepAmount));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameplayInfoComponent.selectedCards.Clear();
        gameplayInfoComponent.selectedHandCount = 0;
        playerComponent.cardsInHandCount -= playedCardCount;
        Playing = false;
        playerComponent.isPlaying = false;

        int nextID = playerComponent.playerID + 1;
        if (nextID > users.Count) nextID = 1;

        playerComponent.isCurrent = false;
        timeSet = false;

        bool nextFound = false;
        int endCount = 0;
        touchBlock.SetActive(false);

        while (!nextFound)
        {
            foreach (User user in users)
            {
                GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                Player currentPlayerComponent = currentPlayer.GetComponent<Player>();
                if (currentPlayerComponent.playerID == nextID && currentPlayerComponent.hands == 0) 
                {
                    nextID++;
                    if (nextID > users.Count) nextID = 1;
                    endCount++;
                }
                if (currentPlayerComponent.playerID == nextID)
                {
                    currentPlayerComponent.isCurrent = true;
                    nextFound = true;
                } 
            }

            if (endCount == users.Count)
            {   

                break;
            }

        }

        BroadcastRemoteMethod("SendPlayedHand", playerComponent.Username, (int)Mathf.Round(score * mult), HandCheckText.GetComponent<TMP_Text>().text);

        playerComponent.hands -= 1;

        if (playerComponent.cardsInHandCount == 0) playerComponent.hands = 0;

        if (playerComponent.hands == 0) BroadcastRemoteMethod("NoHandsLeft");

        scoreGatheredText.SetActive(false);

    }
    
    public void DeckButton()
    {   

        foreach (string fullDeckcard in gameplayInfoComponent.pokerCardImageNames)
        {
            string fullDeckCardRank = fullDeckcard.Split(" ")[1];
            string fullDeckCardSuit = fullDeckcard.Split(" ")[0];
            GameObject.Find($"{fullDeckCardRank} COUNT").GetComponent<TMP_Text>().text = "0";
            GameObject.Find($"{fullDeckCardSuit} COUNT").GetComponent<TMP_Text>().text = "0";
        }
        
        foreach (string card in PlayingCardGroup.GetComponent<HorizontalCardHolder>().deckDict.Keys)
        {
            string rank = card.Split(";")[0].Split(" ")[1];
            string suit = card.Split(";")[0].Split(" ")[0];
            //{   

                // foreach (string fullDeckcard in gameplayInfoComponent.pokerCardImageNames)
                // {
                //     string fullDeckCardRank = fullDeckcard.Split(" ")[1];
                //     string fullDeckCardSuit = fullDeckcard.Split(" ")[0];
                //     if (rank == fullDeckCardRank)
                //     {
                        GameObject.Find($"{rank} COUNT").GetComponent<TMP_Text>().text = (Int32.Parse(GameObject.Find($"{rank} COUNT").GetComponent<TMP_Text>().text) + 1).ToString();
                    // }
                    // if (suit == fullDeckCardSuit)
                    // {
                        GameObject.Find($"{suit} COUNT").GetComponent<TMP_Text>().text = (Int32.Parse(GameObject.Find($"{suit} COUNT").GetComponent<TMP_Text>().text) + 1).ToString();
                    //}
                //}
            //}
        }
    }

    public void HandsScoreButton()
    {   
        HandScorePanel.SetActive(true);
        CardVisuals.SetActive(false);
        foreach (string handName in gameplayInfoComponent.handsNamesList)
        {
            int handScore = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
            int handMult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];

            GameObject handObj = GameObject.Find($"{handName} HAND SCORE TEXT");
            GameObject scoreObj = handObj.transform.GetChild(2).gameObject;
            GameObject multObj = handObj.transform.GetChild(3).gameObject;

            scoreObj.GetComponent<TMP_Text>().text = handScore.ToString();
            multObj.GetComponent<TMP_Text>().text = handMult.ToString();
        }
    }
    public void DiscardingButton(bool outOfTime = false)
    {   
        if (gameplayInfoComponent.selectedHandCount > 0)
        {   
            timerText.SetActive(false);
            timerCircle.SetActive(false);
            timeLeft = playerComponent.TimeLimit;
            Discarding = true;
            playerComponent.isDiscarding = true;
            playerComponent.discards -= 1;
            StartCoroutine(DiscardCards(gameplayInfoComponent.selectedCards, outOfTime));
        }   
    }

    public void PlayButton(bool outOfTime = false)
    {   
        if (gameplayInfoComponent.selectedHandCount > 0)
        {   
            //Playing = true;
            HandsText.GetComponent<TMP_Text>().text = (Int32.Parse(HandsText.GetComponent<TMP_Text>().text) - 1).ToString();
            timerText.SetActive(false);
            timerCircle.SetActive(false);
            timeLeft = playerComponent.TimeLimit;
            StartCoroutine(PlayCards(gameplayInfoComponent.selectedCards, outOfTime));
        }   
    }

    void CheckHand(Transform PlayingCardHolder, Transform PlayedCardHolder)
    {   

        List<Card> hand = new List<Card>();
        bool flipped = false;;
        Card[] children = PlayingCardHolder.GetComponentsInChildren<Card>();
        Card[] children2 = PlayedCardHolder.GetComponentsInChildren<Card>();
        foreach(Card child in children)
        {
            if (child.GetComponent<Card>().selected)
            {
                if (child.GetComponent<Card>().isFlipped) flipped = true;
                hand.Add(child);
            }
        }

        foreach(Card child in children2)
        {
            if (child.GetComponent<Card>().selected)
            {   
                hand.Add(child);
            }
        }

        hand = hand.OrderByDescending(x => gameplayInfoComponent.rankOrderDict[x.GetComponent<Card>().rank]).ToList();

        string suit = "";
        string rank = "";
        string handName = "";
        Dictionary<string, int> countDict = new Dictionary<string, int>();

        #region FIVE OF A KIND
        //-------------------- FIVE OF A KIND
        int count = 0;
        rank = "";
        countDict.Clear();
        if (hand.Count == 5)
        {
            foreach (Card child in hand)
            {
                if (!countDict.ContainsKey(child.rank))
                {   
                    countDict.Add(child.GetComponent<Card>().rank, 1);
                }
                else
                {
                    countDict[child.GetComponent<Card>().rank] += 1;
                }
            }
        }

        foreach (KeyValuePair<string, int> item in countDict)
        {   
            if (item.Value == 5)
            {   
                foreach (Card child in hand)
                {
                    if (child.GetComponent<Card>().rank == item.Key)
                    {
                        child.GetComponent<Card>().isScoring = true;
                    }
                }
                handName = "FIVE OF A KIND";
                HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
                HandCheckText.transform.gameObject.SetActive(true);
                score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
                mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                HandScoreText.transform.gameObject.SetActive(true);
                if (flipped)
                {
                    HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                    HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
                }
                return;
            }
            else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }
        }
        
        //##################### FIVE OF A KIND
        #endregion

        #region ROYAL FLUSH
        //-------------------- ROYAL FLUSH
        int rankCount = 0;
        int suitCount = 0;
        bool aceFound = false;
        bool kingFound = false;
        bool queenFound = false;
        bool jackFound = false;
        bool tenFound = false;
        
        if (hand.Count == 5)
        {
            foreach (Card child in hand)
            {
                if (suitCount == 0)
                {
                    suit = child.GetComponent<Card>().suit;
                }

                if (child.GetComponent<Card>().rank == "A" && !aceFound)
                {
                    rankCount++;
                    aceFound = true;
                }
                else if (child.GetComponent<Card>().rank == "K" && !kingFound)
                {
                    rankCount++;
                    kingFound = true;
                }
                else if (child.GetComponent<Card>().rank == "Q" && !queenFound)
                {
                    rankCount++;
                    queenFound = true;
                }
                else if (child.GetComponent<Card>().rank == "J" && !jackFound)
                {
                    rankCount++;
                    jackFound = true;
                }
                else if (child.GetComponent<Card>().rank == "10" && !tenFound)
                {
                    rankCount++;
                    tenFound = true;
                }

                if(child.GetComponent<Card>().suit == suit)
                {
                    suitCount++;
                    child.GetComponent<Card>().isScoring = true;
                }
            }
        }
        
        
        if (rankCount == 5 && suitCount == 5)
        {   
            handName = "ROYAL FLUSH";
            HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
            HandCheckText.transform.gameObject.SetActive(true);
            score = Player.GetComponent<Player>().handScoresDict[$"STRAIGHT FLUSH SCORE"];
            mult = Player.GetComponent<Player>().handScoresDict[$"STRAIGHT FLUSH MULT"];
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
            HandScoreText.SetActive(true);
            if (flipped)
            {
                HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
            }
            return;
        }
        else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }
        //##################### ROYAL FLUSH
        #endregion

        #region STRAIGHT FLUSH
        //-------------------- STRAIGHT FLUSH
        int prevCardId = -1;
        int currentCardID = 0;
        count = 1;
        string theSuit = "";

        if (hand.Count == 5)
        {
            foreach (Card child in hand)
            {    

            suit = child.GetComponent<Card>().suit;
            rank = child.GetComponent<Card>().rank;
            currentCardID = gameplayInfoComponent.rankOrderDict[rank];

            //currentCardID = gameplayInfoComponent.pokerCardsDict[$"{suit} {rank}"];

                if (prevCardId != -1)
                {
                    if (currentCardID == prevCardId - 1 && suit == theSuit)
                    {
                        count++;
                        child.GetComponent<Card>().isScoring = true;
                    }

                    if (prevCardId == 12)
                    {
                        prevCardId = 4;
                        if (currentCardID == prevCardId - 1 && suit == theSuit)
                        {
                            count++;
                            child.GetComponent<Card>().isScoring = true;
                        }
                    }
                }
                else
                {
                    child.GetComponent<Card>().isScoring = true;
                    theSuit = suit;
                }

            prevCardId = gameplayInfoComponent.rankOrderDict[rank];
            }
        }
        

        if (count == 5)
        {
            handName = "STRAIGHT FLUSH";
            HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
            HandCheckText.transform.gameObject.SetActive(true);
            score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
            mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
            HandScoreText.transform.gameObject.SetActive(true);
            if (flipped)
            {
                HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
            }
            return;
        }
        else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }
        //##################### STRAIGHT FLUSH
        #endregion
        
        #region FOUR OF A KIND
        //-------------------- FOUR OF A KIND
        count = 0;
        rank = "";
        countDict.Clear();
        if (hand.Count >= 4)
        {
            foreach (Card child in hand)
            {
                if (!countDict.ContainsKey(child.rank))
                {   
                    countDict.Add(child.GetComponent<Card>().rank, 1);
                }
                else
                {
                    countDict[child.GetComponent<Card>().rank] += 1;
                }
            }
        }

        foreach (KeyValuePair<string, int> item in countDict)
        {   
            if (item.Value >= 4)
            {   
                foreach (Card child in hand)
                {
                    if (child.GetComponent<Card>().rank == item.Key)
                    {
                        child.GetComponent<Card>().isScoring = true;
                    }
                }
                handName = "FOUR OF A KIND";
                HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
                HandCheckText.transform.gameObject.SetActive(true);
                score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
                mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                HandScoreText.transform.gameObject.SetActive(true);
                if (flipped)
                {
                    HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                    HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
                }
                return;
            }
            else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }
        }
        
        //##################### FOUR OF A KIND
        #endregion

        #region FULL HOUSE

        countDict.Clear();
        List<string> ranks = new List<string>();
        bool twoFound = false;
        bool threeFound = false;

        if (hand.Count == 5)
        {
            foreach (Card child in hand)
            {

                if (!countDict.ContainsKey(child.rank))
                {   
                    countDict.Add(child.GetComponent<Card>().rank, 1);
                }
                else
                {
                    countDict[child.GetComponent<Card>().rank] += 1;
                }
            }
        }
        
        if (countDict.Count == 2)
        {
            foreach (KeyValuePair<string, int> item in countDict)
            {   
                if (item.Value == 2)
                {
                    twoFound = true;
                    foreach (Card child in hand)
                    {
                        if (child.GetComponent<Card>().rank == item.Key)
                        {
                            child.GetComponent<Card>().isScoring = true;
                        }
                    }
                }
                else if(item.Value == 3)
                {
                    threeFound = true;
                    foreach (Card child in hand)
                    {
                        if (child.GetComponent<Card>().rank == item.Key)
                        {
                            child.GetComponent<Card>().isScoring = true;
                        }
                    }
                }
            }

            if (twoFound && threeFound)
            {
                handName = "FULL HOUSE";
                HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
                HandCheckText.transform.gameObject.SetActive(true);
                score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
                mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                HandScoreText.transform.gameObject.SetActive(true);
                if (flipped)
                {
                    HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                    HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
                }
                return;
            }
            else
            {
                foreach (Card child in hand)
                {
                    child.GetComponent<Card>().isScoring = false;
                }
            }
        }
        
        #endregion

        #region FLUSH

        count = 0;

        if (hand.Count == 5)
        {
            foreach (Card child in hand)
            {
                
                if (count == 0)
                {
                    suit = child.GetComponent<Card>().suit;
                    child.GetComponent<Card>().isScoring = true;
                    count++;
                }
                else
                {
                    if (suit == child.GetComponent<Card>().suit)
                    {
                        count++;
                        child.GetComponent<Card>().isScoring = true;
                    }
                }
            }
        }

        if (count == 5)
        {
            handName = "FLUSH";
            HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
            HandCheckText.transform.gameObject.SetActive(true);
            score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
            mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
            HandScoreText.transform.gameObject.SetActive(true);
            if (flipped)
            {
                HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
            }
            return;
        }
        else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }
    
        #endregion
        
        #region STRAIGHT

        count = 1;
        int currentRank = 0;
        int previousRank = -1;

        if (hand.Count == 5)
        {
            foreach (Card child in hand)
            {
                rank = child.GetComponent<Card>().rank;
                currentRank = gameplayInfoComponent.rankOrderDict[rank];

                if (previousRank != -1)
                {
                    if (currentRank == previousRank - 1)
                    {
                        count++;
                        child.GetComponent<Card>().isScoring = true; 
                    }

                    if (previousRank == 12) 
                    {
                        previousRank = 4;
                        if (currentRank == previousRank - 1)
                        {
                            count++;
                            child.GetComponent<Card>().isScoring = true;
                        }
                    }
                }
                else
                {
                    child.GetComponent<Card>().isScoring = true;
                }

            previousRank = gameplayInfoComponent.rankOrderDict[rank];
            }
        }

        if (count == 5)
        {
            handName = "STRAIGHT";
            HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
            HandCheckText.transform.gameObject.SetActive(true);
            score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
            mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
            HandScoreText.transform.gameObject.SetActive(true);
            if (flipped)
            {
                HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
            }
            return;
        }
        else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }

        #endregion
        
        #region THREE OF A KIND

        countDict.Clear();
        if (hand.Count >= 3)
        {
            foreach (Card child in hand)
            {
                if (!countDict.ContainsKey(child.rank))
                {   
                    countDict.Add(child.GetComponent<Card>().rank, 1);
                }
                else
                {
                    countDict[child.GetComponent<Card>().rank] += 1;
                }
            }
        }

        foreach (KeyValuePair<string, int> item in countDict)
        {   
            if (item.Value >= 3)
            {   
                foreach (Card child in hand)
                {
                    if (child.GetComponent<Card>().rank == item.Key)
                    {
                        child.GetComponent<Card>().isScoring = true;
                    }
                }
                handName = "THREE OF A KIND";
                HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
                HandCheckText.transform.gameObject.SetActive(true);
                score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
                mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                HandScoreText.transform.gameObject.SetActive(true);
                if (flipped)
                {
                    HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                    HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
                }
                return;
            }
            else
            {
                foreach (Card child in hand)
                {
                    child.GetComponent<Card>().isScoring = false;
                }
            }
        }

        #endregion
        
         #region TWO PAIR

        count = 0;
        ranks.Clear();
        countDict.Clear();

        if (hand.Count >= 4)
        {
            foreach (Card child in hand)
            {

                if (!countDict.ContainsKey(child.rank))
                {   
                    countDict.Add(child.GetComponent<Card>().rank, 1);
                }
                else
                {
                    countDict[child.GetComponent<Card>().rank] += 1;
                }
            }
        }
        
        countDict = countDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        foreach (KeyValuePair<string, int> item in countDict)
        {   
            if (item.Value == 2)
            {
                count++;
                foreach (Card child in hand)
                {
                    if (child.GetComponent<Card>().rank == item.Key)
                    {
                        child.GetComponent<Card>().isScoring = true;
                    }
                }
            } 
            
        }

        if (count == 2)
        {
            handName = "TWO PAIR";
            HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
            HandCheckText.transform.gameObject.SetActive(true);
            score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
            mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
            HandScoreText.transform.gameObject.SetActive(true);
            if (flipped)
            {
                HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
            }
            return;
        }
        else
        {
            foreach (Card child in hand)
            {
                child.GetComponent<Card>().isScoring = false;
            }
        }
        
        #endregion
        
         #region PAIR

        countDict.Clear();

        if (hand.Count >= 2)
        {
            foreach (Card child in hand)
            {   
                if (!countDict.ContainsKey(child.rank))
                {   
                    countDict.Add(child.GetComponent<Card>().rank, 1);
                }
                else
                {
                    countDict[child.GetComponent<Card>().rank] += 1;
                }
            }
        }
        
        foreach (KeyValuePair<string, int> item in countDict)
        {   
            if (item.Value >= 2)
            {   
                foreach (Card child in hand)
                {
                    if (child.GetComponent<Card>().rank == item.Key)
                    {
                        child.GetComponent<Card>().isScoring = true;
                    }
                }
                handName = "PAIR";
                HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
                HandCheckText.transform.gameObject.SetActive(true);
                score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
                mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
                HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
                HandScoreText.transform.gameObject.SetActive(true);
                if (flipped)
                {
                    HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
                    HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
                }
                return;
            }
            else
            {
                foreach (Card child in hand)
                {
                    child.GetComponent<Card>().isScoring = false;
                }
            }        
        }
        
        #endregion

        #region HIGH CARD
        if (hand.Count >= 1)
        {   
            hand = hand.OrderByDescending(o=> gameplayInfoComponent.rankOrderDict[o.GetComponent<Card>().rank]).ToList();
            hand[0].GetComponent<Card>().isScoring = true;
            handName = "HIGH CARD";
            HandCheckText.transform.GetComponent<TMP_Text>().text = gameplayInfoComponent.handNamesHUN[handName];
            HandCheckText.transform.gameObject.SetActive(true);
            score = Player.GetComponent<Player>().handScoresDict[$"{handName} SCORE"];
            mult = Player.GetComponent<Player>().handScoresDict[$"{handName} MULT"];
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"{score} X {mult}";
            HandScoreText.transform.gameObject.SetActive(true);
            if (flipped)
            {
            HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
            }
            return;
        }
        #endregion

        if (flipped)
        {
            HandCheckText.transform.GetComponent<TMP_Text>().text = "???";
            HandScoreText.transform.GetComponent<TMP_Text>().text = $"? X ?";
        }

        HandCheckText.SetActive(false);
        HandScoreText.SetActive(false);

    }

    public void OrderByRankButton()
    {
        gameplayInfoComponent.orderByRank = true;
        gameplayInfoComponent.initiatedOrdering = true;
    }

    public void OrderBySuitButton()
    {
        gameplayInfoComponent.orderByRank = false;
        gameplayInfoComponent.initiatedOrdering = true;
    }

    public void SellJoker()
    {
        GameObject ownedJokerHolderShop = GameObject.Find("OWNED JOKER CARD HOLDER");
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");

        foreach (Transform child in ownedJokerHolderShop.transform)
        {
            if (child.gameObject.GetComponent<JokerCard>().selected)
            {   
                playerComponent.money += Mathf.Ceil((float)child.gameObject.GetComponent<JokerCard>().price / 2);
                playerComponent.currentJokerCards.Remove($"{child.gameObject.GetComponent<JokerCard>().suit} {child.gameObject.GetComponent<JokerCard>().rank}");
                gameplayInfoComponent.selectedOwnedJokerCount = 0;
                Destroy(child.gameObject);
                GameObject ownedJokerText = GameObject.Find("OWNED JOKER TEXT");
                ownedJokerText.GetComponent<TMP_Text>().text = $"({playerComponent.currentJokerCards.Count} / {playerComponent.gameMaxJokers})";
                GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
                yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
                GameObject jokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
                if (jokerDescription == null) jokerDescription = GameObject.Find("INGAME OWNED JOKER CARD DESCRIPTION");
                jokerDescription.GetComponent<TMP_Text>().text = "";
            }
        }
    }

    public void SellTarot()
    {
        GameObject ownedTarotHolderShop = GameObject.Find("OWNED TAROT CARD HOLDER");
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");

        foreach (Transform child in ownedTarotHolderShop.transform)
        {
            if (child.gameObject.GetComponent<TarotCard>().selected)
            {   
                playerComponent.money += Mathf.Ceil((float)child.gameObject.GetComponent<TarotCard>().price / 2);
                playerComponent.currentTarotCards.Remove($"{child.gameObject.GetComponent<TarotCard>().nameOfCard}");
                gameplayInfoComponent.selectedOwnedTarotCount = 0;
                Destroy(child.gameObject);
                GameObject ownedTarotText = GameObject.Find("OWNED TAROT TEXT");
                ownedTarotText.GetComponent<TMP_Text>().text = $"({playerComponent.currentTarotCards.Count} / {playerComponent.gameMaxTarots})";
                GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
                yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
                GameObject tarotDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
                if (tarotDescription == null) tarotDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                tarotDescription.GetComponent<TMP_Text>().text = "";
            }
        }
    }


    public void HandUpgrade(string handName, Transform card)
    {
        playerComponent.handScoresDict[$"{handName} SCORE"] += gameplayInfoComponent._countyCardScoreMultDict[$"{card.gameObject.GetComponent<CountyCard>().nameOfCard} SCORE"];
        playerComponent.handScoresDict[$"{handName} MULT"] += gameplayInfoComponent._countyCardScoreMultDict[$"{card.gameObject.GetComponent<CountyCard>().nameOfCard} MULT"];;
        playerComponent.money -= card.gameObject.GetComponent<CountyCard>().price; 
        card.gameObject.GetComponent<CountyCard>().BuySellButton.SetActive(false);
        card.gameObject.GetComponent<CountyCard>().BuySellPriceText.SetActive(false);
        card.gameObject.GetComponent<CountyCard>().BuySellText.SetActive(false);
        GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
        yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
        GameObject jokerDescription = GameObject.Find("JOKER DESCRIPTION");
        jokerDescription.GetComponent<TMP_Text>().text = "";
        StartCoroutine(card.gameObject.GetComponent<CountyCard>().DissolveEnum());
        //Destroy(card.gameObject);
    }

    public void BuyCounty()
    {
        GameObject jokerHolderShop = GameObject.Find("JOKER CARD HOLDER");
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");

        foreach (Transform child in jokerHolderShop.transform)
        {   
            CountyCard countyCard = child.gameObject.GetComponent<CountyCard>();
            if (countyCard == null || !countyCard.selected || !(countyCard.price <= Player.GetComponent<Player>().money)) continue;

            if (countyCard.nameOfCard == "KOMÁROM-ESZTERGOM")
            {
                HandUpgrade("HIGH CARD", child);
                break;
            }
            else if (countyCard.nameOfCard == "NÓGRÁD")
            {
                HandUpgrade("PAIR", child);
                break;
            }
            else if (countyCard.nameOfCard == "VAS")
            {
                HandUpgrade("TWO PAIR", child);
                break;
            }
            else if (countyCard.nameOfCard == "HEVES")
            {
                HandUpgrade("THREE OF A KIND", child);
                break;
            }
            else if (countyCard.nameOfCard == "TOLNA")
            {
                HandUpgrade("STRAIGHT", child);
                break;
            }
            else if (countyCard.nameOfCard == "ZALA")
            {
                HandUpgrade("FLUSH", child);
                break;
            }
            else if (countyCard.nameOfCard == "GYŐR-MOSON-SOPRON")
            {
                HandUpgrade("FULL HOUSE", child);
                break;
            }
            else if (countyCard.nameOfCard == "CSONGRÁD-CSANÁD")
            {
                HandUpgrade("FOUR OF A KIND", child);
                break;
            }
            else if (countyCard.nameOfCard == "FEJÉR")
            {
                HandUpgrade("STRAIGHT FLUSH", child);
                break;
            }
            else if (countyCard.nameOfCard == "BARANYA")
            {
                HandUpgrade("FIVE OF A KIND", child);
                break;
            }
        }
    }
    public void BuyTarot()
    {
        GameObject jokerHolderShop = GameObject.Find("JOKER CARD HOLDER");
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");

        foreach (string tarot in playerComponent.currentTarotCards)
        {   
            GameObject currentTarot = GameObject.Find(tarot);
            TarotCard currentTarotComponent = currentTarot.GetComponent<TarotCard>();
            if (currentTarotComponent.selected == true)
            {
                currentTarotComponent.wasSelected = true;
                currentTarotComponent.moved = true;
                currentTarotComponent.selected = false;     
            }
        }

        foreach (Transform child in jokerHolderShop.transform)
        {   
            TarotCard tarotCardComponent = child.gameObject.GetComponent<TarotCard>();
            if (tarotCardComponent == null) continue;

            if (tarotCardComponent.selected && tarotCardComponent.price <= playerComponent.money && tarotCardComponent.nameOfCard == "DIADALSZEKÉR")
            {
                tarotCardComponent.BuySellButton.SetActive(false);
                tarotCardComponent.BuySellPriceText.SetActive(false);
                tarotCardComponent.BuySellText.SetActive(false);
                playerComponent.money -= tarotCardComponent.price; 
                playerComponent.money = playerComponent.money >= 20 ? playerComponent.money + 20 : playerComponent.money * 2;
                GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
                yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
                StartCoroutine(tarotCardComponent.DissolveEnum());
                //Destroy(child.gameObject);
                break;
            }
            else if (tarotCardComponent.selected && tarotCardComponent.price <= playerComponent.money && tarotCardComponent.nameOfCard == "REMETE" && playerComponent.gameMaxJokers < 10)
            {   
                tarotCardComponent.BuySellButton.SetActive(false);
                tarotCardComponent.BuySellPriceText.SetActive(false);
                tarotCardComponent.BuySellText.SetActive(false);
                playerComponent.money -= tarotCardComponent.price; 
                playerComponent.gameMaxJokers += 1;
                GameObject ownedJokerText = GameObject.Find("OWNED JOKER TEXT");
                ownedJokerText.GetComponent<TMP_Text>().text = $"({playerComponent.currentJokerCards.Count} / {playerComponent.gameMaxJokers})";
                GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
                yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
                StartCoroutine(tarotCardComponent.DissolveEnum());
                //Destroy(child.gameObject);
                break;
            }
            else if (tarotCardComponent.selected && tarotCardComponent.price <= playerComponent.money && playerComponent.currentTarotCards.Count < playerComponent.gameMaxTarots)
            {     
                playerComponent.currentTarotCards.Add($"{tarotCardComponent.nameOfCard}");
                tarotCardComponent.isOwned = true;
                tarotCardComponent.selected = false;
                GameObject buySellBtn = child.gameObject.transform.GetChild(0).gameObject;
                GameObject image = child.gameObject.transform.GetChild(1).gameObject;
                buySellBtn.transform.position = buySellBtn.transform.position + (buySellBtn.transform.up * 1.25f * buySellBtn.transform.parent.transform.localScale.x);
                //image.transform.position = image.transform.position + (image.transform.up * -0.5f);
                Transform ownedTarotCardGroup = GameObject.Find("OWNED TAROT CARD HOLDER").transform;
                child.SetParent(ownedTarotCardGroup);
                child.transform.localScale = new Vector3(1, 1.1f, 1);
                //Destroy(child.gameObject);
                //gameplayInfoComponent.selectedJokerCount = 0;
                playerComponent.money -= tarotCardComponent.price; 
                GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
                yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
                GameObject jokerDescription = GameObject.Find("JOKER DESCRIPTION");
                jokerDescription.GetComponent<TMP_Text>().text = "";
                GameObject ownedJokerText = GameObject.Find("OWNED TAROT TEXT");
                ownedJokerText.GetComponent<TMP_Text>().text = $"({playerComponent.currentTarotCards.Count} / {playerComponent.gameMaxTarots})";
                break;               
            }
        }
    }

    public void BuyJoker()
    {   
        GameObject jokerHolderShop = GameObject.Find("JOKER CARD HOLDER");
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");

        foreach (string joker in playerComponent.currentJokerCards)
        {   
            GameObject currentJoker = GameObject.Find(joker);
            JokerCard currentJokerComponent = currentJoker.GetComponent<JokerCard>();
            if (currentJokerComponent.selected == true)
            {
                currentJokerComponent.wasSelected = true;
                currentJokerComponent.moved = true;
                currentJokerComponent.selected = false;     
            }
        }

        foreach (Transform child in jokerHolderShop.transform)
        {   
            JokerCard currentJokerComponent = child.gameObject.GetComponent<JokerCard>();
            if (currentJokerComponent == null) continue;
            if (currentJokerComponent.selected && currentJokerComponent.price <= playerComponent.money && playerComponent.currentJokerCards.Count < playerComponent.gameMaxJokers)
            {      
                playerComponent.currentJokerCards.Add($"{currentJokerComponent.suit} {currentJokerComponent.rank}");
                currentJokerComponent.isOwned = true;
                currentJokerComponent.selected = false;
                GameObject buySellBtn = child.gameObject.transform.GetChild(0).gameObject;
                GameObject image = child.gameObject.transform.GetChild(1).gameObject;
                buySellBtn.transform.position = buySellBtn.transform.position + (buySellBtn.transform.up * 1.25f * buySellBtn.transform.parent.transform.localScale.x);
                //image.transform.position = image.transform.position + (image.transform.up * -0.5f);
                Transform ownedJokerCardGroup = GameObject.Find("OWNED JOKER CARD HOLDER").transform;
                child.SetParent(ownedJokerCardGroup);
                child.transform.localScale = new Vector3(1, 1.1f, 1);
                //Destroy(child.gameObject);
                //gameplayInfoComponent.selectedJokerCount = 0;
                playerComponent.money -= currentJokerComponent.price; 
                GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
                yourMoneyText.GetComponent<TMP_Text>().text = $"{playerComponent.money}€";
                GameObject jokerDescription = GameObject.Find("JOKER DESCRIPTION");
                jokerDescription.GetComponent<TMP_Text>().text = "";
                GameObject ownedJokerText = GameObject.Find("OWNED JOKER TEXT");
                ownedJokerText.GetComponent<TMP_Text>().text = $"({playerComponent.currentJokerCards.Count} / {playerComponent.gameMaxJokers})";
                break;
            }
        }
    }

    public void Exit()
    {
        ExitBtn.SetActive(false);

        room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;

        Player.GetComponent<Player>().currentJokerCards.Clear();
        Player.GetComponent<Player>().cardsInHand.Clear();
        Player.GetComponent<Player>().currentCardsInHand.Clear();
        Player.GetComponent<Player>().cardsInHandCount = 0;
        Player.GetComponent<Player>().money = 0;
        Player.GetComponent<Player>().score = 0;
        Player.GetComponent<Player>().allRoundTotalScore = 0;
        Player.GetComponent<Player>().allRoundTotalWins = 0;
        Player.GetComponent<Player>().wasWinner = false;
        Player.GetComponent<Player>().wasLast = false;
        PlayingCardGroup.gameObject.SetActive(false);
        PlayerStatsPanel.SetActive(false);
        inResultsInitialized = false;
        inResults = false;
        inShop = false;
        roundEnd = false;
        roundEndInitialized = false;
        Player.GetComponent<Player>().currentPlayerID = 1;
        foundRoom = false;

        // foreach (User user in users)
        // {   
        //     GameObject PlayerNameText = GameObject.Find($"PlayerNameText {user.Name}");
        //     PlayerNameText.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
        // }

        // if (multiplayer.Me.IsHost) multiplayer.CurrentRoom.Destroy();
        // else
         multiplayer.CurrentRoom.Leave();
    }

    void Start()
    {   
        Application.runInBackground = true;
        Gameplayinfo = GameObject.Find("GameplayInfo");
        gameplayInfoComponent = Gameplayinfo.GetComponent<GameplayInfo>();
        Application.targetFrameRate = 60;
        room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;
        Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        playerComponent = Player.GetComponent<Player>();
        cameraShake = GameObject.Find("PLAYER").GetComponent<CameraShake>();
        if (playerComponent.playerID == playerComponent.currentPlayerID) playerComponent.isCurrent = true;
    }

    public void RerollShop()
    {   
        
        Player player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>();
        GameObject.Find("JOKER DESCRIPTION").GetComponent<TMP_Text>().text = "";

        gameplayInfoComponent.selectedJokerCount = 0;

        foreach (Transform child in GameObject.Find("JOKER CARD HOLDER").transform)
        {   
            if (child.gameObject.GetComponent<TarotCard>() != null)
            {
                player.tarotDeck.Add($"{child.gameObject.GetComponent<TarotCard>().nameOfCard}");
                Destroy(child.gameObject);
            }
            else if (child.gameObject.GetComponent<JokerCard>() != null)
            {
                player.jokerDeck.Add($"{child.gameObject.GetComponent<JokerCard>().suit} {child.gameObject.GetComponent<JokerCard>().rank}");
                Destroy(child.gameObject);
            }
            else
            {
                player.countyDeck.Add($"{child.gameObject.GetComponent<CountyCard>().nameOfCard}");
                Destroy(child.gameObject);
            }
        }

        if (player.jokerDeck.Count == 0) return;

        player.money -= rerollCost;
        rerollCost += 1;

        GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
        yourMoneyText.GetComponent<TMP_Text>().text = $"{Player.GetComponent<Player>().money}€";

        for (int i = 0; i < 3; i++)
        {   
            int randomCard = 0;
            int jokerOrTarot = rnd.Next(0, 100); // 0 - 79 joker, 80 - 100 tarot
            if (jokerOrTarot < Player.GetComponent<Player>().gameJokerChance)
            {
                randomCard = rnd.Next(player.jokerDeck.Count); 
                string[] suitRank = player.jokerDeck[randomCard].Split(' ');
                player.jokerDeck.RemoveAt(randomCard);
                string suit = suitRank[0];
                string rank = suitRank[1];
                GameObject jokerClone = Instantiate(JokerPrefab, GameObject.Find("JOKER CARD HOLDER").transform);
                JokerCard jokerComponent = jokerClone.GetComponent<JokerCard>();
                jokerClone.transform.name = $"{suit} {rank}";
                jokerComponent.suit = suit;
                jokerComponent.rank = rank;
                jokerComponent.cardName = gameplayInfoComponent.jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[0];
                jokerComponent.cardDescription = gameplayInfoComponent.jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[1];
                jokerComponent.price = gameplayInfoComponent._jokerCardPricesSimple[jokerComponent.rank];
                jokerComponent.description = gameplayInfoComponent._jokerCardDescriptionSimple[jokerComponent.rank];
                Sprite[] cards = Resources.LoadAll<Sprite>(selectedJokerDeck);
                jokerClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._jokerImageNamesDict[$"{suit} {rank}"]];
            }
            else
            {   
                bool tarotFound = false;
                bool countyFound = false;
                int tarotOrCounty = rnd.Next(0, 100); // 0 - 50 tarot, 50 - 100 county

                if (tarotOrCounty <= 50)
                {
                    while (!tarotFound)
                    {
                        randomCard = rnd.Next(player.tarotDeck.Count);
                        tarotFound = true;
                        foreach (Transform child in GameObject.Find("JOKER CARD HOLDER").transform)
                        {   
                            if (child.gameObject.GetComponent<TarotCard>() == null) continue;
                            else if (child.gameObject.GetComponent<TarotCard>().nameOfCard == player.tarotDeck[randomCard])
                            {
                                tarotFound = false;
                                break;
                            }
                        }
                        foreach (string tarot in Player.GetComponent<Player>().currentTarotCards)
                        {
                            if (tarot == player.tarotDeck[randomCard])
                            {
                                tarotFound = false;
                                break;
                            }
                        }
                    }

                    GameObject tarotClone = Instantiate(tarotPrefab, GameObject.Find("JOKER CARD HOLDER").transform);
                    TarotCard tarotComponent = tarotClone.GetComponent<TarotCard>();
                    tarotClone.transform.name = player.tarotDeck[randomCard];
                    tarotClone.transform.localScale = new Vector3(1, 1.1f, 1);

                    tarotComponent.nameOfCard = player.tarotDeck[randomCard];
                    tarotComponent.cardName = player.tarotDeck[randomCard];
                    tarotComponent.cardDescription = player.tarotDeck[randomCard];
                    tarotComponent.price = gameplayInfoComponent._tarotCardPricesSimple[player.tarotDeck[randomCard]];
                    tarotComponent.description = gameplayInfoComponent._tarotCardDescriptionSimple[player.tarotDeck[randomCard]];
                    Sprite[] cards = Resources.LoadAll<Sprite>(selectedTarotDeck);
                    tarotClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._tarotImageNamesDict[player.tarotDeck[randomCard]]];
                }
                else
                {
                    while (!countyFound)
                    {
                        randomCard = rnd.Next(player.countyDeck.Count);
                        countyFound = true;
                        foreach (Transform child in GameObject.Find("JOKER CARD HOLDER").transform)
                        {   
                            if (child.gameObject.GetComponent<CountyCard>() == null) continue;
                            else if (child.gameObject.GetComponent<CountyCard>().nameOfCard == player.countyDeck[randomCard])
                            {
                                countyFound = false;
                                break;
                            }
                        }
                    }

                    GameObject countyClone = Instantiate(countyPrefab, GameObject.Find("JOKER CARD HOLDER").transform);
                    CountyCard countyComponent = countyClone.GetComponent<CountyCard>();
                    countyClone.transform.name = player.countyDeck[randomCard];
                    countyClone.transform.localScale = new Vector3(1, 1.1f, 1);

                    countyComponent.nameOfCard = player.countyDeck[randomCard];
                    countyComponent.cardName = player.countyDeck[randomCard];
                    countyComponent.cardDescription = player.countyDeck[randomCard];
                    countyComponent.price = gameplayInfoComponent._countyCardPricesSimple[player.countyDeck[randomCard]];
                    countyComponent.description = gameplayInfoComponent._countyCardDescriptionSimple[player.countyDeck[randomCard]];
                    Sprite[] cards = Resources.LoadAll<Sprite>(selectedCountyDeck);
                    countyClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._countyImageNamesDict[player.countyDeck[randomCard]]];
                }
            }
        }
    }

    public void ShopInit()
    {   
        rerollCost = gameplayInfoComponent.currentRound;
        
        Player player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>();
        player.score = 0;
        player.cardsInHandCount = 0;
        player.discards = player.maxDiscards;
        player.hands = player.maxHands;
        player.currentCardsInHand.Clear();
        player.cardsInHand.Clear();
        PlayerStatsPanel.transform.GetChild(0).gameObject.SetActive(true);
        PlayerStatsPanel.transform.GetChild(1).gameObject.SetActive(false);
        PlayerStatsPanel.SetActive(false);
        ShopPanel.SetActive(true);
        ingameJokerHolder.SetActive(false);
        ingameTarotHolder.SetActive(false);

        GameObject yourMoneyText = GameObject.Find("YOUR MONEY TEXT");
        GameObject ownedJokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
        GameObject jokerDescription = GameObject.Find("JOKER DESCRIPTION");
        GameObject ownedJokerText = GameObject.Find("OWNED JOKER TEXT");
        GameObject ownedTarotText = GameObject.Find("OWNED TAROT TEXT");
        GameObject ownedJokerCardGroup = GameObject.Find("OWNED JOKER CARD HOLDER");
        GameObject ownedTarotCardGroup = GameObject.Find("OWNED TAROT CARD HOLDER");

        ownedJokerDescription.GetComponent<TMP_Text>().text = "";
        jokerDescription.GetComponent<TMP_Text>().text = "";
        ownedJokerText.GetComponent<TMP_Text>().text = $"({player.currentJokerCards.Count} / {Player.GetComponent<Player>().gameMaxJokers})";
        ownedTarotText.GetComponent<TMP_Text>().text = $"({player.currentTarotCards.Count} / {Player.GetComponent<Player>().gameMaxTarots})";
        yourMoneyText.GetComponent<TMP_Text>().text = $"{player.money}€";
        TotalScoreText.GetComponent<TMP_Text>().text = player.score.ToString();

        foreach (Transform child in ownedJokerCardGroup.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string joker in player.currentJokerCards)
        {
            string suit = joker.Split(" ")[0];
            string rank = joker.Split(" ")[1];
            GameObject ownedJokerClone = Instantiate(JokerPrefab, ownedJokerCardGroup.transform);
            JokerCard jokerComponent = ownedJokerClone.GetComponent<JokerCard>();
            ownedJokerClone.transform.name = $"{suit} {rank}";
            jokerComponent.suit = suit;
            jokerComponent.rank = rank;
            jokerComponent.cardName = gameplayInfoComponent.jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[0];
            jokerComponent.cardDescription = gameplayInfoComponent.jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[1];
            jokerComponent.price = gameplayInfoComponent._jokerCardPricesSimple[jokerComponent.rank];
            jokerComponent.description = gameplayInfoComponent._jokerCardDescriptionSimple[jokerComponent.rank];
            jokerComponent.isOwned = true;
            Sprite[] cards = Resources.LoadAll<Sprite>(selectedJokerDeck);
            ownedJokerClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._jokerImageNamesDict[$"{suit} {rank}"]];
        }

        foreach (Transform child in ownedTarotCardGroup.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string tarot in player.currentTarotCards)
        {
            GameObject ownedTarotClone = Instantiate(tarotPrefab, ownedTarotCardGroup.transform);
            TarotCard tarotComponent = ownedTarotClone.GetComponent<TarotCard>();
            ownedTarotClone.transform.name = tarot;

            tarotComponent.nameOfCard = tarot;
            tarotComponent.cardName = gameplayInfoComponent.tarotDeckCardNamesDescDict[selectedTarotDeck][tarot].Split(';')[0];
            tarotComponent.cardDescription = gameplayInfoComponent.tarotDeckCardNamesDescDict[selectedTarotDeck][tarot].Split(';')[1];
            tarotComponent.price = gameplayInfoComponent._tarotCardPricesSimple[tarot];
            tarotComponent.description = gameplayInfoComponent._tarotCardDescriptionSimple[tarot];
            tarotComponent.isOwned = true;
            Sprite[] cards = Resources.LoadAll<Sprite>(selectedTarotDeck);
            ownedTarotClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._tarotImageNamesDict[tarot]];
        }

        foreach (Transform child in GameObject.Find("JOKER CARD HOLDER").transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {   
            int randomCard = 0;
            int jokerOrTarot = rnd.Next(0, 100); // 0 - 79 joker, 80 - 100 tarot
            if (jokerOrTarot < Player.GetComponent<Player>().gameJokerChance)
            {
                randomCard = rnd.Next(player.jokerDeck.Count); 
                string[] suitRank = player.jokerDeck[randomCard].Split(' ');
                player.jokerDeck.RemoveAt(randomCard);
                string suit = suitRank[0];
                string rank = suitRank[1];
                GameObject jokerClone = Instantiate(JokerPrefab, GameObject.Find("JOKER CARD HOLDER").transform);
                JokerCard jokerComponent = jokerClone.GetComponent<JokerCard>();
                jokerClone.transform.name = $"{suit} {rank}";
                jokerComponent.suit = suit;
                jokerComponent.rank = rank;
                jokerComponent.cardName = gameplayInfoComponent.jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[0];
                jokerComponent.cardDescription = gameplayInfoComponent.jokerDeckCardNamesDescDict[selectedJokerDeck][$"{suit} {rank}"].Split(';')[1];
                jokerComponent.price = gameplayInfoComponent._jokerCardPricesSimple[jokerComponent.rank];
                jokerComponent.description = gameplayInfoComponent._jokerCardDescriptionSimple[jokerComponent.rank];
                Sprite[] cards = Resources.LoadAll<Sprite>(selectedJokerDeck);
                jokerClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._jokerImageNamesDict[$"{suit} {rank}"]];
            }
            else
            {   
                bool tarotFound = false;
                bool countyFound = false;
                int tarotOrCounty = rnd.Next(0, 100); // 0 - 50 tarot, 50 - 100 county

                if (tarotOrCounty <= 50)
                {
                    while (!tarotFound)
                    {
                        randomCard = rnd.Next(player.tarotDeck.Count);
                        tarotFound = true;
                        foreach (Transform child in GameObject.Find("JOKER CARD HOLDER").transform)
                        {   
                            if (child.gameObject.GetComponent<TarotCard>() == null) continue;
                            else if (child.gameObject.GetComponent<TarotCard>().nameOfCard == player.tarotDeck[randomCard])
                            {
                                tarotFound = false;
                                break;
                            }
                        }
                        foreach (string tarot in Player.GetComponent<Player>().currentTarotCards)
                        {
                            if (tarot == player.tarotDeck[randomCard])
                            {
                                tarotFound = false;
                                break;
                            }
                        }
                    }

                    GameObject tarotClone = Instantiate(tarotPrefab, GameObject.Find("JOKER CARD HOLDER").transform);
                    TarotCard tarotComponent = tarotClone.GetComponent<TarotCard>();
                    tarotClone.transform.name = player.tarotDeck[randomCard];
                    tarotClone.transform.localScale = new Vector3(1, 1.1f, 1);

                    tarotComponent.nameOfCard = player.tarotDeck[randomCard];
                    tarotComponent.cardName = player.tarotDeck[randomCard];
                    tarotComponent.cardDescription = player.tarotDeck[randomCard];
                    tarotComponent.price = gameplayInfoComponent._tarotCardPricesSimple[player.tarotDeck[randomCard]];
                    tarotComponent.description = gameplayInfoComponent._tarotCardDescriptionSimple[player.tarotDeck[randomCard]];
                    Sprite[] cards = Resources.LoadAll<Sprite>(selectedTarotDeck);
                    tarotClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._tarotImageNamesDict[player.tarotDeck[randomCard]]];
                }
                else
                {
                    while (!countyFound)
                    {
                        randomCard = rnd.Next(player.countyDeck.Count);
                        countyFound = true;
                        foreach (Transform child in GameObject.Find("JOKER CARD HOLDER").transform)
                        {   
                            if (child.gameObject.GetComponent<CountyCard>() == null) continue;
                            else if (child.gameObject.GetComponent<CountyCard>().nameOfCard == player.countyDeck[randomCard])
                            {
                                countyFound = false;
                                break;
                            }
                        }
                    }

                    GameObject countyClone = Instantiate(countyPrefab, GameObject.Find("JOKER CARD HOLDER").transform);
                    CountyCard countyComponent = countyClone.GetComponent<CountyCard>();
                    countyClone.transform.name = player.countyDeck[randomCard];
                    countyClone.transform.localScale = new Vector3(1, 1.1f, 1);

                    countyComponent.nameOfCard = player.countyDeck[randomCard];
                    countyComponent.cardName = player.countyDeck[randomCard];
                    countyComponent.cardDescription = player.countyDeck[randomCard];
                    countyComponent.price = gameplayInfoComponent._countyCardPricesSimple[player.countyDeck[randomCard]];
                    countyComponent.description = gameplayInfoComponent._countyCardDescriptionSimple[player.countyDeck[randomCard]];
                    Sprite[] cards = Resources.LoadAll<Sprite>(selectedCountyDeck);
                    countyClone.transform.GetChild(1).GetComponent<Image>().sprite = cards[gameplayInfoComponent._countyImageNamesDict[player.countyDeck[randomCard]]];
                }
            }
        }
        roundEnd = false;
        roundEndInitialized = false;
    }

    public void ShopReadyButton()
    {
        ShopReadyBtn = GameObject.Find("SHOP READY BUTTON");
        playerComponent.shopReady = !playerComponent.shopReady;

        if (playerComponent.shopReady) ShopReadyBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "NOT READY";
        else ShopReadyBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "READY";
    }

    public void resultsReadyButton()
    {
        //ResultsReadyBtn = GameObject.Find("RESULTS READY BUTTON");
        playerComponent.resultsReady = !playerComponent.resultsReady;

        if (playerComponent.resultsReady) ResultsReadyBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "NOT READY";
        else ResultsReadyBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "READY";
    }

    void CheckRoundEnd()
    {

        if (inShop || inResults) return;

        int count = 0;

        foreach (User user in users)
        {
            Player player = GameObject.Find($"Playerinfo ({user.Name})").GetComponent<Player>();
            if (player.hands == 0) count++;
        }

        //if (count == users.Count)
        if (noHandsLeftCounter == users.Count) 
        {
            roundEnd = true;
        }
        else
        {
            roundEnd = false;
        } 

    }

    void UpdateStats()
    {
        foreach (User user in users)
        {
            GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
            GameObject PlayerNameText = GameObject.Find($"PlayerNameText {user.Name}");
            if (PlayerNameText != null && !roundEnd) PlayerNameText.GetComponent<TMP_Text>().text = $"<b><color=red>{user.Name}</color></b> | <color=blue>SCORE: {currentPlayer.GetComponent<Player>().score}</color> | <color=#117B00>HANDS: {currentPlayer.GetComponent<Player>().hands}</color> | <color=red>DISCARDS: {currentPlayer.GetComponent<Player>().discards}</color> | <color=#630039>TOTAL POINTS: {currentPlayer.GetComponent<Player>().points}</color> | <color=#9D5800>MONEY: {currentPlayer.GetComponent<Player>().money}</color>";
        }
    }

    [SynchronizableMethod]
    public void SendScore(string username, string score, string hands, string discards, string tokMoney, string totalMoney, string points, string allRoundTotalScore, string allRoundTotalWins, string interest)
    {
        scores.Add($"{username} {score} {hands} {discards} {tokMoney} {totalMoney} {points} {allRoundTotalScore} {allRoundTotalWins} {interest}");
    }

    public IEnumerator PlayerBarsEnum()
    {

        scores = scores.OrderByDescending(x => Int32.Parse(x.Split(" ")[1])).ThenByDescending(x => Int32.Parse(x.Split(" ")[3])).ToList();
        int counter = 0;
        if (gameplayInfoComponent.currentRound < playerComponent.gameRounds)
        {
            foreach (string score in scores)
            {   
                counter++;
                foreach(User user in users)
                {
                    GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                    Player player = currentPlayer.GetComponent<Player>();
            
                    if (player.Username == score.Split(" ")[0] && counter == 1)
                    {      
                        RoundCounter(user, player, scores, firstMoney, firstScore, 0);
                        break;
                    }
                    else if (users.Count > 1 && player.Username == score.Split(" ")[0]  && counter == 2)
                    {   
                        RoundCounter(user, player, scores, secondMoney, secondScore, 1);
                        break;
                    }
                    else if (users.Count > 2 && player.Username == score.Split(" ")[0] && counter == 3)
                    {
                        RoundCounter(user, player, scores, thirdMoney, thirdScore, 2);
                        break;
                    }
                    else if (users.Count > 3 && player.Username == score.Split(" ")[0] && counter == 4)
                    {
                        RoundCounter(user, player, scores, fourthMoney, fourthScore, 3);
                        break;
                    }
                    else if (users.Count > 4 && player.Username == score.Split(" ")[0] && counter == 5)
                    {
                        RoundCounter(user, player, scores, fifthMoney, fifthScore, 4);
                        break;
                    }
                    else if (users.Count > 5 && player.Username == score.Split(" ")[0] && counter == 6)
                    {
                        RoundCounter(user, player, scores, sixthMoney, 0, 5);
                        break;
                    }
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        else if (gameplayInfoComponent.currentRound == playerComponent.gameRounds)
        {    
            counter = 0;
            foreach (string score in scores)
            {   
                counter++;
                foreach(User user in users)
                {
                    GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                    Player player = currentPlayer.GetComponent<Player>();
            
                    if (player.Username == score.Split(" ")[0] && counter == 1)
                    {      
                        EndCounter(user, player, scores, firstMoney, firstScore, 0);
                        break;
                    }
                    else if (player.Username == score.Split(" ")[0] && counter == 2)
                    {   
                        EndCounter(user, player, scores, secondMoney, secondScore, 1);
                        break;
                    }
                    else if (player.Username == score.Split(" ")[0] && counter == 3)
                    {
                        EndCounter(user, player, scores, thirdMoney, thirdScore, 2);
                        break;
                    }
                    else if (player.Username == score.Split(" ")[0] && counter == 4)
                    {
                        EndCounter(user, player, scores, fourthMoney, fourthScore, 3);
                        break;
                    }
                    else if (player.Username == score.Split(" ")[0] && counter == 5)
                    {
                        EndCounter(user, player, scores, fifthMoney, fifthScore, 4);
                        break;
                    }
                    else if (player.Username == score.Split(" ")[0] && counter == 6)
                    {
                        EndCounter(user, player, scores, sixthMoney, 0, 5);
                        break;
                    }
                }
            }

            counter = 0;
            int scoreCounter = 0;
            foreach (string point in endGamePointsList)
            {   
                counter++;
                foreach (User user in users)
                {   
                    GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                    Player player = currentPlayer.GetComponent<Player>();

                    if (user.Name == point.Split(" ")[0] && counter == 1)
                    {   
                        scoreCounter = 0;
                        foreach (string score in scores)
                        {   
                            if (point.Split(" ")[0] == score.Split(" ")[0])
                            {
                                EndBars(user, player, scores, Int32.Parse(point.Split(" ")[4]), Int32.Parse(point.Split(" ")[5]), 0, scoreCounter);
                                break;
                            }
                            scoreCounter++;
                        }
                        break;
                    }
                    else if (user.Name == point.Split(" ")[0] && counter == 2)
                    {   
                        scoreCounter = 0;
                        foreach (string score in scores)
                        {   
                            if (point.Split(" ")[0] == score.Split(" ")[0])
                            {
                                EndBars(user, player, scores, Int32.Parse(point.Split(" ")[4]), Int32.Parse(point.Split(" ")[5]), 1, scoreCounter);
                                break;
                            }
                            scoreCounter++;
                        }
                        break;
                    }
                    else if (user.Name == point.Split(" ")[0] && counter == 3)
                    {   
                        scoreCounter = 0;
                        foreach (string score in scores)
                        {   
                            if (point.Split(" ")[0] == score.Split(" ")[0])
                            {
                                EndBars(user, player, scores, Int32.Parse(point.Split(" ")[4]), Int32.Parse(point.Split(" ")[5]), 2, scoreCounter);
                                break;
                            }
                            scoreCounter++;
                        }
                        break;
                    }
                    else if (user.Name == point.Split(" ")[0] && counter == 4)
                    {   
                        scoreCounter = 0;
                        foreach (string score in scores)
                        {   
                            if (point.Split(" ")[0] == score.Split(" ")[0])
                            {
                                EndBars(user, player, scores, Int32.Parse(point.Split(" ")[4]), Int32.Parse(point.Split(" ")[5]), 3, scoreCounter);
                                break;
                            }
                            scoreCounter++;
                        }
                        break;
                    }
                    else if (user.Name == point.Split(" ")[0] && counter == 5)
                    {   
                        scoreCounter = 0;
                        foreach (string score in scores)
                        {   
                            if (point.Split(" ")[0] == score.Split(" ")[0])
                            {
                                EndBars(user, player, scores, Int32.Parse(point.Split(" ")[4]), Int32.Parse(point.Split(" ")[5]), 4, scoreCounter);
                                break;
                            }
                            scoreCounter++;
                        }
                        break;
                    }
                    else if (user.Name == point.Split(" ")[0] && counter == 6)
                    {   
                        scoreCounter = 0;
                        foreach (string score in scores)
                        {   
                            if (point.Split(" ")[0] == score.Split(" ")[0])
                            {
                                EndBars(user, player, scores, Int32.Parse(point.Split(" ")[4]), Int32.Parse(point.Split(" ")[5]), 5, scoreCounter);
                                break;
                            }
                            scoreCounter++;
                        }
                        break;
                    }
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        roundEndInitialized = true;
        inResults = true;
        yield return null;

    }

    public IEnumerator ShopInitEnum()
    {   
        if (users.Count >= 1)
        {
            StartCoroutine(Utility.SmoothMovementToPoint(playerResultBar1, playerResultBar1FinalPos.transform.position, playerResultBar1StartPos.transform.position, touchBlock, 2));
            yield return new WaitForSeconds(0.3f);
        }
        if (users.Count >= 2)
        {
            StartCoroutine(Utility.SmoothMovementToPoint(playerResultBar2, playerResultBar2FinalPos.transform.position, playerResultBar2StartPos.transform.position, touchBlock, 2));
            yield return new WaitForSeconds(0.3f);
        }
        if (users.Count >= 3)
        {
            StartCoroutine(Utility.SmoothMovementToPoint(playerResultBar3, playerResultBar3FinalPos.transform.position, playerResultBar3StartPos.transform.position, touchBlock, 2));
            yield return new WaitForSeconds(0.3f);
        }
        if (users.Count >= 4)
        {
            StartCoroutine(Utility.SmoothMovementToPoint(playerResultBar4, playerResultBar4FinalPos.transform.position, playerResultBar4StartPos.transform.position, touchBlock, 2));
            yield return new WaitForSeconds(0.3f);
        }
        if (users.Count >= 5)
        {
            StartCoroutine(Utility.SmoothMovementToPoint(playerResultBar5, playerResultBar5FinalPos.transform.position, playerResultBar5StartPos.transform.position, touchBlock, 2));
            yield return new WaitForSeconds(0.3f);
        }
        if (users.Count >= 6)
        {
            StartCoroutine(Utility.SmoothMovementToPoint(playerResultBar6, playerResultBar6FinalPos.transform.position, playerResultBar6StartPos.transform.position, touchBlock, 2));
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.3f);

        playerComponent.resultsReady = false;
        resultsReadyPlayersCount.SetActive(false);
        PlayerStatsPanel.SetActive(false);
        inShop = true;
        ShopInit();
    }

    public IEnumerator ShopReadyEnum()
    {   
        try
        {
            ingameJokerHolder.SetActive(true);
            ingameTarotHolder.SetActive(true);
            PlayingCardGroup.gameObject.SetActive(true);
            RankBtn.SetActive(true);
            SuitBtn.SetActive(true);
            DeckSizeText.SetActive(true);
            DeckBtn.SetActive(true);
            OtherPlayersBtn.SetActive(true);
            HandScoreBtn.SetActive(true);
            CurrentPlayerText.SetActive(true);
            CurrentPlayerTextTop.SetActive(true);
            LeftBGPanel.SetActive(true);
            RoundText.SetActive(true);
            ScoreText.SetActive(true);
            TotalScoreText.SetActive(true);
            HandsText.SetActive(true);
            DiscardsText.SetActive(true);
            HandCheckText.SetActive(true);
            HandScoreText.SetActive(true);
            PlayBtn.SetActive(true);
            DiscardBtn.SetActive(true);
        } catch (Exception e)
        {   
            StatusPopup.Instance.TriggerStatus($"Error: {e}");
        }
        
        yield return null;
    }

    public void PlayerBarInit(GameObject bar, Transform barStart, Transform barFinal, User user, int playerScore, int currentMoney, int moneyGained, int interest, int playerPoints, int pointsGained, int discards, int money, int moneyPoints, int score, int scorePoints)
    {
        StartCoroutine(Utility.SmoothMovementToPoint(bar, barStart.position, barFinal.position, touchBlock, 2));
        bar.transform.GetChild(0).GetComponent<TMP_Text>().text = user.Name;
        bar.transform.GetChild(1).GetComponent<TMP_Text>().text = $"<b>SCORE</b>\n<color=red>{playerScore}";
        bar.transform.GetChild(2).GetComponent<TMP_Text>().text = $"<b>MONEY</b>\n<color=yellow>{currentMoney}€</color><sup>(+{moneyGained})</sup>";
        bar.transform.GetChild(3).GetComponent<TMP_Text>().text = $"INTEREST:\t<color=yellow>{interest}€</color>\nTÖK:\t\t<color=yellow>{tokMoney}€</color>\nPOSITION:\t<color=yellow>{money}€</color>";
        bar.transform.GetChild(4).GetComponent<TMP_Text>().text = $"<b>POINTS</b>\n<color=purple>{playerPoints+pointsGained}</color><sup>(+{pointsGained})</sup>";
        bar.transform.GetChild(5).GetComponent<TMP_Text>().text = $"DISCARDS:\t<color=purple>{discards}</color>\nMONEY:\t<color=purple>{moneyPoints}</color>\nPOSITION:\t<color=purple>{score}</color>\nSCORE:\t\t<color=purple>{scorePoints}</color>";
    }
    
    void EndCounter(User user, Player player, List<string> scores, int money, int score, int place)
    {   
        int moneyPoints = 0;

        int currentMoney = 0;
        int moneyGained = 0;
        int totalMoney = Int32.Parse(scores[place].Split(" ")[5]);
        int tokMoney = Int32.Parse(scores[place].Split(" ")[4]);
        int interest = Int32.Parse(scores[place].Split(" ")[9]);
        int haveWonAddition = 0;

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds) 
        {   
            moneyGained = (tokMoney + interest) * Player.GetComponent<Player>().gameMoneyMult;
            currentMoney = totalMoney + moneyGained;
        }           
        else 
        {   
            moneyGained = (money + tokMoney + interest) * Player.GetComponent<Player>().gameMoneyMult;
            currentMoney = totalMoney + moneyGained;
        }

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds)
        {
            moneyPoints = (int)Mathf.Floor(currentMoney / 2);
            money = 0;
        } 
        
        
        int playerScore = Int32.Parse(scores[place].Split(" ")[1]);
        int hands = Int32.Parse(scores[place].Split(" ")[2]);
        int discards = Int32.Parse(scores[place].Split(" ")[3]);
        int playerPoints = Int32.Parse(scores[place].Split(" ")[6]);
        int allRoundTotalScore = Int32.Parse(scores[place].Split(" ")[7]);
        int allRoundTotalWins = Int32.Parse(scores[place].Split(" ")[8]);
        
        int scorePoints = (int)Mathf.Floor(playerScore/1000);
        int pointsGained = (int)(score+discards+Mathf.Floor(playerScore/1000)+moneyPoints);

        moneyPoints = 0;
        if (gameplayInfoComponent.currentRound < Player.GetComponent<Player>().gameRounds && Player.GetComponent<Player>().Username == user.Name)
        {   
            player.points += pointsGained + moneyPoints;
            player.money += moneyGained;
            player.allRoundTotalScore += playerScore;

            if (place == 0) 
            {   
                player.wasWinner = true;
                player.allRoundTotalWins++;
            }
            if (place + 1 == users.Count) player.wasLast = true;
        }

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds) 
        {   
            if (place == 0) haveWonAddition = 1;
            endGamePointsList.Add($"{user.Name} {playerPoints + pointsGained} {allRoundTotalScore + playerScore} {allRoundTotalWins + haveWonAddition} {score} {money} {scorePoints}");
            endGamePointsList = endGamePointsList.OrderByDescending(x => Int32.Parse(x.Split(" ")[1])).ThenByDescending(x => Int32.Parse(x.Split(" ")[2])).ThenByDescending(x => Int32.Parse(x.Split(" ")[3])).ToList();
        }
        
    }

    public void EndBars(User user, Player player, List<string> scores, int score, int money, int place, int scorePlace)
    {   

        int moneyPoints = 0;

        int currentMoney;
        int moneyGained;
        int totalMoney = Int32.Parse(scores[scorePlace].Split(" ")[5]);
        int tokMoney = Int32.Parse(scores[scorePlace].Split(" ")[4]);
        int interest = Int32.Parse(scores[scorePlace].Split(" ")[9]);
        //int haveWonAddition = 0;

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds) 
        {   
            moneyGained = (tokMoney + interest) * Player.GetComponent<Player>().gameMoneyMult;
            currentMoney = totalMoney + moneyGained;
        }           
        else 
        {   
            moneyGained = (money + tokMoney + interest) * Player.GetComponent<Player>().gameMoneyMult;
            currentMoney = totalMoney + moneyGained;
        }

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds)
        {
            moneyPoints = (int)Mathf.Floor(currentMoney / 2);
            money = 0;
        } 
        
        int playerScore = Int32.Parse(scores[scorePlace].Split(" ")[1]);
        int discards = Int32.Parse(scores[scorePlace].Split(" ")[3]);
        int playerPoints = Int32.Parse(scores[scorePlace].Split(" ")[6]);

        int scorePoints = (int)Mathf.Floor(playerScore/1000);
        int pointsGained = score + discards + scorePoints + moneyPoints;

        if (place == 0) PlayerBarInit(playerResultBar1, playerResultBar1StartPos.transform, playerResultBar1FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 1) PlayerBarInit(playerResultBar2, playerResultBar2StartPos.transform, playerResultBar2FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 2) PlayerBarInit(playerResultBar3, playerResultBar3StartPos.transform, playerResultBar3FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 3) PlayerBarInit(playerResultBar4, playerResultBar4StartPos.transform, playerResultBar4FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 4) PlayerBarInit(playerResultBar5, playerResultBar5StartPos.transform, playerResultBar5FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 5) PlayerBarInit(playerResultBar6, playerResultBar6StartPos.transform, playerResultBar6FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
    }

    void RoundCounter(User user, Player player, List<string> scores, int money, int score, int place)
    {   
        int moneyPoints = 0;

        int currentMoney = 0;
        int moneyGained = 0;
        int totalMoney = Int32.Parse(scores[place].Split(" ")[5]);
        int tokMoney = Int32.Parse(scores[place].Split(" ")[4]);
        int interest = Int32.Parse(scores[place].Split(" ")[9]);

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds) 
        {   
            moneyGained = (tokMoney + interest) * Player.GetComponent<Player>().gameMoneyMult;
            currentMoney = totalMoney + moneyGained;
        }           
        else 
        {   
            moneyGained = (money + tokMoney + interest) * Player.GetComponent<Player>().gameMoneyMult;
            currentMoney = totalMoney + moneyGained;
        }

        if (gameplayInfoComponent.currentRound == Player.GetComponent<Player>().gameRounds)
        {
            moneyPoints = (int)Mathf.Floor(currentMoney / 2);
            money = 0;
        } 
        
        
        int playerScore = Int32.Parse(scores[place].Split(" ")[1]);
        int hands = Int32.Parse(scores[place].Split(" ")[2]);
        int discards = Int32.Parse(scores[place].Split(" ")[3]);
        int playerPoints = Int32.Parse(scores[place].Split(" ")[6]);
        int allRoundTotalScore = Int32.Parse(scores[place].Split(" ")[7]);
        int allRoundTotalWins = Int32.Parse(scores[place].Split(" ")[8]);

        int scorePoints = (int)Mathf.Floor(playerScore/1000);
        int pointsGained = score + discards + scorePoints + moneyPoints;

        if (place == 0) PlayerBarInit(playerResultBar1, playerResultBar1StartPos.transform, playerResultBar1FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 1) PlayerBarInit(playerResultBar2, playerResultBar2StartPos.transform, playerResultBar2FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 2) PlayerBarInit(playerResultBar3, playerResultBar3StartPos.transform, playerResultBar3FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 3) PlayerBarInit(playerResultBar4, playerResultBar4StartPos.transform, playerResultBar4FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 4) PlayerBarInit(playerResultBar5, playerResultBar5StartPos.transform, playerResultBar5FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);
        else if (place == 5) PlayerBarInit(playerResultBar6, playerResultBar6StartPos.transform, playerResultBar6FinalPos.transform, user, playerScore, currentMoney, moneyGained, interest, playerPoints, pointsGained, discards, money, moneyPoints, score, scorePoints);

        moneyPoints = 0;
        if (gameplayInfoComponent.currentRound < Player.GetComponent<Player>().gameRounds && Player.GetComponent<Player>().Username == user.Name)
        {   
            player.points += pointsGained + moneyPoints;
            player.money += moneyGained;
            player.allRoundTotalScore += playerScore;

            if (place == 0) 
            {   
                player.wasWinner = true;
                player.allRoundTotalWins++;
            }
            else
            {
                player.wasWinner = false;
            }
            if (place + 1 == users.Count) player.wasLast = true;
            else player.wasLast = true;
        }
    }

    public IEnumerator SendScoreEnum()
    {
        GameObject.Find("PLAYER").GetComponent<CameraShake>().shakingDict.Clear();                

        //PlayingCardGroup.gameObject.SetActive(false);
        DeckPanel.SetActive(false);
        HandScorePanel.SetActive(false);
        PlayerStatsPanel.SetActive(false);

        GameObject.Find("INGAME OWNED JOKER CARD DESCRIPTION").GetComponent<TMP_Text>().text = "";
        GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION").GetComponent<TMP_Text>().text = "";

        StartCoroutine(Utility.UIElementMoveAndDisableEnum(ingameJokerHolder, touchBlock, 2, false, false));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(ingameTarotHolder, touchBlock, 2, false, false));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(PlayingCardGroup.gameObject, touchBlock, 2, false, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(RankBtn, touchBlock, 2, false, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(SuitBtn, touchBlock, 2, false, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(DeckSizeText, touchBlock, 2, false, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(otherPlayerPlayedHandText, touchBlock, 2, false, true));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(DeckBtn, touchBlock, 2, true, false));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(OtherPlayersBtn, touchBlock, 2, true, false));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(HandScoreBtn, touchBlock, 2, true, false));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(CurrentPlayerText, touchBlock, 2, true, false));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(CurrentPlayerTextTop, touchBlock, 2, true, false));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(LeftBGPanel, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(scoreGatheredText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(RoundText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(ScoreText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(TotalScoreText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(HandsText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(DiscardsText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(HandCheckText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(HandScoreText, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(PlayBtn, touchBlock, 2, true, true));
        StartCoroutine(Utility.UIElementMoveAndDisableEnum(DiscardBtn, touchBlock, 2, true, true));
        
        yield return new WaitForSeconds(0.5f);
        playerComponent.cardsInHand.Clear();
        playerComponent.currentCardsInHand.Clear();

        foreach (Transform child in PlayingCardGroup)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in CardVisuals.transform)
        {
            Destroy(child.gameObject);
        }

        if (gameplayInfoComponent.currentRound == playerComponent.gameRounds) 
        {   
            //TODO ADD "PLAY AGAIN" BUTTON AFTER 4TH ROUND
            ResultsReadyBtn.SetActive(false);
            resultsReadyPlayersCount.SetActive(false);
            ExitBtn.SetActive(true);
        }
    
        if (users.Count == 1)
        {
            firstScore = 2;
            firstMoney = 10;
        } 
        
        if (users.Count == 2)
        {
            firstScore = 2;

            firstMoney = 10;
            secondMoney = 8;
        } 
        else if (users.Count == 3)
        {
            firstScore = 2;
            secondScore = 1;

            firstMoney = 10;
            secondMoney = 9;
            thirdMoney = 8;
        }
        else if (users.Count == 4) 
        {
            firstScore = 4;
            secondScore = 3;
            thirdScore = 2;
            fourthScore = 1;

            firstMoney = 12;
            secondMoney = 11;
            thirdMoney = 10;
            fourthMoney = 9;
            fifthMoney = 8;
        }
        else if (users.Count == 5) 
        {
            firstScore = 5;
            secondScore = 4;
            thirdScore = 3;
            fourthScore = 2;
            fifthScore = 1;

            firstMoney = 13;
            secondMoney = 12;
            thirdMoney = 11;
            fourthMoney = 10;
            fifthMoney = 9;
            sixthMoney = 8;
        }
        
        zoldScore = 0;
        tokMoney = 0;

        foreach (string joker in playerComponent.currentJokerCards)
        {   
            if (joker.Split(" ")[0] == "Zöld")
            {
                zoldScore += 100 * gameplayInfoComponent.currentRound;
            }
        }         

        foreach (string joker in playerComponent.currentJokerCards)
        {   
            
            if (joker.Split(" ")[0] == "Tök")
            {
                tokMoney += 2;
            }
        }

        int interest = (int)playerComponent.money >= 25 ? 5 : (int)Mathf.Floor(playerComponent.money / 5);

        playerComponent.score += zoldScore;
        //playerComponent.money += interest;

        BroadcastRemoteMethod("SendScore", playerComponent.Username, playerComponent.score.ToString(), playerComponent.hands.ToString(), playerComponent.discards.ToString(), tokMoney.ToString(), playerComponent.money.ToString(), playerComponent.points.ToString(), playerComponent.allRoundTotalScore.ToString(), playerComponent.allRoundTotalWins.ToString(), interest.ToString());
    }

    // Update is called once per frame
    void Update()
    {   
    
        room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;
        //Player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
        users = multiplayer.GetUsers();

        UpdateStats();
        CheckRoundEnd();
        
        //currentPlayer();
        if (!foundRoom)
        {
            PlayBtn = GameObject.Find("PLAY BUTTON");
            DiscardBtn = GameObject.Find("DISCARD BUTTON");
            SuitBtn = GameObject.Find("SUIT ORDER");
            RankBtn = GameObject.Find("RANK ORDER");
            //HandCheckText = GameObject.Find("HAND CHECK TEXT");
            //HandScoreText = GameObject.Find("HAND SCORE TEXT");
            //TotalScoreText = GameObject.Find("SCORE (NUMBER)");
            HandsText = GameObject.Find("HANDS");
            DiscardsText = GameObject.Find("DISCARDS");
            //DeckSizeText = GameObject.Find("DECK SIZE");
            //CurrentPlayerText = GameObject.Find("CURRENT PLAYER BOTTOM");
            //RoundText = GameObject.Find("ROUND TEXT");
            PlayingCardGroup = GameObject.Find("PlayingCardGroup")?.transform;
            PlayedHandGroup = GameObject.Find("PlayedHandGroup");
            OtherPlayersBtn = GameObject.Find("OTHER PLAYERS");

            SuitBtn.GetComponent<Button>().onClick.AddListener(OrderBySuitButton);
            RankBtn.GetComponent<Button>().onClick.AddListener(OrderByRankButton);
            PlayBtn.GetComponent<Button>().onClick.AddListener(delegate{PlayButton(false);});
            DiscardBtn?.GetComponent<Button>().onClick.AddListener(delegate{DiscardingButton(false);});
            if (playerComponent.playerID == playerComponent.currentPlayerID) playerComponent.isCurrent = true;
            if (DiscardBtn != null) foundRoom = true;
            else return;
        }

        if(otherPlayerPlayedHand)
        {
            otherPlayerPlayedHandTimer += Time.deltaTime;

            if (otherPlayerPlayedHandTimer >= 5)
            {
                otherPlayerPlayedHand = false;
                otherPlayerPlayedHandText.SetActive(false);
            }
        }

        if (DeckSizeText != null)   DeckSizeText.GetComponent<TMP_Text>().text = $"{playerComponent.remainingDeck} / {playerComponent.fullDeck}";
        if (DiscardsText != null) DiscardsText.GetComponent<TMP_Text>().text = playerComponent.discards.ToString();
        if (HandsText != null) HandsText.GetComponent<TMP_Text>().text = playerComponent.hands.ToString();

        if (multiplayer != null && DiscardBtn != null)
        {   
            foreach (User user in users)
            {
                GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                if (currentPlayer.GetComponent<Player>().isCurrent) CurrentPlayerText.GetComponent<TMP_Text>().text = currentPlayer.GetComponent<Player>().Username;
            }
            
            RoundText.GetComponent<TMP_Text>().text = $"ROUND {gameplayInfoComponent.currentRound}";
        }

        if (!Playing && !Discarding)
        {
            OtherPlayersBtn.GetComponent<Button>().interactable = true;
            DeckBtn.GetComponent<Button>().interactable = true;
            HandScoreBtn.GetComponent<Button>().interactable = true;
        }

        if (playerComponent.isCurrent && !roundEnd)
        {   
            
            if (!timeSet)
            {
                timeLeft = playerComponent.TimeLimit;
                timeSet = true;
            }
            
            //Time limit handling
            if (!Playing && !Discarding && timeLeft > 0 && !inResults && !inShop)
            {   
                if (!timerText.activeSelf || !timerCircle.activeSelf)
                {
                    timerText.SetActive(true);
                    timerCircle.SetActive(true);
                }
                timeLeft -= Time.unscaledDeltaTime;
                timerText.GetComponent<TMP_Text>().text = Mathf.Ceil(timeLeft).ToString();

                float aColor = 0;
                float bColor = 255;

                if (aColor < 255) aColor = (playerComponent.TimeLimit - timeLeft) * (255 / playerComponent.TimeLimit) * 2;
                if (aColor > 255) aColor = 255;
                if (aColor == 255) bColor = timeLeft / (playerComponent.TimeLimit / 2) * 255;

                timerCircle.GetComponent<Image>().fillAmount = timeLeft / playerComponent.TimeLimit;
                timerCircle.GetComponent<Image>().color = new Color32((byte)aColor, (byte)bColor, 0, 255);
            }

            if (timeLeft <= 0)
            {   

                PlayerStatsPanel.SetActive(false);
                DeckPanel.SetActive(false);
                HandScorePanel.SetActive(false);
                CardVisuals.SetActive(true);

                foreach (GameObject card in playerComponent.cardsInHand)
                {
                    if (card.transform.GetChild(0).gameObject.GetComponent<Card>().selected)
                    {
                        card.transform.GetChild(0).gameObject.GetComponent<Card>().selected = false;
                        card.transform.GetChild(0).gameObject.transform.localPosition = Vector3.zero;
                    }
                }

                gameplayInfoComponent.selectedCards.Clear();

                gameplayInfoComponent.selectedHandCount = 1;

                int random = rnd.Next(0, playerComponent.cardsInHand.Count);

                if (playerComponent.discards > 0)
                {
                    playerComponent.cardsInHand[random].transform.GetChild(0).gameObject.GetComponent<Card>().isDiscarded = true;
                    playerComponent.cardsInHand[random].transform.GetChild(0).gameObject.GetComponent<Card>().selected = true;
                    gameplayInfoComponent.selectedCards.Add(playerComponent.cardsInHand[random].transform.GetChild(0).gameObject);
                    CheckHand(PlayingCardGroup, PlayedHandGroup.transform);
                    DiscardingButton(true);
                }

                else
                {
                    playerComponent.cardsInHand[random].transform.GetChild(0).gameObject.GetComponent<Card>().isPlayed = true;
                    playerComponent.cardsInHand[random].transform.GetChild(0).gameObject.GetComponent<Card>().selected = true;
                    gameplayInfoComponent.selectedCards.Add(playerComponent.cardsInHand[random].transform.GetChild(0).gameObject);
                    CheckHand(PlayingCardGroup, PlayedHandGroup.transform);
                    PlayButton(true);
                }

            }

            scoresShowing = false;

            if (!vibrated && !Playing && !Discarding && !inResults && !inShop)
            {
                Handheld.Vibrate();
                yourTurnSymbol.SetActive(true);

                vibrated = true;
            }

            if (playerComponent.discards == 0 || gameplayInfoComponent.selectedCards.Count == 0) DiscardBtn.GetComponent<Button>().interactable = false;
            if (playerComponent.hands == 0 || gameplayInfoComponent.selectedCards.Count == 0) PlayBtn.GetComponent<Button>().interactable = false;

            if (gameplayInfoComponent.selectedCards.Count > 0 && !Discarding && !Playing && DiscardBtn != null)
            {   
                if (playerComponent.discards > 0)
                {
                    DiscardBtn.GetComponent<Button>().interactable = true;
                }
                if (playerComponent.hands > 0)
                {
                    PlayBtn.GetComponent<Button>().interactable = true;
                }

                CheckHand(PlayingCardGroup, PlayedHandGroup.transform);
            }
            else if ((Playing || Discarding) && DiscardBtn != null)
            {
                //HandCheckText.transform.gameObject.SetActive(false);
                //HandScoreText.transform.gameObject.SetActive(false);
                DiscardBtn.GetComponent<Button>().interactable = false;
                PlayBtn.GetComponent<Button>().interactable = false;
                OtherPlayersBtn.GetComponent<Button>().interactable = false;
                DeckBtn.GetComponent<Button>().interactable = false;
                HandScoreBtn.GetComponent<Button>().interactable = false;
                
            }
            else if (gameplayInfoComponent.selectedCards.Count == 0 && HandCheckText != null)
            {
                HandCheckText?.SetActive(false);
                HandScoreText?.SetActive(false);
            }
        }  
        else if (!roundEnd)
        {   
            //playerComponent.shopReady = false;
            HandCheckText?.SetActive(false);
            HandScoreText?.SetActive(false);
            DiscardBtn.GetComponent<Button>().interactable = false;
            PlayBtn.GetComponent<Button>().interactable = false;
            scoresShowing = false;
        }

        if (inResults)
        {    

            if (!inResultsInitialized)
            {   
                playerComponent.resultsReady = false;
                playerComponent.shopReady = false;
                inResultsInitialized = true;
                //LeftBGPanel.SetActive(false);
                //StealBtn.SetActive(false);
            }

            // int resultsReadyCount = 0;
            // foreach(User user in users)
            // {
            //     GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
            //     Player player = currentPlayer.GetComponent<Player>();
            //     if (player.resultsReady) resultsReadyCount++;
            // }

            // resultsReadyPlayersCount.GetComponent<TMP_Text>().text = $"{resultsReadyCount} / {users.Count}";

            //if (resultsReadyCount == users.Count)
            if (Input.GetMouseButtonDown(0) && gameplayInfoComponent.currentRound < Player.GetComponent<Player>().gameRounds)
            {   
                
                inResults = false;
                inResultsInitialized = false;
                StartCoroutine(ShopInitEnum());
            }
        }

        if (inShop)
        {   

            if (playerComponent.money < rerollCost) RerollBtn.GetComponent<Button>().interactable = false;
            else RerollBtn.GetComponent<Button>().interactable = true;
            RerollBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = $"{rerollCost}€";
            ShopPanel.SetActive(true);

            

            if (!inShopInitialized)
            {
                shopReadyPlayersCount.SetActive(true);
                playerComponent.shopReady = false;
                inShopInitialized = true;
                gameplayInfoComponent.currentRound++;
                shopRoundText.GetComponent<TMP_Text>().text = $"ROUND {gameplayInfoComponent.currentRound}";
                ShopReadyBtn.transform.GetChild(0).GetComponent<TMP_Text>().text = "READY";
                scores.Clear();

                foreach (string joker in playerComponent.currentJokerCards)
                {   
                    GameObject currentJoker = GameObject.Find(joker);
                    currentJoker.GetComponent<JokerCard>().selected = false;
                    currentJoker.GetComponent<JokerCard>().wasSelected = false;
                }

                gameplayInfoComponent.selectedOwnedJokerCount = 0;
                gameplayInfoComponent.selectedJokerCount = 0;

            }

            int shopReadyCount = 0;
            foreach(User user in users)
            {
                GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                Player player = currentPlayer.GetComponent<Player>();
                if (player.shopReady) shopReadyCount++;
            }

            shopReadyPlayersCount.GetComponent<TMP_Text>().text = $"{shopReadyCount} / {users.Count}";

            if (shopReadyCount == users.Count)
            {   
                try
                {
                    noHandsLeftCounter = 0;
                    inShop = false;
                    inShopInitialized = false;
                    shopReadyPlayersCount.SetActive(false);
                    ShopPanel.SetActive(false);
                    StealBtn.SetActive(true);
                    StartCoroutine(ShopReadyEnum());
                } catch (Exception e)
                {   
                    StatusPopup.Instance.TriggerStatus($"Error: {e}");
                }
            }
        }

        if (roundEnd)
        {   
            if (!roundEndInitialized)
            {   
                StartCoroutine(SendScoreEnum());
                roundEndInitialized = true;
            }

            if (scores.Count == users.Count && !scoresShowing)
            {   
                StartCoroutine(PlayerBarsEnum());
                scoresShowing = true;
            }
        }
    }
}
