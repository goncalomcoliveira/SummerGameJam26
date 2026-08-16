using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fall : MonoBehaviour
{
    [Header("Fall Settings")]
    [Range(0f, 100f)] [SerializeField] private float fallSpeed = 20f;
    [SerializeField] private float fallHeight = 100f;

    private float timer = 0f;

    [SerializeField] private bool isFalling = true;

    private TMP_Text textMesh;
    private Mesh mesh;

    private Vector3[] vertices;
    
    void Start() {
        textMesh = GetComponent<TMP_Text>();
    }

    public void Update(){

        DoFall();

        if (!isFalling){
            timer = 0f;
            isFalling = true;
        }
    }

    void DoFall(){

        if (isFalling){
            timer += Time.deltaTime;
        }

        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        Debug.Log(MoveOffset(timer));

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            Vector3 offset = FallOffset(timer);

            if (fallHeight >= MoveOffset(timer)){
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 FallOffset(float time) {
        return new Vector2(0f, fallHeight - MoveOffset(time));
    }

    float MoveOffset(float time){
        return fallSpeed * 10 * time;
    }

    public void TriggerFall(){
        isFalling = false;
    }
}