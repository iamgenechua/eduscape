# 🚀 Eduscape

![Eduscape poster](poster.png)
Eduscape is a single-player VR Escape Room aimed at making learning Physics fun and efficient for secondary school students.

As of v1.0, Eduscape covers *🔥 Thermal Physics*.

Check out the latest release [here](https://github.com/eduscapevr/eduscape/releases),
or see a video playthrough of the game [here](https://www.youtube.com/watch?v=bl38ZaQcHwc)!

## Features
- Immersive
  - Allows players to possess and use elements (metal, water, fire) to learn about the different states of matter
- Guided
  - Tutorial at the start of game on the controls
  - Hints given for difficult puzzles
- Enjoyable
  - Teleportation instead of movement to prevent motion sickness, especially for first-time VR users
  - Ambient music and beautiful scenes
  - Exciting end-game sequence to reward players for solving the puzzles
- Educational
  - Each puzzle links to the [Singapore-Cambridge GCE O Level Physics Syllabus](https://www.seab.gov.sg/docs/default-source/national-examinations/syllabus/olevel/2021syllabus/6091_y21_sy.pdf)
  - Learning objective of each puzzle summarised for players after solving it, reminding them of important concepts

## Playing

Download the latest release [here](https://github.com/eduscapevr/eduscape/releases) for Oculus Quest or Oculus Rift.
For the Quest, you will need [Android Debug Bridge (ADB)](https://developer.oculus.com/documentation/native/android/mobile-adb/)
and will have to follow the instructions below:

1. Open the command prompt or terminal on your computer. Navigate to the directory where ADB is installed.
2. Connect your Oculus Quest. You will need it to be in [Developer mode](https://developer.oculus.com/documentation/native/android/mobile-device-setup/).
3. Check that the Quest is detected by running the command `adb devices`. If you see that the device is "unauthorized", put on the headset and click "Allow" in the popup that appears.
4. Install Eduscape.apk from the latest release onto the Quest by running the command `adb install path/to/Eduscape.apk`
5. In your Oculus device, go to the Apps menu, select Unknown Sources, and click on Eduscape to begin the game.

On the Quest, the game might take up to 30 seconds to load.

## Building

**Required**:
- Unity
- VR Device

Eduscape was developed and tested on an *Oculus Quest*.
The instructions below include steps for the Oculus Rift, but we cannot guarantee they work.
Instructions for other headsets such as the HTC Vive are not provided, but the project should be buildable regardless of the VR device chosen.

1. Clone this repository.
2. Add the root directory of the project to Unity Hub and open the project.
3. To see the scene, double-click the `SampleScene.unity` file in the Scenes folder in the project window in Unity.
4. In Unity, go to `File > Build Settings`.
5. Ensure `Scenes/SampleScene` is *checked* and `Scenes/Main Menu` is *unchecked*.
6. If you are building for Oculus Quest:
   - Switch to the `Android` platform
   - Connect your Oculus Quest and select it as the `Run Device`
   - Press `Build and Run`
7. If you are building for Oculus Rift:
   - Ensure the `PC, Mac & Linux Standalone` platform is selected
   - Press `Build and Run`

### Planned Features

There is great potential for Eduscape in becoming a widely-used VR tool in education.

- Timer to simulate actual escape rooms
- Develop VR curriculum for more Physics topics (eg. Newtonian Mechanics, Electricity & Magnetism)

## Acknowledgements
Eduscape was created for the module CS4240 Interaction Design for Virtual and Augmented Reality at the National University of Singapore in Spring 2021, under the supervision of [Senior Lecturer Anand BHOJAN](https://www.comp.nus.edu.sg/cs/bio/bhojan/).

## Credits

### Assets
- [AllSky Free - 10 Sky / Skybox Set](https://assetstore.unity.com/packages/2d/textures-materials/sky/allsky-free-10-sky-skybox-set-146014)
- [Asteroids Pack](https://assetstore.unity.com/packages/3d/environments/asteroids-pack-84988)
- [Hi-Rez Spaceships Creator Free Sample](https://assetstore.unity.com/packages/3d/vehicles/space/hi-rez-spaceships-creator-free-sample-153363)
- [Sci Fi Doors](https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-doors-162876)
- [Sci-Fi Styled Modular Pack](https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-styled-modular-pack-82913)
- [Unity Particle Pack](https://assetstore.unity.com/packages/essentials/tutorial-projects/unity-particle-pack-127325)
- [VR Cinema for Mobile](https://assetstore.unity.com/packages/3d/props/interior/vr-cinema-for-mobile-150120)
- [Universal Sound FX](https://assetstore.unity.com/packages/audio/sound-fx/universal-sound-fx-17256)
