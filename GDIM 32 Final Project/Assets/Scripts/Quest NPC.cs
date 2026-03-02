using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class KeyQuestNPC : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Quest Items (5)")]
    [SerializeField] private ItemId[] requiredItems = new ItemId[5];

    [Header("Reward (choose in Inspector)")]
    [SerializeField] private ItemId rewardItem;
    [SerializeField] private bool consumeRequiredItems = true;

    [Header("Talk UI")]
    [SerializeField] private GameObject talkPrompt;
    [SerializeField] private TMP_Text talkText;
    [SerializeField] private string talkMsg = "Click to talk";

    private Camera cam;
    private bool introASeen;
    private bool introBSeen;
    private bool questStarted;
    private bool questCompleted;
    private Player currentPlayer;
    [Header("Gyroid Conversation Text")]
    [TextArea] [SerializeField] private string introLine =
        "You are somewhere between memory and nightmare. This place does not like visitors.";

    [SerializeField] private string choiceATitle = "Why am I here?";
    [TextArea] [SerializeField] private string choiceAReply =
        "Not everyone who enters remembers why. Maybe the house chose you.";

    [SerializeField] private string choiceBTitle = "How do I get out?";
    [TextArea] [SerializeField] private string choiceBReply =
        "Escape is possible. But nothing here comes without a price. Find the things I lost… and I may help you.";

    [TextArea] [SerializeField] private string introEndingLine =
        "Be aware of what is in the darkness. If you stay too long… this place will start taking pieces of you.";

    [TextArea] [SerializeField] private string notReadyLine =
        "You are not ready. Find the things I lost.";

    [TextArea] [SerializeField] private string readyLine =
        "I can feel them… The things I lost. Will you give them to me?";

    [TextArea] [SerializeField] private string giveKeyLine =
        "You have done well. Take the key. And find the way out.";

    private void Start()
    {
        cam = Camera.main;

        if (talkPrompt != null)
            talkPrompt.SetActive(false);

        if (talkText != null)
            talkText.text = talkMsg;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (talkPrompt != null && !questCompleted) 
            talkPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (talkPrompt != null)
            talkPrompt.SetActive(false);
    }
    public string GetHint() => "Talk";
    public void Interact(Player player)
    {
        
        if (player == null) return;
        currentPlayer = player;
        if (talkPrompt != null)
            talkPrompt.SetActive(false);

        if (dialogueUI == null) dialogueUI = FindObjectOfType<DialogueUI>();

        if (questCompleted)
        {
            dialogueUI.ShowOne("Go. Before the house notices you again.", "OK", () => dialogueUI.Hide());
            return;
        }

        if (!questStarted)
        {
            ShowIntroMenu();
            return;
        }

        ShowQuestCheck();
    }

     private void ShowIntroMenu()
    {
        if (introASeen && introBSeen)
        {
            dialogueUI.ShowOne(introEndingLine, "OK", () =>
            {
                questStarted = true;
                dialogueUI.Hide();
            });
            return;
        }

        dialogueUI.ShowTwoChoices(
            introLine,
            choiceATitle, showA: !introASeen,
            choiceBTitle, showB: !introBSeen,
            OnIntroPick
        );
    }

     private void OnIntroPick(int idx)
    {
        if (idx == 0)
        {
            introASeen = true;
            dialogueUI.ShowOne(choiceAReply, "Back", ShowIntroMenu);
        }
        else
        {
            introBSeen = true;
            dialogueUI.ShowOne(choiceBReply, "Back", ShowIntroMenu);
        }

    }

    private void ShowQuestCheck()
    {
        if (!QuestManager.Instance.HasAll(currentPlayer, requiredItems))
        {
            dialogueUI.ShowOne(notReadyLine, "OK", () => dialogueUI.Hide());
            return;
        }

        dialogueUI.ShowTwoChoices(
            readyLine,
            "Give items", true,
            "Not yet", true,
            OnTurnInPick
        );
    }

    private void OnTurnInPick(int idx)
    {
        if (idx == 1)
        {
            dialogueUI.Hide();
            return;
        }

        if (consumeRequiredItems)
            QuestManager.Instance.TryConsumeAll(currentPlayer, requiredItems);

        currentPlayer.Inventory.Add(rewardItem);
        questCompleted = true;
        currentPlayer.Inventory.Add(rewardItem);

        var keyVisual = currentPlayer.GetComponent<Player>();
        if (keyVisual != null)
         keyVisual.RefreshKeyInHand(currentPlayer.Inventory);

        dialogueUI.ShowOne(giveKeyLine, "OK", () => dialogueUI.Hide());
    }
   
}



