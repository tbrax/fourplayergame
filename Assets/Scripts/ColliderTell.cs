using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTell : MonoBehaviour
{
    private PlayerMove m_Character;
    private BaseThrowable m_Throw;
    private GenericEnemy m_Enemy;
    public int type = 0;

    private void Start()
    {
        findParent();

    }


    void findParent()
    {
        if (type == 0)
        {
            m_Character = transform.parent.GetComponent<PlayerMove>();
        }
        else if (type == 1)
        {
            m_Throw = transform.parent.GetComponent<BaseThrowable>();
        }

        else if (type == 2)
        {
            m_Enemy = transform.parent.parent.parent.GetComponent<GenericEnemy>();
        }
    }


    void checkParent()
    {
        switch (type)
        {
            case 0:
                if (m_Character == null)
                {
                    findParent();
                }
                break;
            case 1:
                if (m_Throw == null)
                {
                    findParent();
                }
                break;
            case 2:
                if (m_Enemy == null)
                {
                    findParent();
                }
                break;
        }

    }

    void addCollTrigger(GameObject o)
    {
        if (type == 0)
        {
            m_Character.addCollision(o);
        }
        else if (type == 1)
        {
            m_Throw.addCollision(o);
        }
        else if (type == 2)
        {
            
            m_Enemy.addCollision(o);
            m_Enemy.CollideGet(transform.name, true);
        }
    }



    void removeCollTrigger(GameObject o)
    {
        if (type == 0)
        {
            m_Character.removeCollision(o);
        }
        else if (type == 1)
        {
            m_Throw.removeCollision(o);
        }
        else if (type == 2)
        {
            m_Enemy.removeCollision(o);
        }

        
    }

    void addCollCollide(GameObject o)
    {
        if (type == 0)
        {
            m_Character.addCollision(o);
            m_Character.CollideGet(transform.name, true);
        }
        else if (type == 1)
        {
            m_Throw.addCollision(o);
            m_Throw.CollideGet(transform.name, true);
        }
        else if (type == 2)
        {
            m_Enemy.addCollision(o);
            m_Enemy.CollideGet(transform.name, true);
        }
    }

    void removeCollCollide(GameObject o)
    {
        if (type == 0)
        {
            m_Character.removeCollision(o);
            m_Character.CollideGet(transform.name, false);
        }
        else if (type == 1)
        {
            m_Throw.removeCollision(o);
            m_Throw.CollideGet(transform.name, false);
        }
        else if (type == 2)
        {
            m_Enemy.removeCollision(o);
            m_Enemy.CollideGet(transform.name, false);
        }
    }

    void stayCollCollide(GameObject o)
    {
        if (type == 0)
        {
            Debug.Log(transform.name);
            m_Character.CollideGet(transform.name, true);
        }
        else if (type == 1)
        {
            m_Throw.CollideGet(transform.name, true);
        }
        else if (type == 2)
        {
            m_Enemy.CollideGet(transform.name, true);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        addCollTrigger(col.gameObject);

        //m_Character.addCollision(col.gameObject);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        removeCollTrigger(col.gameObject);
        //m_Character.removeCollision(col.gameObject);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //stayCollCollide(col.gameObject);
        //m_Character.removeCollision(col.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        checkParent();
        addCollCollide(collision.gameObject);
        //m_Character.CollideGet(transform.name, true);
        //m_Character.addCollision(collision.gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //stayCollCollide(collision.gameObject);
        //m_Character.CollideGet(transform.name, true);
    }
    

    void OnCollisionExit2D(Collision2D collision)
    {
        removeCollCollide(collision.gameObject);
        //m_Character.CollideGet(transform.name, false);
        //m_Character.removeCollision(collision.gameObject);
    }
}
