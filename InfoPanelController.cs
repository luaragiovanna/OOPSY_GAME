using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 offset;
    private RectTransform rectTransform;
    private Camera cinemachineCamera; //flag da camera 
    public Text infoText;
    private bool isStatic = false; // Flag para tornar o painel estático

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on InfoPanel.");
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
    }

    void Update()
    {
        if (isStatic || targetObject == null || rectTransform == null || cinemachineCamera == null)
        {
            return;
        }

        Vector3 screenPosition = cinemachineCamera.WorldToScreenPoint(targetObject.position + offset);
        rectTransform.position = screenPosition;
    }

    public void UpdateInfoText(string text)
    {
        if (infoText != null)
        {
            infoText.text = text;

            RectTransform rectTransform = GetComponent<RectTransform>();
            float preferredWidth = infoText.preferredWidth + 20;
            float preferredHeight = infoText.preferredHeight + 20;
            rectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);
        }
        else
        {
            Debug.LogError("InfoText is not assigned.");
        }
    }

   
    public void SetStatic()
    {
       
        this.enabled = false;
    }

}
