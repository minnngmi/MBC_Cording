using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Post-process Volume 컴포넌트를 연결할 변수
    public PostProcessVolume volume;

    // 흑백 효과를 제어할 Color Grading 설정
    private ColorGrading colorGrading;
    // Depth of Field 효과를 제어할 DepthOfField 설정
    private DepthOfField depthOfField;


    private void Start()
    {
        // volume에서 ColorGrading 설정을 가져옵니다.
        volume.profile.TryGetSettings(out colorGrading);
        // volume에서 DepthOfField 설정을 가져옵니다.
        volume.profile.TryGetSettings(out depthOfField);

        // 게임 시작 시 블러 효과를 비활성화
        if (depthOfField != null)
        {
            depthOfField.enabled.value = false;
        }
    }

    private void Update()
    {
        // ESC 키를 누르면
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 두 메서드를 모두 호출
            ToggleBlackAndWhite();
            ToggleBlur();
        }
    }

    // 흑백 효과를 토글합니다. (채도 -50)

    public void ToggleBlackAndWhite()
    {
        if (colorGrading != null)
        {
            // 현재 채도 값에 따라 흑백 상태를 판단하고 토글합니다.
            bool isCurrentlyBlackAndWhite = colorGrading.saturation.value < -25f;
            colorGrading.saturation.value = isCurrentlyBlackAndWhite ? 0f : -50f;
        }
    }
    // 심도(블러) 효과를 토글합니다.
    public void ToggleBlur()
    {
        if (depthOfField != null)
        {
            // Depth of Field 효과의 enabled 속성 값을 직접 반전시킵니다.
            depthOfField.enabled.value = !depthOfField.enabled.value;
        }
    }
}
