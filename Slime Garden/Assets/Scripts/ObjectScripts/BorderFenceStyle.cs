using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderFenceStyle : MonoBehaviour
{
    private Renderer render;
    private MeshFilter mesh;

    public List<FenceStyle> fenceStyles = new List<FenceStyle>();

    [System.Serializable]
    public class FenceStyle
    {
        public Mesh style;
        public Material[] materials;
    }

    private void Awake()
    {
        render = this.GetComponentInChildren<Renderer>();
        mesh = this.GetComponentInChildren<MeshFilter>();
    }

    public void ChangeStyle(int styleIndex, int matIndex)
    {
        mesh.sharedMesh = fenceStyles[styleIndex].style;
        render.material = fenceStyles[styleIndex].materials[matIndex];
    }
}
