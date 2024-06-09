using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BombObject : BaseInteractable
{
    [SerializeField] private int _damageValue = 1;
    private CinemachineVirtualCamera _camera;
    CinemachineBasicMultiChannelPerlin _perlin;

    private bool _active = false;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("CineCam").GetComponent<CinemachineVirtualCamera>();
        _perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_AmplitudeGain = 0f;
    }

    public override void OnInteract()
    {
        if (_active) return;

        _active = true;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _perlin.m_AmplitudeGain = 1f;

        _playerStats.OnHealthDecrease(_damageValue);
        
        yield return new WaitForSeconds(0.15f);

        _perlin.m_AmplitudeGain = 0f;
        Destroy(gameObject);

    }
}
