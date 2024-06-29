using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Alteruna
{
    public class Lobby : AttributesSync
    {   

        private Multiplayer multiplayer;
        private Room room;
        int currentUserCount = 0;
        [SerializeField] GameObject playerRow;
        [SerializeField] GameObject rows;
        [SerializeField] GameObject PlayerObject;
        [SerializeField] GameObject gameHandler;
        [SerializeField] GameObject inviteCode;
        List<User> users;
        int playerID;
        GameplayInfo Gameplayinfo;
        bool imReady = false;
        GameObject readyButton;
        GameObject startgameButton;
        GameObject myPlayer;

        private int readyCount = 0;
        private bool joined = false;                

        // Start is called before the first frame update
        void Start()
        {
            multiplayer = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>().Multiplayer;
            room = multiplayer.CurrentRoom;
            rows = GameObject.Find("RowsContainer");
            Gameplayinfo = GameObject.Find("GameplayInfo").GetComponent<GameplayInfo>();
            startgameButton = GameObject.Find("Start Game Button");
            startgameButton.SetActive(false);
        }

        // Update is called once per frame

        public void ReadyButton()
        {   
            myPlayer.GetComponent<Player>().Ready = !myPlayer.GetComponent<Player>().Ready;
            imReady = !imReady;
        }

        // [SynchronizableMethod]
        // public void SendOutGameInfo(int maxPlayers, int startingDiscards, int startingHands, int timeLimit, int rounds, int maxJokers)
        // {
        //     myPlayer.GetComponent<Player>().gameMaxPlayers = maxPlayers;
        //     myPlayer.GetComponent<Player>().maxDiscards = startingDiscards;
        //     myPlayer.GetComponent<Player>().maxHands = startingHands;
        //     myPlayer.GetComponent<Player>().TimeLimit = timeLimit;
        //     myPlayer.GetComponent<Player>().gameRounds = rounds;
        //     myPlayer.GetComponent<Player>().gameMaxJokers = maxJokers;
        // }

        public void StartButton()
        {  
            inviteCode.SetActive(false);
            GameObject.Find($"Playerinfo ({multiplayer.Me.Name})").GetComponent<Player>().StartGame = true;
        }

        public void StartCheck()
        {
            foreach (User user in users)
            {   
                GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                if (user.IsHost && currentPlayer.GetComponent<Player>().StartGame)
                {   
                    //multiplayer.LockRoom();
                    multiplayer.LockRoom();
                    myPlayer.GetComponent<Player>().Ready = false;
                    PlayerObject.SetActive(true);
                    transform.gameObject.SetActive(false);
                    gameHandler.SetActive(true);
                }  
            }
        }

        public void CheckingReady()
        {   
            int localReadyCount = 0;
            foreach (User user in users)
            {   
                foreach (Transform row in rows.transform)
                {   
                    if (row.name == user.Name)
                    foreach (Transform child in row)
                    {   
                        if (child.name == "READY") 
                        {   
                            GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                            if (currentPlayer != null && currentPlayer.GetComponent<Player>().Ready) 
                            {
                                localReadyCount++;
                                child.GetChild(0).GetComponent<TMP_Text>().text = "READY";
                            }
                            else child.GetChild(0).GetComponent<TMP_Text>().text = "";
                        }
                    }
                }
            }
            readyCount = localReadyCount;
        }

        void Update()
        {            

            multiplayer = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>().Multiplayer;
            room = multiplayer.CurrentRoom;
            Gameplayinfo = GameObject.Find("GameplayInfo").GetComponent<GameplayInfo>();
               
            
            if (multiplayer != null && room != null)
            {   

                if (!joined)
                {
                    foreach (User user in room.Users)
                    {
                        if (user.Index != multiplayer.Me.Index && user.Name == multiplayer.Me.Name)
                        {
                            room.Leave();
                            transform.gameObject.SetActive(false);
                            return;
                        }
                    }

                    myPlayer = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
                    
                    if (multiplayer.Me.IsHost && myPlayer != null)
                    {
                        myPlayer.GetComponent<Player>().maxHands = Gameplayinfo.GetComponent<GameplayInfo>().maxHands;
                        myPlayer.GetComponent<Player>().maxDiscards = Gameplayinfo.GetComponent<GameplayInfo>().maxDiscards;
                        myPlayer.GetComponent<Player>().gameMaxPlayers = Gameplayinfo.GetComponent<GameplayInfo>().gameMaxPlayers;
                        myPlayer.GetComponent<Player>().TimeLimit = Gameplayinfo.GetComponent<GameplayInfo>().TimeLimit;
                        myPlayer.GetComponent<Player>().gameRounds = Gameplayinfo.GetComponent<GameplayInfo>().gameRounds;
                        myPlayer.GetComponent<Player>().gameMaxJokers = Gameplayinfo.GetComponent<GameplayInfo>().gameMaxJokers;
                        myPlayer.GetComponent<Player>().gameMoneyMult = Gameplayinfo.GetComponent<GameplayInfo>().gameMoneyMult;
                        myPlayer.GetComponent<Player>().gameJokerChance = Gameplayinfo.GetComponent<GameplayInfo>().gameJokerChance;
                    }
                    
                    joined = true;
                }

                readyButton = GameObject.Find("Ready Button");

                users = room.Users;

                myPlayer = GameObject.Find($"Playerinfo ({multiplayer.Me.Name})");
                myPlayer.GetComponent<Player>().Username = multiplayer.Me.Name;
                myPlayer.GetComponent<Player>().playerID = multiplayer.Me.Index + 1;

                if (myPlayer != null && !multiplayer.Me.IsHost)
                    {
                        foreach (User user in room.Users)
                        {
                            if (user.IsHost)
                            {
                                Player hostPlayer = GameObject.Find($"Playerinfo ({user.Name})").GetComponent<Player>();
                                myPlayer.GetComponent<Player>().maxHands = hostPlayer.maxHands;
                                myPlayer.GetComponent<Player>().maxDiscards = hostPlayer.maxDiscards;
                                myPlayer.GetComponent<Player>().gameMaxPlayers = hostPlayer.gameMaxPlayers;
                                myPlayer.GetComponent<Player>().TimeLimit = hostPlayer.TimeLimit;
                                myPlayer.GetComponent<Player>().gameRounds = hostPlayer.gameRounds;
                                myPlayer.GetComponent<Player>().gameMaxJokers = hostPlayer.gameMaxJokers;
                                myPlayer.GetComponent<Player>().gameMoneyMult = hostPlayer.gameMoneyMult;
                                myPlayer.GetComponent<Player>().gameJokerChance = hostPlayer.gameJokerChance;
                                break;
                            }
                        }
                    }

                if (!multiplayer.Me.IsHost && startgameButton.activeSelf)
                {  
                    startgameButton.SetActive(false);
                }
                else if (multiplayer.Me.IsHost && !startgameButton.activeSelf)
                {
                    startgameButton.SetActive(true);
                }

                //if (multiplayer.Me.IsHost && readyCount == users.Count && users.Count > 1)
                if (multiplayer.Me.IsHost && readyCount == users.Count)
                {   
                    startgameButton.GetComponent<Button>().interactable = true;
                }
                else if (multiplayer.Me.IsHost)
                {
                    startgameButton.GetComponent<Button>().interactable = false;
                }
   
                List<User> usersSortedList = users.OrderBy(o=>o.Index).ToList();
                
                if (users.Count != currentUserCount)
                {   
                    //if (multiplayer.Me.IsHost) BroadcastRemoteMethod("SendOutGameInfo", myPlayer.GetComponent<Player>().gameMaxPlayers, myPlayer.GetComponent<Player>().maxDiscards, myPlayer.GetComponent<Player>().maxHands, myPlayer.GetComponent<Player>().TimeLimit, myPlayer.GetComponent<Player>().gameRounds, myPlayer.GetComponent<Player>().gameMaxJokers);

                    rows = GameObject.Find("RowsContainer");
                    currentUserCount = 0;
                    int count = 1;

                    foreach (Transform child in rows.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    foreach (User user in usersSortedList)
                    {   
                        GameObject currentPlayer = GameObject.Find($"Playerinfo ({user.Name})");
                        GameObject clone = Instantiate(playerRow);
                        clone.name = user.Name;
                        clone.transform.SetParent(rows.transform);
                        clone.transform.localScale = new Vector3(1, 1, 1);
                        foreach (Transform child in clone.transform)
                        {   
                            if (child.name == "Player Name") child.GetChild(0).transform.GetComponent<TMP_Text>().text = user.Name;
                            if (child.name == "Player ID") child.GetChild(0).transform.GetComponent<TMP_Text>().text = (user.Index + 1).ToString();
                            if (child.name == "READY") child.GetChild(0).transform.GetComponent<TMP_Text>().text = "";
                        }
                        currentUserCount++;
                        count++;
                    }
                }

                CheckingReady();
                StartCheck();
            }   
        }
    }
}
