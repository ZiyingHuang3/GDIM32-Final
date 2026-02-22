using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _forwardSpeed = 1f;
    [SerializeField] private float _turnSpeed = 1f;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        float translation = Input.GetAxis("Vertical") * _forwardSpeed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * _turnSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
        

        /*float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // left/right movement 
        Vector3 movement = (transform.forward * vertical + transform.right * horizontal).normalized;
        _playerRigidbody.MovePosition(_playerRigidbody.position + movement * _moveSpeed * Time.deltaTime);
        bool isWalking = Mathf.Abs(vertical) > 0.1f || Mathf.Abs(horizontal) > 0.1f;
       */ //_animator.SetBool("IsWalking", isWalking);
    }

    
}
