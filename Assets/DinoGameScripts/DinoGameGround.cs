using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DinoGameGround : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float speed = DinoGameManager.Instance.gameSpeed/transform.localScale.x;
        meshRenderer.material.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }
}
