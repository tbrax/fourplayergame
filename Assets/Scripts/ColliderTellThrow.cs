using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTellThrow : MonoBehaviour
{
    private BaseThrowable m_Character;

    private void Awake()
    {
        m_Character = transform.parent.GetComponent<BaseThrowable>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        m_Character.CollideGet(transform.name, true);

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        m_Character.CollideGet(transform.name, false);

    }
}
