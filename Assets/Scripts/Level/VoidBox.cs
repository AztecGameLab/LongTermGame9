using System;
using CactusBushes;
using UnityEngine;

public class VoidBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.TryGetComponent<CactusSpine>(out _) || other.gameObject.TryGetComponent<BushSeed>(out _))
        {
            other.transform.position += new Vector3(0f, 40f, 0f);
        }
    }
}
