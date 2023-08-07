using UnityEngine;

public class CamerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject player;


    [SerializeField]
    float maxSpeed;

    public bool freeze;

    private void Update()
    {
        if (freeze)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -20), maxSpeed*.2f);
            return;
        }
        transform.position =  Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x,player.transform.position.y,-20),maxSpeed);
    }

}
