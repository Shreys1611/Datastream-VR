using UnityEngine;

public class PacketDestinationTrigger : MonoBehaviour
{
    public NodeType myNode;
    private ControlPanelManager controlPanel;

    void Start()
    {
        // --- THIS IS THE UPDATED LINE ---
        // Use FindFirstObjectByType instead of the obsolete FindObjectOfType
        controlPanel = FindFirstObjectByType<ControlPanelManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        PathFollower packet = other.GetComponent<PathFollower>();
        if (packet != null)
        {
            if (controlPanel != null)
            {
                controlPanel.OnPacketArrivedAtNode(myNode);
            }
            else
            {
                // Add a check in case the manager isn't found
                Debug.LogError("PacketDestinationTrigger could not find the ControlPanelManager in the scene!");
            }
            Destroy(other.gameObject);
        }
    }
}