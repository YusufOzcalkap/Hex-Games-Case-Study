using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]

public class ShootController : MonoBehaviour
{
    public static ShootController instance;

    public int _reflections;
    public float _maxLenght;
    public GameObject blast;

    public LineRenderer _lineRenderer;
    private Ray _ray;
    private RaycastHit _hit;
    private Vector3 _direction;
    public List<Vector3> _playersTransform = new List<Vector3>();

    private Animator CameraAnim;
    private Animator PlayerAnim;
    [HideInInspector]public GameObject _ball;
    [HideInInspector]public GameObject _balls;
    [HideInInspector]public float _targetPos;
    void Awake()
    {
        instance = this;
        _lineRenderer = GetComponent<LineRenderer>();
        CameraAnim = GameObject.Find("CamController").gameObject.GetComponent<Animator>();
        PlayerAnim = GameObject.Find("Soccer").gameObject.GetComponent<Animator>();
        _balls = GameObject.Find("Balls").gameObject;
    }

    void Update()
    {
        _ray = new Ray(transform.position, transform.forward);

        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
        float remainingLenght = _maxLenght;

        #region Ball destinations are drawn with LineRenderer
        if (Gamemanager.instance._isStarting && !IsMouseOverUI())
        {
            for (int i = 0; i < _reflections; i++)
            {
                if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainingLenght))
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _hit.point);
                    remainingLenght -= Vector3.Distance(_ray.origin, _hit.point);
                    _ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
                    if (_hit.collider.tag == "Goal") Gamemanager.instance._isGoalKeeper = true; else Gamemanager.instance._isGoalKeeper = false;
                    if (_hit.collider.tag != "Reflect") break;                   
                }
                else
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _ray.origin + _ray.direction * remainingLenght);
                }
            }

            if (Input.GetMouseButton(0))
                transform.eulerAngles = new Vector3(0, (Input.mousePosition.x - Input.mousePosition.y), 0);
            
            if (Input.GetMouseButtonUp(0))
            {
                if (Gamemanager.instance._isGoal && Gamemanager.instance._isShoot && Gamemanager.instance._isGoalKeeper) StartCoroutine(SetShoot());
                if (Gamemanager.instance._isGoal == false && Gamemanager.instance._isShoot) StartCoroutine(SetShootNotGoal());
            }
        }
        #endregion

        #region setting where ball should go
        if (_targetPos > 0)
        {
            if (transform.eulerAngles.y < _targetPos + 3 && transform.eulerAngles.y > _targetPos -3)
                        transform.eulerAngles = new Vector3(0, _targetPos, 0);
            
            if (transform.eulerAngles.y == _targetPos)
            {
                Gamemanager.instance._isGoal = true;
                _maxLenght = 40;
            }
            else
            {
                Gamemanager.instance._isGoal = false;
                _maxLenght = 25;
            }
        }

        if (_targetPos < 0)
        {
            if (transform.eulerAngles.y-360 < _targetPos + 3 && transform.eulerAngles.y-360 > _targetPos - 3)
                   transform.eulerAngles = new Vector3(0, _targetPos, 0);
            
            if (transform.eulerAngles.y-360 == _targetPos)
            {
                Gamemanager.instance._isGoal = true;
                _maxLenght = 40;
            }
            else
            {
                Gamemanager.instance._isGoal = false;
                _maxLenght = 25;
            }
        }
        #endregion
    }

    //function where player hits ball if player throws ball correctly
    IEnumerator SetShoot()
    {
        Camera.main.gameObject.transform.parent.GetComponent<CameraController>().enabled = true;
        CameraAnim.SetBool("CamAction", true);

        yield return new WaitForSeconds(1f);

        PlayerAnim.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.5f);
        Gamemanager.instance._particles[0].GetComponent<ParticleSystem>().Play();
        _balls.transform.GetChild(0).GetComponent<BallController>()._shoot = true;
        for (int i = 0; i < _lineRenderer.positionCount; i++) transform.GetChild(i).transform.position = _lineRenderer.GetPosition(i);
    }

    //function where player hits ball if player throws ball incorrectly
    IEnumerator SetShootNotGoal()
    {
        Gamemanager.instance._ballCount++;
        if (Gamemanager.instance._ballCount == 1) Gamemanager.instance._ballsSquare[0].GetComponent<Animator>().SetBool("Square", true);
        if (Gamemanager.instance._ballCount == 2) Gamemanager.instance._ballsSquare[1].GetComponent<Animator>().SetBool("Square", true);
        if (Gamemanager.instance._ballCount == 3) Gamemanager.instance._ballsSquare[2].GetComponent<Animator>().SetBool("Square", true);
        PlayerAnim.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.5f);
        Gamemanager.instance._particles[0].GetComponent<ParticleSystem>().Play();
        _balls.transform.GetChild(0).GetComponent<BallController>()._shoot = true;
        for (int i = 0; i < _lineRenderer.positionCount; i++) transform.GetChild(i).transform.position = _lineRenderer.GetPosition(i);

        yield return new WaitForSeconds(1.5f);
        Destroy(_balls.transform.GetChild(0).transform.gameObject);
        GameObject newBall = Instantiate(_ball);
        newBall.transform.parent = _balls.transform;
    }

    // function written so that if raycast hit UI it won't do anything in the game
    bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
