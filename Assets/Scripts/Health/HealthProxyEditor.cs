using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
[CustomEditor(typeof(HealthProxy))]
public class HealthProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var healthProxy = target as HealthProxy;

        healthProxy.targetHealth = (Health)EditorGUILayout.ObjectField("Target Health", healthProxy.targetHealth, typeof(Health), true);
    }
}
#endif