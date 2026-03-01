# GDIM32-Final
## Check-In
### Team Member Name Evelina
The item draft was completed on 2.21. This enables the items to be picked up and detected by NPCs. Scary NPC script and player health finished on 2.22. Change interact from click F to click the mouse. 2.27, Added a visual inspection mode for picking up items.I also wrote the script and dialogue for the mission NPC, enabling it to have conversations and make choices. I also asked Ziying to do the beautification work.
### Team Member Name Nicole Yang
In the Unity Scene, I created mesh colliders for the ground and walls. I created the Player GameObject and attached all the components needed
such as Rigidbody and colliders. I created the player singleton and wrote code for player movement in the FixedUpdate() method:
Vector3 movement = transform.forward * vertical * _moveSpeed * Time.fixedDeltaTime; This was for forward and backwards movement. 
_playerRigidbody.MovePosition(_playerRigidbody.position + movement); rotate float turn = horizontal * _turnSpeed * Time.fixedDeltaTime; 
Quaternion turnRotation = Quaternion.Euler(0, turn, 0); _playerRigidbody.MoveRotation(_playerRigidbody.rotation * turnRotation); This was for player 
rotation movement. Our scripts were originally coupled and the Player script, UI, and Inventory called directly from each other. I wrote a new script
GameEvents to decouple the code to adhere to a Model-View-Controller pattern. I wrote the player events public static Action<int> OnHealthChanged; and 
public static Action OnPlayerDied; so that the player script can invoke the events if the player's health changed or if the player died and script 
such as the UI can subscribe to the event and call its own method. In the UI script I added the following code to onEnable():  GameEvents.OnHealthChanged 
+= SetHealth; GameEvents.OnPlayerDied += ShowGameOver; The event for UI in GameEvents is public static Action OnGameStarted; and the event for 
inventory is public static Action<ItemId> OnItemPickedUp; public static Action<ItemId> OnItemRemoved; These events are also so that the three scripts 
didn't need a direct reference to each other. 

I think our proposal and breakdwon was pretty helpful to start building our game. The proposal gave us the foundation in what we 
needed to start building and coding. The breakdown also gave us the foundation to start coding scripts but it mostly only covered the basic concepts 
and we came across more specific things that needed more details such as we thought a GameController might be able to manage the overall gameplay but 
we ended up splitting the items and inventory and related gameplay to separate scripts. 
### Team Member Name 3 Ziying Huang
At this stage of the project, I worked primarily on the player interaction system, game art integration inside Unity, and UI management. I created the UIManager class, which manages the healthText, timerText, startPanel, and gameOverPanel. 
I also wrote the game state logic, including handling the Start and Restart flow. I added a countdown system using the variables timeLeft and timerRunning, which controls the game timer and triggers the game over state. I built several systems directly in Unity.
I created a Canvas for NPC interaction prompts and set up collider components (BoxCollider and SphereCollider) to define interaction ranges. I created the NPC Animator controller and adjusted lighting for the atmosphere of the environment. I also applied post-processing color grading 
to strengthen the mood of the scene and refined interactable UI placement.I also added AudioSource for bgm and sounds. In the process of building these systems, I resolved several interaction bugs. I fixed an issue where the player could move before pressing Start, as well as a bug where movement was still enabled after death. 
I also corrected problems with UI prompts not hiding properly during state transitions through SetActive() and OnTriggerEnter/Exit.

Our proposal helped define the overall theme and core mechanics (exploration, collection, NPC quest progression). But I think it was not detailed enough as we began working, we realized many architectural details were missing, especially regarding Interaction 
and State management. We had to refine our architecture as we built. For planning tools, we mostly relied on discussion(Discord) to saved time and prevented overlap or confusion, and google doc task breakdown for iteration. Overall, the Proposal gave us direction,
but the actual building process required more detailed design.

### Group Devlog
In our project, we encountered a technical issue with the NPC detection system. The NPC was supposed to detect the player only when the player was within a specific alert range and inside its field of view. 
However, during testing, the NPC would sometimes attack even when the player was behind it or outside of what we visually considered its detection zone. This made the behavior feel inconsistent. 

To diagnose this issue, we chose to use Gizmos.We chose Gizmos because it allow us to visually represent invisible logic directly in the Scene view. Instead of guessing whether the distance and angle calculations were working properly, 
we could draw the detection range and viewing cone in real time. We used OnDrawGizmos() in the NPC script and used:Gizmos.DrawWireSphere() to visualize the alert radius. Gizmos.DrawRay() to draw the left and right boundaries of the NPC’s field of view.
After visualizing the detection system, we discovered that the NPC was only checking distance at first, which meant it could detect the player in a full 360 degree radius. By adding the angle check and adjusting the viewAngle parameter while observing 
the Gizmos in the Scene view, we were able to change the detection cone to make it more accurate. Gizmos turned vector math into something we could clearly see and adjust. Instead of relying only on console logs or error, we could visually 
know whether the detection area matched our intended gameplay design.


## Final Submission
### Group Devlog
Put your group Devlog here.

### Evelina Wang
Put your individual final Devlog here
### Nicole Yang
Put your individual final Devlog here.
### Team Member Name 3
Put your individual final Devlog here.

## Open-Source Assets
Cite any open-source assets here. Put them in a LIST, and use correctly formatted LINKS.
