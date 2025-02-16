# HoloZoo

## TO DO LIST

- [x] Popraviti komunikaciju da radi (samo bluetooth, maknut websockete)
- [ ] Obavijesti o greškama ili stanju konekcije (za krajnjeg korisnika)
- [x] staviti jezike na oba uređaja odvojeno
- [x] popraviti server URL u skriptama
- [ ] popraviti kordinate učitavanja modela (x,y)
- [ ] IndexOutOfRangeException: Index was outside the bounds of the array. Quiz.nextQuestion () (at Assets/Scripts/Quiz.cs:176)
- [x] životinja profil - slova ispravi
- [x] kviz šapa je na engleskom
- [ ] prilagoditi veličinu ekrana
- [x] area dodati jezike u bazu

- [x] fix: session not syncing with host on app relaunch
- [x] feat: smooth model movement on tablet with interpolation
- [x] fix: initial model should now be id 1, not 0
- [ ] fix: animal rotation sliders ranges to play nice with smoothing
- [x] feat: notify user if not connected to internet
- [ ] feat: notify user on remote host errors
- [ ] check: translations for error messages when checking connection

## Cloning and running the project

- Clone the project
```sh
git clone https://github.com/mihaelakri/HoloZoo.git
```
- Run Unity Hub in Administrator mode
- Install *Unity 2021.3.14f1* (no addons) through Unity Hub.
- Modify the Unity installation by adding Android tools.
- Add the project to Unity Hub by **Add project from disk** and open it
- Open the *Package manager*, view **My Assets** and import **ANIMALS FULL PACK**
- Move the `ANIMALS FULL PACK` directory from `Assets` to `Assets/Resources`