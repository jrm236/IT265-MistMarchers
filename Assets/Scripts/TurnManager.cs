using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [Header("Current Turn Data")]
    public PlayerID currentPlayer = PlayerID.Player1;

    [Header("Action Points Data")]
    public int currentActionPoints;
    public int maxActionPoints = 3;

    [Header("Turn Info")]
    public int turnNumber = 1;
    public int maxTurns = 20;

    private void Start()
    {
        startTurn();
    }

    private void startTurn()
    {
        currentActionPoints = maxActionPoints;

        Debug.Log(currentPlayer + " turn started. AP: " + currentActionPoints);
    }

    public bool IsPlayerTurn(PlayerID player)
    {
        return currentPlayer == player;
    }

    public bool canSpendActionPoints(int amount)
    {
        return currentActionPoints >= amount;
    }

    public bool spendActionPoints(int amount)
    {
        if (!canSpendActionPoints(amount))
        {
            Debug.Log(currentPlayer + " does not have enough AP to spend.");
            return false;
        }

        currentActionPoints -= amount;
        Debug.Log(currentPlayer + " spent " + amount + " AP. There is: " + currentActionPoints + " AP remaining.");

        return true;
    }

    public void EndTurn()
    {
        if (currentPlayer == PlayerID.Player1)
        {
            currentPlayer = PlayerID.Player2;
        }
        else
        {
            currentPlayer = PlayerID.Player1;
            turnNumber++;
        }

        startTurn();
    }   
}
