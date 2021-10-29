using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [Header("Game UI and Text")]
    public GameObject _startUI;
    public GameObject _nextUI;
    public GameObject _failUI;
    public TextMeshProUGUI _levelText;

    [HideInInspector]public bool _isStarting;
    [HideInInspector]public bool _isGoal;
    [HideInInspector]public bool _isGoalKeeper;
    [HideInInspector]public bool _isPlayerPos;
    public GameObject _posGameobject;

    [Header("Game Tools")]
    public GameObject[] _particles;
    public GameObject[] _ballsSquare;
    public GameObject[] _levels;

    [HideInInspector]public int _ballCount;
    [HideInInspector]public static int _level;
    public float[] _targetPosition;
    private GameObject _LevelsParent;
    [HideInInspector]public bool _isShoot;
    void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetInt("Level", _level) == 0) _level++;
        _LevelsParent = GameObject.Find("Levels").gameObject;
        PlayerPrefs.GetInt("Level", _level);
    }

    // Update is called once per frame
    void Update()
    {
        #region controls of puppets that rotate and change position
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse clicked");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Reflect" && hit.transform.childCount == 2)
                {
                    hit.transform.eulerAngles = new Vector3(hit.transform.rotation.x, hit.transform.eulerAngles.y + 30, hit.transform.rotation.z);
                    _isPlayerPos = false;
                    StartCoroutine(SetShoot());
                }
                else if (hit.transform.tag == "Reflect" && hit.transform.childCount == 3)
                {
                    _isPlayerPos = true;
                    _posGameobject = hit.transform.gameObject;
                    _isShoot = false;
                }
                else if (hit.transform.tag != "Reflect")
                {
                    _isPlayerPos = false;
                    _isShoot = true;

                }
            }
        }
        if (Input.GetMouseButton(0) && _isPlayerPos)
        {
            Vector3 boundry = _posGameobject.transform.GetChild(0).transform.localPosition;
            boundry.z = Mathf.Clamp(boundry.z, -2f, 2f);
            _posGameobject.transform.GetChild(0).localPosition = boundry;

            _posGameobject.transform.GetChild(0).localPosition += new Vector3(0, 0, (Input.mousePosition.x * Time.deltaTime) * 0.1f);
        }
        #endregion

        if (_ballCount == 3) _failUI.SetActive(true);

        #region Levels Settings
        _levelText.text = "LEVEL " + PlayerPrefs.GetInt("Level", _level);

        if (PlayerPrefs.GetInt("Level", _level) == 1 || PlayerPrefs.GetInt("Level", _level) == 9)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 2 || PlayerPrefs.GetInt("Level", _level) == 10)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 3 || PlayerPrefs.GetInt("Level", _level) == 11)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 4 || PlayerPrefs.GetInt("Level", _level) == 12)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 5 || PlayerPrefs.GetInt("Level", _level) == 13)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 6 || PlayerPrefs.GetInt("Level", _level) == 14)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 7 || PlayerPrefs.GetInt("Level", _level) == 15)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }

        if (PlayerPrefs.GetInt("Level", _level) == 8 || PlayerPrefs.GetInt("Level", _level) == 16)
        {
            _levels[PlayerPrefs.GetInt("Level", _level) - 1].SetActive(true);
            ShootController.instance._targetPos = _targetPosition[PlayerPrefs.GetInt("Level", _level) - 1];
        }
        #endregion
    }

    // Start Button Function
    public void SetStartButton()
    {
        _isStarting = true;
        _startUI.SetActive(false);
    }

    // Next Button Function
    public void SetNextButton()
    {
        Destroy(ShootController.instance._balls.transform.GetChild(0).gameObject);
        GameObject newBall = Instantiate(ShootController.instance._ball);
        newBall.transform.parent = ShootController.instance._balls.transform;
        CameraController.instance.anim.SetBool("CamAction", false);
        Camera.main.transform.parent.transform.position = new Vector3(0, 18.4f, -2.27f);
        _nextUI.SetActive(false);
        _level++;
        PlayerPrefs.SetInt("Level", _level);
        for (int i = 0; i < _LevelsParent.transform.childCount; i++) _LevelsParent.transform.GetChild(i).gameObject.SetActive(false);
        _ballCount = 0;
        _ballsSquare[0].GetComponent<Animator>().SetBool("Square", false);
        _ballsSquare[1].GetComponent<Animator>().SetBool("Square", false);
        _ballsSquare[2].GetComponent<Animator>().SetBool("Square", false);

    }

    // Restart Button Function
    public void SetRestartButton()
    {
        Destroy(ShootController.instance._balls.transform.GetChild(0).gameObject);
        GameObject newBall = Instantiate(ShootController.instance._ball);
        newBall.transform.parent = ShootController.instance._balls.transform;
        CameraController.instance.anim.SetBool("CamAction", false);
        Camera.main.transform.parent.transform.position = new Vector3(0, 18.4f, -2.27f);
        _nextUI.SetActive(false);
        _failUI.SetActive(false);
        _ballCount = 0;
        for (int i = 0; i < _LevelsParent.transform.childCount; i++) _LevelsParent.transform.GetChild(i).gameObject.SetActive(false);

        _ballsSquare[0].GetComponent<Animator>().SetBool("Square", false);
        _ballsSquare[1].GetComponent<Animator>().SetBool("Square", false);
        _ballsSquare[2].GetComponent<Animator>().SetBool("Square", false);
    }

    // function written so that if raycast hit UI it won't do anything in the game
    bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    IEnumerator SetShoot()
    {
        _isShoot = false;
        yield return new WaitForSeconds(1f);
        _isShoot = true;
    }
}
