using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class GlobalMaterialSettings : MonoBehaviour
{
    public List<ColorList> globalColors = new List<ColorList>();
	public List<FloatList> globalFloats = new List<FloatList>();
	public List<VectorList> globalVectors = new List<VectorList>();

    [Serializable]
    public struct ColorList
    {
        public string globalName;
        public Color globalValue;
    }

    [Serializable]
    public struct FloatList
    {
        public string globalName;
        public float globalValue;
    }

    [Serializable]
    public struct VectorList
    {
        public string globalName;
        public Vector4 globalValue;
    }

    private void OnEnable()
    {
        UpdateGlobalUniform();
    }

    private void OnDestroy()
    {
        globalColors.Clear();
        globalFloats.Clear();
        globalVectors.Clear();
    }

    public void UpdateGlobalUniform()
    {
        foreach (var c in globalColors)
        {
            Shader.SetGlobalColor(c.globalName, c.globalValue);
        }

        foreach (var f in globalFloats)
        {
            Shader.SetGlobalFloat(f.globalName, f.globalValue);
        }

        foreach (var v in globalVectors)
        {
            Shader.SetGlobalVector(v.globalName, v.globalValue);
        }
    }
}
