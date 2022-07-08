using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowSubject : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _cameraSpeed;
    private Transform _cameraTransform;
    private Vector3 _distance;
    private Vector3 _nextCameraPosition;
    private float _playerSpeed;


    private void Awake()
    {
        _cameraTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        _distance = _cameraTransform.position - _playerTransform.position;
    }

    private void LateUpdate()
    {
        _playerSpeed = _playerRigidbody.velocity.magnitude;
        _nextCameraPosition = _playerTransform.position + _distance;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _nextCameraPosition, _cameraSpeed * Time.deltaTime);
    }
}
