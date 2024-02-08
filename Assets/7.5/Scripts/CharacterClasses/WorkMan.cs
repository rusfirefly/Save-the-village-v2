using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkMan : Unit
{
    public static Action<Collider2D> Working;

    
    public override void StartedWork(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                Debug.Log("�������� ����� �������� ������");
                Working?.Invoke(collider);
                break;
            case "MeatMine":
                Debug.Log("�������� ����� �������� ����");
                Working?.Invoke(collider);
                break;
            case "WoodMine":
                Debug.Log("�������� ����� �������� ������");
                Working?.Invoke(collider);
                break;
            case "Castle":
                Debug.Log("�������� ������ ������� � �����!");
                Working?.Invoke(collider);
                break;
        }

        Destroy(gameObject);
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
