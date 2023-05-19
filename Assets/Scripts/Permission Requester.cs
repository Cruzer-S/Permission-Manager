using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Android;

using 

[Flags]
public enum PermissionType : Int32
{
    Microphone = 1,
    Camera = 2,
    Vibrate = 4
};

public static class PermissionData
{
    private static Dictionary<PermissionType, string> name = new Dictionary<PermissionType, string>()
    {
        [PermissionType.Microphone] = Permission.Microphone,
        [PermissionType.Camera] = Permission.Camera,
    };

    private static Dictionary<string, PermissionType> type = new Dictionary<string, PermissionType>()
    {
        [Permission.Microphone] = PermissionType.Microphone,
        [Permission.Camera] = PermissionType.Camera,
    };

    public static string[] T2N(PermissionType types)
    {
        List<string> result = new List<string>();

        foreach (PermissionType type in Enum.GetValues(typeof(PermissionType)))
        {
            if ((types & type) == 0)
                continue;

            if (!name.ContainsKey(type))
                return null;

            result.Add(name[type]);
        }

        return result.ToArray();
    }
}

public class PermissionRequester
{
    private PermissionCallbacks callbacks;
    private Dictionary<string, bool> permissionRequestResult;
    private string[] permissions;
    private int counter;
    private bool result;
    private Action<bool> resultSender;

    public PermissionRequester(PermissionType types)
    {
        permissionRequestResult = new Dictionary<string, bool>();

        callbacks = new PermissionCallbacks();

        callbacks.PermissionDenied += PermissionDenied;
        callbacks.PermissionGranted += PermissionGranted;
        callbacks.PermissionDeniedAndDontAskAgain += PermissionDeniedAndDontAskAgain;

        permissions = PermissionData.T2N(types);
    }

    public void Request(Action<bool> result)
    {
        RemoveExistPermission();
        if (permissions.Length <= 0) {
            result.Invoke(true);
            return;
        }

        ResetResult();
        resultSender = result;

        Permission.RequestUserPermissions(permissions, callbacks);
    }

    private void RemoveExistPermission()
    {
        List<string> result = new List<string>();

        foreach (string permission in permissions)
        {
            if (Permission.HasUserAuthorizedPermission(permission)) {
                Debug.Log($"Permission {permission} already authorized.");
                continue;
            }

            result.Add(permission);
        }

        permissions = result.ToArray();
    }

    private void ResetResult()
    {
        permissionRequestResult.Clear();

        foreach (string permission in permissions)
            permissionRequestResult[permission] = false;

        counter = permissionRequestResult.Count;
        result = true;
    }

    private void PermissionDenied(string permissionName) => Set(permissionName, false);
    private void PermissionGranted(string permissionName) => Set(permissionName, true);
    private void PermissionDeniedAndDontAskAgain(string permissionName) => Set(permissionName, false);

    private void Set(string permission, bool result)
    {
        permissionRequestResult[permission] = result;
        if (result == false)
            this.result = false;

        if (--counter <= 0)
            resultSender(this.result);
    }
}