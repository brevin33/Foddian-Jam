using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControllableVerts : MonoBehaviour
{

    [SerializeField]
    float strength = .14f;

    [SerializeField]
    Edge middleEdge;

    [SerializeField]
    float maxMoveDist;

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    float returnPower;

    Camera mainCamera;

    bool trackingMousePos;

    float lerpValue;
    bool returning = false;

    CamerMovement camerMovement;

    Vector2 mousePos;
    Vector2 prevMousePos;
    Vector2 prevMovingVertPos;
    Vector2 movingVertPos;
    Vector2 clickPos;
    bool clicked;
    public static bool movedVert;

    MeshFilter meshFilter;

    EdgeCollider2D[] edgeColliders;

    Vector3[] basePos;

    public Vector2[] power { get; set; }

    float maxDist;

    bool justclicked;

    int[] fourthEdgeIndexs;

    int[] vertsOfClickedEdge;

    bool edgeClicked;

    Vector2 edgeFinalVertPos1;
    Vector2 edgeFinalVertPos2;

    Vector2 v1;
    Vector2 v2;


    private void Awake()
    {
        mainCamera = Camera.main;
        camerMovement = mainCamera.gameObject.GetComponent<CamerMovement>();
        meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        basePos = mesh.vertices;
        power = new Vector2[5];
        power[0] = Vector2.zero;
        power[1] = Vector2.zero;
        power[2] = Vector2.zero;
        power[3] = Vector2.zero;
        power[4] = Vector2.zero;
        maxDist = maxMoveDist * ((transform.localScale.x + transform.localScale.y)*.5f);
        edgeColliders = GetComponentsInChildren<EdgeCollider2D>();
        fourthEdgeIndexs = new int[2];
        fourthEdgeIndexs[0] = 3;
        fourthEdgeIndexs[0] = 0;
        mousePos = Vector2.one * 9999;
        clickPos = Vector2.one * 9999;
        Bounds b = mesh.bounds;
        b.Expand(10.8f);
        mesh.bounds = b;
    }

    private void Update()
    {
        if (!returning)
        {
            Vector2 m = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                clickPos = mousePos;
                clicked = true;
                trackingMousePos = true;
                justclicked = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                clicked = false;
                trackingMousePos = false;
                StartCoroutine(goBack());
            }
        }
    }

    private void FixedUpdate()
    {
        if (!returning)
        {
            if (trackingMousePos)
            {
                prevMousePos = mousePos;
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(mousePos, clickPos) > maxDist)
                {
                    mousePos = clickPos + ((mousePos - clickPos).normalized * maxDist);
                }
            }
        }
        Mesh mesh = meshFilter.mesh;
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector2 vert = verts[i];
            Vector2 baseVert = basePos[i];
            Vector2 baseVertWorld = transform.TransformPoint(baseVert);
            if (Vector2.Distance(baseVertWorld, clickPos) <= .8f)
            {
                edgeClicked = false;
                if (justclicked)
                {
                    v1 = vert;
                    if (i == 1 || i == 2)
                    {
                        fourthEdgeIndexs[0] = 2;
                        fourthEdgeIndexs[1] = 1;
                        middleEdge.index = fourthEdgeIndexs;
                        mesh.triangles = new int[] { 2, 1, 0, 2, 3, 1 };
                    }
                    else
                    {
                        fourthEdgeIndexs[0] = 0;
                        fourthEdgeIndexs[1] = 3;
                        middleEdge.index = fourthEdgeIndexs;
                        mesh.triangles = new int[] { 0, 3, 1, 3, 0, 2 };
                    }
                }
                prevMovingVertPos = vert;
                if (clicked)
                {
                    movedVert = true;
                    v1 += (Vector2)(transform.InverseTransformPoint(mousePos) - transform.InverseTransformPoint(prevMousePos));
                    vert = Vector2.MoveTowards(vert, v1, maxSpeed * Time.fixedDeltaTime);
                    edgeFinalVertPos1 = vert;
                }
                else
                {
                    vert = Vector2.Lerp(edgeFinalVertPos1, baseVert, lerpValue);
                }
                movingVertPos = vert;
                if (!returning)
                {
                    power[i] = (((movingVertPos - prevMovingVertPos) * strength) / Time.fixedDeltaTime);
                }
                else
                {
                    power[i] = (((movingVertPos - prevMovingVertPos) * strength) / Time.fixedDeltaTime) * returnPower;
                }
            }
            else
            {
                if (!edgeClicked)
                {
                    power[i] = Vector2.zero;
                    vert = baseVert;
                }
            }
            verts[i] = new Vector3(vert.x, vert.y, basePos[i].z);
        }
        if (edgeClicked)
        {
            Vector2 vert1 = verts[vertsOfClickedEdge[0]];
            Vector2 vert2 = verts[vertsOfClickedEdge[1]];
            Vector2 baseVert1 = basePos[vertsOfClickedEdge[0]];
            Vector2 baseVert2 = basePos[vertsOfClickedEdge[1]];
            if (justclicked)
            {
                v1 = vert1;
                v2 = vert2;
            }
            prevMovingVertPos = vert1;
            if (clicked)
            {
                movedVert = true;
                v1 += (Vector2)(transform.InverseTransformPoint(mousePos) - transform.InverseTransformPoint(prevMousePos));
                v2 += (Vector2)(transform.InverseTransformPoint(mousePos) - transform.InverseTransformPoint(prevMousePos));
                vert1 = Vector2.MoveTowards(vert1, v1, maxSpeed * Time.fixedDeltaTime);
                vert2 = Vector2.MoveTowards(vert2, v2, maxSpeed * Time.fixedDeltaTime);
                edgeFinalVertPos1 = vert1;
                edgeFinalVertPos2 = vert2;
            }
            else
            {
                vert1 = Vector2.Lerp(edgeFinalVertPos1, baseVert1, lerpValue);
                vert2 = Vector2.Lerp(edgeFinalVertPos2, baseVert2, lerpValue);
            }
            movingVertPos = vert1;
            if (!returning)
            {
                power[vertsOfClickedEdge[0]] = (((movingVertPos - prevMovingVertPos) * strength) / Time.fixedDeltaTime);
                power[vertsOfClickedEdge[1]] = (((movingVertPos - prevMovingVertPos) * strength) / Time.fixedDeltaTime);
            }
            else
            {
                power[vertsOfClickedEdge[0]] = (((movingVertPos - prevMovingVertPos) * strength) / Time.fixedDeltaTime) * returnPower;
                power[vertsOfClickedEdge[1]] = (((movingVertPos - prevMovingVertPos) * strength) / Time.fixedDeltaTime) * returnPower;
            }
            verts[vertsOfClickedEdge[0]] = new Vector3(vert1.x, vert1.y, 0);
            verts[vertsOfClickedEdge[1]] = new Vector3(vert2.x, vert2.y, 0);
        }




        mesh.vertices = verts;
        edgeColliders[0].SetPoints(new List<Vector2> { verts[0], verts[1] });
        edgeColliders[1].SetPoints(new List<Vector2> { verts[0], verts[2] });
        edgeColliders[2].SetPoints(new List<Vector2> { verts[3], verts[1] });
        edgeColliders[3].SetPoints(new List<Vector2> { verts[3], verts[2] });
        edgeColliders[4].SetPoints(new List<Vector2> { verts[fourthEdgeIndexs[0]], verts[fourthEdgeIndexs[1]] });
        justclicked = false;
        camerMovement.freeze = movedVert;
    }

    IEnumerator goBack()
    {
        movedVert = false;
        returning = true;
        lerpValue = 0;
        yield return new WaitUntil(goingBack);
        lerpValue = 1;
        edgeClicked = false;
        clickPos = mousePos;
        returning = false;
    }


    public Vector2 getVertexPosition(int index)
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] verts = mesh.vertices;
        return transform.TransformPoint(verts[index]);
    }
    bool goingBack()
    {
        lerpValue += Time.deltaTime * 4f;
        return lerpValue >= 1;
    }

    public void clickedEdge(int[] indexes)
    {
        edgeClicked = true;
        vertsOfClickedEdge = indexes;
    }

}
