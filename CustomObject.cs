using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObject : MonoBehaviour
{
    public string objectColor;
    public string objectShape; //atributos do et01
    public int numDeOlhos;

    public void SetAttributes(string color, string shape, int olhos)
    {
        objectColor = color;
        objectShape = shape;
        numDeOlhos = olhos;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer != null)
        {
           
            if (renderer.material != null)
            {
                renderer.material = new Material(renderer.material);

                switch (objectColor.ToLower())
                {
                    case "azul":
                        renderer.material.SetColor("_Color", new Color(0.329f, 0.369f, 0.953f)); // 545EF3
                        break;
                    case "vermelho":
                        renderer.material.SetColor("_Color", new Color(1f, 0f, 0.011f)); // FF0003
                        break;
                    case "verde":
                        renderer.material.SetColor("_Color", new Color(0.082f, 1f, 0f)); // 15FF00
                        break;
                    case "roxo":
                        renderer.material.SetColor("_Color", new Color(0.5f, 0f, 0.5f)); // Roxo
                        break;
                    default:
                        renderer.material.color = Color.white;
                        break;
                }
            }
            else
            {
                Debug.LogError("Renderer material is null.");
            }
        }
        else
        {
            Debug.LogError("SkinnedMeshRenderer component not found on the instantiated object or its children.");
        }
    }
}
