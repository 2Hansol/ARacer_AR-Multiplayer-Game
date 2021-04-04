using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyFlagSync2 : MonoBehaviour, IPunObservable
{

    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    // lag compensation
    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;

    private GameObject battleArenaGameobject;
    private int fieldNum;

    private bool IsOver;

    // Update is called once per frame


    void Update()
    {

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();

        IsOver = false;

        battleArenaGameobject = GameObject.Find("CityTrack");
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            //original position -> networked position 시간만큼
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance * (1.0f / PhotonNetwork.SerializationRate));
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }

        /*
        //이것도 위에 업데이드 실행한다음에 하게
        if (!PhotonNetwork.IsMasterClient && !photonView.IsMine)
        {
            Vector3 temp = new Vector3(networkedPosition.x, networkedPosition.y - 8f, networkedPosition.z);

            //original position -> networked position 시간만큼
            rb.position = Vector3.MoveTowards(rb.position, temp, distance * (1.0f / PhotonNetwork.SerializationRate));
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }
        */
    }


    IEnumerator Stop()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<CarMovement>().enabled = false;
            transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(false);
            transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(false);
            yield return new WaitForSeconds(5); //WaitForSeconds객체를 생성해서 반환
            transform.GetComponent<CarMovement>().enabled = true;
            transform.GetComponent<CarMovement>().joystick1.gameObject.SetActive(true);
            transform.GetComponent<CarMovement>().joystick2.gameObject.SetActive(true);
        }
    }






    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Then, photonView is mine and I am the one who controls this player.
            //should send position, velocity etc. data to the other players
            stream.SendNext(rb.position - battleArenaGameobject.transform.position);
            stream.SendNext(rb.rotation);

            if (synchronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if (synchronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {
            //Called on my player gameobject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext() + battleArenaGameobject.transform.position;
            networkedRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(rb.position, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    rb.position = networkedPosition;

                }
            }
            // 렉걸림 방지 코드 
            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkedPosition += rb.velocity * lag;

                    distance = Vector3.Distance(rb.position, networkedPosition);
                }

                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                    angle = Quaternion.Angle(rb.rotation, networkedRotation);
                }
            }
        }
    }
}