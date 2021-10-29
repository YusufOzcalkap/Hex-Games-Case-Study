using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform PlayerTransform;
    private Vector3 _cameraOffset;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;

    [HideInInspector]public Animator anim;
    void Start()
    {
        instance = this;
        _cameraOffset = transform.position - PlayerTransform.position;
        anim = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        PlayerTransform = ShootController.instance._balls.transform.GetChild(0).transform;

        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }
}
