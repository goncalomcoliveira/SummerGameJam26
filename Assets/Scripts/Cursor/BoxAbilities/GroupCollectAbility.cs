using System.Collections.Generic;
using UnityEngine;

public class GroupCollectAbility : MonoBehaviour, IBoxAbility {
    
    public void Execute(List<Collider2D> selectedObjects) {
        
        foreach (var selected in selectedObjects) {
            
            var coin =
                selected.GetComponent<Coin>();

            if (coin != null) {
                coin.Collect();
            }
        }
    }
}