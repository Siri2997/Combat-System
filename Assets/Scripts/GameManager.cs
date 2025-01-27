using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<Transform> aliveCharacters = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterCharacter(Transform character)
    {
        aliveCharacters.Add(character);
    }

    public void DeregisterCharacter(Transform character)
    {
        aliveCharacters.Remove(character);

        // Check for victory condition
        if (aliveCharacters.Count == 1)
        {
            DisplayVictory(aliveCharacters[0].name);
        }
    }

    public List<Transform> GetAliveCharacters()
    {
        return aliveCharacters.FindAll(c => c.gameObject.activeSelf);
    }

    private void DisplayVictory(string winnerName)
    {
        Debug.Log($"Winner: {winnerName}");
        // Here UI logic to display the winner
    }
}
