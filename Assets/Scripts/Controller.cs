using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Controller : MonoBehaviour
{
    private float m_previousX;
    public float dX;
    public float dX_Sum;
    [Range(0.0f, 10.0f)]
    [SerializeField] float Controlsensivity;

    [Range(0.0f, 20.0f)]
    public float moveSpeed = 15;
    [SerializeField] public float steeringSpeed = 180;

    public float Xmove, Steer, Speed;
    [SerializeField] Transform left, right, followControl;
    private void Update()
    {
        if (Globals.isGameActive && !Globals.finish)
        {
            gameUpdate();
            Move(Xmove, moveSpeed);
        }
    }
    private void gameUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_previousX = Input.mousePosition.x;
            dX = 0f;
            dX_Sum = 0f;
        }
        if (Input.GetMouseButton(0))
        {
            dX = (Input.mousePosition.x - m_previousX) / 10f;
            dX_Sum += dX;

            m_previousX = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dX_Sum = 0f;
            dX = 0f;
        }
        Xmove = Controlsensivity * dX / (Time.deltaTime * 25);
    }
    public void moveReset()
    {
        Xmove = 0;
        Steer = 0;
    }
    public void Move(float _swipe, float _speed)
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime);
        if (_swipe > 0)
        {
            followControl.transform.position = Vector3.MoveTowards(followControl.transform.position, right.position, Time.deltaTime * Mathf.Abs(_swipe));
        }
        if (_swipe < 0)
        {
            followControl.transform.position = Vector3.MoveTowards(followControl.transform.position, left.position, Time.deltaTime * Mathf.Abs(_swipe));
        }
    }
}
