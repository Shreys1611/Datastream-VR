using UnityEngine;
using System.Collections.Generic;

// Define these enums outside the class so other scripts can access them.
public enum CastingType { Unicast, Multicast, Broadcast }
public enum NodeType { A, B, C }

[System.Serializable]
public class TaskData
{
    [Header("Display Information")]
    public string taskName;             // e.g., "Private Email"
    public string detailDescription;
    public string sourceIP;
    public List<string> destinationIPs;
    public string fileType;
    public string fileSize;

    [Header("Correct Answers")]
    public List<string> correctProtocols;
    public CastingType correctCastingType;
    public NodeType correctSourceNode;
    public List<NodeType> correctDestinationNodes;

    [Header("State")]
    public bool isCompleted = false;
}