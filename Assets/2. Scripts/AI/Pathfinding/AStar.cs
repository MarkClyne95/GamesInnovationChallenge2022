using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] GameObject startNode;
    [SerializeField] GameObject endNode;
    GameObject[] nodes;

    private void Awake()
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");
    }
    void FindPath(Vector3 start, Vector3 end)
    {
        while(nodes.Length > 0)
        {
            GameObject currentNode = nodes[0];
            for(int t = 1; t < nodes.Length; t++)
            {
                
            }
        }
    }
}
