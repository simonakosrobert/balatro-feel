using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TarotCard : MonoBehaviour, IPointerClickHandler
{   
    public string nameOfCard = null;
    public int price;
    public string description;
    public bool selected = false;
    public bool wasSelected = false;
    public bool moved = false;
    public bool isOwned = false;
    public bool deselectedByOtherCard = false;
    public bool otherPlayerPlaysOrDiscards = false;
    public string cardName = "";
    public string cardDescription = "";
    //private GameObject Gameplayinfo;
    private GameObject jokerDescription;
    private GameplayInfo Gameplayinfo;
    private Image image;
    private Material material;
    public float dissolveAmount = 1;
    private Multiplayer multiplayer;
    GameObject touchBlock;
    public GameObject BuySellButton;
    public GameObject BuySellText;
    public GameObject BuySellPriceText;
    private System.Random rnd = new System.Random();
    public void OnPointerClick(PointerEventData eventData)
    {   
        //Checks if the player plays a hand so they can't select the joker or tarot cards
        if (GameObject.Find("GameHandler").GetComponent<GameHandler>().Playing || eventData.pointerPress.gameObject.GetComponent<Selectable>().interactable == false) return;

        if (!selected && eventData.selectedObject.name == $"{nameOfCard}")
        {   
            if (isOwned)
            {
                foreach (string joker in GameObject.Find($"Playerinfo ({multiplayer.Me.Name})")?.GetComponent<Player>().currentJokerCards)
                {   
                    GameObject currentJoker = GameObject.Find(joker);
                    if (currentJoker.GetComponent<JokerCard>().selected == true)
                    {
                        currentJoker.GetComponent<JokerCard>().wasSelected = true;
                        currentJoker.GetComponent<JokerCard>().moved = true;
                        currentJoker.GetComponent<JokerCard>().selected = false;     
                        currentJoker.GetComponent<JokerCard>().deselectedByOtherCard = true;     
                    }
                }
                foreach (string tarot in GameObject.Find($"Playerinfo ({multiplayer.Me.Name})")?.GetComponent<Player>().currentTarotCards)
                {   
                    GameObject currentTarot = GameObject.Find(tarot);
                    if (currentTarot.GetComponent<TarotCard>().selected == true)
                    {
                        currentTarot.GetComponent<TarotCard>().wasSelected = true;
                        currentTarot.GetComponent<TarotCard>().moved = true;
                        currentTarot.GetComponent<TarotCard>().selected = false;     
                        currentTarot.GetComponent<TarotCard>().deselectedByOtherCard = true;     
                    }
                }
            }
            else
            {
                foreach (Transform child in transform.parent)
                {   
                    GameObject currentCard = child.gameObject;
                    if (currentCard.GetComponent<TarotCard>() != null)
                    {
                        if (currentCard.GetComponent<TarotCard>().selected == true && nameOfCard != currentCard.GetComponent<TarotCard>().nameOfCard)
                        {
                            currentCard.GetComponent<TarotCard>().wasSelected = true;
                            currentCard.GetComponent<TarotCard>().moved = true;
                            currentCard.GetComponent<TarotCard>().selected = false;
                            currentCard.GetComponent<TarotCard>().deselectedByOtherCard = true;
                        }
                    }
                    else if (currentCard.GetComponent<JokerCard>() != null)
                    {
                        if (currentCard.GetComponent<JokerCard>().selected == true)
                        {
                            currentCard.GetComponent<JokerCard>().wasSelected = true;
                            currentCard.GetComponent<JokerCard>().moved = true;
                            currentCard.GetComponent<JokerCard>().selected = false;
                            currentCard.GetComponent<JokerCard>().deselectedByOtherCard = true;
                        }
                    }
                    else
                    {
                        if (currentCard.GetComponent<CountyCard>().selected == true)
                        {
                            currentCard.GetComponent<CountyCard>().wasSelected = true;
                            currentCard.GetComponent<CountyCard>().moved = true;
                            currentCard.GetComponent<CountyCard>().selected = false;
                            currentCard.GetComponent<CountyCard>().deselectedByOtherCard = true;
                        }
                    }
                }
            }

            selected = true;
            moved = true;
            deselectedByOtherCard = false;
        }

        else if (selected && eventData.selectedObject.name == $"{nameOfCard}")
        {   
            selected = false;
            moved = true;
            deselectedByOtherCard = false;
        }

        else if (!isOwned)
        {
            GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
            gameHandler.BuyTarot();
        }
        else if (isOwned)
        {   
            if (BuySellText.GetComponent<TMP_Text>().text == "USE")
            {   
                if (nameOfCard == "A MÁGUS")
                {
                    GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";

                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    gameHandler.magicianActivated = true;
                    //gameHandler.MagicianBroadcast();  
                    StartCoroutine(DissolveEnum());
                          
                }
                if (nameOfCard == "A FŐPAPNŐ")
                {
                    GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";

                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    gameHandler.FlipCardsBroadcast();
                    StartCoroutine(DissolveEnum());               
                }
                if (nameOfCard == "AZ URALKODÓNŐ")
                {
                    string suit = "";
                    string rank = "";
                    string edition = "";
                    GameObject playingCardGroup = GameObject.Find("PlayingCardGroup");
                    foreach (GameObject card in Gameplayinfo.selectedCards)
                    {
                        suit = card.GetComponent<Card>().suit;
                        rank = card.GetComponent<Card>().rank;
                        edition = card.GetComponent<Card>().edition;
                    }
                    for (int i = 0; i < 2; i++) 
                    {   
                        int randomID = rnd.Next(10000000);
                        playingCardGroup.GetComponent<HorizontalCardHolder>().AddCards(false, suit, rank, edition, randomID.ToString());
                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckList.Add($"{suit} {rank}");
                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict[$"{suit} {rank};{randomID}"] = edition;
                        GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().cardsInHandCount += 1;  
                    }

                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().fullDeck += 2;
                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";
                    foreach (GameObject card in Gameplayinfo.GetComponent<GameplayInfo>().selectedCards)
                    {   
                        card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * -50;
                        card.GetComponent<Card>().selected = false;
                    }
                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount = 0;
                    StartCoroutine(DissolveEnum());               
                }

                if (nameOfCard == "A BOLOND")
                {
                    GameObject playingCardGroup = GameObject.Find("PlayingCardGroup");
                    foreach (GameObject card in Gameplayinfo.selectedCards)
                    {
                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckList.Remove($"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank}");
                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict.Remove($"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank};{card.GetComponent<Card>().cardID}");
                        card.GetComponent<Card>().cardSlot = card.transform.parent.gameObject;
                        //card.GetComponent<Card>().isDiscarded = true;
                        //card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * -600;
                        GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentCardsInHand.Remove($"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank}");
                        GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().cardsInHand.Remove(card.GetComponent<Card>().cardSlot);
                        Destroy(card);
                        Destroy(card.GetComponent<Card>().cardVisualObj);
                        Destroy(card.GetComponent<Card>().cardSlot);
                        GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().cardsInHandCount -= 1;  
                    }

                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount = 0;
                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().fullDeck -= 2;
                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";
                    foreach (GameObject card in Gameplayinfo.GetComponent<GameplayInfo>().selectedCards)
                    {   
                        card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * -50;
                        card.GetComponent<Card>().selected = false;
                    }
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount = 0;
                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    StartCoroutine(DissolveEnum());               
                }
                //+4 szorzó
                if (nameOfCard == "SZERETŐK")
                {
                    GameObject playingCardGroup = GameObject.Find("PlayingCardGroup");
                    foreach (GameObject card in Gameplayinfo.selectedCards)
                    {
                        card.GetComponent<Card>().edition = "POLYCHROME";
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image = card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().m = new Material(card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material);
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material = card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().m;
                        for (int j = 0; j < card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.enabledKeywords.Length; j++)
                        {
                            card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.DisableKeyword(card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.enabledKeywords[j]);
                        }
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.EnableKeyword($"_EDITION_{card.GetComponent<Card>().edition}");

                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict[$"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank};{card.GetComponent<Card>().cardID}"] = "POLYCHROME";
                        
                    }

                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";
                    foreach (GameObject card in Gameplayinfo.GetComponent<GameplayInfo>().selectedCards)
                    {   
                        card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * -50;
                        card.GetComponent<Card>().selected = false;
                    }
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount = 0;
                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    StartCoroutine(DissolveEnum());            
                }
                //+25 pont
                if (nameOfCard == "AZ URALKODÓ")
                {
                    GameObject playingCardGroup = GameObject.Find("PlayingCardGroup");
                    foreach (GameObject card in Gameplayinfo.selectedCards)
                    {
                        card.GetComponent<Card>().edition = "FOIL";
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image = card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().m = new Material(card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material);
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material = card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().m;
                        for (int j = 0; j < card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.enabledKeywords.Length; j++)
                        {
                            card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.DisableKeyword(card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.enabledKeywords[j]);
                        }
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.EnableKeyword($"_EDITION_{card.GetComponent<Card>().edition}");

                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict[$"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank};{card.GetComponent<Card>().cardID}"] = "FOIL";
                    }

                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";
                    foreach (GameObject card in Gameplayinfo.GetComponent<GameplayInfo>().selectedCards)
                    {   
                        card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * -50;
                        card.GetComponent<Card>().selected = false;
                    }
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount = 0;
                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    StartCoroutine(DissolveEnum());                
                }
                //NEGATÍV
                if (nameOfCard == "ERŐ")
                {
                    GameObject playingCardGroup = GameObject.Find("PlayingCardGroup");
                    foreach (GameObject card in Gameplayinfo.selectedCards)
                    {
                        card.GetComponent<Card>().edition = "NEGATIVE";
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image = card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().m = new Material(card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material);
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material = card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().m;
                        for (int j = 0; j < card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.enabledKeywords.Length; j++)
                        {
                            card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.DisableKeyword(card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.enabledKeywords[j]);
                        }
                        card.GetComponent<Card>().cardVisualObj.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<ShaderCode>().image.material.EnableKeyword($"_EDITION_{card.GetComponent<Card>().edition}");

                        playingCardGroup.GetComponent<HorizontalCardHolder>().fullDeckDict[$"{card.GetComponent<Card>().suit} {card.GetComponent<Card>().rank};{card.GetComponent<Card>().cardID}"] = "NEGATIVE";
                        GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().cardsInHandCount -= 1;
                    }

                    GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"";
                    foreach (GameObject card in Gameplayinfo.GetComponent<GameplayInfo>().selectedCards)
                    {   
                        card.transform.localPosition += card.GetComponent<Card>().cardVisual.transform.up * -50;
                        card.GetComponent<Card>().selected = false;
                    }
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Clear();
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount = 0;
                    BuySellButton.SetActive(false);
                    BuySellPriceText.SetActive(false);
                    BuySellText.SetActive(false);
                    StartCoroutine(DissolveEnum());
                    //Destroy(gameObject);                
                }
            }
            else if (BuySellText.GetComponent<TMP_Text>().text == "SELL")
            {   
                GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
                gameHandler.SellTarot();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        
        GameObject Gamehandler = GameObject.Find("GameHandler");
        Gameplayinfo = GameObject.Find("GameplayInfo").GetComponent<GameplayInfo>();
        RoomBrowser room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;
        touchBlock = Gamehandler.GetComponent<GameHandler>().touchBlock;
        image = transform.GetChild(1).GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
        //StartCoroutine(DissolveEnum());
    }

    // Update is called once per frame

    public IEnumerator DissolveEnum()
    {   
        while (dissolveAmount > 0)
        {
            touchBlock.SetActive(true);
            dissolveAmount -= Time.deltaTime * 2;
            material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
        touchBlock.SetActive(false);
        GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
        Destroy(gameObject);
    }

    public IEnumerator DissolveExternalEnum()
    {   
        image = transform.GetChild(0).GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
        float dissolveAmount = 1;
        while (dissolveAmount > 0)
        {
            dissolveAmount -= Time.deltaTime;
            material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
    }

    public IEnumerator CondenseEnum()
    {   
        float condenseAmount = 0;
        image = transform.GetChild(0).GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
        while (condenseAmount < 1)
        {
            //touchBlock.SetActive(true);
            condenseAmount += Time.deltaTime;
            material.SetFloat("_DissolveAmount", condenseAmount);
            yield return null;
        }
        //touchBlock.SetActive(false);
        //GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards.Remove(nameOfCard);
        //Destroy(gameObject);
    }

    void Update()
    {   
        if (!isOwned)
        {
            if (selected && moved)
            {
                //transform.localPosition += transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(this.gameObject, 0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, -0.7f * transform.localScale.x, touchBlock, 10f, false));
                BuySellButton.GetComponent<Image>().color = Color.green;
                BuySellText.GetComponent<TMP_Text>().text = "BUY";
                BuySellButton.GetComponent<Selectable>().interactable = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().money >= price && !(GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().gameMaxJokers >= 10 && nameOfCard == "REMETE") ? true : false;
                BuySellPriceText.GetComponent<TMP_Text>().text = $"{price}€";
                moved = false;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = cardName;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = cardDescription;
                //gameObject.transform.GetChild(0).gameObject.SetActive(true);
                //Gameplayinfo.selectedJokerCount++;
                wasSelected = true;
                jokerDescription = GameObject.Find("JOKER DESCRIPTION");
                jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo._tarotCardDescriptionSimple[nameOfCard]}";
            }
            else if (!selected && moved && wasSelected)
            {   
                StartCoroutine(Utility.SmoothMovement(this.gameObject, -0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, 0.7f * transform.localScale.x, touchBlock, 10f, false));
                moved = false;
                //Gameplayinfo.selectedJokerCount--;
                wasSelected = false;
                if (!deselectedByOtherCard) jokerDescription.GetComponent<TMP_Text>().text = "";
                //gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {   
            otherPlayerPlaysOrDiscards = false;

            foreach (User user in GameObject.Find("GameHandler").GetComponent<GameHandler>().users)
            {
                GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                if (currentPlayer.GetComponent<Player>().isPlaying || currentPlayer.GetComponent<Player>().isDiscarding)
                {
                    otherPlayerPlaysOrDiscards = true;
                    break;
                } 
                
            }

            if (otherPlayerPlaysOrDiscards || (GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().gameMaxJokers >= 10 && nameOfCard == "REMETE")) BuySellButton.GetComponent<Selectable>().interactable = false;
            else if ((nameOfCard == "AZ URALKODÓNŐ" && Gameplayinfo.selectedHandCount == 1) || (nameOfCard == "A BOLOND" && Gameplayinfo.selectedHandCount > 0 && Gameplayinfo.selectedHandCount < 3) || (nameOfCard == "SZERETŐK" && Gameplayinfo.selectedHandCount > 0 && Gameplayinfo.selectedHandCount < 3) || (nameOfCard == "AZ URALKODÓ" && Gameplayinfo.selectedHandCount > 0 && Gameplayinfo.selectedHandCount < 3) || (nameOfCard == "ERŐ" && Gameplayinfo.selectedHandCount > 0 && Gameplayinfo.selectedHandCount < 3) || nameOfCard == "A FŐPAPNŐ" || nameOfCard == "A MÁGUS" || transform.parent.name == "OWNED TAROT CARD HOLDER")
            {
                BuySellButton.GetComponent<Selectable>().interactable = true;
            }
            else
            {
                BuySellButton.GetComponent<Selectable>().interactable = false;
            }

            if (selected && moved)
            {
                //transform.localPosition += transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(this.gameObject, 0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, -0.7f * transform.localScale.x, touchBlock, 10f, false));
                
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = cardName;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = cardDescription;
                //gameObject.transform.GetChild(0).gameObject.SetActive(true);
                moved = false;
                //Gameplayinfo.selectedOwnedJokerCount++;
                wasSelected = true;
                jokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
                if (jokerDescription != null) 
                {
                    jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo._tarotCardDescriptionSimple[nameOfCard]}";
                    BuySellButton.GetComponent<Image>().color = Color.red;
                    BuySellText.GetComponent<TMP_Text>().text = "SELL";
                    BuySellText.GetComponent<TMP_Text>().color = Color.white;
                    BuySellPriceText.GetComponent<TMP_Text>().text = $"{Mathf.Ceil((float)Gameplayinfo._tarotCardPricesSimple[nameOfCard] / 2)}€";
                    BuySellPriceText.GetComponent<TMP_Text>().color = Color.white;
                }            
                else
                {
                    jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo._tarotCardDescriptionSimple[nameOfCard]}";
                    //Player player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>();
                    
                    BuySellButton.GetComponent<Image>().color = Color.blue;
                    BuySellText.GetComponent<TMP_Text>().text = "USE";
                    BuySellText.GetComponent<TMP_Text>().color = Color.white;
                    BuySellPriceText.GetComponent<TMP_Text>().text = $"";
                }
                
            }
            else if (!selected && moved && wasSelected)
            {   
                //transform.localPosition -= transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(this.gameObject, -0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, 0.7f * transform.localScale.x, touchBlock, 10f, false));
                //gameObject.transform.GetChild(0).gameObject.SetActive(false);
                moved = false;
                //Gameplayinfo.selectedOwnedJokerCount--;
                wasSelected = false;
                jokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
                if (jokerDescription == null) jokerDescription = GameObject.Find("INGAME OWNED TAROT CARD DESCRIPTION");
                if (!deselectedByOtherCard) jokerDescription.GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
