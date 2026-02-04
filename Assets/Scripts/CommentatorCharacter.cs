using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class CommentatorCharacter : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public UnityEvent OnStartMonolog;
    public UnityEvent OnHideMonolog;

    public List<AudioClip> commentatorSounds;
    public AudioSource aSrc;

    public bool disablePlaying;
    public bool pausePlayingIdle;

    [Header("UI")]
    [SerializeField] private TMP_Text textField;

    [Header("Timing")]
    [SerializeField] private float waitPerCharMultiplier = 0.05f;
    [SerializeField] private Vector2 idleDelayRange = new Vector2(4f, 9f);

    private Coroutine showRoutine;
    private Coroutine idleRoutine;

    private Queue<string> successQueue;
    private Queue<string> failQueue;
    private Queue<string> idleQueue;
    private Queue<string> annoyedQueue;
    private Queue<string> hoverQueue;
    private Queue<string> hitQueue;

    private bool isShowing;

    [SerializeField] float _delayAddedFromHit = 10;
    float _lastClickTime;
    float _delayFromHit;

    [SerializeField] GameObject _glove;
    [SerializeField] int _linesToShowGlove = 12;
    int _linesPassed;
    bool _displayGlove = false;

    private void Start()
    {
        successQueue = new Queue<string>(CommentatorLines.Success);
        failQueue = new Queue<string>(CommentatorLines.Fail);
        idleQueue = new Queue<string>(CommentatorLines.Idle);
        annoyedQueue = new Queue<string>(CommentatorLines.Annoyed);
        hoverQueue = new Queue<string>(CommentatorLines.Hover);
        hitQueue = new Queue<string>(CommentatorLines.Hit);

        GraffitiGuessGame.I.OnCorrect.AddListener(() => PlayFromQueue(successQueue));
        GraffitiGuessGame.I.OnWrong.AddListener(() => PlayFromQueue(failQueue));

        idleRoutine = StartCoroutine(IdleRoutine());

        PlayFromQueue(idleQueue);
    }
    private void Update()
    {
        _delayFromHit -= Time.deltaTime;
        if (_delayFromHit < 0)
        {
            _delayFromHit = 0;
        }

        _glove.SetActive(_displayGlove);
    }
    public void SetDisablePlaying(bool value)
    {
        disablePlaying = value;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayFromQueue(hoverQueue);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - _lastClickTime > 7)
        {
            annoyedQueue = new Queue<string>(CommentatorLines.Annoyed);
        }
        _lastClickTime = Time.time;
        
        PlayFromQueue(annoyedQueue);
    }

    public void OnHit()
    {
        float current = _delayFromHit;
        _delayFromHit = 0;//чтобы выполнился метод
        PlayFromQueue(hitQueue);
        _delayFromHit = current + _delayAddedFromHit;
    }
    void PlayFromQueue(Queue<string> queue)
    {
        if (queue.Count == 0)
            return;
        string line = queue.Dequeue();
        line = line.ToLower();
        queue.Enqueue(line);
        PlayFromQueue(line);
    }
    private void PlayFromQueue(string line)
    {
        if (_delayFromHit > 0)
            return;
        if (disablePlaying)
            return;
        
        if (showRoutine != null)
            StopCoroutine(showRoutine);

        aSrc.PlayOneShot(commentatorSounds[0]);

        showRoutine = StartCoroutine(ShowLine(line));
        StartCoroutine(WaitAfterLine());

        _linesPassed++;
        if (_linesPassed == _linesToShowGlove)
        {
            StartCoroutine(GloveAppear());
        }
    }
    private IEnumerator GloveAppear()
    {
        yield return new WaitForSeconds(2);
        _displayGlove = true;
        PlayFromQueue(CommentatorLines.BoxingGloveAppear);
    }
    private IEnumerator ShowLine(string line)
    {
        isShowing = true;

        textField.text = line;
        textField.gameObject.SetActive(true);

        float time = line.Length * waitPerCharMultiplier;

        if (time < 2f)
        {
            time = 2f;
        }

        OnStartMonolog?.Invoke();

        yield return new WaitForSeconds(time);

        textField.gameObject.SetActive(false);
        isShowing = false;

        OnHideMonolog?.Invoke();
    }

    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => disablePlaying);

            float delay = Random.Range(idleDelayRange.x, idleDelayRange.y);
            yield return new WaitForSeconds(delay);

            yield return new WaitWhile(() => pausePlayingIdle);
            yield return new WaitWhile(() => _delayFromHit > 0);

            if (!isShowing)
                PlayFromQueue(idleQueue);
        }
    }
    private IEnumerator WaitAfterLine()
    {
        pausePlayingIdle = true;
        yield return new WaitForSeconds(1.5f);
        pausePlayingIdle = false;
    }

}
