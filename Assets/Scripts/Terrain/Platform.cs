using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    float local_time = 1.0f;

    public Vector2 currentSpeed = new Vector2(0, 0);
    float movX;
    float movY;


    List<GameObject> toMove = new List<GameObject>();


    EntityList entity_parent;

    void Start()
    {
        GameObject e = GameObject.Find("Entities");
        entity_parent = e.GetComponent<EntityList>();
    }

    private void move(float t)
    {
        movX = currentSpeed.x * t;
        movY = currentSpeed.y * t;

        transform.position += new Vector3(movX, movY, 0);
    }

    void moveOtherItems()
    {
        foreach(GameObject o in toMove)
        {
            
            o.transform.position += new Vector3(movX, movY, 0);
        }
    }


    bool checkHitPlayer(Collision2D col)
    {
        return entity_parent.checkHitPlayer(col);
    }

    bool checkHitThrow(Collision2D col)
    {
        return entity_parent.checkHitThrow(col);
    }

    void addMoveItem(GameObject o)
    {
        if (!toMove.Contains(o))
        {
            toMove.Add(o);
        }
    }

    void removeMoveItem(GameObject o)
    {
        if (toMove.Contains(o))
        {
            toMove.Remove(o);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (checkHitPlayer(collision) || checkHitThrow(collision))
        {
            
            addMoveItem(collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        removeMoveItem(collision.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime * local_time;
        move(t);
        moveOtherItems();

    }
}
