using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private ItemId itemId; //  = ItemId.Mirror;
    [Header("Pickup Settings")]
    [SerializeField] private float interactDistance = 3f;
    [Header("Inspect View")]
    [SerializeField] private float inspectDistanceFromCamera = 0.6f; 
    [SerializeField] private float inspectScaleMultiplier = 1.0f;   
    [SerializeField] private float rotateSpeed = 180f; 
    [SerializeField] private KeyCode confirmKey = KeyCode.E; 
    [Header("Audio")]
    [SerializeField] private AudioClip inspectSfx; 
    [SerializeField] private AudioClip pickupSfx; 
    [SerializeField] private bool playInspectSound = false;
[SerializeField] private bool playPickupSound = true;

    //private Player player;
    private Camera cam;
    private Collider col;
    private bool isInspecting;
    private bool playerRange;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector3 originalScale;
    private Transform originalParent;
    private Rigidbody rb;
    private UIManager ui;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //player = FindObjectOfType<Player>();
        cam = Camera.main;
        ui = FindObjectOfType<UIManager>();
    }
    private void OnMouseDown()
    {
        if (!playerRange) return;
        if (isInspecting) return;
        if (cam == null) return;
        //if (player == null || cam == null) return;
        Vector3 camPos = cam.transform.position;
        float distance = Vector3.Distance(camPos, col.ClosestPoint(camPos));
        if (distance > interactDistance)
        {
            Debug.Log("Too far to inspect/pick up");
            return;
        }
        EnterInspect();
    }
    private void Update()
    {
        if (!isInspecting) return;
        if (Input.GetMouseButton(0))
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");
            float boost = 6f;
            transform.Rotate(cam.transform.up, -mx * rotateSpeed * boost, Space.World);
            transform.Rotate(cam.transform.right, my * rotateSpeed * boost, Space.World);
        }
        if (Input.GetKeyDown(confirmKey))
        {
            ConfirmPickup();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerRange = true;
        ui.ShowPrompt($"Click to pick up {itemId}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerRange = false;
        ui.HidePrompt();
    }
    private void EnterInspect()
    {
        ui.ShowPrompt("Press E to put into bag / Left click to rotate");
        playerRange = false;
        isInspecting = true;
        originalParent = transform.parent;
        originalPos = transform.position;
        originalRot = transform.rotation;
        originalScale = transform.localScale;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        col.enabled = false;
        transform.SetParent(cam.transform, worldPositionStays: true);
        transform.localPosition = new Vector3(0f, 0f, inspectDistanceFromCamera);
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale * inspectScaleMultiplier;

       if (playInspectSound)
        {
            PlayClip(inspectSfx);
        }
    }
    private void ConfirmPickup()
{
    GameEvents.OnItemPickedUp?.Invoke(itemId);

    if (playPickupSound)
    {
        PlayClip(pickupSfx);
    }

    gameObject.SetActive(false);
    ui.HidePrompt();
}

    private void ExitInspect(bool restore)
    {
        isInspecting = false;
        if (!restore) return;
        transform.SetParent(originalParent, worldPositionStays: true);
        transform.position = originalPos;
        transform.rotation = originalRot;
        transform.localScale = originalScale;
          col.enabled = true;
        if (rb != null) rb.isKinematic = false;
    }
    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

}