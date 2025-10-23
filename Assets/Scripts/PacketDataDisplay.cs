// PacketDataDisplay.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic; // Needed for List

public class PacketDataDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI sourceIPText;
    public TextMeshProUGUI destIPText;
    public TextMeshProUGUI protocolText;
    public TextMeshProUGUI sourceNodeText;
    public TextMeshProUGUI destNodeText;

    // This is the main function that receives the data
    public void InitializeDisplay(TaskData data, NodeType userSourceNode, List<NodeType> userDestNodes)
    {
        if (data == null) return;

        // Populate the text fields with data from the task
        sourceIPText.text = $"SRC IP: {data.sourceIP}";
        destIPText.text = $"DEST IP: {string.Join(", ", data.destinationIPs)}";
        protocolText.text = $"PROTO: {string.Join(", ", data.correctProtocols)}";

        // Display the journey this specific packet is taking
        sourceNodeText.text = $"FROM: Node {userSourceNode}";
        destNodeText.text = $"TO: Node {string.Join(", ", userDestNodes)}";
    }
}