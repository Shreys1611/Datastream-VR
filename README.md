# DataStream VR: A VR Simulator for Computer Networking

<p align="center">
  <img src="Images/datastream-vr-gameplay.gif" alt="Gameplay GIF of DataStream VR" width="800"/>
</p>

## An Expo-Showcased VR Educational Simulator

Welcome to the repository for **DataStream VR**, a team-developed serious game designed to help college students visualize and understand complex computer networking concepts in an immersive, hands-on environment.

This project translates abstract data flow concepts into a tangible, physical metaphor: a high-tech factory. Instead of just reading theory, students can see and interact with data packets, network hubs, and data paths to build a strong, intuitive understanding of the subject.

This project was successfully demonstrated at a university expo, where it was tested on the **Meta Quest 2** and received high praise from both faculty and visiting industry experts, earning our team a memento for its innovative approach to education.

---

### 🖼️ Project Showcase

| The Main Hub | Visualizing Data Flow | VR Interaction |
| :---: | :---: | :---: |
| ![A screenshot of the central control panel where the player receives objectives.](Images/datastream-01.png) | ![A screenshot showing "data packet" boxes moving along conveyor belts.](Images/datastream-02.png) | ![A screenshot of the player grabbing a "packet" box using the VR controllers.](Images/datastream-03.png) |

---

### ✨ Core Features

* **Immersive VR Learning:** A hands-on simulation using the Unity XR Plugin and tested on the Meta Quest 2.
* **Physical Data Metaphor:** The "factory" environment, with conveyor belts as data paths and boxes as packets, makes abstract concepts easy to grasp.
* **Interactive Control Hub:** A central UI panel where players receive and manage objectives, driving the game's logic.
* **Objective-Based Gameplay:** Players learn by completing tasks, such as correctly routing a "packet" to its intended destination.

---

### 🏆 My Role: Gameplay & VR Interaction Programmer

As a key programmer on the team, my role was to build the core systems that brought the simulation to life. I was responsible for:

* **Packet Traversal Logic:** I engineered the C# scripts that governed the "packet" (box) movement. This system managed the flow of boxes along all conveyor belts, ensuring they followed the correct paths based on the player's choices.
* **Core Game Logic & UI:** I developed the primary game manager and the logic for the central control panel. This system was responsible for assigning objectives to the player and tracking their progress and completion state.
* **VR Interaction System:** I implemented the VR interaction mechanics using the Unity XR Plugin, allowing players to physically grab, hold, and manipulate the data packets and other key items.
* **Level Design & Asset Contribution:** I assisted the level design team by populating the warehouse environment with custom-made 3D assets (like shelves and boxes) that I modeled in Blender.

---

### 🔧 Tech Stack

* **Engine:** Unity
* **VR Platform:** Unity XR Plugin
* **Hardware:** Meta Quest 2
* **3D Modeling:** Blender
* **Language:** C#
* **Version Control:** Git
