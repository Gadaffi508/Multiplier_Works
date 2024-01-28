using System;
using System.Collections;
using FishNet.Object;
using UnityEngine;

public class PlayerFN : NetworkBehaviour
{
    public float MoveSpeed = 5f;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(!base.IsOwner)
            return;
        
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        
        Vector3 offset = new Vector3(X,Physics.gravity.y,Z) * (MoveSpeed * Time.deltaTime);

        _controller.Move(offset);
    }
}
