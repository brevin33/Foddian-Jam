using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public int[] index;

    [SerializeField]
    ControllableVerts controllableVerts;

    private void OnMouseDown()
    {
        controllableVerts.clickedEdge(index);
    }
}
