using System;
using System.Collections;
using FishNet.Object;
using UnityEngine;

public class PlayerFN : NetworkBehaviour
{
    public float MoveSpeed = 5f;
    public float RotateSpeed = 150f;
    public GameObject Camera;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(base.IsOwner)
            Camera.SetActive(true);
    }

    private void Update()
    {
        if(!base.IsOwner)
            return;
        
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        
        transform.Rotate(new Vector3(0,X * RotateSpeed * Time.deltaTime));
        
        Vector3 offset = new Vector3(0,Physics.gravity.y,Z) * (MoveSpeed * Time.deltaTime);

        _controller.Move(offset);
    }
}
