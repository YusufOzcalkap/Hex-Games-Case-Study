using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public GameObject _lineRenderer;
    public Vector3 go;
    public int index;

    private float _ballSpeed;
    private int _passCount;
    [HideInInspector] public bool _shoot;
    void Start()
    {
        index = 1;
        _ballSpeed = 0;
        _lineRenderer = GameObject.Find("LineController").gameObject;
    }

    void Update()
    {
        go = new Vector3(_lineRenderer.transform.GetChild(index).position.x, 0.7f, _lineRenderer.transform.GetChild(index).position.z);

        if (_shoot)
            transform.position = Vector3.Slerp(transform.position, go, (2.5f + _ballSpeed) * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reflect"))
        {
            GameObject ps = Instantiate(Gamemanager.instance._particles[0], new Vector3(other.transform.position.x, other.transform.position.y + 1.5f , other.transform.position.z), Quaternion.identity);
            ps.GetComponent<ParticleSystem>().Play();
            index++;
            _passCount++;
            _ballSpeed += 0.5f;
            //other.transform.DOShakePosition(2.0f, strength: new Vector3(0.5f, 0, 1.5f), vibrato: 3, randomness: 2, snapping: false, fadeOut: false);
            other.transform.GetChild(0).transform.DOShakeRotation(1.2f, new Vector3(10, 0, 10), randomness: 1).SetLoops(0);
        }

        if (other.gameObject.CompareTag("Goal"))
        {
            if (_passCount >= 3)
            {
                for (int i = 1; i < Gamemanager.instance._particles.Length; i++) Gamemanager.instance._particles[i].GetComponent<ParticleSystem>().Play();
                Gamemanager.instance._nextUI.SetActive(true);
            }
        }
    }
}
