using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _turnSpeed = 1f;
    [SerializeField] private float _moveSpeed;

    private float vertical;
    private float horizontal;
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate()
    {
        // forward/back movement 
        Vector3 movement = transform.forward * vertical * _moveSpeed * Time.fixedDeltaTime;
        _playerRigidbody.MovePosition(_playerRigidbody.position + movement);

        // rotate
        float turn = horizontal * _turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * turnRotation);    
    }


}
