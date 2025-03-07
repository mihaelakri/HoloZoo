# HoloZoo

## TO DO LIST

- [ ] \(**@mihaelakri**) feat: dodati logout na HologramTablet
- [ ] \(**@mihaelakri**) feat: dodati privacy policy kod registacije i negdje scenu sa privacy policy 
- [ ] \(**@mihaelakri**) feat: dodati obavijest o novoj životinji kad ju korisnik osvoji (na kraju kviza)  
<br/>
  
- [ ] ~~\(**@izelentrovic**) build: add SQLite natively as instructed by [this](https://old.reddit.com/r/Unity3D/comments/sayh3r/ill_simply_add_sqlite_to_my_unity_project_and/kg91fm0/) and [this comment](https://github.com/praeclarum/sqlite-net/issues/1023#issuecomment-821950695)~~
- [x] \(**@izelentrovic**) feat: replace remote database with local JSON files
- [x] \(**@izelentrovic**) feat: remember previous scene for animal profile scene (globe/list)
- [x] \(**@izelentrovic**) feat: change "area" DB table so that animals can have multiple areas
- [x] \(**@izelentrovic**) feat: modify globe to have ocean as a clickable area and change Eurasia  
<br/>

- [x] fix: animal model spawn coordinates
- [x] fix: animal model scaling on Game device
- [x] fix: animal model scaling on Hologram device
- [x] fix: screen size
- [x] build!: remove unused animal models
- [x] feat: remove all calls to remote host
- [x] feat: remove internet connection checks
- [ ] chore: turn unused coroutines to methods
- [x] fix: globe initial weird rendering

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