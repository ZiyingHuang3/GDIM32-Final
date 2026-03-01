# GDIM32-Final
## Check-In
### Team Member Name Evelina
At this stage of the project, I was mainly responsible for the item pickup system, the damage system for non-main NPCs, and the main NPC dialogue and quest script. I implemented the item pickup system, allowing players to click on objects, inspect them, and add them to the inventory. I connected the pickup logic with the Inventory class and ensured that items are properly removed from the scene after being collected. I also adjusted interaction distance and added sound effects for better feedback.For non-main NPCs, I implemented the damage system. I created trigger-based collision logic so that when the player enters the NPC’s range, the player loses health over time. I also added cooldown control to prevent continuous damage from triggering too quickly.For the main NPC, I developed the full dialogue and quest system. I implemented branching dialogue options using the DialogueUI system, including an interaction loop where both choices must be selected before the dialogue progresses. I also built the quest logic that checks whether the player has collected the required five items before giving the key. This system connects dialogue, inventory checking, and quest progression together.


I think our proposal and the breakdown plan are very helpful. They have enabled us to set the most basic goals and also divided the entire game into several parts, making it easier for us to assign tasks. However, during the execution, some problems were encountered. There were some missing details in the "itempickup" section, which caused me to write the wrong code. Later, I corrected it. Also, because the two people were writing the same part, we split the "player" and "player controller" into two separate sections. Later on, we realized that they could be combined. However, the proposal still provided us with an overall direction.
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
