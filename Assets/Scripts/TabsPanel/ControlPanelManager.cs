using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControlPanelManager : MonoBehaviour
{
    // --- MODIFIED SECTION ---
    [Header("Task & Panel Management")]
    [Tooltip("The master list of all tasks in the order they should appear.")]
    public TaskData[] allTasksInOrder; // Renamed from allTasks
    private List<TaskData> activeTasks = new List<TaskData>(); // The tasks currently visible to the player
    private int nextTaskIndex = 0; // Tracks the next task to be released from the master list
    // --- END MODIFIED SECTION ---

    public GameObject[] panels;
    private int currentPanelIndex = 0;
    private TaskData currentTask;
    private int selectedTaskIndex = -1; // This will now store the index from the MASTER list

    [Header("Inbox Panel")]
    public GameObject taskButtonPrefab;
    public Transform newTasksParent;
    public Transform completedTasksParent;

    // --- NEW VARIABLE ---
    [Header("Task Timing")]
    [Tooltip("How many seconds to wait before releasing a new task into the inbox.")]
    public float newTaskInterval = 60f; // e.g., a new task appears every 60 seconds

    // ... (The rest of your Header references are the same)
    [Header("Details Panel")]
    public TextMeshProUGUI detailDescriptionText;
    public TextMeshProUGUI detailSourceIPText;
    public TextMeshProUGUI detailDestIPText;
    public TextMeshProUGUI detailFileTypeTest;
    public TextMeshProUGUI detailFileSizeText;
    public TextMeshProUGUI detailSourceNodeText;
    public TextMeshProUGUI detailDestNodeText;
    [Header("Protocol Panel")]
    public List<Toggle> protocolToggles;
    [Header("Casting Panel")]
    public TMP_Dropdown castingDropdown;
    [Header("IP Details Panel (Dynamic)")]
    public GameObject unicastInput_IP;
    public GameObject multicastInput_IP;
    public GameObject broadcastInput_IP;
    public TMP_InputField commonSourceIPField;
    public TMP_InputField uniDestIPField;
    public TMP_InputField multiDestIPField1;
    public TMP_InputField multiDestIPField2;
    public TMP_InputField broadDestIPField1;
    public TMP_InputField broadDestIPField2;
    public TMP_InputField broadDestIPField3;
    [Header("Node Selection Panel (Dynamic)")]
    public GameObject unicastInput_Nodes;
    public GameObject multicastInput_Nodes;
    public GameObject broadcastInput_Nodes;
    public TMP_Dropdown uniSourceNodeDropdown;
    public TMP_Dropdown uniDestNodeDropdown;
    public TMP_Dropdown multiSourceNodeDropdown;
    public TMP_Dropdown multiDestNodeDropdown1;
    public TMP_Dropdown multiDestNodeDropdown2;
    public TMP_Dropdown broadSourceNodeDropdown;
    [Header("System References")]
    public PathManager pathManager;
    public Light[] allLights;
    public AudioSource sirenAudioSource;
    public Color defaultLightColor = Color.white;
    private Coroutine activePacketAnimation;
    [Header("Animation Settings")]
    public float packetStreamDuration = 10f;
    public float packetSpawnInterval = 1.5f;
    private bool isHandlingOutcome = false;

    void Start()
    {
        // --- MODIFIED: Start the simulation with only the first task ---
        if (allTasksInOrder.Length > 0)
        {
            // Unlock the first task immediately
            activeTasks.Add(allTasksInOrder[0]);
            nextTaskIndex = 1;

            // Start the timer to release subsequent tasks
            StartCoroutine(TaskReleaseRoutine());
        }

        PopulateInbox();
        ShowPanel(0);
    }

    // --- NEW: A coroutine to release new tasks over time ---
    private IEnumerator TaskReleaseRoutine()
    {
        // Keep running as long as there are more tasks to release
        while (nextTaskIndex < allTasksInOrder.Length)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(newTaskInterval);

            // "Release" the next task
            Debug.Log($"New task released: {allTasksInOrder[nextTaskIndex].taskName}");
            activeTasks.Add(allTasksInOrder[nextTaskIndex]);
            nextTaskIndex++;

            // Refresh the inbox to show the new task
            PopulateInbox();
        }
    }

    // --- MODIFIED: Populates the inbox using the 'activeTasks' list ---
    void PopulateInbox()
    {
        foreach (Transform child in newTasksParent) { Destroy(child.gameObject); }
        foreach (Transform child in completedTasksParent) { Destroy(child.gameObject); }

        // Iterate over the list of currently ACTIVE tasks
        foreach (TaskData task in activeTasks)
        {
            Transform parentPanel = task.isCompleted ? completedTasksParent : newTasksParent;
            GameObject buttonGO = Instantiate(taskButtonPrefab, parentPanel);
            buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = task.taskName;

            Image buttonImage = buttonGO.GetComponent<Image>();
            buttonImage.color = task.isCompleted ? Color.green : Color.red;

            // Find the original index of this task in the master list
            int originalTaskIndex = System.Array.IndexOf(allTasksInOrder, task);

            Button button = buttonGO.GetComponent<Button>();
            if (!task.isCompleted)
            {
                button.onClick.AddListener(() => SelectTask(originalTaskIndex));
            }
        }
    }

    // This function now receives the index from the MASTER list
    public void SelectTask(int taskIndex)
    {
        if (allTasksInOrder[taskIndex].isCompleted) return;

        selectedTaskIndex = taskIndex;
        currentTask = allTasksInOrder[selectedTaskIndex]; // Get the task from the master list

        // ... (rest of the function is the same)
        detailDescriptionText.text = $"Description: {currentTask.detailDescription}";
        detailSourceIPText.text = $"Source IP: {currentTask.sourceIP}";
        detailDestIPText.text = $"Dest. IP(s): {string.Join(", ", currentTask.destinationIPs)}";
        detailFileTypeTest.text = $"File Type: {currentTask.fileType}";
        detailFileSizeText.text = $"File Size: {currentTask.fileSize}";
        detailSourceNodeText.text = $"Source Node: {currentTask.correctSourceNode}";
        detailDestNodeText.text = $"Dest. Node(s): {string.Join(", ", currentTask.correctDestinationNodes)}";
        GoToPanel(1);
    }

    // ... (The rest of your script: ShowPanel, GoToPanel, ShowNew/Completed, OnSubmit, Validation, etc. all remain the same)
    void ShowPanel(int index) { currentPanelIndex = index; for (int i = 0; i < panels.Length; i++) { panels[i].SetActive(i == index); } if (index == 0) { ShowNewTasks(); } if (panels.Length > 4 && castingDropdown != null) { CastingType selectedCasting = (CastingType)castingDropdown.value; if (index == 4) { unicastInput_IP.SetActive(selectedCasting == CastingType.Unicast); multicastInput_IP.SetActive(selectedCasting == CastingType.Multicast); broadcastInput_IP.SetActive(selectedCasting == CastingType.Broadcast); } if (index == 5) { unicastInput_Nodes.SetActive(selectedCasting == CastingType.Unicast); multicastInput_Nodes.SetActive(selectedCasting == CastingType.Multicast); broadcastInput_Nodes.SetActive(selectedCasting == CastingType.Broadcast); } } }
    public void GoToPanel(int index) { if (index >= 0 && index < panels.Length) { ShowPanel(index); } }
    public void ShowNewTasks() { newTasksParent.gameObject.SetActive(true); completedTasksParent.gameObject.SetActive(false); }
    public void ShowCompletedTasks() { newTasksParent.gameObject.SetActive(false); completedTasksParent.gameObject.SetActive(true); }
    public void OnCastingTypeChanged(int value) { Debug.Log("Casting type changed to: " + (CastingType)value); }
    public void OnSubmitPressed() { if (currentTask == null) return; isHandlingOutcome = false; NodeType userSourceNode = NodeType.A; List<NodeType> userDestNodes = new List<NodeType>(); CastingType selectedCasting = (CastingType)castingDropdown.value; if (selectedCasting == CastingType.Unicast) { userSourceNode = (NodeType)uniSourceNodeDropdown.value; userDestNodes.Add((NodeType)uniDestNodeDropdown.value); } else if (selectedCasting == CastingType.Multicast) { userSourceNode = (NodeType)multiSourceNodeDropdown.value; userDestNodes.Add((NodeType)multiDestNodeDropdown1.value); userDestNodes.Add((NodeType)multiDestNodeDropdown2.value); } else { userSourceNode = (NodeType)broadSourceNodeDropdown.value; userDestNodes.Add(NodeType.A); userDestNodes.Add(NodeType.B); userDestNodes.Add(NodeType.C); } if (activePacketAnimation != null) StopCoroutine(activePacketAnimation); activePacketAnimation = StartCoroutine(PacketAnimationSequence(userSourceNode, userDestNodes)); }
    private bool ValidateAllUserInput() { Debug.Log("--- Running Full Validation ---"); HashSet<string> userInput_Protocols = new HashSet<string>(); foreach (var toggle in protocolToggles) { if (toggle.isOn) { userInput_Protocols.Add(toggle.GetComponentInChildren<TextMeshProUGUI>().text); } } if (userInput_Protocols.Count != currentTask.correctProtocols.Count) { Debug.LogError($"VALIDATION FAILED: Protocol count mismatch. Expected {currentTask.correctProtocols.Count}, but user selected {userInput_Protocols.Count}."); return false; } foreach (var requiredProtocol in currentTask.correctProtocols) { if (!userInput_Protocols.Contains(requiredProtocol)) { Debug.LogError($"VALIDATION FAILED: Missing required protocol '{requiredProtocol}'."); return false; } } Debug.Log("Protocols: OK"); if ((CastingType)castingDropdown.value != currentTask.correctCastingType) { Debug.LogError($"VALIDATION FAILED: Casting Type mismatch. Expected '{currentTask.correctCastingType}', but user selected '{(CastingType)castingDropdown.value}'."); return false; } Debug.Log("Casting Type: OK"); if (commonSourceIPField.text != currentTask.sourceIP) { Debug.LogError($"VALIDATION FAILED: Source IP mismatch. Expected '{currentTask.sourceIP}', but user entered '{commonSourceIPField.text}'."); return false; } switch (currentTask.correctCastingType) { case CastingType.Unicast: if (uniDestIPField.text != currentTask.destinationIPs[0]) { Debug.LogError($"VALIDATION FAILED: Unicast Destination IP mismatch. Expected '{currentTask.destinationIPs[0]}', but user entered '{uniDestIPField.text}'."); return false; } break; case CastingType.Multicast: if (multiDestIPField1.text != currentTask.destinationIPs[0] || multiDestIPField2.text != currentTask.destinationIPs[1]) { Debug.LogError("VALIDATION FAILED: Multicast Destination IP mismatch."); return false; } break; case CastingType.Broadcast: if (broadDestIPField1.text != currentTask.destinationIPs[0] || broadDestIPField2.text != currentTask.destinationIPs[1] || broadDestIPField3.text != currentTask.destinationIPs[2]) { Debug.LogError("VALIDATION FAILED: Broadcast Destination IP mismatch."); return false; } break; } Debug.Log("IP Addresses: OK"); Debug.Log("--- All User Data Validated Successfully! ---"); return true; }
    private IEnumerator PacketAnimationSequence(NodeType start, List<NodeType> destinations) { GoToPanel(-1); pathManager.StartPacketJourney(start, destinations, currentTask); yield return new WaitForSeconds(15f); StartCoroutine(HandleFailureSequence("Packet Delivery Timed Out")); }
    public void OnPacketArrivedAtNode(NodeType arrivedNode) { if (isHandlingOutcome) return; if (activePacketAnimation != null) StopCoroutine(activePacketAnimation); bool dataIsCorrect = ValidateAllUserInput(); bool nodeIsCorrect = currentTask.correctDestinationNodes.Contains(arrivedNode); if (dataIsCorrect && nodeIsCorrect) { StartCoroutine(HandleSuccessSequence()); } else { StartCoroutine(HandleFailureSequence($"Data Correct: {dataIsCorrect}, Node Correct: {nodeIsCorrect}")); } }
    private IEnumerator PacketSpawningLoop(NodeType start, List<NodeType> destinations) { while (true) { pathManager.StartPacketJourney(start, destinations, currentTask); yield return new WaitForSeconds(packetSpawnInterval); } }
    private IEnumerator HandleSuccessSequence() { isHandlingOutcome = true; Debug.Log("Success! Starting continuous packet stream."); Coroutine spawningCoroutine = StartCoroutine(PacketSpawningLoop(currentTask.correctSourceNode, currentTask.correctDestinationNodes)); yield return new WaitForSeconds(packetStreamDuration); StopCoroutine(spawningCoroutine); Debug.Log("Stream finished. Completing task."); yield return new WaitForSeconds(5f); currentTask.isCompleted = true; PopulateInbox(); GoToPanel(0); }
    private IEnumerator HandleFailureSequence(string reason) { if (isHandlingOutcome) yield break; isHandlingOutcome = true; Debug.Log($"Failure! Reason: {reason}"); PathFollower[] remainingPackets = FindObjectsByType<PathFollower>(FindObjectsSortMode.None); foreach (var packet in remainingPackets) { PacketFader fader = packet.GetComponent<PacketFader>(); if (fader != null) { fader.StartFadeOut(1.0f); } else { Destroy(packet.gameObject); } } foreach (var light in allLights) { light.color = Color.red; } sirenAudioSource.Play(); yield return new WaitForSeconds(10f); sirenAudioSource.Stop(); foreach (var light in allLights) { light.color = defaultLightColor; } PopulateInbox(); GoToPanel(0); }
}