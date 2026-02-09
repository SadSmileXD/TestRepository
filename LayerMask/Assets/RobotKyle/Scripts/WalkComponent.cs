using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkComponent : MonoBehaviour, IMove
{
    [SerializeField] private float _speed = 5f;

    public float Invoke(Vector2 input)
    {
        if(input.magnitude <= 0f) return 0f;

        Vector3 dir = Vector3.forward * input.y;
        transform.Translate(dir * _speed * Time.deltaTime);

        return 0.5f;
    }
}
