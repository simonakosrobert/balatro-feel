using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CountyCard : MonoBehaviour, IPointerClickHandler
{   
    public string nameOfCard = null;
    public int price;
    public string description;
    public bool selected = false;
    public bool wasSelected = false;
    public bool moved = false;
    public bool isOwned = false;
    public bool deselectedByOtherCard = false;
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
    public void OnPointerClick(PointerEventData eventData)
    {   
        //Checks if the player plays a hand so they can't select the joker or tarot cards
        if (GameObject.Find("GameHandler").GetComponent<GameHandler>().Playing || eventData.pointerPress.gameObject.GetComponent<Selectable>().interactable == false) return;

        if (!selected && eventData.selectedObject.name == $"{nameOfCard}")
        {   
            if (isOwned)
            {
                foreach (string joker in GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().currentJokerCards)
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
            gameHandler.BuyCounty();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Gameplayinfo = GameObject.Find("GameplayInfo");
        GameObject Gamehandler = GameObject.Find("GameHandler");
        Gameplayinfo = GameObject.Find("GameplayInfo").GetComponent<GameplayInfo>();
        RoomBrowser room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        multiplayer = room.Multiplayer;
        touchBlock = Gamehandler.GetComponent<GameHandler>().touchBlock;
        image = transform.GetChild(1).GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
    }

    public IEnumerator DissolveEnum()
    {   
        touchBlock.SetActive(true);
        while (dissolveAmount > 0)
        {
            dissolveAmount -= Time.deltaTime * 2;
            material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
        touchBlock.SetActive(false);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOwned)
        {
            if (selected && moved)
            {
                StartCoroutine(Utility.SmoothMovement(this.gameObject, 0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, -0.7f * transform.localScale.x, touchBlock, 10f, false));
                BuySellButton.GetComponent<Image>().color = Color.green;
                BuySellText.GetComponent<TMP_Text>().text = "BUY";
                BuySellButton.GetComponent<Selectable>().interactable = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().money >= price ? true : false;
                BuySellPriceText.GetComponent<TMP_Text>().text = $"{price}€";
                moved = false;

                wasSelected = true;
                jokerDescription = GameObject.Find("JOKER DESCRIPTION");
                Player player = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>();
                Dictionary<string, string> countyToHand = Gameplayinfo._countyCardNameToHandNameDict;
                int origScore = player.handScoresDict[$"{countyToHand[nameOfCard]} SCORE"];
                int origMult = player.handScoresDict[$"{countyToHand[nameOfCard]} MULT"];
                int scoreAdd = Gameplayinfo.GetComponent<GameplayInfo>()._countyCardScoreMultDict[$"{nameOfCard} SCORE"];
                int multAdd = Gameplayinfo.GetComponent<GameplayInfo>()._countyCardScoreMultDict[$"{nameOfCard} MULT"];
                jokerDescription.GetComponent<TMP_Text>().text = $"{Gameplayinfo._countyCardDescriptionSimple[nameOfCard]}\n<color=blue>{origScore}</color> X <color=red>{origMult}</color> <color=#005500>>>></color> <color=#F000FF>{origScore + scoreAdd}</color> X <color=#7000FF>{origMult + multAdd}</color>";
            }
            else if (!selected && moved && wasSelected)
            {   
                StartCoroutine(Utility.SmoothMovement(this.gameObject, -0.5f * transform.localScale.x, touchBlock, 10f, false));
                StartCoroutine(Utility.SmoothMovement(BuySellButton, 0.7f * transform.localScale.x, touchBlock, 10f, false));
                moved = false;
                wasSelected = false;
                if (!deselectedByOtherCard) jokerDescription.GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
