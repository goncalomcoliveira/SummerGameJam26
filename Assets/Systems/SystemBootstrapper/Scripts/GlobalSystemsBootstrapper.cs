using UnityEngine;

public class GlobalSystemsBootstrapper : MonoBehaviour {
    
    [SerializeField] private SystemBootstrapper[] systemBootstrappers;
    
    private void Awake() {
        CreateBootstrappersIfNeeded();
    }

    private void CreateBootstrappersIfNeeded() {
        foreach (var systemBootstrapper in systemBootstrappers) {
            systemBootstrapper.CreateIfNeeded();
        }
    }
}