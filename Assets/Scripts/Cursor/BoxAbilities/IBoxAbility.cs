using System.Collections.Generic;
using UnityEngine;

public interface IBoxAbility {
    void Execute(List<Collider2D> selectedObjects);
}