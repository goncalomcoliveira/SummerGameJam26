using GoncaloMCOliveira.AudioSystem;
using UnityEngine;

public class AOLDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorCloseClip;

    private Sound doorOpenSound;
    private Sound doorCloseSound;
    
    private void Start() {
        doorOpenSound = new Sound(doorOpenClip);
        doorCloseSound = new Sound(doorCloseClip);
    }
    
    
    public void Open() {
        doorOpenSound.Play();
        if (animator != null) {
            animator.SetTrigger("Open");
        }
    }

    public void Close() {
        doorCloseSound.Play();
        if (animator != null) {
            animator.SetTrigger("Close");
        }
    }

    public void SetYPosition(float y) {
        Vector3 position =
            transform.position;

        position.y = y;

        transform.position = position;
    }
}