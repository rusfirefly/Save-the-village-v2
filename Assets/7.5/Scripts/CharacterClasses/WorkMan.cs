using System;
using UnityEngine;

public class WorkMan : Unit
{
    public static Action<Collider2D> Working;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartedWork(collision);
    }

    public override void StartedWork(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        bool state = false;
        switch (tag)
        {
            case "GoldMine":
                Debug.Log("работник пошел добывать золото");
                Working?.Invoke(collider);
                state = true;
                break;
            case "MeatMine":
                Debug.Log("работник пошел добывать мясо");
                Working?.Invoke(collider);
                state = true;
                break;
            case "WoodMine":
                Debug.Log("работник пошел добывать дерево");
                Working?.Invoke(collider);
                state = true;
                break;
            case "Castle":
                Debug.Log("работник принес ресурсы в замок!");
                Working?.Invoke(collider);
                state = true;
                break;
        }

        if(state)
            Destroy(gameObject);
    }

    public void SetNewPosition(Transform position) => MoveTo(position);

    public void GoCastle(Transform castlePosition) => MoveTo(castlePosition);

}
