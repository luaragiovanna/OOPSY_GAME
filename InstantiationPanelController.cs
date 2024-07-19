using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InstantiationPanelController : MonoBehaviour
{
    public Text desafioText;
    public Button instantiateButton;
    public GameObject extraterrestrialPrefab;
    public GameObject infoPanel;
    public GameObject challengePanel;
    private Camera cinemachineCamera;
    private GameObject instantiatedObject;
    private Vector3 initialPosition;
    private Vector3 groundPosition;

    private string color = "roxo";
    private string method = "flutuar";
    private string methodValue = "true";
    public Button compositionButton;
    public GameObjectDialogue dialogueManager;
    public GameObject dialogueBackground;

    private void Start()
    {
        Debug.Log("InstantiationPanelController initialized.");

        if (instantiateButton != null)
        {
            instantiateButton.onClick.AddListener(InstantiateObject);
            instantiateButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Instantiate button not found on the challenge panel.");
        }

        if (compositionButton != null)
        {
            compositionButton.onClick.AddListener(OnCompositionButtonClicked);
            compositionButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Composition button not found on the challenge panel.");
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

        if (extraterrestrialPrefab == null)
        {
            Debug.LogError("Extraterrestrial prefab is not assigned in the Inspector.");
        }
    }

    public void ShowPseudocode(string pseudocode)
    {
        if (desafioText != null)
        {
            desafioText.text = pseudocode;
            gameObject.SetActive(true);
            Debug.Log("Pseudocode shown: " + pseudocode);
        }
        else
        {
            Debug.LogError("Desafio text not found on the challenge panel.");
        }
    }

    public void HidePseudocode()
    {
        gameObject.SetActive(false);
        Debug.Log("Pseudocode hidden");
    }

    public void SetColor(string color)
    {
        this.color = color;
        Debug.Log("Color set: " + color);
    }

    public void SetMethod(string method)
    {
        this.method = method;
        Debug.Log("Method set: " + method);
    }

    public void SetMethodValue(string methodValue)
    {
        this.methodValue = methodValue;
        Debug.Log("Method value set: " + methodValue);
    }

    public void EnableInstantiateButton()
    {
        if (instantiateButton != null)
        {
            instantiateButton.gameObject.SetActive(true);
            Debug.Log("Instantiate button enabled");
        }
    }

    public void DisableInstantiateButton()
    {
        if (instantiateButton != null)
        {
            instantiateButton.gameObject.SetActive(false);
            Debug.Log("Instantiate button disabled");
        }
    }

    private void InstantiateObject()
    {
        Debug.Log("InstantiateObject called");
        if (extraterrestrialPrefab == null)
        {
            Debug.LogError("Extraterrestrial prefab is not assigned.");
            return;
        }

        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            Debug.Log("Previous instantiated object destroyed");
        }

        if (cinemachineCamera != null)
        {
            Vector3 cameraForward = cinemachineCamera.transform.forward;
            Vector3 spawnPosition = cinemachineCamera.transform.position + cameraForward * 3.0f + new Vector3(0, -1, 0);
            instantiatedObject = Instantiate(extraterrestrialPrefab, spawnPosition, Quaternion.identity);
            instantiatedObject.SetActive(true);
            Debug.Log("New object instantiated");

            initialPosition = instantiatedObject.transform.position;
            groundPosition = new Vector3(initialPosition.x, -2.07f, initialPosition.z);

            UpdateInfoPanel();

            DisableInstantiateButton();

            StartCoroutine(HandlePostInstantiationDialogue());

            challengePanel.SetActive(false);
            Debug.Log("Challenge panel deactivated");
        }
        else
        {
            Debug.LogError("Cinemachine camera not found.");
        }
    }

    private void UpdateInfoPanel()
    {
        if (infoPanel != null)
        {
            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.targetObject = instantiatedObject.transform;
                panelController.offset = new Vector3(0, 2, 0);
                infoPanel.SetActive(true);

                string objectInfo = $"ser:Extraterrestre\nmodelo: circular\ncor: {color}\nnúmero de olhos: 1\nmétodo: {method}({methodValue})\n";
                panelController.UpdateInfoText(objectInfo);
                panelController.SetStatic();

                RectTransform rectTransform = panelController.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(513.6f, 313f);
                    Debug.Log("Info panel size set");
                }
            }
        }
    }

    private IEnumerator HandlePostInstantiationDialogue()
    {
        Debug.Log("HandlePostInstantiationDialogue started");

        if (dialogueManager != null && dialogueBackground != null)
        {
            dialogueBackground.SetActive(true);
            dialogueManager.gameObject.SetActive(true);

            dialogueManager.StartDialogue(new string[] { "Uau!", "Posso levá-lo para a Terra mesmo?" });
            Debug.Log("Player dialogue started: Uau!, Posso levá-lo para a Terra mesmo?");
            yield return new WaitForSeconds(4);

            dialogueManager.StartDialogue(new string[] { "Sim! O objeto 'ser' agora é seu!" });
            Debug.Log("Alien dialogue started: Sim! O objeto 'ser' agora é seu!");
            yield return new WaitForSeconds(2);

            compositionButton.gameObject.SetActive(true);
            Debug.Log("Composition button shown");
        }
        else
        {
            Debug.LogError("Dialogue manager or background is not assigned.");
        }
    }

    public void OnCompositionButtonClicked()
    {
        Debug.Log("Composition button clicked");
        StartCoroutine(HandleCompositionDialogue());
    }

    private IEnumerator HandleCompositionDialogue()
    {
        Debug.Log("Starting HandleCompositionDialogue");

      
        if (instantiatedObject != null)
        {
            instantiatedObject.SetActive(false);
            Debug.Log("Instantiated object deactivated");
        }

        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            Debug.Log("Info panel hidden");
        }

    
        dialogueManager.StartDialogue(new string[] { "O que é esse botão?" });
        Debug.Log("Player dialogue started: O que é esse botão?");
        yield return new WaitForSeconds(2);


        dialogueManager.StartDialogue(new string[] { "Composição!", "É quando um objeto é responsável por outro!", "Neste caso, você pode fazer o que quiser com o objeto 'ser'." });
        Debug.Log("Alien dialogue started: Composição!, É quando um objeto é responsável por outro!, Neste caso, você pode fazer o que quiser com o objeto 'ser'.");
        yield return new WaitForSeconds(4);

    
        dialogueManager.StartDialogue(new string[] { "Obrigada, agora finalizei minha missão." });
      
        yield return new WaitForSeconds(3);

        dialogueManager.StartDialogue(new string[] { "Espero que volte nos visitar!", "Ainda tem muito sobre POO que você pode aprender!" });
        Debug.Log("Alien dialogue started: Espero que volte nos visitar!, Ainda tem muito sobre POO que você pode aprender!");
        yield return new WaitForSeconds(4);

        compositionButton.gameObject.SetActive(false);
        Debug.Log("Composition button hidden");
    }
}
