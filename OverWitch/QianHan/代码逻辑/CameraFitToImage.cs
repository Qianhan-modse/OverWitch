using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // 允许编辑器实时预览
public class AdvancedScreenAdapter : MonoBehaviour
{
    // 核心参数配置
    const float DESIGN_WIDTH = 2560f;
    const float DESIGN_HEIGHT = 1600f;
    const float DESIGN_ASPECT = DESIGN_WIDTH / DESIGN_HEIGHT; // 1.6 (16:10)

    [Header("关键组件")]
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RawImage backgroundImage;

    [Header("适配模式")]
    [SerializeField] private AdaptationMode adaptationMode = AdaptationMode.Expand;

    private RectTransform _canvasRect;
    private Vector2 _lastScreenSize;

    // 适配模式枚举
    private enum AdaptationMode
    {
        StrictMatch,    // 严格匹配设计比例
        Expand,         // 扩展填充
        Shrink          // 收缩适应
    }

    private void Start()
    {
        InitializeComponents();
        ApplyFullscreenAdaptation();
    }

    private void Update()
    {
        // 实时监控分辨率变化
        if (Screen.width != _lastScreenSize.x || Screen.height != _lastScreenSize.y)
        {
            ApplyFullscreenAdaptation();
        }
    }

    private void InitializeComponents()
    {
        if(uiCanvas == null) { Debug.LogError("UI画布没有被赋值"); return; }
        if (uiCamera == null){ Debug.LogError("uiCamera 未被赋值!");return;}
        if (backgroundImage == null){ Debug.LogError("backgroundImage 未被赋值!");return;}
        _canvasRect = uiCanvas.GetComponent<RectTransform>();
        _lastScreenSize = new Vector2(Screen.width, Screen.height);

        // 强制设置纹理参数
        var texture = backgroundImage.texture;
        if (texture != null)
        {
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
        }

        // 配置Canvas Scaler
        var scaler = uiCanvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(DESIGN_WIDTH, DESIGN_HEIGHT);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }

    private void ApplyFullscreenAdaptation()
    {
        // 第一步：计算当前屏幕比例
        float currentAspect = (float)Screen.width / Screen.height;

        // 第二步：动态调整相机
        ConfigureUICamera(currentAspect);

        // 第三步：适配背景图像
        AdaptBackgroundImage(currentAspect);

        // 第四步：强制刷新布局
        Canvas.ForceUpdateCanvases();
        _lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private void ConfigureUICamera(float currentAspect)
    {
        uiCamera.orthographic = true;

        // 根据适配模式计算正交尺寸
        switch (adaptationMode)
        {
            case AdaptationMode.StrictMatch:
                uiCamera.orthographicSize = currentAspect > DESIGN_ASPECT ?
                    DESIGN_HEIGHT / 200f :
                    DESIGN_WIDTH / (200f * currentAspect);
                break;

            case AdaptationMode.Expand:
                uiCamera.orthographicSize = DESIGN_HEIGHT / 200f;
                break;

            case AdaptationMode.Shrink:
                uiCamera.orthographicSize = DESIGN_WIDTH / (200f * currentAspect);
                break;
        }

        // 设置相机裁剪平面
        uiCamera.nearClipPlane = 0.1f;
        uiCamera.farClipPlane = 1000f;
    }

    private void AdaptBackgroundImage(float currentAspect)
    {
        // 计算适配比例
        float scaleRatio = currentAspect > DESIGN_ASPECT ?
            Screen.height / DESIGN_HEIGHT :
            Screen.width / DESIGN_WIDTH;

        // 设置图像缩放
        backgroundImage.rectTransform.localScale = new Vector3(
            scaleRatio,
            scaleRatio,
            1
        );

        // 动态调整UV
        if (currentAspect != DESIGN_ASPECT)
        {
            float uvOffset = Mathf.Abs(1 - (currentAspect / DESIGN_ASPECT)) / 2;
            backgroundImage.uvRect = new Rect(
                currentAspect > DESIGN_ASPECT ? uvOffset : 0,
                currentAspect < DESIGN_ASPECT ? uvOffset : 0,
                currentAspect > DESIGN_ASPECT ? 1 - uvOffset * 2 : 1,
                currentAspect < DESIGN_ASPECT ? 1 - uvOffset * 2 : 1
            );
        }
    }

    // 编辑器可视化辅助
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            ApplyFullscreenAdaptation();
        }
    }
#endif
}