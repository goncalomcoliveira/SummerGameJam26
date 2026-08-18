using UnityEngine;

public class LogoTrail : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LogoController logo;

    [SerializeField]
    private GameObject trailPiecePrefab;

    [Header("Trail Settings")]
    [SerializeField]
    private float spawnInterval = 0.1f;

    private bool isUnlocked;

    private float spawnTimer;

    public bool IsUnlocked => isUnlocked;

    private void Update()
    {
        if (!isUnlocked)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnTrailPiece();

            spawnTimer = 0f;
        }
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;

        spawnTimer = 0f;

        Debug.Log("Logo Trail unlocked!");
    }

    private void SpawnTrailPiece()
    {
        GameObject piece =
            Instantiate(
                trailPiecePrefab,
                transform.position,
                Quaternion.identity
            );

        LogoTrailPiece trailPiece =
            piece.GetComponent<LogoTrailPiece>();

        if (trailPiece != null)
        {
            trailPiece.Initialize(logo);
        }
    }
}