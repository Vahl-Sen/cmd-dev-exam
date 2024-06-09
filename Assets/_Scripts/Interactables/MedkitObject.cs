using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitObject : BaseInteractable
{
    [SerializeField] private int _healValue = 1;

    public override void OnInteract()
    {
        _playerStats.OnHealthIncrease(_healValue);

        base.OnInteract();
    }
}
