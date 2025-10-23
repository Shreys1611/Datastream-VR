using UnityEngine;
using System.Collections;

public class MissionMarker : MonoBehaviour
{
    [Header("Marker Settings")]
    public float rotationSpeed = 50f;
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public Color markerColor = Color.yellow;
    
    [Header("Interaction")]
    public string missionName = "Test Mission";
    public KeyCode interactionKey = KeyCode.E;
    public float interactionRange = 2f;
    
    [Header("UI")]
    public GameObject interactionPrompt;
    public Canvas worldCanvas;
    
    private Transform playerTransform;
    private Renderer markerRenderer;
    private Vector3 originalScale;
    private bool playerInRange = false;
    private Material markerMaterial;
    
    void Start()
    {
        // Get the renderer and create material instance
        markerRenderer = GetComponent<Renderer>();
        if (markerRenderer != null)
        {
            markerMaterial = new Material(markerRenderer.material);
            markerRenderer.material = markerMaterial;
            markerMaterial.color = markerColor;
            
            // Make it emissive for glow effect
            if (markerMaterial.HasProperty("_EmissionColor"))
            {
                markerMaterial.EnableKeyword("_EMISSION");
                markerMaterial.SetColor("_EmissionColor", markerColor * 0.5f);
            }
        }
        
        // Store original scale
        originalScale = transform.localScale;
        
        // Find player (assuming player has "Player" tag)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        // Setup interaction prompt
        SetupInteractionPrompt();
    }
    
    void Update()
    {
        // Rotate the marker continuously
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Pulse effect
        float pulse = Mathf.Lerp(minScale, maxScale, 
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = originalScale * pulse;
        
        // Check player distance
        CheckPlayerDistance();
        
        // Handle interaction
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            TriggerMission();
        }
    }
    
    void CheckPlayerDistance()
    {
        if (playerTransform == null) return;
        
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRange;
        
        // Show/hide interaction prompt
        if (playerInRange != wasInRange)
        {
            ShowInteractionPrompt(playerInRange);
        }
    }
    
    void SetupInteractionPrompt()
    {
        if (worldCanvas == null)
        {
            // Create world space canvas for UI
            GameObject canvasObj = new GameObject("InteractionCanvas");
            canvasObj.transform.SetParent(transform);
            canvasObj.transform.localPosition = Vector3.up * 3f;
            
            worldCanvas = canvasObj.AddComponent<Canvas>();
            worldCanvas.renderMode = RenderMode.WorldSpace;
            worldCanvas.worldCamera = Camera.main;
            
            // Scale down the canvas
            canvasObj.transform.localScale = Vector3.one * 0.01f;
            
            // Add CanvasScaler for better scaling
            var scaler = canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }
        
        // Create interaction prompt text
        if (interactionPrompt == null)
        {
            interactionPrompt = new GameObject("InteractionPrompt");
            interactionPrompt.transform.SetParent(worldCanvas.transform);
            interactionPrompt.transform.localPosition = Vector3.zero;
            interactionPrompt.transform.localRotation = Quaternion.identity;
            
            var text = interactionPrompt.AddComponent<UnityEngine.UI.Text>();
            text.text = $"Press {interactionKey} to start {missionName}";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 24;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            // Add outline for better visibility
            var outline = interactionPrompt.AddComponent<UnityEngine.UI.Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, 1);
        }
        
        // Initially hide the prompt
        ShowInteractionPrompt(false);
    }
    
    void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);
        }
        
        if (worldCanvas != null)
        {
            // Make canvas face the camera
            if (show && Camera.main != null)
            {
                worldCanvas.transform.LookAt(Camera.main.transform);
                worldCanvas.transform.Rotate(0, 180, 0); // Flip to face camera properly
            }
        }
    }
    
    void TriggerMission()
    {
        Debug.Log($"Mission triggered: {missionName}");
        
        // Add your mission logic here
        StartMission();
        
        // Optionally destroy the marker after use
        // Destroy(gameObject);
    }
    
    void StartMission()
    {
        // This is where you'd implement your actual mission logic
        // For example:
        // - Load a new scene
        // - Activate mission objectives
        // - Show mission briefing UI
        // - Set player state to "in mission"
        
        Debug.Log("Mission started!");
        
        // Example: Hide the marker temporarily
        StartCoroutine(HideMarkerTemporarily());
    }
    
    IEnumerator HideMarkerTemporarily()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(5f); // Mission duration example
        gameObject.SetActive(true);
        Debug.Log("Mission completed, marker reactivated!");
    }
    
    // Gizmos for editor visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}