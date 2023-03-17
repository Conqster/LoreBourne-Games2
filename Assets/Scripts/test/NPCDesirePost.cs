using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne.AI;
using UnityEngine.AI;

public class NPCDesirePost : MonoBehaviour
{

    private Dictionary<Vector3, bool> postIsSafe = new Dictionary<Vector3, bool>();
    [SerializeField] private NPCPostCreation postCreator;
    [SerializeField] private bool checkForDesirePost, move; 
    private NavMeshAgent agent;
    [SerializeField] private List<Vector3> done = new List<Vector3>();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }



    private void Update()
    {

        if (!agent.pathPending)
            checkForDesirePost = true;

        if (move)
            SetAgentDestination();

        if (checkForDesirePost) 
            GetPost();
    }

    private void SetAgentDestination()
    {
        //Dictionary<Vector3, bool> done = new Dictionary<Vector3, bool>();
        bool destinationIsSet = false;
        Vector3 destination;
        List<Vector3> posts = new List<Vector3>();
        foreach(var safeNode in postIsSafe)
        {
            if(safeNode.Value == true)
            {
                posts.Add(safeNode.Key);
            }
        }

        //check for closest post
        if(posts.Count > 0)
        {
            while(!destinationIsSet)
            {
                destination = ClosetPost(agent.transform.position, posts);
                if (!done.Contains(destination) || posts.Count == 1)
                {
                    agent.SetDestination(destination);
                    destinationIsSet = true;
                    done.Add(destination);
                }
                else
                {
                    posts.Remove(destination);
                    destination = ClosetPost(agent.transform.position, posts);
                }
            }
        }
        move = false;
        
    }




    private void GetPost()
    {
        postIsSafe = postCreator.SafePost();
        checkForDesirePost = false;
    }


    private Vector3 ClosetPost(Vector3 agentPos, List<Vector3> posts)
    {

        Vector3 closestPost = new Vector3();
        //float closestDist = float.MaxValue;
        float minDistance = float.MaxValue;

        foreach(Vector3 position in posts)
        {
            float distance = Vector3.Distance(agentPos, position);
            if(minDistance > distance)
            {
                minDistance = distance;
                closestPost = position;
            }
        }

        return closestPost;
    }


}
