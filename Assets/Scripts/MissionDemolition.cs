using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    [SerializeField] private Text _uitLevel;
    [SerializeField] private Text _uitShots;
    [SerializeField] private Text _uitButton;
    [SerializeField] private Vector3 _castlePos;
    [SerializeField] private GameObject[] _castles;

    private int _level;
    private int _levelMax;
    private int _shotsTaken;
    private GameObject _castle;
    private GameMode _mode = GameMode.idle;
    private string _showing = "Show Slingshot";

    private void Start()
    {
        S = this;

        _level = 0;
        _levelMax = _castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if(_castle != null)
        {
            Destroy(_castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        _castle = Instantiate(_castles[_level]);
        _castle.transform.position = _castlePos;
        _shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;
        UpdateGUI();
        _mode = GameMode.playing;
    }

    private void UpdateGUI()
    {
        _uitLevel.text = $"Level: {_level + 1} of {_levelMax}";
        _uitShots.text = $"Shots Taken: {_shotsTaken}";
    }

    private void Update()
    {
        UpdateGUI();

        if((_mode == GameMode.playing) && Goal.goalMet)
        {
            _mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        _level++;
        if(_level == _levelMax)
        {
            _level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if(eView == "")
        {
            eView = _uitButton.text;
        }
        _showing = eView;

        switch(_showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                _uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S._castle;
                _uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                _uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        S._shotsTaken++;
    }
}
