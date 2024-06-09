using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    [Header("Key Components")]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Rigidbody2D _body;
    [SerializeField] protected CapsuleCollider2D _collider;
}
