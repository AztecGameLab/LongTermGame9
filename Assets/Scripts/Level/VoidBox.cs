using System;
using UnityEngine;

public class VoidBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position += new Vector3(0f, 40f, 0f);
    }
}
