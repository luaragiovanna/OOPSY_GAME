using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool missionComplete = false;
    public GameObjectDialogue dialogueManager; // ref ao objeto DialogueManager

    public void CompleteMission()
    {
        missionComplete = true;
    }

    void OnMouseDown()
    {
        if (missionComplete)
        {
            Debug.Log("Missão completa, vamos para casa!");
            
        }
        else
        {
            Debug.Log("Não posso ir agora... Tenho que completar minha missão.");
            if (dialogueManager != null)
            {
                dialogueManager.ShowRocketDialogue();
            }
        }
    }
}
