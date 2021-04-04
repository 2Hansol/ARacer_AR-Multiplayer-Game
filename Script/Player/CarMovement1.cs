using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement1 : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheel_collider = new WheelCollider[4];//FR RL RR RL
    [SerializeField] private Transform[] wheel_transform = new Transform[4];//FR RL RR RL

    public Joystick joystick1;
    public Joystick joystick2;

    public float inputValue1, joyVal1, joyVal2;

    public int maxTorque;
    public float moveSpeed, getBackSpeed;

    Rigidbody rb;

    void Start()
    {
        //maxTorque = 1000000 - 1000;
        //moveSpeed = 10000-1000;
        maxTorque = 30;
        moveSpeed = 5;
        getBackSpeed = 10;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0); // 무게중심이 높으면 차가 쉽게 전복된다
        for (int i = 0; i < 4; i++) { wheel_transform[i] = wheel_collider[i].transform; }
    }

    void FixedUpdate()
    {
        if (!SpawnManagerRacing.gameStart) return;

        joyVal1 = joystick1.Vertical;
        joyVal2 = joystick2.Horizontal;
        if (SpawnManagerRacing.gameOver)
        {
            inputValue1 = 0;
            joyVal1 = 0;
            joyVal2 = 0;

        }
        if (Mathf.Abs(joystick1.Vertical) <= 0.05f)
        {

            if (Mathf.Abs(inputValue1) >= 0.5f)
            {
                int opositeDir1 = (inputValue1 > 0) ? -1 : 1;
                inputValue1 += Time.deltaTime * getBackSpeed * opositeDir1;
                if (inputValue1 * opositeDir1 >= 0) inputValue1 = 0; //원래 양수였다가 음수로가지 않게
            }
            else inputValue1 = 0;

        }
        else if (Mathf.Abs(joystick1.Vertical) > 0.05f)
        {
            inputValue1 += Time.deltaTime * joystick1.Vertical;
            inputValue1 = Mathf.Clamp(inputValue1, -1, 1);
        }



        if (moveSpeed * inputValue1 < maxTorque)
        {
            wheel_collider[2].motorTorque = moveSpeed * inputValue1;
            wheel_collider[3].motorTorque = moveSpeed * inputValue1;
        }
        else
        {
            wheel_collider[2].motorTorque = maxTorque;
            wheel_collider[3].motorTorque = moveSpeed * maxTorque;

        }

        wheel_collider[0].steerAngle = 40 * joyVal2;
        wheel_collider[1].steerAngle = 40 * joyVal2;

        wheel_transform[0].Rotate(wheel_collider[0].rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheel_transform[1].Rotate(wheel_collider[1].rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheel_transform[2].Rotate(wheel_collider[2].rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);
        wheel_transform[3].Rotate(wheel_collider[3].rpm / 60 * 360 * Time.fixedDeltaTime, 0, 0);


    }
}