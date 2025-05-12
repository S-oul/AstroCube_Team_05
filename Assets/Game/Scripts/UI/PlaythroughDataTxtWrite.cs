using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class PlaythroughDataTxtWrite : MonoBehaviour
{
    TrackerTestDesign _ttd;

    float _completionTime;
    int _numOfMoves;
    string _fileName;


    /// <summary>
    /// WARN SACHA WINDOWS ONLY
    /// </summary>
    string path;
    void Awake()
    {
        Debug.LogWarning("Playtrhough Data only works on Windows");
        string exeDirectory = Directory.GetParent(Application.dataPath).FullName;
        if (GameManager.Instance.Settings.playthoughDataFileName == null)
        {
            Debug.Log("MAKING NEW FILE");
            _fileName = "PlaythroughData_" + DateTime.Now.ToString() + ".txt";
            _fileName = _fileName.Replace(" ", "_");
            _fileName = _fileName.Replace("/", ".");
            _fileName = _fileName.Replace(":", ".");
            GameManager.Instance.Settings.playthoughDataFileName = _fileName;
        }
        else
        {
            _fileName = GameManager.Instance.Settings.playthoughDataFileName;
        }
        path = Path.Combine(Application.persistentDataPath, _fileName);
    }
    void Start()
    {
        _ttd = GetComponent<TrackerTestDesign>();
        CreateTextFile();
    }

    void CreateTextFile()
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "PlaythoughDataLog\n\n");
        }
    }

    private void OnEnable()
    {
        EventManager.OnPlayerWin += AddDataEntry;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerWin -= AddDataEntry;
    }

    void AddDataEntry()
    {
        _completionTime = _ttd.timeTracker;
        _numOfMoves = _ttd.rotationTracker;

        string contentOutline = "Date: {0}\nScene Name: {1}\nTime Completed: {2}\nNumber Of Moves: {3}\n\n";
        string content = string.Format(contentOutline, 
            System.DateTime.Now.ToString(), 
            SceneManager.GetActiveScene().name, 
            _completionTime, 
            _numOfMoves
            );
        File.AppendAllText(path, content);

        Debug.Log("DataEntry in scene " + SceneManager.GetActiveScene().name);
    }
}

/* Date: XXX
 * Scene Name: XXX
 * Time Completed: XXX 
 * Number of Moves: XXX 
 * /n
 */
