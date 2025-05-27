using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;

public class PlaythroughDataTxtWrite : MonoBehaviour
{
    TrackerTestDesign _ttd;

    float _completionTime;
    int _numOfMoves;
    string _fileName;



    string _folderPath;
    string _filePath;

#if !UNITY_EDITOR
    void Awake()
    {
        Debug.LogWarning("Playtrhough Data only works on Windows");
        string exeDirectory = Directory.GetParent(Application.dataPath).FullName;
        _folderPath = Path.Combine(exeDirectory, "Playthrough_Data");

        if (Directory.Exists(_folderPath) == false)
        {
            Directory.CreateDirectory(_folderPath);
            Debug.Log("Created folder: " + _folderPath);
        }

        if (GameManager.Instance.Settings.playthoughDataFileName == null)
        {
            Debug.Log("MAKING NEW FILE");
            _fileName = "Playthrough_Data\\PlaythroughData_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".txt";
            GameManager.Instance.Settings.playthoughDataFileName = _fileName;
        }
        else
        {
            _fileName = GameManager.Instance.Settings.playthoughDataFileName;
        }
        //path = Path.Combine(Application.persistentDataPath, _fileName);
        _filePath = Path.Combine(exeDirectory, _fileName);
        //Debug.Log(_filePath);
    }
    void Start()
    {
        _ttd = GetComponent<TrackerTestDesign>();
        CreateTextFile();
    }

    void CreateTextFile()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "PlaythoughDataLog\n\n");
        }
    }

    private void OnEnable()
    {
        if (SystemInfo.operatingSystem.Contains("Windows "))
        EventManager.OnPlayerWin += AddDataEntry;
    }

    private void OnDisable()
    {
        if (SystemInfo.operatingSystem.Contains("Windows "))
            EventManager.OnPlayerWin -= AddDataEntry;
    }

    void AddDataEntry()
    {
        _completionTime = _ttd.timeTracker;
        _numOfMoves = _ttd.rotationTracker / 2; // THIS IS TO FIX ANOTHER BUG (rotationTracker is always double the # of moves.)

        string contentOutline = "Date: {0}\nScene Name: {1}\nTime Completed: {2}\nNumber Of Moves: {3}\n\n";
        string content = string.Format(contentOutline, 
            System.DateTime.Now.ToString(), 
            SceneManager.GetActiveScene().name, 
            _completionTime, 
            _numOfMoves
            );
        File.AppendAllText(_filePath, content);
    }
#endif
}
/* Date: XXX
 * Scene Name: XXX
 * Time Completed: XXX 
 * Number of Moves: XXX 
 * /n
 */
