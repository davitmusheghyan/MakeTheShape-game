using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] GameManager gameManager;

    [SerializeField] Transform center, left, left_up, right, right_up;
    [SerializeField] Transform rayCastCenterPos, rayCastLeftDownPos, rayCastRightDownPos;
    [SerializeField] Transform playerBorders;

    int step;
    float speed;
    float rayDistance;

    public bool input;
    public bool hitWall;

    void Start()
    {
        input = true;

        if (Application.platform == RuntimePlatform.Android)
            step = 15; //ANDROID
        else
            step = 10; //UNITY

        speed = 0.01f;
        rayDistance = 0.75f;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && input && gameManager.gameStart && !gameManager.pause)
        {
            if (Input.mousePosition.x < Screen.width / 2)
                DetectMoveDirection(false);
            else
                DetectMoveDirection(true);

            input = false;
        }

        Debug.DrawRay(rayCastCenterPos.position, Vector3.forward * rayDistance, Color.black);
        Debug.DrawRay(rayCastCenterPos.position, Vector3.back * rayDistance, Color.black);
        Debug.DrawRay(rayCastCenterPos.position, Vector3.down * rayDistance, Color.black);
        Debug.DrawRay(rayCastLeftDownPos.position, rayCastLeftDownPos.TransformDirection(Vector3.forward) * rayDistance, Color.black);
        Debug.DrawRay(rayCastRightDownPos.position, rayCastRightDownPos.TransformDirection(Vector3.forward) * rayDistance, Color.black);
    }

    void DetectMoveDirection(bool dir)
    {
        LayerMask layer_mask = LayerMask.GetMask("Default");

        bool down_left;
        bool down_right;
        bool down;

        RaycastHit down_leftRay;
        if (Physics.Raycast(rayCastCenterPos.position, rayCastLeftDownPos.TransformDirection(Vector3.forward), out down_leftRay, rayDistance, layer_mask))
            down_left = true;
        else
            down_left = false;

        RaycastHit down_rightRay;
        if (Physics.Raycast(rayCastCenterPos.position, rayCastRightDownPos.TransformDirection(Vector3.forward), out down_rightRay, rayDistance, layer_mask))
            down_right = true;
        else
            down_right = false;

        RaycastHit downRay;
        if (Physics.Raycast(rayCastCenterPos.position, Vector3.down, out downRay, rayDistance, layer_mask))
            down = true;
        else
            down = false;

        if (!dir)
        {
            RaycastHit leftRay;
            if (Physics.Raycast(rayCastCenterPos.position, Vector3.back, out leftRay, rayDistance, layer_mask))
            {
                audioManager.PlayerMove();
                StartCoroutine(Move(left_up.position, Vector3.left));
                playerBorders.position += new Vector3(0, 1, 0);
            }
            else
            {
                if (down_right && !down)
                {
                    audioManager.PlayerMove();
                    StartCoroutine(Move(right.position, Vector3.left));
                    playerBorders.position += new Vector3(0, -1, 0);
                }
                else
                {
                    if (playerBorders.position.z != -3)
                    {
                        audioManager.PlayerMove();
                        StartCoroutine(Move(left.position, Vector3.left));
                        playerBorders.position += new Vector3(0, 0, -1);
                    }
                    else
                        StartCoroutine(Move(Vector3.zero, Vector3.zero));
                }
            }
        } else
        {
            RaycastHit rightRay;
            if (Physics.Raycast(rayCastCenterPos.position, Vector3.forward, out rightRay, rayDistance, layer_mask))
            {
                audioManager.PlayerMove();
                StartCoroutine(Move(right_up.position, Vector3.right));
                playerBorders.position += new Vector3(0, 1, 0);
            }
            else
            {
                if(down_left && !down)
                {
                    audioManager.PlayerMove();
                    StartCoroutine(Move(left.position, Vector3.right));
                    playerBorders.position += new Vector3(0, -1, 0);
                }
                else
                {
                    if (playerBorders.position.z != 3)
                    {
                        audioManager.PlayerMove();
                        StartCoroutine(Move(right.position, Vector3.right));
                        playerBorders.position += new Vector3(0, 0, 1);
                    }
                    else
                        StartCoroutine(Move(Vector3.zero, Vector3.zero));
                }
            }
        }
    }
    IEnumerator Move(Vector3 axis, Vector3 direction)
    {
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(axis, direction, step);
            yield return new WaitForSeconds(speed);
        }
        center.position = transform.position;
        rayCastCenterPos.position = transform.position;

        input = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("wall") && !hitWall)
        {
            hitWall = true;
            StartCoroutine(gameManager.GameOver(other.gameObject));
        }
    }
}
