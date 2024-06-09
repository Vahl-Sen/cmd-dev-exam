using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    private Transform _camera;
    private Transform _player;

    private float _startX;
    private float _startY;

    [SerializeField] private Vector2 _parallaxSpeed;

    private void Start()
    {
        _camera = Camera.main.transform;
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _startX = transform.position.x;
        _startY = transform.position.y;
    }

    private void FixedUpdate()
    {
        Vector2 relativeDist = (Vector2)_camera.position * _parallaxSpeed;
        float cameraMoveY = _player.position.y;

        transform.position = new Vector3(_startX + relativeDist.x, cameraMoveY + relativeDist.y, transform.position.z);
    }
}
