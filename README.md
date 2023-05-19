# Prerequisites
- API Requirement: Edit > Project Settings > Other Settings > `Minimum API Level` to at least `23`
- Android Manifest: Edit > Project Settings > Publishing Settings > Check `Custom Main Manifest`
- Permissions: Write a permissions in the `AndroidManifest.xml` (Assets/Plugins/Android)</br>
  Form: 
  ```xml
  <manifest>
    ...
    <application>
      ...
    </application>
    <uses-permission android:name="android.permission.WHAT_YOU_WANTS" />
  </manifest>
  ```

Reference: https://docs.unity3d.com/Manual/android-RequestingPermissions.html
