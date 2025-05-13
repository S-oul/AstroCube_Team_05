using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class TextAnimation : MonoBehaviour
{
    [Header("Editor Settings")]
    [SerializeField] bool _updateAutomatically;

    [Header("Animation Settings")]
    [SerializeField] float _durationFadeInText = 2.0f;
    [SerializeField] float _durationDescrambleForEachLetter = 1.0f;
    [SerializeField] float _waitDurationAfterDescramble = 5.0f;
    [SerializeField] float _durationMoveToLineForEachLetter = 3.0f;
    [SerializeField] float _intervalBetweenEachLetterMove = 0.3f;
    [SerializeField] float _durationFadeOutText = 2.0f;

    [Header("Display Settings")]
    [SerializeField] private string text;
    [SerializeField][Range(0.0f, 1.0f)] private float _textLerp;
    [SerializeField][Range(0.0f, 1.0f)] private float _intervalSquare;
    [SerializeField][Range(0.0f, 1.0f)] private float _intervalDetangled;
    [SerializeField] private Transform startPointSquare;
    [SerializeField] private Transform startPointDetangled;
    [SerializeField] private GameObject letterPrefab;
    private List<LetterInfo> spawnedText = new();

    // Start is called before the first frame update
    void Start()
    {
        SpawnText();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateTextPosition();
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying && _updateAutomatically)
            SpawnText();
    }

    public Vector3 GetCharPosFromIndex(int index)
    {
        return (Vector3.Lerp(spawnedText[index].squarePosition, spawnedText[index].linePosition, _textLerp));
    }

    [Button("Spawn Text")]
    public void SpawnText()
    {
        ClearText();
        InitPositions();
        string tempNoSpace = text.Replace(" ", string.Empty);
        for (int i = 0; i < tempNoSpace.Length; i++)
        {
            char c = tempNoSpace[i];
            RectTransform spawnedLetter = Instantiate(letterPrefab, new Vector3(GetCharPosFromIndex(i).x, GetCharPosFromIndex(i).y, 0), Quaternion.identity, transform).GetComponent<RectTransform>();
            spawnedLetter.localPosition = new Vector3(spawnedLetter.localPosition.x,spawnedLetter.localPosition.y, 0.0f);
            spawnedText[i].index = i;
            spawnedText[i].text = spawnedLetter.transform.GetComponent<Text>();
            spawnedText[i].text.material = Instantiate(spawnedText[i].text.material);
            spawnedText[i].text.text = c.ToString();
        }
    }

    [Button("Update Text Position")]
    public void UpdateTextPosition()
    {
        string tempNoSpace = text.Replace(" ", string.Empty);
        for (int i = 0; i < tempNoSpace.Length; i++)
        {
            spawnedText[i].text.transform.position = GetCharPosFromIndex(i);
            var r = spawnedText[i].text.GetComponent<RectTransform>();
            r.localPosition = new Vector3(r.localPosition.x, r.localPosition.y, 0.0f); 

        }
    }

    public void InitPositions()
    {
        string tempNoSpace = text.Replace(" ", string.Empty);

        for (int i = 0; i < tempNoSpace.Length; i++) 
            spawnedText.Add(new LetterInfo());

        float squareSize = Mathf.Floor(Mathf.Sqrt(tempNoSpace.Length));
        for (int i = 0; i < tempNoSpace.Length; i++)
        {
            Vector3 newPos = new Vector3(startPointSquare.position.x + ((i % squareSize) * _intervalSquare),
                                        startPointSquare.position.y - (Mathf.Floor(i / squareSize) * _intervalSquare), 0.0f);
            spawnedText[i].squarePosition = newPos;
        }
        int noSpaceIndex = 0;
        for (int i = 0; i < text.Length; i++)
        {
            Vector3 newPos = new Vector3(startPointDetangled.position.x + (i * _intervalDetangled),
                                        startPointDetangled.position.y, 0.0f);
            if (text[i] != ' ')
            {
                spawnedText[noSpaceIndex++].linePosition = newPos;
            }
        }
    }


    [Button("Clear Text")]
    public void ClearText()
    {
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        spawnedText.Clear();
    }

    public IEnumerator StartDisplayText()
    {
        List<LetterInfo> randomLetters = spawnedText.ToList();
        Shuffle(randomLetters);

        foreach (LetterInfo letter in randomLetters)
        {
            DOTween.To(() => letter.text.material.GetFloat("_Alpha"), x => letter.text.material.SetFloat("_Alpha", x), 1f, _durationFadeInText).SetEase(Ease.InCirc);
        }
        yield return new WaitForSeconds(_durationFadeInText);

        foreach (LetterInfo letter in randomLetters)
        {
            DOTween.To(() => letter.text.material.GetFloat("_Distort"), x => letter.text.material.SetFloat("_Distort", x), 0f, _durationDescrambleForEachLetter).SetEase(Ease.OutCirc);
            StartCoroutine(DescrambleLetter(letter));
            yield return new WaitForSeconds(_intervalBetweenEachLetterMove);
        }

        yield return new WaitForSeconds(_waitDurationAfterDescramble);

        foreach (LetterInfo letter in randomLetters)
        {
            DOTween.To(() => letter.text.material.GetFloat("_Alpha"), x => letter.text.material.SetFloat("_Alpha", x), .0f, _durationFadeOutText).SetEase(Ease.OutCirc);
        }
    }

    public void Init()
    {
        foreach (LetterInfo letter in spawnedText)
        {
            letter.text.material.SetFloat("_Alpha", 0.0f);
            letter.text.material.SetFloat("_Distort", 1.0f);
        }
        _textLerp = .0f;
    }

    public void Shuffle<T>(IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    IEnumerator DescrambleLetter(LetterInfo letter)
    {
        var r = letter.text.GetComponent<RectTransform>();
        DOTween.To(() => r.transform.position, x => r.transform.position = x, new Vector3(letter.linePosition.x, letter.linePosition.y, 0.0f), _durationMoveToLineForEachLetter).SetEase(Ease.InOutSine);

        for (float t = 0.0f; t < _durationMoveToLineForEachLetter; t += Time.deltaTime)
        {
            r.localPosition = new Vector3(r.localPosition.x, r.localPosition.y, 0.0f); 
            yield return null;
        }
        r.localPosition = new Vector3(r.localPosition.x, r.localPosition.y, 0.0f);

    }
    class LetterInfo
    {
        public float index;
        public Text text;
        public Vector3 squarePosition;
        public Vector3 linePosition;

        public LetterInfo()
        {
            index = 0;
            text = null;
            squarePosition = new Vector3();
            linePosition = new Vector3();
        }
    }
}
