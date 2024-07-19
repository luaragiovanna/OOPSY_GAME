using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator anime;
    private NavMeshAgent agent;
    private Camera cinemachineCamera;

    public Text dialogueText;
    [SerializeField] private GameObject dialogueBackground;
    public GameObjectDialogue dialogueManager; // Referência ao objeto DialogueManager na cena
    public GameObject alienNPC; // Referência ao alien
    public GameObject hintPanel; // Painel de dica
    public Text hintText; // Texto da dica
    public ItemCollectionManager itemCollectionManager; // Referência ao ItemCollectionManager na cena

    private string[] initialDialogues = {
        "Finalmente cheguei!",
    };

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();

        if (dialogueManager == null || dialogueText == null || dialogueBackground == null || hintPanel == null || hintText == null)
        {
            Debug.LogError("DialogueManager, DialogueText, DialogueBackground, HintPanel, or HintText is not assigned.");
        }
        else
        {
            dialogueManager.dialogueText = dialogueText;
            dialogueManager.dialogueBackground = dialogueBackground;
            dialogueManager.hintPanel = hintPanel;
            dialogueManager.hintText = hintText;
        }

        Alien alienScript = alienNPC.GetComponent<Alien>();
        if (alienScript != null)
        {
            SetDialogueManager(alienScript, dialogueManager);
        }
        else
        {
            Debug.LogError("Alien script not found on alienNPC object.");
        }

        if (dialogueManager != null)
        {
            Debug.Log("Starting dialogue with initial dialogues.");
            dialogueManager.StartDialogue(initialDialogues);
        }
        else
        {
            Debug.LogError("DialogueManager is not set correctly.");
        }

        GameObject cameraObject = GameObject.FindWithTag("CinemachineCamera");
        if (cameraObject != null)
        {
            cinemachineCamera = cameraObject.GetComponent<Camera>();
            if (cinemachineCamera == null)
            {
                Debug.LogError("No Camera component found on the object with tag 'CinemachineCamera'.");
            }
        }
        else
        {
            Debug.LogError("No object with tag 'CinemachineCamera' found.");
        }

        itemCollectionManager = FindObjectOfType<ItemCollectionManager>();
        if (itemCollectionManager == null)
        {
            Debug.LogError("ItemCollectionManager not found in the scene.");
        }
    }

    private void SetDialogueManager(Alien alien, GameObjectDialogue manager)
    {
        alien.dialogueManager = manager;
    }

    public void Update()
    {
        HandleMouseClick();
        UpdateAnimator();
    }

    void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cinemachineCamera == null)
            {
                Debug.LogError("Cinemachine Camera not found.");
                return;
            }

            Ray ray = cinemachineCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null)
                {
                    Debug.LogError("Hit collider is null.");
                    return;
                }

              
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                if (hit.collider.CompareTag("Ground"))
                {
                    if (agent == null)
                    {
                        Debug.LogError("NavMeshAgent not found.");
                        return;
                    }

                    agent.SetDestination(hit.point);
                }
                else if (hit.collider.CompareTag("Alien"))
                {
                    Alien alien = hit.collider.GetComponent<Alien>();
                    if (alien != null)
                    {
                        alien.OnMouseDown(); 
                    }
                }
                else if (hit.collider.CompareTag("Collectible"))
                {
                    CollectItem(hit.collider.gameObject);
                }
            }
        }
    }

    void CollectItem(GameObject item)
    {
        CollectibleItem collectible = item.GetComponent<CollectibleItem>();
        if (collectible != null)
        {
            collectible.Collect();
            if (itemCollectionManager != null)
            {
                itemCollectionManager.ItemCollected(collectible.itemType, collectible.value);
            }
            Destroy(item);
        }
    }

    void UpdateAnimator()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            anime.SetInteger("transition", 1);
        }
        else
        {
            anime.SetInteger("transition", 0);
        }
    }
}
