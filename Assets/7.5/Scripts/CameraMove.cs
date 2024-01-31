using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float _speed = 10.0f;
    public float _zoomSpeed=0.5f;
    public float _minZoom = 3f;
    public float _maxZoom = 6f;

    private Vector3 _newPosition;
    private void Start()
    {
        SetNewPosition(transform.position);
    }

    private void Update()
    {
        ZoomCamera();
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

        
    private void MoveCamera()
    {
        if (GameMenu.isPaused) return;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        _newPosition += move * _speed * Time.deltaTime;
        
        _newPosition.x = Mathf.Clamp(_newPosition.x, -15f, 30f);
        _newPosition.y = Mathf.Clamp(_newPosition.y, -10f, 4f);

        transform.position = _newPosition;
    }

    private void ZoomCamera()
    {
        if (GameMenu.isPaused) return;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * _zoomSpeed, _minZoom, _maxZoom);
    }

    private void SetNewPosition(Vector3 newPosition)
    {
        _newPosition = newPosition;
    }
}
