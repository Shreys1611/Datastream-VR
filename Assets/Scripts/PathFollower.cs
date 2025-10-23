// PathFollower.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PacketController))]
public class PathFollower : MonoBehaviour
{
    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private float speed = 2.0f; // You can adjust this speed
    private PacketController packetController;
    private bool isPathFinished = false; // Add a flag to prevent issues

    void Awake()
    {
        packetController = GetComponent<PacketController>();
    }

    public void FollowPath(List<Transform> path)
    {
        this.waypoints = path;
        this.currentWaypointIndex = 0;

        // Make the packet kinematic so physics doesn't interfere with path following
        packetController.SetKinematic(true);
    }

    void Update()
    {
        // Don't do anything if the path is already finished
        if (isPathFinished) return;

        // If the player is holding the packet, PAUSE the path following.
        // The component will remain, remembering its path and progress.
        if (packetController.isHeld)
        {
            return; // <-- CHANGE THIS LINE. Replaces "Destroy(this);"
        }

        // If there's no path, do nothing.
        if (waypoints == null || waypoints.Count == 0)
        {
            return;
        }

        // Have we reached the end of the path?
        if (currentWaypointIndex >= waypoints.Count)
        {
            isPathFinished = true; // Set flag
            Destroy(gameObject);   // Destroy self
            return;
        }

        // Move towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Also update rotation to look towards the next waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
        {
            currentWaypointIndex++;
        }
    }
}