using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    List<Players> players;
    List<Player> playersInGame;

    public GameObject playerPrefab;
    public GameObject npcPrefab;


    struct Players
{
        public GameObject Player;
        public string name;
        public int currency;
        public bool isAI;
};

    // Start is called before the first frame update
    void Start()
    {
        createPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void createPlayers()
    {

        //Players player;
        //player.Player = new Player();
        //player.name = "Player";
        //player.currency = 100;
        //player.isAI = false;
        //players.Add(player);

        //Players npc1;
        //npc1.Player = npcPrefab;
        //npc1.name = "NPC1";
        //npc1.currency = 100;
        //npc1.isAI = false;
        //players.Add(npc1);

    }

    #endregion

    #region Events
    public class StartGameEvent
    {
        public PlayerInfo eventSystem;
        public List<Player> players;

        public StartGameEvent(List<Player> newPlayers)
        {
            players = newPlayers;
        }
    }

    public static event System.EventHandler<StartGameEvent> StartGame;

    public void SendStartGame(List<Player> players)
    {
            StartGame?.Invoke(this, new StartGameEvent(players));
    }



    public class SendPlayerInfoEvent
    {
        public Player player;
        public string playerName;
        public int playerCurrency;
        public bool playerIsAI;

        public SendPlayerInfoEvent(Player newPlayer, string newName, int newCurrency, bool isAI)
        {
            player = newPlayer;
            playerName = newName;
            playerCurrency = newCurrency;
            playerIsAI = isAI;
        }
    }

    public static event System.EventHandler<SendPlayerInfoEvent> SendPlayer;

    public void SendPlayerInfo(Player player, string name, int currency, bool isAI)
    {
        SendPlayer?.Invoke(this, new SendPlayerInfoEvent(player, name, currency, isAI));
    }



    public void Player_Ask(object sender, Player.AskForInfo args) 
    {
        //loop through the players list and return the senders info
    }


    #endregion
}
