using UnityEngine;

public class Coin : MonoBehaviour {
    public int value = 1;

    public void Collect() {
        if (GameManager.Instance != null) {
            GameManager.Instance.AddCoins(value);
        }

        Destroy(gameObject);
    }
}