using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class HUDScreen : MonoBehaviour
{
    [SerializeField] StatsDataSO _stats;
    [SerializeField] TextMeshProUGUI _coinLabel;
    [SerializeField] TextMeshProUGUI _healthLabel;
    [SerializeField] GameObject _deathPanel;

    private int _currentCoins;
    private int _currentHealth;

    private void Awake()
    {
        UpdateHealth();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => _stats.CurrentCoins != _currentCoins)
            .Subscribe(_ => UpdateCoinCount());

        this.UpdateAsObservable()
            .Where(_ => _stats.CurrentHealth != _currentHealth)
            .Subscribe(_ => UpdateHealth());

        this.UpdateAsObservable()
            .Where(_ => _stats.PlayerDead == true)
            .Subscribe(_ => OnPlayerDeath());
    }

    private void UpdateCoinCount()
    {
        _currentCoins = _stats.CurrentCoins;
        _coinLabel.text = _stats.CurrentCoins.ToString();
    }

    private void UpdateHealth()
    {
        _currentHealth = _stats.CurrentHealth;
        _healthLabel.text = _stats.CurrentHealth.ToString();
    }

    private void OnPlayerDeath()
    {
        _deathPanel.SetActive(true);
    }
}
