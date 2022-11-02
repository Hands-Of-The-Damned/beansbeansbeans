using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEventSystem : MonoBehaviour
{
    /* EVENT 1
     * Betting Round Event
     * send current pot ammount, current bet ammount, current betting round, string of current players turn
     */

    /* EVENT 2
     * General Major Arcana
     * Probably different events
     * Tell players and GUI what major arcana was played and its effects, send effects to players and GUI
     */

    /* EVENT HANDLE    
     * Player Betting Round Event
     * receive bet amount, if player folded, if player played major arcana
     */

    /* EVENT HANDLE
     * Major Arcana Effects
     * receive effects: force player to fold for a round, deal cards to a player, force a bet increase
     * There will be others
     * May have to be separate events
     */

    /* EVENT HANDLE    
    * Game Start Event
    * receive an array of the players in the game to initialize the game
    */

}
