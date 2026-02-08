
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    private CharacterController Controller;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Input")]
    private float moveInput;
    private float turnInput;



    private void Start()
    {
        Controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        InputManagement();
        Movement();
    }


    private void Movement()
    {
        GroundMovement();
    }   
    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);

        move.y = 0;
        move *= moveSpeed;

        Controller.Move(move * Time.deltaTime);
       }


    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }


}
