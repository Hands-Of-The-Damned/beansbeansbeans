using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class StartGameEvent
    {
        public List<Player> players;

        public StartGameEvent(List<Player> newPlayers)
        {
            players = newPlayers;
        }

        public static event System.EventHandler<StartGameEvent> StartGame;

        public void SendStartGame(List<Player> players)
        {
            StartGame?.Invoke(this, new StartGameEvent(players));
        }
    }

}
