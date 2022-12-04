using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMoney : MonoBehaviour
{

    public GameObject bronze;
    public GameObject silver;
    public GameObject gold;

    public Player Player;

    private int bronzeTracker;
    private int silverTracker;
    private int goldTracker;
    // Start is called before the first frame update
    
    public void spawnBronze()
    {
        Instantiate(bronze, transform.position, transform.rotation);
        bronzeTracker += 1;
        
    }

    public void spawnSilver()
    {
        Instantiate(silver, transform.position, transform.rotation);
        silverTracker += 1;
        
    }

    public void spawnGold()
    {
        Instantiate(gold, transform.position, transform.rotation);
        goldTracker += 1;
        
    }
}
