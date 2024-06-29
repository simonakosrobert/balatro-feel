using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class MultiplayerManager : AttributesSync
{   
    private Alteruna.Avatar avatar;
    private RoomBrowser room;
    [SynchronizableField] public static string Gecc;
    private GameObject GameCanvas;
    private GameObject Player;
    private bool foundRoom = false;

    void Start()
    {
        GameCanvas = GameObject.Find("Game Canvas");
    }

    // Update is called once per frame
    void Update()
    {   
        
        room = GameObject.Find("Room Browser Window").GetComponent<RoomBrowser>();

        if (room.Multiplayer.InRoom && !foundRoom)
        {   
            Player = GameObject.Find($"PLAYER ({Multiplayer.Me.Name})");
            Player.transform.SetParent(GameCanvas.transform, false);
            avatar = Player.GetComponent<Alteruna.Avatar>();
            if (!avatar.IsMe)
                return;
            Gecc = Multiplayer.Me.Name;
            foundRoom = true;
        }
    }
}
