using System.Collections;
using UnityEngine;

public class LogoShield : MonoBehaviour, IDamageBlocker {
    
    [Header("Settings")]
    [SerializeField] private float regenerationTime = 5f;

    [Header("Visual")]
    [SerializeField] private GameObject shieldVisual;

    private bool isUnlocked;
    private bool shieldActive;
    private Coroutine regenerationRoutine;

    public bool IsUnlocked => isUnlocked;
    public bool IsShieldActive => shieldActive;

    private void Awake()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;

        RestoreShield();

        Debug.Log("Logo Shield unlocked!");
    }

    public bool TryBlockDamage()
    {
        if (!isUnlocked)
            return false;

        if (!shieldActive)
            return false;

        BreakShield();

        return true;
    }

    private void BreakShield()
    {
        shieldActive = false;

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }

        if (regenerationRoutine != null)
        {
            StopCoroutine(regenerationRoutine);
        }

        regenerationRoutine =
            StartCoroutine(RegenerateShield());
    }

    private IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(
            regenerationTime
        );

        RestoreShield();

        regenerationRoutine = null;
    }

    private void RestoreShield()
    {
        shieldActive = true;

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(true);
        }
    }
}