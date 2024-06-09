using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractable : MonoBehaviour, IInteract
{
    [SerializeField] protected StatsDataSO _playerStats;

    public virtual void OnInteract()
    {
        Destroy(gameObject);
    }
}
