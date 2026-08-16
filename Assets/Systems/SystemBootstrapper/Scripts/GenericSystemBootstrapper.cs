using GoncaloMCOliveira.Singleton;
using UnityEngine;

public abstract class GenericSystemBootstrapper<T> : SystemBootstrapper where T : Component {
    
    [SerializeField] private GameObject systemPrefab;

    public override void CreateIfNeeded() {
        if (PersistentSingleton<T>.TryGetInstance() != null)
            return;
        Instantiate(systemPrefab);
    }
}