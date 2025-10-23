using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [Header("Path")]
    public Transform startPoint;
    public Transform endPoint;


    [Header("Packet")]
    public GameObject packetPrefab; // should contain PacketController & XRGrabInteractable
    public int initialPacketCount = 3;

    [Header("Movement")]
    public float speed = 1f; // meters per second along the belt
    public bool running = true;


    [Header("Detection")]
    [Tooltip("Trigger collider that covers the belt area. Set IsTrigger = true on this collider.")]
    public Collider beltAreaTrigger;


    // internal
    private float pathLength = 1f;
    private readonly List<PacketController> packets = new List<PacketController>();

    void Awake()
    {
        if (startPoint == null || endPoint == null)
            Debug.LogError("ConveyorBelt requires startPoint and endPoint references.");


        pathLength = Vector3.Distance(startPoint.position, endPoint.position);
        if (pathLength <= Mathf.Epsilon)
            pathLength = 1f;
    }
    void Start()
    {
        //SpawnInitialPackets();
    }

    void Update()
    {
        if (!running) return;


        // iterate in reverse in case packets unregister themselves
        for (int i = packets.Count - 1; i >= 0; i--)
        {
            var p = packets[i];
            if (p == null)
            {
                packets.RemoveAt(i);
                continue;
            }


            if (p.isOnBelt && !p.isHeld)
            {
                // advance progress based on speed in meters/sec
                p.progress += (speed * Time.deltaTime) / pathLength;


                // wrap around
                if (p.progress >= 1f)
                    p.progress -= 1f;


                UpdatePacketTransform(p);
            }
        }
    }

    void SpawnInitialPackets()
    {
        if (packetPrefab == null) return;


        for (int i = 0; i < initialPacketCount; i++)
        {
            float normalizedPos = (float)i / initialPacketCount; // evenly spaced
            var go = Instantiate(packetPrefab, Vector3.Lerp(startPoint.position, endPoint.position, normalizedPos), Quaternion.identity);


            var pc = go.GetComponent<PacketController>();
            if (pc == null)
            {
                Debug.LogWarning("Packet prefab does not contain PacketController. Adding one automatically.");
                pc = go.AddComponent<PacketController>();
            }


            AttachPacket(pc, normalizedPos);
        }
    }

    void UpdatePacketTransform(PacketController p)
    {
        Vector3 pos = Vector3.Lerp(startPoint.position, endPoint.position, p.progress);
        p.transform.position = pos;


        Vector3 dir = (endPoint.position - startPoint.position).normalized;
        if (dir.sqrMagnitude > 0.0001f)
            p.transform.rotation = Quaternion.LookRotation(dir);
    }

    /// <summary>
    /// Attach a packet to this belt. Packet will be registered with this belt and its transform driven by the belt.
    /// </summary>
    public void AttachPacket(PacketController packet, float initialProgress = -1f)
    {
        if (packet == null) return;


        // if the packet belonged to another belt, unregister it there
        if (packet.currentBelt != null && packet.currentBelt != this)
        {
            packet.currentBelt.UnregisterPacket(packet);
        }


        RegisterPacket(packet);
        packet.currentBelt = this;
        packet.isOnBelt = true;


        // if not provided, compute nearest t on the line so it snaps where you dropped it
        if (initialProgress < 0f)
            packet.progress = ComputeNearestProgress(packet.transform.position);
        else
            packet.progress = Mathf.Clamp01(initialProgress);


        // the belt drives the transform: make kinematic and zero velocities
        packet.SetKinematic(true);
        UpdatePacketTransform(packet);
    }

    /// <summary>
    /// Detach packet from this belt. It will be left free (physics/VR) and removed from belt control.
    /// </summary>
    public void DetachPacket(PacketController packet)
    {
        if (packet == null) return;
        if (packet.currentBelt == this)
            UnregisterPacket(packet);


        packet.currentBelt = null;
        packet.isOnBelt = false;
        packet.SetKinematic(false);
    }


    public void RegisterPacket(PacketController packet)
    {
        if (!packets.Contains(packet))
            packets.Add(packet);
    }


    public void UnregisterPacket(PacketController packet)
    {
        if (packets.Contains(packet))
            packets.Remove(packet);
    }

    /// <summary>
    /// Compute closest normalized progress (0..1) along the straight segment [startPoint, endPoint] to worldPos.
    /// </summary>
    public float ComputeNearestProgress(Vector3 worldPos)
    {
        Vector3 a = startPoint.position;
        Vector3 b = endPoint.position;
        Vector3 ab = b - a;
        float ab2 = Vector3.Dot(ab, ab);
        if (ab2 <= Mathf.Epsilon) return 0f;
        float t = Vector3.Dot(worldPos - a, ab) / ab2;
        return Mathf.Clamp01(t);
    }


    public void SetRunning(bool run)
    {
        running = run;
    }
}
