using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerGameData
{
    public GameObject Player { get; set; }
    public bool InRound { get; set; }
    public bool InGame { get; set; }
    public bool PlayedCurrentRound { get; set; }
    public bool HasBeenBigBlind { get; set; }
    public bool HasBeenSmallBlind { get; set; }
    public bool IsBigBlind { get; set; }
    public bool IsSmallBlind { get; set; }
    public int Currency { get; set; }
    public int[] HandRank { get; set; }
}
