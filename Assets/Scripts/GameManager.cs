using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    [SerializeField] private HashSet<Transform> aliveCharacters = new HashSet<Transform>(); // Use HashSet for better performance
    [SerializeField] private Dictionary<Transform, Transform> playerTargets = new Dictionary<Transform, Transform>(); // Track player-target pairs

    [Header("UI References")]
    [SerializeField] private GameObject winnerCanvas; // UI canvas for the winner
    [SerializeField] private TMPro.TMP_Text winnerText; // Text UI to display the winner's name

    private void Awake()
    {
        // Ensure the singleton instance is properly set up
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        winnerCanvas.SetActive(false);
    }

    // Register a new player
    public void RegisterCharacter(Transform character)
    {
        if (!aliveCharacters.Contains(character))
        {
            aliveCharacters.Add(character); // Add the character to the alive list
            playerTargets[character] = null; // No target initially
        }
    }

    // Deregister a player when they die
    public void DeregisterCharacter(Transform character)
    {
        if (aliveCharacters.Remove(character)) // Remove from alive characters
        {
            playerTargets.Remove(character); // Remove their target if any

            // Check if only one player remains
            if (aliveCharacters.Count == 1)
            {
                DisplayVictory(aliveCharacters.FirstOrDefault()?.name); // Display the winner
            }
        }
    }

    // Set the target for a specific player
    public void SetTarget(Transform player, Transform target)
    {
        if (player != null)
        {
            playerTargets[player] = target;
        }
    }

    // Get the target of a player
    public Transform GetTarget(Transform player)
    {
        if (playerTargets.ContainsKey(player))
        {
            return playerTargets[player];
        }
        return null; // No target assigned
    }

    // Get a random target for players (you can use this for AI targeting logic)
    public Transform GetRandomTarget()
    {
        if (aliveCharacters.Count > 0)
        {
            // Randomly return one of the alive characters as a target
            return aliveCharacters.ElementAt(Random.Range(0, aliveCharacters.Count)); // Random target
        }
        return null; // No available targets
    }

    // Get a list of all alive characters
    public List<Transform> GetAliveCharacters()
    {
        // Return a new list with all currently alive players (excluding inactive players)
        Debug.Log("Alive Players: " + string.Join(", ", aliveCharacters.Select(player => player.name).ToArray()));
        return aliveCharacters.Where(player => player.gameObject.activeSelf).ToList(); // Only include active players

        


    }

    // Display the winner and show the winner canvas
    private void DisplayVictory(string winnerName)
    {
        Debug.Log($"Winner: {winnerName}");

        winnerCanvas.SetActive(true);

        if (winnerCanvas != null && winnerText != null)
        {
            winnerCanvas.SetActive(true); // Show the winner canvas
            winnerText.text = $"{winnerName} is the Winner!"; // Display the winner's name
        }
    }
}
