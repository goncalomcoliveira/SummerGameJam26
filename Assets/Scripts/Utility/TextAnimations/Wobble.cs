using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wobble : MonoBehaviour
{
        [Header("Speed Settings")]
    [Range(0f, 100f)] [SerializeField] private float verticalSpeed = 20f;
    [Range(0f, 100f)] [SerializeField] private float horizontalSpeed = 20f;

    [Header("Radius Settings")]
    [Range(0f, 50f)] [SerializeField] private float verticalRadius = 1f;
    [Range(0f, 50f)] [SerializeField] private float horizontalRadius = 1f;

    private TMP_Text textMesh;
    private Mesh mesh;

    private Vector3[] vertices;

    void Start() {
        textMesh = GetComponent<TMP_Text>();
    }

    void Update() {
        DoWobble();
    }

    void DoWobble(){

        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            Vector3 offset = GetOffset(Time.time + i);
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 GetOffset(float time) {
        return new Vector2(Mathf.Sin(time * horizontalSpeed) * horizontalRadius, Mathf.Cos(time * verticalSpeed) * verticalRadius);
    }
}