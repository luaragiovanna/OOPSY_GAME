using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alien : MonoBehaviour
{
    public GameObjectDialogue dialogueManager; //gerenciador das falas
    public GameObject extraterrestrialPrefab; //prefab do et01
    public GameObject infoPanel; //painel de informações do et01
    public Text infoText;
    public Button blueButton; //botoes atributos
    public Button greenButton;
    public Button redButton;
    public GameObject optionsPanel; // Painel de opções de diálogo
    public Button option1Button;
    public Button option2Button;
    public Button methodButton; // Botão para acionar o método
    public Button understoodButton; // Botão "Entendi"
    public GameObject classPanel; // Painel da classe
    public GameObject challengePanel;
    private GameObject instantiatedObject;
    private bool isTalking = false; // Flag para verificar se o alien está falando
    private bool isFloating = true; // Estado de flutuação do ET01
    private Camera cinemachineCamera;
    private Vector3 initialPosition; // Posição inicial do ET01
    private Vector3 groundPosition; // Posição no solo
    private ClassPanelController classPanelController; // Controlador do painel da classe
    private InstantiationPanelController instantiationPanelController;
    public ItemCollectionManager itemCollectionManager; 
    public Animator animator;

  


    void Start()
    {
        GameObject cameraObject = GameObject.FindWithTag("CinemachineCamera"); //achar a camera pra colocar os objetos
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
        animator = GetComponent<Animator>();
        infoPanel.SetActive(false); //apinel de informações do obj inicialmente desativado
        optionsPanel.SetActive(false);
        methodButton.gameObject.SetActive(false);
        understoodButton.gameObject.SetActive(false);
        classPanel.SetActive(false);

        blueButton.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(false);
        redButton.gameObject.SetActive(false);

        blueButton.onClick.AddListener(() => ChangeColor(Color.blue, "Azul"));//botoes de atributos
        greenButton.onClick.AddListener(() => ChangeColor(Color.green, "Verde"));
        redButton.onClick.AddListener(() => ChangeColor(Color.red, "Vermelho"));

        option1Button.onClick.AddListener(() => OnAttributeResponse(0));
        option2Button.onClick.AddListener(() => OnAttributeResponse(1));
        methodButton.onClick.AddListener(() => ToggleFloat());
        understoodButton.onClick.AddListener(() => EndMethodPhase());

        classPanelController = classPanel.GetComponent<ClassPanelController>();
        if (classPanelController == null)
        {
            Debug.LogError("ClassPanelController not found on classPanel.");
        }

    
        instantiationPanelController = challengePanel.GetComponent<InstantiationPanelController>();
        if (instantiationPanelController != null)
        {
            instantiationPanelController.HidePseudocode();
            challengePanel.SetActive(false); 
        }
        else
        {
            Debug.LogError("InstantiationPanelController not found on challengePanel.");
        }
    }

    public void OnMouseDown()
    {
        if (isTalking)
        {
            Debug.Log("Alien is still talking.");
            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue manager reference is not set in the Alien script.");
            return;
        }

        StartCoroutine(ExplainAndCreateObject());
    }

    private IEnumerator ExplainAndCreateObject()
    {
        isTalking = true;

        string[] explanationDialogues = {
        " ... Oi?",
        " Olá! Sou o OBJEX! ",
        " Onde estou??",
        "Este é o Planeta POO!",
        " POO?",
        " POO é o planeta ORIENTADO A OBJETOS!!",
        " ?????",
        " Deixe-me te mostrar como funcionam as coisas por aqui!.",
        " ok..",
        " Os seres aqui são chamados de OBJETOS!",
        " Um objeto tem ATRIBUTOS como cor e forma.",
        " Vou criar um objeto do tipo Extraterrestre."
    };
        dialogueManager.StartDialogue(explanationDialogues);

        yield return new WaitForSeconds(2 * explanationDialogues.Length);

        string[] creationDialogues = {
        "Veja, este é o nosso objeto.",
    };
        dialogueManager.StartDialogue(creationDialogues);

        yield return new WaitForSeconds(2 * creationDialogues.Length);

        if (extraterrestrialPrefab == null)
        {
            Debug.LogError("Extraterrestrial prefab is not assigned.");
            isTalking = false;
            yield break;
        }

        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
        }

        if (cinemachineCamera != null)
        {
            Vector3 cameraForward = cinemachineCamera.transform.forward; //pega posicao da camera
            Vector3 spawnPosition = cinemachineCamera.transform.position + cameraForward * 3.0f + new Vector3(0, -1, 0); //pra calcular onde o et01 será instanciado
            instantiatedObject = Instantiate(extraterrestrialPrefab, spawnPosition, Quaternion.identity);
            instantiatedObject.SetActive(true);

            // Defina a posição inicial e a posição no solo
            initialPosition = instantiatedObject.transform.position;
            groundPosition = new Vector3(initialPosition.x, -2.07f, initialPosition.z);

            CustomObject customObject = instantiatedObject.GetComponent<CustomObject>();
            if (customObject != null)
            {
                customObject.SetAttributes("roxo", "circular", 1); //atributos do et01 setados
            }

            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.targetObject = instantiatedObject.transform; 
                panelController.offset = new Vector3(0, 2, 0);

                //posição do painel de informações em relação ao ET01
                Vector3 panelPosition = instantiatedObject.transform.position + new Vector3(2, 0, 0); 
                infoPanel.transform.position = panelPosition;

                
                RectTransform rectTransform = infoPanel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(429.1f, 268.5f); //  tamanho do painel
                    rectTransform.anchoredPosition3D = new Vector3(-500.68f, 190.02f, -12.2f);
                    Debug.Log("Info panel size set");
                }

                infoPanel.SetActive(true);
            }

            DisplayObjectInfo(instantiatedObject);
        }
        else
        {
            Debug.LogError("Cinemachine camera not found.");
            isTalking = false;
            yield break;
        }

        yield return new WaitForSeconds(3);

        string[] attributeDialogues = {
        "   São os ATRIBUTOS do objeto!",
        "   Esse objeto, do tipo extraterrestre, se chama ET01!",
        "   Você gostaria de saber mais sobre atributos?"
    };
        dialogueManager.StartDialogue(attributeDialogues);

        yield return new WaitForSeconds(3); // tempo de exibição da pergunta

        ShowOptionsPanel(); // mostrar opções após a pergunta
    }


    private void ShowOptionsPanel()
    {
        Debug.Log("Showing options panel.");
        dialogueManager.EndDialogue(); // remove o texto de diálogo antes de mostrar os botões
        optionsPanel.SetActive(true);
    }

    public void OnAttributeResponse(int responseIndex)
    {
        optionsPanel.SetActive(false);
        string[] responses = {
        "Uau! Muito legal!",
        "Por favor!"
    };

        if (responseIndex >= 0 && responseIndex < responses.Length)
        {
            string response = responses[responseIndex];
            if (responseIndex == 0) // "Legal!" vai direto pra metodo
            {
                string[] followUpDialogues = { response, "Concordo!" };
                dialogueManager.StartDialogue(followUpDialogues);
                StartCoroutine(FinalizeInteraction());
            }
            else if (responseIndex == 1) // "Atributos?" explica e mostra botoes
            {
                string[] followUpDialogues = { response, "  Atributos são características..", "Que objetos de uma mesma classe compartilham!", "Vamos! agora experimente!", "Altere o atributo cor!" };
                dialogueManager.StartDialogue(followUpDialogues);

                // Assine o evento para saber quando o diálogo terminar
                dialogueManager.OnDialogueEnded += ShowColorButtonsWithDelay;
            }
        }
    }

    private void ShowColorButtonsWithDelay()
    {
        StartCoroutine(ShowColorButtonsCoroutine());
    }

    private IEnumerator ShowColorButtonsCoroutine()
    {
        // 1,5 segundos antes de mostrar os botões
        yield return new WaitForSeconds(1.5f);

        blueButton.gameObject.SetActive(true);
        greenButton.gameObject.SetActive(true);
        redButton.gameObject.SetActive(true);

 
        dialogueManager.OnDialogueEnded -= ShowColorButtonsWithDelay;
    }



    private IEnumerator FinalizeInteraction()
    {
        yield return new WaitForSeconds(3); // tempo para  "Concordo!"
        string[] finalDialogues = { "..." };
        dialogueManager.StartDialogue(finalDialogues);

        yield return new WaitForSeconds(1.5f); // add uma pausa antes de iniciar a explicação do método
        isTalking = false;
        StartCoroutine(ExplainMethod());
    }

    private IEnumerator ShowColorButtons()
    {
        yield return new WaitForSeconds(1.5f);// tempo para o diálogo "Vamos! agora experimente....
        blueButton.gameObject.SetActive(true);
        greenButton.gameObject.SetActive(true);
        redButton.gameObject.SetActive(true);
    }

    private void ChangeColor(Color color, string colorName)
    {
        if (instantiatedObject == null)
        {
            Debug.LogError("Instantiated object is null.");
            return;
        }

        SkinnedMeshRenderer renderer = instantiatedObject.GetComponentInChildren<SkinnedMeshRenderer>(); //componente pra pegar material do prefab
        if (renderer != null)
        {
            // instancia o material do prefab
            renderer.material = new Material(renderer.material);

            // muda a cor do Albedo com base no nome da cor clicada
            switch (colorName.ToLower())
            {
                case "vermelho":
                    renderer.material.SetColor("_Color", new Color(1f, 0f, 0.011f)); // FF0003
                    break;
                case "verde":
                    renderer.material.SetColor("_Color", new Color(0.082f, 1f, 0f)); // 15FF00
                    break;
                case "azul":
                    renderer.material.SetColor("_Color", new Color(0.329f, 0.369f, 0.953f)); // 545EF3
                    break;
                default:
                    renderer.material.color = color;
                    break;
            }
            Debug.Log($"Material albedo color changed to {colorName}");

            CustomObject customObject = instantiatedObject.GetComponent<CustomObject>();
            if (customObject != null)
            {
                customObject.SetAttributes(colorName.ToLower(), customObject.objectShape, customObject.numDeOlhos);
                DisplayObjectInfo(instantiatedObject);
            }
        }
        else
        {
            Debug.LogError("No SkinnedMeshRenderer component found on the instantiated object or its children.");
        }

        blueButton.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(false);
        redButton.gameObject.SetActive(false);

        string[] finalDialogues = {
        "   Agora, você alterou o atributo cor do extraterrestre!",
        "   Muito legal!!"
    };
        dialogueManager.StartDialogue(finalDialogues);
        isTalking = false; 
        StartCoroutine(ExplainMethod());
    }



    private IEnumerator ExplainMethod()
    {
        string[] methodDialogues = {
            "   Agora que você sabe o que atributos são, vamos para os métodos!",
            "   Métodos????",
            "   Lembra deste objeto né? O ET01! ",
            "   Consegue ver que agora ele tem um método?",
            "   Consegue ver esse 'true'?",
            "   Sim...",
            "   Significa que o método está ativo!",
            "   Vamos! Experimente acionar o método pra ver o que acontece!"
        };

        yield return StartCoroutine(StartDialogueWithPause(methodDialogues, 1.5f));

        methodButton.gameObject.SetActive(true);
        understoodButton.gameObject.SetActive(true);

        DisplayMethodInfo(instantiatedObject);
    }
    private IEnumerator StartDialogueWithPause(string[] dialogues, float pauseTime)
    {
        foreach (string dialogue in dialogues)
        {
            dialogueManager.StartDialogue(new string[] { dialogue });
            yield return new WaitForSeconds(pauseTime);
        }
        dialogueManager.EndDialogue();
    }

    private void ToggleFloat()
    {
        isFloating = !isFloating;

        
        if (isFloating)
        {
            instantiatedObject.transform.position = initialPosition;
            animator.SetBool("isFloating", true);
        }
        else
        {
            instantiatedObject.transform.position = groundPosition;
            animator.SetBool("isFloating", false);
            StartCoroutine(PlayDieAnimation());
        }

        DisplayMethodInfo(instantiatedObject);
    }

    private IEnumerator PlayDieAnimation()
    {
        animator.Play("Die");
        yield return new WaitForSeconds(1.0f); 
        animator.speed = 0; 
    }

    private void DisplayMethodInfo(GameObject obj)
    {
        CustomObject customObject = obj.GetComponent<CustomObject>();
        if (customObject != null)
        {
            string objectInfo = $"ET01:Extraterrestre\n" +
                                $"modelo: {customObject.objectShape}\n" +
                                $"cor: {customObject.objectColor}\n" +
                                $"número de olhos: {customObject.numDeOlhos}\n" +
                                $"flutuar: {isFloating}\n";

            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.UpdateInfoText(objectInfo);
                panelController.SetStatic(); 
            }
        }
    }

    private void DisplayObjectInfo(GameObject obj)
    {
        CustomObject customObject = obj.GetComponent<CustomObject>();
        if (customObject != null)
        {
            string objectInfo = $"ET01:Extraterrestre\n" +
                                $"modelo: {customObject.objectShape}\n" +
                                $"cor: {customObject.objectColor}\n" +
                                $"número de olhos: {customObject.numDeOlhos}\n";

            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.UpdateInfoText(objectInfo);
                panelController.SetStatic(); // deixar painel estatico
            }
        }
    }

    private void EndMethodPhase()
    {
        methodButton.gameObject.SetActive(false);
        understoodButton.gameObject.SetActive(false);
        infoPanel.SetActive(false);
        if (instantiatedObject != null)
        {
            instantiatedObject.SetActive(false);
        }
        dialogueManager.EndDialogue();
        StartCoroutine(ExplainClasses());
    }

    private IEnumerator ExplainClasses()
    {
        string[] classDialogues = {
            "  Agora que sabemos a estrutura de um objeto...", "precisamos saber de onde ele vem...",
            "  Primeiro, temos a nossa SUPERCLASSE!", 
            "  Ela se chama PlanetaPOO!",
            "  E dentro dessa superclasse temos a classe",
            "  EXTRATERRESTRE!",

            "  Todos os seres deste planeta pertencem a SUPERCLASSE..",
            "  Mas apenas eu e o ET01 pertencemos a classe",
            "  EXTRATERRESTRE!",
            " Veja:"
        };
        dialogueManager.StartDialogue(classDialogues);

        yield return new WaitForSeconds(2 * classDialogues.Length);

        if (classPanelController != null)
        {
            string classInfo = "Extraterrestre\n- cor: string\n- modelo: string\n- numOlhos: int\n+ flutuar(): boolean";
            classPanelController.ShowClassInfo(classInfo);
            Debug.Log("Class panel activated and showing info.");
        }
        else
        {
            Debug.LogError("ClassPanelController is not assigned.");
        }

        yield return new WaitForSeconds(3);

        string[] playerQuestion = {
            " Então você também é um objeto do tipo EXTRATERRESTRE???"
        };
        dialogueManager.StartDialogue(playerQuestion);

        yield return new WaitForSeconds(3);

        string[] alienResponse = {
            "Sim! Assim como ET01 sou um objeto desta classe",
            "Isso significa que posso usar o método flutuar!"
        };
        dialogueManager.StartDialogue(alienResponse);

        yield return new WaitForSeconds(2 * alienResponse.Length);

        // fazer o alien flutuar por 3 segundos e depois voltar ao chão
        Vector3 alienInitialPosition = transform.position;
        Vector3 floatingPosition = new Vector3(alienInitialPosition.x, -1.16f, alienInitialPosition.z);

        transform.position = floatingPosition;
        yield return new WaitForSeconds(3);
        transform.position = alienInitialPosition;

        if (classPanelController != null)
        {
            classPanelController.HideClassInfo(); // esconder o painel da classe após a demonstração
        }
        string[] newDialogues = {
        "Muito legal, mas tenho uma missão",
        "Levar algo para a Terra!",
        "Hmmm...",
        "OK! Mas não vai ser tão fácil",
        "O que preciso fazer?",
        "Instanciar um objeto Extraterrestre!",
        "COMO??",
        "Colete os itens que faltam para completar a sua missão"
    };
        yield return StartCoroutine(StartDialogueWithPause(newDialogues, 1.5f));
      
        ShowChallengePanel();
        
        string[] newObjectDialogue = {
            

            "Um novo ser está sendo instânciado",
            
        };
        dialogueManager.StartDialogue(newObjectDialogue);

        yield return new WaitForSeconds(2 * newObjectDialogue.Length);

        InstantiationPanelController instantiationPanelController = FindObjectOfType<InstantiationPanelController>();
        if (instantiationPanelController != null)
        {
            instantiationPanelController.ShowPseudocode("ser = novo Extraterrestre()\nser.cor = [ ??? ]\nser.numOlhos = 1\nser.modelo = circular\n---------Métodos---------\nser.[ ]();");
        }

        yield return new WaitForSeconds(3);

        string[] searchItemsDialogue = {
            "Procure o valor do atributo cor, o método e seu parâmetro"
        };
        dialogueManager.StartDialogue(searchItemsDialogue);

        yield return new WaitForSeconds(2 * searchItemsDialogue.Length);

        // esconder o painel de instanciação após a fala "Procure o valor do atributo cor, o método e seu parâmetro"
        if (instantiationPanelController != null)
        {
            instantiationPanelController.HidePseudocode();
        }
    }

    private void ShowChallengePanel()
    {
        if (instantiationPanelController != null)
        {
            Debug.Log("Showing challenge panel.");
            challengePanel.SetActive(true); // ativa o painel quando necessário
            instantiationPanelController.ShowPseudocode("ser = novo Extraterrestre()\nser.cor = [ ??? ]\nser.numOlhos = 1\nser.modelo = circular\n---------Métodos---------\nser.[ ]();");
        }
        else
        {
            Debug.LogError("InstantiationPanelController not found on challengePanel.");
        }
    }
    
}