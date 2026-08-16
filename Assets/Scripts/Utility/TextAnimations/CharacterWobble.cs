using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWobble : MonoBehaviour
{
    //general
    private TMP_Text textMesh;
    private Mesh mesh;

    private Vector3[] vertices;

    //wobble animation
    public float verticalSpeed = 3f;
    public float horizontalSpeed = 3f;

    public float verticalRange = 1f;
    public float horizontalRange = 1f;

    //fall down animation
    public float startHeight;

    void Start() {
        textMesh = GetComponent<TMP_Text>();
    }

    void Update() {
        //RandomWobble();
        GoDown();
    }

    void RandomWobble(){

        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            Vector3 offset = Wobble(Time.time + i);
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time) {
        return new Vector2(Mathf.Sin(time * horizontalSpeed) * horizontalRange, Mathf.Cos(time * verticalSpeed) * verticalRange);
    }

    void GoDown(){

        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            Vector3 offset = Move(Time.time);

            if (startHeight >= MoveOffset(Time.time)){
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Move(float time) {
        return new Vector2(0f, startHeight - MoveOffset(time));
    }

    float MoveOffset(float time){
        return verticalSpeed * 10 * Time.time;
    }
}