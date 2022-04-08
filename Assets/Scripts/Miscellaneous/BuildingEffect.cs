using System;
using Core;
using UnityEngine;

public abstract class BuildingEffect : MonoBehaviour
{
    protected SpriteRenderer _sprite;
    protected int contacts;

    protected void OnCollisionEnter2D(Collision2D other)
    {
        contacts++;
        OnContactsUpdate();
    }

    protected void OnCollisionExit2D(Collision2D other)
    {
        contacts--;
        OnContactsUpdate();
    }

    protected void OnContactsUpdate()
    {
        if (contacts > 0)
        {
            ControlManager.Instance.CanBuild = false;
            _sprite.color = Color.red;
        }
        else if (contacts == 0)
        {
            ControlManager.Instance.CanBuild = true;
            _sprite.color = Color.green;
        }
    }
}
