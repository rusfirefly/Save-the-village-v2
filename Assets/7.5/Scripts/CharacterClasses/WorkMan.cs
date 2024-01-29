using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMan : Unit
{
    public static Action<GameObject, Collider2D> Working;

    public override void StartedWork(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                Debug.Log("работник пошел добывать золото");
                Working?.Invoke(gameObject, collider);
                break;
            case "MeatMine":
                Debug.Log("работник пошел добывать мясо");
                Working?.Invoke(gameObject, collider);
                break;
            case "WoodMine":
                Debug.Log("работник пошел добывать дерево");
                Working?.Invoke(gameObject, collider);
                break;
            case "Castle":
                Debug.Log("работник принес ресурсы в замок!");
                Working?.Invoke(gameObject, collider);
                break;
        }
    }

    public void SetNewPosition(Transform position)
    {
        MoveTo(position);
    }

    public void GoCastle(Transform castlePosition)
    {
        MoveTo(castlePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartedWork(collision);
    }

}
