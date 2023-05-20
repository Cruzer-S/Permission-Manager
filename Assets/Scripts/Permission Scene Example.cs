using System.Collections;

using UnityEngine;

using TMPro;

public class PermissionSceneExample : MonoBehaviour
{
    [SerializeField] private TMP_Text result;

    private IEnumerator Start()
    {
        result.text = "Wait...";

        yield return StartCoroutine(PermissionManager.Instance.RequestPermissionsUntilEnd(
            PermissionType.Microphone | PermissionType.Camera
        ));

        result.text = $"Done! ({PermissionManager.Instance.HasPermissions(PermissionType.Microphone | PermissionType.Camera)})";
    }
}
