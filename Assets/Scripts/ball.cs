using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField]
    Slowmo slowmo;
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
    [SerializeField]
    Material wallMat;

    [SerializeField]
    AudioSource hitBall;

    bool hitting;

    float smallDelay;

    float remainingJumps;

    [SerializeField]
    LayerMask notPlayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        smallDelay = 0;
        hitting = false;
        lineRenderer.enabled = false;
        remainingJumps = jumps;
        wallMat.SetFloat("_Hit_Time_2", 0);
        wallMat.SetVector("_Hit_Point_2", Vector2.one * 9999999);
        wallMat.SetFloat("_Hit_Time", 0);
        wallMat.SetVector("_Hit_Point", Vector2.one * 9999999);
    }

    private void LateUpdate()
    {
        timeLastSoundEffect += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            clicked = true;
        }
        else if (!ControllableVerts.movedVert && Input.GetMouseButton(0) && slowmo.slow)
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            drawPowerLine();
        }
        else if (!ControllableVerts.movedVert  && (Input.GetMouseButtonUp(0) && hitting))
        {
            smallDelay = 0;
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            shootBall();
        }
    }


    private void drawPowerLine()
    {
        if (remainingJumps <= 0)
        {
            return;
        }
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
        if (remainingJumps <= 0)
        {
            return;
        }
        Vector2 hitLine = mousePos - startMousePos;
        hitLine = hitLine.normalized * Mathf.Min(lineDist, hitLine.magnitude);
        hitLine.x = -hitLine.x;
        hitLine.y = -hitLine.y;
        rb.velocity = rb.velocity * .3f;
        rb.AddForce(hitLine * hitPower);
        hitting = false;
        remainingJumps -= 1;
        if(Physics2D.Raycast(transform.position, Vector2.down, 2f,notPlayer) || Physics2D.Raycast(transform.position + (Vector3.left * .5f), Vector2.down, 2f, notPlayer) || Physics2D.Raycast(transform.position + (Vector3.right*.5f), Vector2.down, 2f, notPlayer))
        {
            remainingJumps = jumps;
        }
        lineRenderer.enabled = false;
        hitBall.Play();
    }

    float timeLastSoundEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 avgNormal = Vector3.zero;
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        for (int i = 0; i < contacts.Length; i++)
        {
            ContactPoint2D contact = contacts[i];
            avgNormal += contact.normal;
        }
        avgNormal = avgNormal / contacts.Length;
        if (avgNormal.normalized.y > .7f)
        {
            remainingJumps = jumps;
        }
        if (hitting)
        {
            return;
        }
        GameObject other = collision.gameObject;
        if (other.tag == "movingVert")
        {
            Debug.Log("here");
            mv = other.GetComponentInParent<ControllableVerts>();
            int[] edgeIndex = other.GetComponent<Edge>().index;
            float distfromVert1 = Vector2.Distance(mv.getVertexPosition(edgeIndex[0]), contacts[0].point);
            float distfromVert2 = Vector2.Distance(mv.getVertexPosition(edgeIndex[1]), contacts[0].point);
            float lerpValue = distfromVert1 / (distfromVert1 + distfromVert2);
            Vector2 power = Vector2.Lerp(mv.power[edgeIndex[0]], mv.power[edgeIndex[1]], lerpValue);
            force = (avgNormal.normalized * power.magnitude * Mathf.Max(Vector3.Dot(power.normalized, avgNormal), 0));
        }
    }

    Vector2 force;
    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 avgNormal = Vector3.zero;
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        for (int i = 0; i < contacts.Length; i++)
        {
            ContactPoint2D contact = contacts[i];
            avgNormal += contact.normal;
        }
        avgNormal = avgNormal / contacts.Length;
        if (avgNormal.normalized.y > .7f)
        {
            remainingJumps = jumps;
        }
        if (hitting)
        {
            return;
        }
        GameObject other = collision.gameObject;
        if (other.tag == "movingVert")
        {
            mv = other.GetComponentInParent<ControllableVerts>();
            int[] edgeIndex = other.GetComponent<Edge>().index;
            float distfromVert1 = Vector2.Distance( mv.getVertexPosition(edgeIndex[0]), contacts[0].point);
            float distfromVert2 = Vector2.Distance(mv.getVertexPosition(edgeIndex[1]), contacts[0].point);
            float lerpValue = distfromVert1/(distfromVert1+distfromVert2);
            Vector2 power = Vector2.Lerp(mv.power[edgeIndex[0]], mv.power[edgeIndex[1]],lerpValue);
            Vector2 testForce = (avgNormal.normalized * power.magnitude * Mathf.Max(Vector3.Dot(power.normalized, avgNormal), 0));
            force = testForce.magnitude > force.magnitude ? testForce : force;
        }
    }

    ControllableVerts mv;

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "movingVert")
        {
            mv = other.GetComponentInParent<ControllableVerts>();
            mv.undoClick = true;
            mv = null;
            slowmo.freezeTime(false);
            rb.AddForce(force);
            Debug.Log(force);
            force = Vector2.zero;
        }
    }
}
