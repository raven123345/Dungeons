using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamFX : MonoBehaviour
{
    public Shader shader;

    [Range(0f, 1f)]
    public float amount;

    private Material Material;

    Material material
    {
        get
        {
            if (!Material)
            {
                Material = new Material(shader);
                material.hideFlags = HideFlags.HideAndDontSave;
            }
            return Material;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!shader)
        {
            enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(shader)
        {
            material.SetFloat("_amount", amount);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
