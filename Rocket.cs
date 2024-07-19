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
            Debug.Log("Miss�o completa, vamos para casa!");
            
        }
        else
        {
            Debug.Log("N�o posso ir agora... Tenho que completar minha miss�o.");
            if (dialogueManager != null)
            {
                dialogueManager.ShowRocketDialogue();
            }
        }
    }
}
