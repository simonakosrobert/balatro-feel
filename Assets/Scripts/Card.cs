
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    [SerializeField] private GameObject Gameplayinfo;
    public VisualCardsHandler visualHandler;
    private Vector3 offset;
    
    [SerializeField] public string suit;
    [SerializeField] public string rank;
    [Header("Movement")]
    [SerializeField] private float moveSpeedLimit = 50;

    [Header("Selection")]
    public bool selected;
    public float selectionOffset = 25;
    private float pointerDownTime;
    private float pointerUpTime;
    [Header("Visual")]
    [SerializeField] public GameObject cardVisualPrefab;
    [HideInInspector] public CardVisual cardVisual;
    [HideInInspector] public GameObject cardVisualObj;
    [HideInInspector] public GameObject cardSlot;
    [HideInInspector] public GameObject Player;


    [Header("States")]
    public bool isHovering;
    public bool isDragging;
    public bool isDiscarded;
    public bool isPlayed;
    public bool isFlipped;
    public bool isScoring = false;
    public int PlayerID;
    public string cardID;
    public string edition;
    [HideInInspector] public bool wasDragged;

    [Header("Events")]
    [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
    [HideInInspector] public UnityEvent<Card> PointerExitEvent;
    [HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<Card> PointerDownEvent;
    [HideInInspector] public UnityEvent<Card> BeginDragEvent;
    [HideInInspector] public UnityEvent<Card> EndDragEvent;
    [HideInInspector] public UnityEvent<Card, bool> SelectEvent;

    void Start()
    {   
        // deck = Gameplayinfo.GetComponent<GameplayInfo>().pokerCardImageNames;
        // //transform.localPosition = new Vector3(1, 1, 1);
        // int randomCard = rnd.Next(deck.Count); 
        // string[] suitRank = deck[randomCard].Split(' ');
        // deck.RemoveAt(randomCard);
        // suit = suitRank[0];
        // rank = suitRank[1];
        Gameplayinfo = GameObject.Find("GameplayInfo");
        imageComponent = GetComponent<Image>();
        selectionOffset = 50;
        Player = GameObject.Find("PLAYER");
        PlayerID = Player.GetComponent<PlayerScript>().playerID;

        if (!instantiateVisual)
            return;

        // visualHandler = FindObjectOfType<VisualCardsHandler>();
        // cardSlot = transform.parent.gameObject;
        // cardVisualObj = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform);
        // cardVisual = cardVisualObj.GetComponent<CardVisual>();
        // //cardVisual.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        // cardVisual.Initialize(this);
        // cardVisualObj.GetComponent<CardVisual>().suit = suit;
        // cardVisualObj.GetComponent<CardVisual>().rank = rank;
    }

    void Update()
    {
        ClampPosition();

        if (isDragging)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    void ClampPosition()
    {   
        if (!isDiscarded && transform.parent.parent.parent.name != "OTHER PLAYERS CARDHOLDERS")
        {
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
        }  
    }

    public void OnBeginDrag(PointerEventData eventData)
    {   
        if (!isPlayed && transform.parent.parent.parent.name != "OTHER PLAYERS CARDHOLDERS")
        {
            BeginDragEvent.Invoke(this);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = mousePosition - (Vector2)transform.position;
            isDragging = true;
            canvas.GetComponent<GraphicRaycaster>().enabled = false;
            imageComponent.raycastTarget = false;

            wasDragged = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {   
        if (!isPlayed && transform.parent.parent.parent.name != "OTHER PLAYERS CARDHOLDERS")
            {
            EndDragEvent.Invoke(this);
            isDragging = false;
            canvas.GetComponent<GraphicRaycaster>().enabled = true;
            imageComponent.raycastTarget = true;

            StartCoroutine(FrameWait());

            IEnumerator FrameWait()
            {
                yield return new WaitForEndOfFrame();
                wasDragged = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {   
        if (!isPlayed && transform.parent.parent.parent.name != "OTHER PLAYERS CARDHOLDERS")
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            return;

            PointerDownEvent.Invoke(this);
            pointerDownTime = Time.time;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {   
        string parentName = transform.parent.parent.parent.name;
        if (parentName == "OTHER PLAYERS CARDHOLDERS" && GameObject.Find("GameHandler").GetComponent<GameHandler>().magicianActivated)
        {
            if (eventData.button != PointerEventData.InputButton.Left || GameObject.Find("GameHandler").GetComponent<GameHandler>().Player.GetComponent<Player>().Username == transform.parent.parent.name.Split(" ")[1])
            return;

            selected = !selected;
            SelectEvent.Invoke(this, selected);
            if (selected && Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCards < 1)
            {
                transform.localPosition += cardVisual.transform.up * selectionOffset;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCards++;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCardObj = transform.gameObject;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCardUsername = transform.parent.parent.name.Split(" ")[1];
            }
            else if (Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCards == 1 && selected)
            {
                selected = !selected;
                return;
            }
            else if (!selected)
            {
                transform.localPosition = Vector3.zero;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCards = 0;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCardObj = null;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedMagicianCardUsername = "";
            }
        }
        if (!isPlayed && !isDiscarded && parentName != "OTHER PLAYERS CARDHOLDERS" && !GameObject.Find("GameHandler").GetComponent<GameHandler>().Playing && !GameObject.Find("GameHandler").GetComponent<GameHandler>().Discarding)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            return;
            pointerUpTime = Time.time;

            PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);

            if (pointerUpTime - pointerDownTime > .2f)
                return;

            if (wasDragged)
                return;

            selected = !selected;
            SelectEvent.Invoke(this, selected);

            if (selected && Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount < 5)
            {
                transform.localPosition += cardVisual.transform.up * selectionOffset;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount++;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Add(transform.gameObject);
                if (Gameplayinfo.GetComponent<GameplayInfo>().orderByRank)
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards = Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.OrderByDescending(o=> Gameplayinfo.GetComponent<GameplayInfo>().rankOrderDict[o.GetComponent<Card>().rank]).ToList();
                else
                    Gameplayinfo.GetComponent<GameplayInfo>().selectedCards = Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.OrderBy(o=> Gameplayinfo.GetComponent<GameplayInfo>().suitOrderDict[o.GetComponent<Card>().suit]).ToList();
                //foreach(GameObject card in Gameplayinfo.GetComponent<GameplayInfo>().selectedCards) Debug.Log(card.GetComponent<Card>().suit + " " + card.GetComponent<Card>().rank);
                
            }
            else if (Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount == 5 && selected)
            {
                selected = !selected;
                return;
            }
            else if (!selected)
            {
                transform.localPosition = Vector3.zero;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedHandCount--;
                Gameplayinfo.GetComponent<GameplayInfo>().selectedCards.Remove(transform.gameObject);
            }
        }
    }

    public void Deselect()
    {
        if (selected && transform.parent.parent.parent.name != "OTHER PLAYERS CARDHOLDERS")
        {
            selected = false;
            if (selected)
                transform.localPosition += (cardVisual.transform.up * 50);
            else
                transform.localPosition = Vector3.zero;
        }
    }


    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }

    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    public float NormalizedPosition()
    {
        return transform.parent.CompareTag("Slot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
    }

    private void OnDestroy()
    {
        if(cardVisual != null)
        Destroy(cardVisual.gameObject);
    }
}
