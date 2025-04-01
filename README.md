# HoloZoo

## TO DO LIST

### \(**@mihaelakri**)
- [ ] fix: UI scalability 

### \(**@izelentrovic**)
- [ ] refactor: bluetooth sending in FixedUpdate
- [ ] fix?: revisit bluetooth sending and FixedUpdate

### Unsorted
- [ ]

## BUGS noticed

 - [ ] In LogIn screen :  Object reference not set to an instance of an object
Login+<Log_in>d__4.MoveNext () (at Assets/Scripts/Login.cs:20) što je ubiti redak    var (user, response) = GameData.Instance.CheckUserCredentials(usernameField.text, passwordField.text) 
 - [ ] In LogIn, Home and globe : NullReferenceException: Object reference not set to an instance of an object
ApplyAccessibility.LoadObjects () (at Assets/Scripts/Accessibility/ApplyAccessibility.cs:134)

## Publishing to Google Play Store

- Build Settings
    - Identification 
        - Package name (`com.companyname.appname`), must be unique
        - Version (SemVer, e.g. `1.0.0`) to display to users
            - Version Code (increment for every upload to Google Play)
        - Minimum API level - based on required features
        - Target API level - should match the latest Google Play requirement (usually set to highest available)
    - Functional
        - Release build (uncheck **Development Build**)
        - **Compression Method** to `LZ4HC`
        - Target architecture (ARMv7 for older, ARM64 for newer devices - required)
        - *(questionable)* Disable **Auto Graphics API** unless necessary
            - Vulkan (Optional, good performance)
            - OpenGLES3 (Widely supported)
        - *(questionable)* Enable **SRGB Write** and verify color settings for correct rendering
    - Build system
        - Use **Gradle (recommended)** as the build system
        - Export as a `.aab` *(Android App Bundle)*
- Google Play requirements    
    - Privacy Policy (if app collects personal data)
    - 64-bit support (ARM64 support)
    - App Bundle (.aab) submission
    - **Prepare assets**:
        - Screenshots
        - Feature Graphic
        - App Icon (512x512)
        - Short & Full Descriptions

### Keystore

There are 2 main ways to manage a keystore, *manual* and *Google managed*.
Google managed is the easier, newer and safer way.  
The process is described [here in Android docs](https://developer.android.com/studio/publish/app-signing#app-signing-google-play).
Main feature is it removes the risk of getting locked out of updating the app if keystore is lost.  
It seems that in both approaches the usual way is to put a secure password on the keystore and host it on the same repo as the rest of the source code.

### Analytics/Crash reports

If actual ongoing support is desired, crash reports need to be collected using e.g. [Unity Cloud Diagnostics](https://docs.unity.com/ugs/manual/cloud-diagnostics/manual/CloudDiagnostics/WelcometoCloudDiagnostics), [Firebase](https://firebase.google.com/docs/unity/setup) or [Sentry](https://docs.sentry.io/platforms/unity/). Some SDKs like Firebase can also collect analytics to collect data about user behaviour and app usage.

## Done List

- [x] Popraviti komunikaciju da radi (samo bluetooth, maknut websockete)
- [x] Obavijesti o greškama ili stanju konekcije (za krajnjeg korisnika)
- [x] staviti jezike na oba uređaja odvojeno
- [x] popraviti server URL u skriptama
- [x] IndexOutOfRangeException: Index was outside the bounds of the array. Quiz.nextQuestion () (at Assets/Scripts/Quiz.cs:176)
- [x] životinja profil - slova ispravi
- [x] kviz šapa je na engleskom
- [x] area dodati jezike u bazu

- [x] fix: session not syncing with host on app relaunch
- [x] feat: smooth model movement on tablet with interpolation
- [x] fix: initial model should now be id 1, not 0
- [x] fix: animal rotation sliders ranges to play nice with smoothing
- [x] feat: notify user if not connected to internet
- [ ] ~~feat: notify user on remote host errors~~
- [x] check: translations for error messages when checking connection
- [x] refactor: ApplyAccessibility more granularly, not all text/buttons
- [x] chore: tag all images and text for contrast
- [x] refactor: Quiz and Animal_list scene to use indices instead of tags
- [x] refactor: Accessibility panel into a prefab that can be instantiated anywhere
- [x] fix: accessibility panel on/off text has no translation
- [x] fix: Quiz answers contrast colors
- [x] fix: Quiz score not tracking 
- [x] feat: loading indicator when fetching from host
- [x] fix: Animal_list contrast colors
- [x] \(**@mihaelakri**) feat: dodati logout na HologramTablet
- [x] \(**@mihaelakri**) feat: dodati privacy policy kod registacije i negdje scenu sa privacy policy 
- [x] \(**@mihaelakri**) feat: dodati obavijest o novoj životinji kad ju korisnik osvoji (na kraju kviza)  
- [ ] ~~\(**@izelentrovic**) build: add SQLite natively as instructed by [this](https://old.reddit.com/r/Unity3D/comments/sayh3r/ill_simply_add_sqlite_to_my_unity_project_and/kg91fm0/) and [this comment](https://github.com/praeclarum/sqlite-net/issues/1023#issuecomment-821950695)~~
- [x] \(**@izelentrovic**) feat: replace remote database with local JSON files
- [x] \(**@izelentrovic**) feat: remember previous scene for animal profile scene (globe/list)
- [x] \(**@izelentrovic**) feat: change "area" DB table so that animals can have multiple areas
- [x] \(**@izelentrovic**) feat: modify globe to have ocean as a clickable area and change Eurasia  
- [x] fix: animal model spawn coordinates
- [x] fix: animal model scaling on Game device
- [x] fix: animal model scaling on Hologram device
- [x] fix: screen size
- [x] build!: remove unused animal models
- [x] feat: remove all calls to remote host
- [x] feat: remove internet connection checks
- [x] fix: globe initial weird rendering

- [x] fix: not showing animals in oceas/south america (randomly started to work?)
- [x] fix: not showing animals with muliple area ids
- [x] fix: not applying dyslexia font on globe popup font 
- [x] fix: info screen dyslexia font not applying on project number text 
- [x] fix: globe popup not big enough for 3rd row (?)
- [x] fix: level scrollbar not interactable 
- [x] chore: add more and animals
- [x] chore: turn unused coroutines to methods
- [x] fix: translations for PrivacyPolicy
- [x] refactor: bluetooth sending in FixedUpdate
- [x] fix: animal mobile camera clipping (e.g. on elephant)
- [x] fix: animal model pivot should be centered
- [x] fix: score unlocked animals shouldn't show up when empty
- [x] feat?: remove login from Hologram device

- [x] chore add more questions
- [x] chore: add more animals per level 
- [x] fix: FillScore - ShowUnlockedAnimals: add translation
- [x] feat: manual Bluetooth turn on/off/connect
- [x] fix: re-do low resolution sprites
- [x] feat: BT overlays translations

## Cloning and running the project

- Clone the project
```sh
git clone https://github.com/mihaelakri/HoloZoo.git
```
1. Run Unity Hub in Administrator mode
2. Install *Unity 2021.3.14f1* (no addons) through Unity Hub.
3. Modify the Unity installation by adding Android tools.
4. Add the project to Unity Hub by **Add project from disk** and open it
5. Open the *Package manager*, view **My Assets** and import **ANIMALS FULL PACK**
6. Open `Editor > UsedAssets.cs` and edit the desired assets
7. Go to `HoloTools`
    1. Run `Convert Animal PNG textures to JPG` 
    2. Run `Copy Used Animals to Resources`
    3. Run `Modify Animal Animations`