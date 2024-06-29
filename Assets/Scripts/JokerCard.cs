using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class JokerCard : MonoBehaviour, IPointerClickHandler
{   

    public string suit;
    public string rank;
    public int price;
    public string description;
    public bool selected = false;
    public bool wasSelected = false;
    public bool deselectedByOtherCard = false;
    public bool moved = false;
    public bool isOwned = false;
    public string cardName = "";
    public string cardDescription = "";
    private GameObject Gameplayinfo;
    private GameObject jokerDescription;
    private Multiplayer multiplayer;
    [SerializeField] GameObject touchBlock;
    [SerializeField] GameObject BuySellButton;
    [SerializeField] GameObject BuySellText;
    [SerializeField] GameObject BuySellPriceText;
    public void OnPointerClick(PointerEventData eventData)
    {   
        //Checks if the player plays a hand so they can't select the joker cards
        if (GameObject.Find("GameHandler").GetComponent<GameHandler>().Playing) return;

        if (!selected && eventData.selectedObject.name == $"{suit} {rank}")
        {   
            if (isOwned)
            {
                foreach (string joker in GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentJokerCards)
                {   
                    GameObject currentJoker = GameObject.Find(joker);
                    if (currentJoker.GetComponent<JokerCard>().selected == true && (suit != currentJoker.GetComponent<JokerCard>().suit || rank != currentJoker.GetComponent<JokerCard>().rank))
                    {
                        currentJoker.GetComponent<JokerCard>().wasSelected = true;
                        currentJoker.GetComponent<JokerCard>().moved = true;
                        currentJoker.GetComponent<JokerCard>().selected = false;     
                        currentJoker.GetComponent<JokerCard>().deselectedByOtherCard = true;     
                    }
                }
                foreach (string tarot in GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentTarotCards)
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
                        if (currentCard.GetComponent<TarotCard>().selected == true)
                        {
                            currentCard.GetComponent<TarotCard>().wasSelected = true;
                            currentCard.GetComponent<TarotCard>().moved = true;
                            currentCard.GetComponent<TarotCard>().selected = false;
                            currentCard.GetComponent<TarotCard>().deselectedByOtherCard = true;
                        }
                    }
                    else if (currentCard.GetComponent<JokerCard>() != null)
                    {
                        if (currentCard.GetComponent<JokerCard>().selected == true && (suit != currentCard.GetComponent<JokerCard>().suit || rank != currentCard.GetComponent<JokerCard>().rank))
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

        else if (selected && eventData.selectedObject.name == $"{suit} {rank}")
        {   
            selected = false;
            moved = true;
            deselectedByOtherCard = false;
        }

        else if (!isOwned)
        {
            GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
            gameHandler.BuyJoker();
        }
        else if (isOwned)
        {
            GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
            gameHandler.SellJoker();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Gameplayinfo = GameObject.Find("GameplayInfo");
        GameObject Gamehandler = GameObject.Find("GameHandler");
        RoomBrowser room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;
        touchBlock = Gamehandler.GetComponent<GameHandler>().touchBlock;
    }

    // Update is called once per frame
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
                BuySellPriceText.GetComponent<TMP_Text>().text = $"{price}€";
                BuySellButton.SetActive(true);
                moved = false;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = cardName;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = cardDescription;
                //gameObject.transform.GetChild(0).gameObject.SetActive(true);
                //Gameplayinfo.GetComponent<GameplayInfo>().selectedJokerCount++;
                wasSelected = true;
                jokerDescription = GameObject.Find("JOKER DESCRIPTION");
                jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[rank]}\n{Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[suit]}";
            }
            else if (!selected && moved && wasSelected)
            {   
                StartCoroutine(Utility.SmoothMovement(this.gameObject, -0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, 0.7f * transform.localScale.x, touchBlock, 10f, false));
                moved = false;
                //Gameplayinfo.GetComponent<GameplayInfo>().selectedJokerCount--;
                wasSelected = false;
                if (!deselectedByOtherCard) jokerDescription.GetComponent<TMP_Text>().text = "";
                //gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (selected && moved)
            {
                //transform.localPosition += transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(this.gameObject, 0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, -0.7f * transform.localScale.x, touchBlock, 10f, false));
                BuySellButton.GetComponent<Image>().color = Color.red;
                BuySellText.GetComponent<TMP_Text>().text = "SELL";
                BuySellText.GetComponent<TMP_Text>().color = Color.white;
                BuySellPriceText.GetComponent<TMP_Text>().text = $"{Mathf.Ceil((float)Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardPricesSimple[rank] / 2)}€";
                BuySellPriceText.GetComponent<TMP_Text>().color = Color.white;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = cardName;
                //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = cardDescription;
                //gameObject.transform.GetChild(0).gameObject.SetActive(true);
                moved = false;
                //Gameplayinfo.GetComponent<GameplayInfo>().selectedOwnedJokerCount++;
                wasSelected = true;
                jokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
                if (jokerDescription != null) jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[rank]}\n{Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[suit]}";

                else if (jokerDescription == null) 
                {   
                    BuySellButton.SetActive(false);
                    jokerDescription = GameObject.Find("INGAME OWNED JOKER CARD DESCRIPTION");
                    jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[rank]}\n{Gameplayinfo.GetComponent<GameplayInfo>()._jokerCardDescriptionSimple[suit]}";
                }
                
            }
            else if (!selected && moved && wasSelected)
            {   
                //transform.localPosition -= transform.up * 50;
                StartCoroutine(Utility.SmoothMovement(this.gameObject, -0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, 0.7f * transform.localScale.x, touchBlock, 10f, false));
                //gameObject.transform.GetChild(0).gameObject.SetActive(false);
                moved = false;
                //Gameplayinfo.GetComponent<GameplayInfo>().selectedOwnedJokerCount--;
                wasSelected = false;
                jokerDescription = GameObject.Find("OWNED JOKER DESCRIPTION");
                if (jokerDescription == null) jokerDescription = GameObject.Find("INGAME OWNED JOKER CARD DESCRIPTION");
                if (!deselectedByOtherCard) jokerDescription.GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
