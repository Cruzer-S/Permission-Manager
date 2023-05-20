using System.Collections;

using UnityEngine;

using TMPro;

public class PermissionSceneExample : MonoBehaviour
{
    [SerializeField] private TMP_Text result;

    private void Awake()
    {
        result.text = "Wait...";
    }

    private IEnumerator Start()
    {
        yield return PermissionManager.Instance.RequestPermissionsUntilEnd(
            PermissionType.Microphone | PermissionType.Camera
        );

        result.text = $"Done! ({PermissionManager.Instance.HasPermissions(PermissionType.Microphone | PermissionType.Camera)})";
    }
}
