# GDIM32-Final
## Check-In
### Evelina
At this stage of the project, I was mainly responsible for the item pickup system, the damage system for non-main NPCs, and the main NPC dialogue and quest script. I implemented the item pickup system, allowing players to click on objects, inspect them, and add them to the inventory. I connected the pickup logic with the Inventory class and ensured that items are properly removed from the scene after being collected. I also adjusted interaction distance and added sound effects for better feedback.For non-main NPCs, I implemented the damage system. I created trigger-based collision logic so that when the player enters the NPC’s range, the player loses health over time. I also added cooldown control to prevent continuous damage from triggering too quickly.For the main NPC, I developed the full dialogue and quest system. I implemented branching dialogue options using the DialogueUI system, including an interaction loop where both choices must be selected before the dialogue progresses. I also built the quest logic that checks whether the player has collected the required five items before giving the key. This system connects dialogue, inventory checking, and quest progression together.

I think our proposal and the breakdown plan are very helpful. They have enabled us to set the most basic goals and also divided the entire game into several parts, making it easier for us to assign tasks. However, during the execution, some problems were encountered. There were some missing details in the "itempickup" section, which caused me to write the wrong code. Later, I corrected it. Also, because the two people were writing the same part, we split the "player" and "player controller" into two separate sections. Later on, we realized that they could be combined. However, the proposal still provided us with an overall direction.
### Nicole Yang
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
didn't need a direct reference to each other. I also added some environment assets such as chairs, tables, clocks, and mirrors and rearranged the scene 
so that interaction with NPC that damage the player is more meaningful. I added player jump movement so that players can reach items that are on tables or 
higher then them. 

I think our proposal and breakdwon was pretty helpful to start building our game. The proposal gave us the foundation in what we 
needed to start building and coding. The breakdown also gave us the foundation to start coding scripts but it mostly only covered the basic concepts 
and we came across more specific things that needed more details such as we thought a GameController might be able to manage the overall gameplay but 
we ended up splitting the items and inventory and related gameplay to separate scripts. 
### Ziying Huang
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
Model-View-Controller: The MVC design pattern can be found in the GameEvents script where the events are present. Some of the events are 
public static Action<int> OnHealthChanged; public static Action OnPlayerDied; public static Action OnGameStarted;
public static Action<string> OnSetChecklistText;These events are then invoked in their respective scripts where the method and actions are carried out. 
For example in the Player script, the method TakeDamage() is responsible for player health. In this method
when player is taking damage, the event OnHealthChanged is invoked and other scripts like UImanager subscribes and applies its own method, OnHealthChanged +=
SetHealth() which sets the player health on the screen as text. The MVC helped structured our project as the decoupled code makes it 
so scripts do not need direct references to each other and we do not need to manually set it in the inspector. Also if we change one part of a script,
we can avoid console errors from having to change all the other scripts. It also makes the code easier to read, as we don't need to understand
what code the other members wrote, we just need to write our own method and apply it to the event.

Finite State Machine：We used FSM pattern to control enemy behavior and animation in our game. This can be found in the NPC script where we defined an enum with different states: Idle and Attack, and used a SetState() method to switch between them. In the code, the enemy checks whether the player is inside its detection range and field of view. If the player is detected, the state changes from Idle to Attack and if the player leaves the range or is no longer visible, the state changes back to Idle. The Animator is also connected to this system, so the enemy’s animation changes depending on the current state. This pattern was useful because it made the behavior logic much easier to organize. FSM made the code easier to read and expand later. For example, if we want to add more enemy behaviors in the future, such as Chase, we can do so by adding new states without rewriting the whole system. 

### Evelina Wang
Since the check-in, I have contributed several improvements and bug fixes to the project. First, I modified the dialogue system to improve how conversations are displayed and handled. I worked on scripts such as DialogueUI and KeyQuestNPC to refine the dialogue flow and player interaction. In the DialogueUI class, I adjusted methods including ShowOne() and ShowTwoChoices() so that dialogue lines and player choices are displayed correctly. I also ensured that button callbacks trigger the appropriate dialogue responses. On the NPC side, I updated the KeyQuestNPC script so that the conversation logic properly handles different states of the quest, such as the introduction dialogue, checking whether the player has collected the required items, and giving the key reward once the quest is completed. These changes improved the structure and clarity of the dialogue system and made the interaction with the NPC more stable. Second, I fixed a bug where dialogue buttons could appear on the screen but could not be clicked by the player. This required debugging the UI interaction logic. I reviewed the button setup in Unity and corrected the event binding in the DialogueUI script. Specifically, I updated the button listener setup in the Awake() method and ensured that the Button.onClick events correctly call the dialogue selection logic. I also checked the UI hierarchy and interaction settings to make sure the buttons were not blocked by other UI elements. After these changes, the dialogue buttons now respond correctly when the player clicks them. Third, I fixed a gameplay bug related to the game timer. Previously, when the timer reached zero, the game would display the game over UI, but the player could still move and interact with objects. To fix this, I modified the UIManager script and connected it to the Player script. When the timer runs out (timeLeft <= 0), the UIManager now triggers the game-over logic. I added a method called ForceGameOver() in the Player class, which sets _canMove = false and resets movement input variables such as vertical and horizontal. This prevents the player from moving, jumping, or interacting after the game has ended. I also ensured that the GameEvents.OnPlayerDied event is invoked so that the UI and other systems properly transition to the game-over state.
### Nicole Yang
Since the check-in, I created an audio indication and flashing red screen when player takes damage. In the Audio script, I wrote
the method PlayAudio with the code audioSource.PlayOneShot(audioClip); and had the method subscribe to
GameEvents.OnHealthChanged. I implemented camera movement with right click of mouse because some of the objects that needed to be picked up 
were below the player. Allowing the player to look around the screen made it easier for item pickups. I wrote the following code for this: 
if (Input.GetMouseButton(1)) {float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;... Because the camera now follows the mouse, when player is talking to
npc they can still look around so I locked the camera when player is speaking to the NPC. I also fixed a few collider issues with the chair and table
with huggy. The chair was tagged as ground so the player can jump on it but the table was not so there was some incidents where the player eneded up
not being able to jump at all. I also hadded a collider underneath the table with Springtrap because player could somehow get under it. I added text in
the start menu to tell wasd for movement and right click for camera rotation. 
### Ziying Huang
Put your individual final Devlog here.

## Open-Source Assets
[Freddy 3D model](https://sketchfab.com/3d-models/freddy-a086d08b955e476ca41f8af04adceaa0)-NPC

[Poppy 3D model](https://sketchfab.com/3d-models/poppy-playtime-chapter-5-huggy-huggy-9635231455214cff91e0cc70a5983ef2)-NPC

[Gyroid 3D model](https://sketchfab.com/3d-models/animal-crossing-gyroid-dd02039f30e2424b8c9c79cfbec5bacc)-NPC

[Oldpocketwatch 3D model](https://models.spriters-resource.com/playstation_vita/littlebigplanetpsvita/asset/466891/)-Item

[Cup 3D model](https://models.spriters-resource.com/nintendo_switch/miitopia/asset/333602/)-Item

[Mirror 3D model](https://models.spriters-resource.com/playstation_3/ratchetclankupyourarsenalhd/asset/312365/)-Item

[Musicbox 3D model](https://models.spriters-resource.com/pc_computer/madeinabyssbinarystarfallingintodarkness/asset/344599/)-Item

[Doll 3D model](https://models.spriters-resource.com/pc_computer/fivenightsatfreddysvrhelpwanted/asset/340641/)-Item

[Key 3D model](https://models.spriters-resource.com/pc_computer/kanpeki/asset/334368/)-Item

[Backroom 3D model](https://sketchfab.com/3d-models/original-backrooms-e5c6b30995ff442d9852a1dd697aaef1)-Environment

[Font](https://www.dafont.com/vcr-osd-mono.font)-UI

[BGM](https://www.youtube.com/watch?v=u1ENryv3WB4)-Background music

[Sand Clock](https://assetstore.unity.com/packages/3d/props/interior/sand-clock-8466)- Environment asset

[Cheval Mirror](https://assetstore.unity.com/packages/3d/props/cheval-mirror-259424)- Environment asset

[Table & Chair](https://assetstore.unity.com/packages/3d/props/3d-game-asset-table-chair-sets-199726)- Environment asset
