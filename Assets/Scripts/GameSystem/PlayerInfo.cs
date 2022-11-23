using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerInfo : MonoBehaviour
{
    string playerFile;
    string npc1File;
    string npc2File;
    string npc3File;
    string npc4File;

    private void Awake()
    {
        playerFile = Application.dataPath + "/Scripts/Player Data/Player.txt";
        npc1File = Application.dataPath + "/Scripts/Player Data/NPC1.txt";
        npc2File = Application.dataPath + "/Scripts/Player Data/NPC2.txt";
        npc3File = Application.dataPath + "/Scripts/Player Data/NPC3.txt";
        npc4File = Application.dataPath + "/Scripts/Player Data/NPC4.txt";
    }

    [System.Serializable]
    public class playerData
    {
        public string playerName =null;
        public int currency = 0;
        public bool ai = false;
    }

    private void OnEnable()
    {
        Player.AskInfo += Player_Ask;
    }

    private void OnDisable()
    {
        Player.AskInfo -= Player_Ask;
    }

    #region Functions

    /// <summary>
    /// Check what file name to return
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string checkFile(string fileName)
    {
        switch (fileName) {
            case "player":
                return playerFile;

            case "npc1":
                return npc1File;

            case "npc2":
                return npc2File;

            case "npc3":
                return npc3File;

            case "npc4":
                return npc4File;


            default:
                return null;

        }

    }

    #endregion

    #region Events
    public class StartGameEvent
    {
        public PlayerInfo eventSystem;
        public List<GameObject> players;

        public StartGameEvent(List<GameObject> newPlayers)
        {
            players = newPlayers;
        }
    }

    public static event System.EventHandler<StartGameEvent> StartGame;

    public void SendStartGame(List<GameObject> players)
    {
            StartGame?.Invoke(this, new StartGameEvent(players));
    }



    public class SendPlayerInfoEvent
    {
        public GameObject player;
        public string playerName;
        public int playerCurrency;
        public bool playerIsAI;

        public SendPlayerInfoEvent(GameObject newPlayer, string newName, int newCurrency, bool isAI)
        {
            player = newPlayer;
            playerName = newName;
            playerCurrency = newCurrency;
            playerIsAI = isAI;
        }
    }

    public static event System.EventHandler<SendPlayerInfoEvent> SendPlayer;

    public void SendPlayerInfo(GameObject player, string name, int currency, bool isAI)
    {
        SendPlayer?.Invoke(this, new SendPlayerInfoEvent(player, name, currency, isAI));
    }



    public void Player_Ask(object sender, Player.AskForInfo args) 
    {
        string file = checkFile(args.file);
        string saveString = File.ReadAllText(file);
        playerData newPlayer;
        newPlayer = JsonUtility.FromJson<playerData>(saveString);
        SendPlayerInfo(args.player, newPlayer.playerName, newPlayer.currency, newPlayer.ai);
    }

    

    #endregion
}
