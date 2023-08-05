using UnityEditor.UIElements;
using UnityEngine;

public class ball : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 mousePos;
    Vector2 startMousePos;
    Camera mainCamera;
    bool clicked;
    [SerializeField]
    int jumps = 2;
    LineRenderer lineRenderer;
    [SerializeField]
    float lineDist;
    [SerializeField]
    float hitPower;

    bool hitting;

    float smallDelay;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        smallDelay = 0;
        hitting = false;
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            clicked = true;
        }
        else if (!ControllableVerts.movedVert && Input.GetMouseButton(0))
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            drawPowerLine();
        }
        else if (Input.GetMouseButtonUp(0) && hitting)
        {
            smallDelay = 0;
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            shootBall();
        }
    }

    private void drawPowerLine()
    {
        smallDelay += Time.deltaTime;
        if (smallDelay < 0.001f)
        {
            return;
        }
        hitting = true;
        Vector2 lineEnd = mousePos - startMousePos;
        lineEnd = lineEnd.normalized * Mathf.Min(lineDist,lineEnd.magnitude);
        lineEnd += (Vector2)transform.position;
        Vector3[] linePositions = new Vector3[]
        {
            lineEnd, transform.position
        };
        lineRenderer.SetPositions(linePositions);
        lineRenderer.enabled = true;
    }

    private void shootBall()
    {
        Vector2 hitLine = mousePos - startMousePos;
        hitLine = hitLine.normalized * Mathf.Min(lineDist, hitLine.magnitude);
        hitLine.x = -hitLine.x;
        hitLine.y = -hitLine.y;
        rb.velocity = rb.velocity * .3f;
        rb.AddForce(hitLine * hitPower);
        hitting = false;
        lineRenderer.enabled = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (hitting)
        {
            return;
        }
        GameObject other = collision.gameObject;
        if (other.tag == "movingVert")
        {
            ControllableVerts mv = other.GetComponentInParent<ControllableVerts>();
            int[] edgeIndex = other.GetComponent<Edge>().index;
            Vector2 avgNormal = Vector3.zero;
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            for (int i = 0; i < contacts.Length; i++)
            {
                ContactPoint2D contact = contacts[i];
                avgNormal += contact.normal;
            }
            avgNormal = avgNormal / contacts.Length;
            float distfromVert1 = Vector2.Distance( mv.getVertexPosition(edgeIndex[0]), contacts[0].point);
            float distfromVert2 = Vector2.Distance(mv.getVertexPosition(edgeIndex[1]), contacts[0].point);
            float lerpValue = distfromVert1/(distfromVert1+distfromVert2);
            Vector2 power = Vector2.Lerp(mv.power[edgeIndex[0]], mv.power[edgeIndex[1]],lerpValue);
            rb.AddForce(avgNormal.normalized * power.magnitude * Mathf.Max(Vector3.Dot(power.normalized, avgNormal), 0));
        }
    }
}
