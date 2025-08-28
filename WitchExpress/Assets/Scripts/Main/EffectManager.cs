using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Post-process Volume ������Ʈ�� ������ ����
    public PostProcessVolume volume;

    // ��� ȿ���� ������ Color Grading ����
    private ColorGrading colorGrading;
    // Depth of Field ȿ���� ������ DepthOfField ����
    private DepthOfField depthOfField;


    private void Start()
    {
        // volume���� ColorGrading ������ �����ɴϴ�.
        volume.profile.TryGetSettings(out colorGrading);
        // volume���� DepthOfField ������ �����ɴϴ�.
        volume.profile.TryGetSettings(out depthOfField);

        // ���� ���� �� �� ȿ���� ��Ȱ��ȭ
        if (depthOfField != null)
        {
            depthOfField.enabled.value = false;
        }
    }

    private void Update()
    {
        // ESC Ű�� ������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �� �޼��带 ��� ȣ��
            ToggleBlackAndWhite();
            ToggleBlur();
        }
    }

    // ��� ȿ���� ����մϴ�. (ä�� -50)

    public void ToggleBlackAndWhite()
    {
        if (colorGrading != null)
        {
            // ���� ä�� ���� ���� ��� ���¸� �Ǵ��ϰ� ����մϴ�.
            bool isCurrentlyBlackAndWhite = colorGrading.saturation.value < -25f;
            colorGrading.saturation.value = isCurrentlyBlackAndWhite ? 0f : -50f;
        }
    }
    // �ɵ�(��) ȿ���� ����մϴ�.
    public void ToggleBlur()
    {
        if (depthOfField != null)
        {
            // Depth of Field ȿ���� enabled �Ӽ� ���� ���� ������ŵ�ϴ�.
            depthOfField.enabled.value = !depthOfField.enabled.value;
        }
    }
}
