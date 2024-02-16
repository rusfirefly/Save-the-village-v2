using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHadler : MonoBehaviour
{
    private GameHadler _gameHadler;

    public void Initialize(GameHadler gameHadler)
    {
        _gameHadler = gameHadler;
    }

    void Update()
    {
        DeSelectedAll();
    }

    private void DeSelectedAll()
    {
        Vector2 CurMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D rayHit = Physics2D.Raycast(CurMousePos, Vector2.zero);
            if (rayHit.transform == null)
            {
                _gameHadler.DefaultStatePanel();
            }
        }
    }
}
