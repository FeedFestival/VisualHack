using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    private Main _main;

    private Image _loadingContainer;
    [HideInInspector]
    public Text LoadingIconText;
    [HideInInspector]
    public Text LoadingIconCircle;


    private GameObject _loadingIcon;
    [HideInInspector]
    public GameObject LoadingIcon
    {
        get { return _loadingIcon; }
        set
        {
            _loadingIcon = value;

            _loadingIcon.GetComponent<RectTransform>().localPosition = new Vector3(
                Logic.GetPercent(_main.GameProperties.Width / 2, 80),
                -Logic.GetPercent(_main.GameProperties.Height / 2, 70),
                0
                );

            _loadingContainer = _loadingIcon.transform.parent.GetComponent<Image>();
            _loadingContainer.gameObject.SetActive(true);
            _loadingContainer.rectTransform.sizeDelta = new Vector2(_main.GameProperties.Width, _main.GameProperties.Height);
            StartCoroutine(FinishLoaderWait());
        }
    }

    private float _fadeLerpTime;
    private bool _startFadeIn, _startFadeOut;

    public void Initialize(Main main)
    {
        _main = main;
    }

    void Update()
    {
        if (_startFadeIn)
            FadeAnim();
        if (_startFadeOut)
            FadeAnim();

        if (_loadingContainer.gameObject.activeSelf && _startFadeIn == false && _startFadeOut == false)
            LoadingIcon.transform.Rotate(new Vector3(0, 0, 100) * Time.deltaTime, Space.World);
    }

    public IEnumerator LoadMapWithText(string mapName)
    {
        if (_loadingContainer.gameObject.activeSelf) yield break;

        StartFade();

        LoadingIcon.SetActive(false);
        LoadingIconText.gameObject.SetActive(true);
        LoadingIconText.text = mapName;

        yield return new WaitForSeconds(2.3f);

        _main.ExecuteInitGame();
    }

    public IEnumerator LoaderWaitThenExecute(LoadThenExecute executeAfter)
    {
        if (_loadingContainer.gameObject.activeSelf) yield break;

        StartFade();

        LoadingIcon.SetActive(true);
        if (string.IsNullOrEmpty(LoadingIconCircle.text))
            LoadingIconCircle.text = Logic.circle;
        LoadingIconText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        if (executeAfter == LoadThenExecute.Button)
            _main.ExecuteButtonAction();
        else
            _main.ExecuteInitGame();
    }

    private void StartFade()
    {
        _startFadeIn = true;
        _loadingContainer.gameObject.SetActive(true);
    }

    public IEnumerator FinishLoaderWait()
    {
        yield return new WaitForSeconds(0.1f);

        _startFadeOut = true;
    }

    private void FadeAnim()
    {
        if (_startFadeIn)
        {
            _loadingContainer.color = Color.Lerp(Logic.Transparent, Logic.Black, _fadeLerpTime);
            LoadingIconCircle.color = Color.Lerp(Logic.Transparent, Logic.WhiteTransparent, _fadeLerpTime);
            LoadingIconText.color = Color.Lerp(Logic.Transparent, Logic.White, _fadeLerpTime);
        }
        else if (_startFadeOut)
        {
            _loadingContainer.color = Color.Lerp(Logic.Black, Logic.Transparent, _fadeLerpTime);
            LoadingIconCircle.color = Color.Lerp(Logic.WhiteTransparent, Logic.Transparent, _fadeLerpTime);
            LoadingIconText.color = Color.Lerp(Logic.White, Logic.Transparent, _fadeLerpTime);
        }

        _fadeLerpTime = _fadeLerpTime + Logic.LerpRatio * Logic.LerpSpeed;

        if (!(_fadeLerpTime >= 1)) return;

        if (_startFadeIn)
        {
            _loadingContainer.color = Logic.Black;
            _startFadeIn = false;
        }
        else if (_startFadeOut)
        {
            _loadingContainer.color = Logic.Transparent;
            _startFadeOut = false;

            _loadingContainer.gameObject.SetActive(false);
        }
        _fadeLerpTime = 0;
    }
}