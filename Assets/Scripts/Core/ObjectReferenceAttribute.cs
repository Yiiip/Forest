using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class ObjectReferenceAttribute : Attribute
{
    string relativePath;
    public ObjectReferenceAttribute(string relativePath)
    {
        this.relativePath = relativePath;
    }

    public static void GetReferences<T>(T context) where T : MonoBehaviour
    {
        Transform transform = context.transform;
        var target = transform.GetComponent<T>();
        Type targetType = typeof(T);
        var fieldInfos = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fieldInfos)
        {
            var attr = field.GetCustomAttribute<ObjectReferenceAttribute>();
            if (attr != null)
            {
                var trans = transform.Find(attr.relativePath);
                if (trans == null)
                {
                    Debug.LogError($"object not found at path {attr.relativePath}", context);
                    continue;
                }
                field.SetValue(target, trans.GetComponent(field.FieldType));
            }
        }


    }
}
