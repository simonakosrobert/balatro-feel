using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
public class PlayerScript : MonoBehaviour
{   

    private GameObject Gameplayinfo;
    [SerializeField] private GameObject PlayingCardGroup;
    [SerializeField] private GameObject PlayedHandGroup;
    [SerializeField] private GameObject GameUI;
    public int playerID;
    private Alteruna.Avatar avatar;
    private RoomBrowser room;

    // Start is called before the first frame update
    void Start()
    {   
        avatar = GetComponent<Alteruna.Avatar>();
        if (!avatar.IsMe) return;
        room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();
        Gameplayinfo = GameObject.Find("GameplayInfo");
        PlayingCardGroup = transform.Find("PlayingCardGroup").gameObject;
        PlayedHandGroup = transform.Find("PlayedHandGroup").gameObject;
        GameUI = transform.Find("gameUI").gameObject;

        Gameplayinfo.GetComponent<GameplayInfo>().playerCount += 1;

        playerID = Gameplayinfo.GetComponent<GameplayInfo>().playerCount;

        if (Gameplayinfo.GetComponent<GameplayInfo>().playerCount == 1)
        {
            Gameplayinfo.GetComponent<GameplayInfo>().currentPlayer = 1;
        }

        if (playerID == Gameplayinfo.GetComponent<GameplayInfo>().currentPlayer)
        {   
            PlayingCardGroup.SetActive(true);
            PlayedHandGroup.SetActive(true);
            GameUI.SetActive(true);
            Gameplayinfo.GetComponent<GameplayInfo>().currentPlayerName = room.Multiplayer.Me.Name;           
        }
        else
        {
            PlayingCardGroup.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {   
        transform.SetSiblingIndex(0);
    }
}
