using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
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

            //_loadingIcon.GetComponent<RectTransform>().localPosition = new Vector3(
            //    UiUtils.GetPercent(_main.GameProperties.Width / 2, 80),
            //    -UiUtils.GetPercent(_main.GameProperties.Height / 2, 70),
            //    0
            //    );

            _loadingContainer = _loadingIcon.transform.parent.GetComponent<Image>();
            _loadingContainer.gameObject.SetActive(true);
            //_loadingContainer.rectTransform.sizeDelta = new Vector2(_main.GameProperties.Width, _main.GameProperties.Height);
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
            _loadingContainer.color = Color.Lerp(UiUtils.Transparent, UiUtils.Black, _fadeLerpTime);
            LoadingIconCircle.color = Color.Lerp(UiUtils.Transparent, UiUtils.WhiteTransparent, _fadeLerpTime);
            LoadingIconText.color = Color.Lerp(UiUtils.Transparent, UiUtils.White, _fadeLerpTime);
        }
        else if (_startFadeOut)
        {
            _loadingContainer.color = Color.Lerp(UiUtils.Black, UiUtils.Transparent, _fadeLerpTime);
            LoadingIconCircle.color = Color.Lerp(UiUtils.WhiteTransparent, UiUtils.Transparent, _fadeLerpTime);
            LoadingIconText.color = Color.Lerp(UiUtils.White, UiUtils.Transparent, _fadeLerpTime);
        }

        _fadeLerpTime = _fadeLerpTime + Utils.LerpRatio * Utils.LerpSpeed;

        if (!(_fadeLerpTime >= 1)) return;

        if (_startFadeIn)
        {
            _loadingContainer.color = UiUtils.Black;
            _startFadeIn = false;
        }
        else if (_startFadeOut)
        {
            _loadingContainer.color = UiUtils.Transparent;
            _startFadeOut = false;

            _loadingContainer.gameObject.SetActive(false);
        }
        _fadeLerpTime = 0;
    }
}