using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Setup : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         agent.destination = target.position;
    }
}
