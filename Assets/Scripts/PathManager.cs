using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointPath
{
    public string pathName; // e.g., "A to B" for easy identification
    public NodeType startNode;
    public NodeType endNode;
    public List<Transform> waypoints;
}

public class PathManager : MonoBehaviour
{
    [Header("References")]
    public GameObject packetPrefab;

    [Header("All Possible Journeys")]
    public List<WaypointPath> allPaths;

    public void StartPacketJourney(NodeType start, List<NodeType> destinations, TaskData taskData)
    {
        if (packetPrefab == null) return;

        foreach (var dest in destinations)
        {
            WaypointPath pathToFollow = FindPath(start, dest);
            if (pathToFollow != null && pathToFollow.waypoints.Count > 0)
            {
                GameObject packetGO = Instantiate(packetPrefab, pathToFollow.waypoints[0].position, pathToFollow.waypoints[0].rotation);

                // --- NEW LOGIC ---
                // Find the display script on the new packet
                PacketDataDisplay display = packetGO.GetComponent<PacketDataDisplay>();
                if (display != null)
                {
                    // Initialize it with the task data and this specific journey's nodes
                    display.InitializeDisplay(taskData, start, new List<NodeType> { dest });
                }
                // --- END NEW LOGIC ---

                packetGO.AddComponent<PathFollower>().FollowPath(pathToFollow.waypoints);
            }
            else
            {
                Debug.LogWarning($"No path found in PathManager from node {start} to {dest}. Please define it.");
            }
        }
    }

    private WaypointPath FindPath(NodeType start, NodeType end)
    {
        foreach (var path in allPaths)
        {
            if (path.startNode == start && path.endNode == end)
            {
                return path;
            }
        }
        return null;
    }
}