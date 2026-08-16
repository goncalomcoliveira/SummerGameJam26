using UnityEngine;

public class SceneManagementExample : MonoBehaviour {
    
    public void LoadVideoScene() {
        SceneEvents.RaiseLoadScene("SplashVideoScene");
    }
}
