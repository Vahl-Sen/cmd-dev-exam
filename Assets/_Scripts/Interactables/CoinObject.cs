using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : BaseInteractable
{
    [SerializeField] private int _coinValue = 1;

    public override void OnInteract()
    {
        _playerStats.OnCoinIncrease(_coinValue);

        //StartCoroutine(_playerStats.OnCoinCollect(_coinValue));

        base.OnInteract();
    }
}
