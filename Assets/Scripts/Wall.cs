using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    Material material;

    static float lastHit;
    static float lastHit2;


    [SerializeField]
    AudioSource hitWall;

    Rigidbody2D ball;
    private void Awake()
    {
        material.SetFloat("_Hit_Time", Time.time);
        GameObject b = FindObjectOfType<ball>().gameObject;
        ball = b.GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Player")
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
            if (Vector3.Project(ball.velocity,avgNormal).magnitude < 1.1f)
            {
                return;
            }
            if (Time.time - lastHit < 2f)
            {
                if (Time.time - lastHit2 < 2f || Time.time - lastHit < 0.6f)
                {
                    return;
                }
                hitWall.Play();
                lastHit2 = Time.time;
                material.SetFloat("_Hit_Time_2", lastHit2);
                material.SetVector("_Hit_Point_2", collision.contacts[0].point);
                return;
            }
            hitWall.Play();
            lastHit = Time.time;
            material.SetFloat("_Hit_Time", lastHit);
            material.SetVector("_Hit_Point", collision.contacts[0].point);
        }
    }
}
