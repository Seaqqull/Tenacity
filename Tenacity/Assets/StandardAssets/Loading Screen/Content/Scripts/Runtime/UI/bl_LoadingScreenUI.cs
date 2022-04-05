using Lovatto.SceneLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bl_LoadingScreenUI : MonoBehaviour
{
    [Header("References")]
    public Text SceneNameText = null;
    public Text DescriptionText = null;
    public Text ProgressText = null;
    public Text TipText = null;
    public Image BackgroundImage = null;
    public Image FilledImage = null;
    public Slider LoadBarSlider = null;
    public GameObject ContinueUI = null;
    public GameObject RootUI;
    public GameObject FlashImage = null;
    public GameObject SkipKeyText = null;
    public RectTransform LoadingCircle = null;
    public CanvasGroup LoadingCircleCanvas = null;
    public CanvasGroup FadeImageCanvas = null;

    public CanvasGroup RootAlpha { get; set; }
    private CanvasGroup BackgroundAlpha = null;
    private CanvasGroup LoadingBarAlpha = null;
    private bl_SceneLoaderManager Manager;
    private bl_SceneLoader m_SceneLoader;
    private List<string> cacheTips = null;
    private List<Sprite> cacheBackgrounds = new List<Sprite>();
    private bool isTipFadeOut = false;
    private int CurrentTip = 0;
    private int CurrentBackground = 0;

    /// <summary>
    /// 
    /// </summary>
    public void Init(bl_SceneLoader loader)
    {
        m_SceneLoader = loader;
        Manager = bl_SceneLoaderManager.Instance;
        RootAlpha = RootUI.GetComponent<CanvasGroup>();
        if (LoadBarSlider != null) { LoadingBarAlpha = LoadBarSlider.GetComponent<CanvasGroup>(); }
        if (BackgroundImage != null) { BackgroundAlpha = BackgroundImage.GetComponent<CanvasGroup>(); }
        RootUI.SetActive(false);
        if (ContinueUI != null) ContinueUI.SetActive(false);
        if (FlashImage != null) { FlashImage.SetActive(false); }
        if (FadeImageCanvas != null)
        {
            FadeImageCanvas.alpha = 1;
            StartCoroutine(FadeOutCanvas(FadeImageCanvas));
        }
        if (SkipKeyText != null) { SkipKeyText.SetActive(false); }

        if (Manager.HasTips) { cacheTips = Manager.tipsList; }
        if (FilledImage != null) { FilledImage.type = Image.Type.Filled; FilledImage.fillAmount = 0; }

        Source.volume = 0;
        Source.loop = true;
        Source.clip = m_SceneLoader.BackgroundAudio;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void OnSceneChange(Scene from, Scene to)
    {
        RootUI.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateLoadProgress(float value, float delayedValue)
    { 
        //update the progress bar and text
        if (FilledImage != null) { FilledImage.fillAmount = value; }
        if (LoadBarSlider != null) { LoadBarSlider.value = value; }
        if (ProgressText != null && m_SceneLoader != null)
        {
            string percent = (delayedValue * 100).ToString("F0");
            ProgressText.text = string.Format(m_SceneLoader.LoadingTextFormat, percent);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnUpdate()
    {
        LoadingRotator();
    }

    /// <summary>
    /// 
    /// </summary>
    public void TransitionToScene()
    {
        //fade audio loop
        StartCoroutine(FadeAudio(false));
        StartCoroutine(LoadNextSceneIE());
    }

    /// <summary>
    /// 
    /// </summary>
    void LoadingRotator()
    {
        if (LoadingCircle == null)
            return;

        LoadingCircle.Rotate(-Vector3.forward * m_SceneLoader.DeltaTime * m_SceneLoader.LoadingCircleSpeed);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetUI()
    {
        FadeImageCanvas.alpha = 0;
        UpdateLoadProgress(0, 0);
        RootUI.SetActive(false);
        SkipKeyText?.SetActive(false);
        ContinueUI?.SetActive(false);
        FlashImage?.SetActive(false);
        LoadBarSlider?.gameObject?.SetActive(true);
        FilledImage?.gameObject?.SetActive(true);
        if (LoadingCircleCanvas != null) { LoadingCircleCanvas.alpha = 1; }
        if (LoadingBarAlpha != null) { LoadingBarAlpha.alpha = 1; }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnFinish()
    {
        switch (m_SceneLoader.GetSkipType)
        {
            case SceneSkipType.Button:
                ContinueUI?.SetActive(true);
                break;
            case SceneSkipType.Instant:
            case SceneSkipType.InstantComplete:
                break;
            case SceneSkipType.AnyKey:
                SkipKeyText?.SetActive(true);
                break;
        }

        if (LoadingCircleCanvas != null) { StartCoroutine(FadeOutCanvas(LoadingCircleCanvas, 0.5f)); }
        if (m_SceneLoader.FadeLoadingBarOnFinish)
        {
            if (LoadingBarAlpha != null) { StartCoroutine(FadeOutCanvas(LoadingBarAlpha, 1)); }
            else { LoadBarSlider?.gameObject?.SetActive(false); FilledImage?.gameObject?.SetActive(false); }
        }
        FlashImage?.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetupUIForScene(bl_SceneLoaderInfo info)
    {
        if(m_SceneLoader == null)
        {
            m_SceneLoader = transform.GetComponentInParent<bl_SceneLoader>();
            if(m_SceneLoader == null)
            {
                m_SceneLoader = bl_SceneLoaderUtils.GetLoader;
            }

            if(m_SceneLoader == null)
            {
                Debug.LogError("No scene loader has been found in this scene.");
                return;
            }

            Init(m_SceneLoader);
        }

        if (BackgroundImage != null && m_SceneLoader.useBackgrounds)
        {
            if (info.Backgrounds.Length > 1)
            {
                cacheBackgrounds.AddRange(info.Backgrounds);
                StartCoroutine(BackgroundTransition());
                BackgroundImage.color = Color.white;
            }
            else if (info.Backgrounds != null && info.Backgrounds.Length > 0)
            {
                BackgroundImage.sprite = info.Backgrounds[0];
                BackgroundImage.color = Color.white;
            }
        }
        if (SceneNameText != null) { SceneNameText.text = info.DisplayName; }
        if (DescriptionText != null)
        {
            if (m_SceneLoader.ShowDescription) { DescriptionText.text = info.Description; }
            else
            {
                DescriptionText.text = string.Empty;
            }
        }
        if (LoadBarSlider != null) { LoadBarSlider.value = 0; }
        if (ProgressText != null)
        {
            ProgressText.text = string.Format(m_SceneLoader.LoadingTextFormat, 0);
        }
        if (Manager.HasTips && TipText != null)
        {
            if (m_SceneLoader.RandomTips)
            {
                CurrentTip = Random.Range(0, cacheTips.Count);
                TipText.text = cacheTips[CurrentTip];
            }
            else
            {
                TipText.text = cacheTips[0];
            }
            StartCoroutine(TipsLoop());
        }
        //Show all UI
        RootAlpha.alpha = 0;
        RootUI.SetActive(true);

        //start audio loop
        Source.Play();
        StartCoroutine(FadeAudio(true));
    }

    #region Coroutines
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator BackgroundTransition()
    {
        while (true)
        {
            BackgroundImage.sprite = cacheBackgrounds[CurrentBackground];
            while (BackgroundAlpha.alpha < 1)
            {
                BackgroundAlpha.alpha += DeltaTime * m_SceneLoader.FadeBackgroundSpeed * 0.8f;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(m_SceneLoader.TimePerBackground);
            while (BackgroundAlpha.alpha > 0)
            {
                BackgroundAlpha.alpha -= DeltaTime * m_SceneLoader.FadeBackgroundSpeed;
                yield return new WaitForEndOfFrame();
            }
            CurrentBackground = (CurrentBackground + 1) % cacheBackgrounds.Count;
            yield return new WaitForSeconds(m_SceneLoader.TimeBetweenBackground);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator TipsLoop()
    {
        if (TipText == null)
            yield break;

        Color alpha = TipText.color;
        if (isTipFadeOut)
        {
            while (alpha.a < 1)
            {
                alpha.a += DeltaTime * m_SceneLoader.FadeTipsSpeed;
                TipText.color = alpha;
                yield return null;
            }
            StartCoroutine(WaitNextTip(m_SceneLoader.TimePerTip));
        }
        else
        {
            while (alpha.a > 0)
            {
                alpha.a -= DeltaTime * m_SceneLoader.FadeTipsSpeed;
                TipText.color = alpha;
                yield return null;
            }
            StartCoroutine(WaitNextTip(0.75f));
        }
        if (isTipFadeOut)
        {
            if (m_SceneLoader.RandomTips)
            {
                int lastTip = CurrentTip;
                CurrentTip = Random.Range(0, cacheTips.Count);
                while (CurrentTip == lastTip)
                {
                    CurrentTip = Random.Range(0, cacheTips.Count);
                    yield return null;
                }
                TipText.text = cacheTips[CurrentTip];
            }
            else
            {
                CurrentTip = (CurrentTip + 1) % cacheTips.Count;
                TipText.text = cacheTips[CurrentTip];
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeAudio(bool FadeIn)
    {
        if (m_SceneLoader.BackgroundAudio == null)
            yield break;

        if (FadeIn)
        {
            while (Source.volume < m_SceneLoader.AudioVolume)
            {
                Source.volume += DeltaTime * m_SceneLoader.FadeAudioSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (Source.volume > 0)
            {
                Source.volume -= DeltaTime * m_SceneLoader.FadeAudioSpeed * 3;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator WaitNextTip(float t)
    {
        isTipFadeOut = !isTipFadeOut;
        yield return new WaitForSeconds(t);
        StartCoroutine(TipsLoop());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNextSceneIE()
    {
        FadeImageCanvas.alpha = 0;
        while (FadeImageCanvas.alpha < 1)
        {
            FadeImageCanvas.alpha += DeltaTime * m_SceneLoader.FadeInSpeed;
            yield return null;
        }
        m_SceneLoader.sceneAsyncOp.allowSceneActivation = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator FadeOutCanvas(CanvasGroup alpha, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        while (alpha.alpha > 0)
        {
            alpha.alpha -= DeltaTime * m_SceneLoader.FadeOutSpeed;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator FadeInCanvas(CanvasGroup alpha, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        while (alpha.alpha < 1)
        {
            alpha.alpha += DeltaTime * m_SceneLoader.FadeInSpeed;
            yield return null;
        }
    }
    #endregion

    public float DeltaTime => m_SceneLoader.DeltaTime;

    private AudioSource m_aSource;
    private AudioSource Source
    {
        get
        {
            if(m_aSource == null)
            {
                m_aSource = GetComponent<AudioSource>();
                if (m_aSource == null)
                {
                    m_aSource = gameObject.AddComponent<AudioSource>();
                    m_aSource.playOnAwake = false;
                }
            }
            return m_aSource;
        }
    }
}