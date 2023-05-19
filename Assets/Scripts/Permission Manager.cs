using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    private static PermissionManager __Instance;
    public static PermissionManager Instance {
        get {
            if (__Instance == null) {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<PermissionManager>();

                __Instance = gameObject.GetComponent<PermissionManager>();
            }

            return __Instance;
        }
    }

    public bool HasPermissions(PermissionType types)
    {
        string[] permissions = PermissionData.T2N(types);

        foreach (string permission in permissions)
        {
            if (Permission.HasUserAuthorizedPermission(permission))
                continue;

            return false;
        }

        return true;
    }

    public void RequestPermissions(PermissionType types, Action<bool> result) => new PermissionRequester(types).Request(result);
    public IEnumerator RequestPermissionsUntilEnd(PermissionType types)
    {
        bool done = false;

        new PermissionRequester(types).Request(
            (result) => {
                done = true;
            }
        );

        while ( !done )
            yield return new WaitForEndOfFrame();
    }
}