# Grid War Showdown

Grid War Showdown is a local, two-player, turn-based grid battle game written in C# using MonoGame. Players select item queues and take turns moving, attacking, healing, and using utility items.

## Run the included application (Windows)
1. Open the following folder inside the extracted files:

   ```text
   GridWarShowdown\GridWarShowdown\bin\Debug
   ```

2. Double-click **GridWarShowdown.exe**.

Keep the executable together with its DLLs, `Content` folder, `x86` and `x64` folders, and `Stats.txt`. Copying only the executable will leave required dependencies and game assets behind.

The application targets **.NET Framework 4.6.1** and needs a compatible .NET Framework runtime installed on Windows. A modern .NET SDK alone does not provide this runtime.

### Launch from PowerShell

Open PowerShell in the extracted outer `GridWarShowdown` folder (the one containing `GridWarShowdown.sln`), then run:

```powershell
Set-Location .\GridWarShowdown\bin\Debug
.\GridWarShowdown.exe
```

Launching from this directory also ensures the game reads and writes the bundled `Stats.txt` in the expected location.

## Start a game

1. Use **Up/Down** to select **Play** from the main menu and press **Enter**.
2. Use the mouse to browse items and add them to each player's queue. Both queues must be full before the game starts.
3. Adjust the maximum deaths setting if desired, then click **Fight**.
4. Players take turns using the same keyboard. Consult the menu's instructions and item manual for the game rules and individual item effects.

### Controls

| Input | Action |
| --- | --- |
| Up / Down | Navigate the main menu or select an item during play |
| Enter | Confirm a menu selection or use the selected item when available |
| W / A / S / D | Move or choose a direction for applicable items |
| Space | Toggle movement-only mode during play |
| 1 / 2 / 3 / 4 | Select top-left / top-right / bottom-left / bottom-right destinations for applicable utility items |
| Left / Right | Change pages in the instructions or item manual |
| Escape | Return from setup, instructions, item manual, or statistics |
| Mouse | Configure item queues and match settings; click the pause button during play |

Some actions depend on the selected item, cooldown, and available grid positions.

## Build and run from source (Windows)

Use Visual Studio with C#/.NET desktop development support and the **.NET Framework 4.6.1 targeting/developer pack**. The project references **MonoGame.Framework.DesktopGL 3.7.0.1708**, which is included in the ZIP's `packages` directory.

### 1. Restore the missing local library

The project references `Libraries\Animation2D.dll`, but that file is absent from the supplied source library folder. A copy exists in `bin\Debug`.

From the outer `GridWarShowdown` folder containing the solution, run:

```powershell
Copy-Item .\GridWarShowdown\bin\Debug\Animation2D.dll .\GridWarShowdown\Libraries\Animation2D.dll
```

`GameUtility.dll` is already present in `Libraries`.

### 2. Open and build the solution

1. Open **GridWarShowdown.sln** in Visual Studio.
2. If package references are unresolved, right-click the solution and choose **Restore NuGet Packages**. Keep the MonoGame package at the version specified in `packages.config`.
3. Set **GridWarShowdown** as the startup project if necessary.
4. Select **Debug** and **Any CPU**.
5. Choose **Build > Build Solution**.
6. Press **F5** to launch with debugging, or **Ctrl+F5** to launch without debugging.

### 3. Keep the bundled compiled assets

The ZIP includes compiled `.xnb` assets and `.ogg` music in `bin\Debug\Content`. Use the existing Debug output folder and keep these assets intact.

The project copies files from `Content\bin\DesktopGL`, but the supplied copy of that directory does not include the complete audio assets found in `bin\Debug\Content`. If you clean or delete the Debug output, restore the **entire `bin\Debug\Content` folder** from the original ZIP before launching again. For a Release build, copy that same complete folder into `bin\Release\Content` after building.

## Troubleshooting

| Problem | What to check |
| --- | --- |
| Windows reports a missing .NET Framework runtime | Install a compatible .NET Framework runtime, then launch again. |
| Visual Studio reports missing .NET Framework 4.6.1 reference assemblies | Install the matching targeting/developer pack. The runtime and targeting pack serve different purposes. |
| Build fails because `Animation2D` cannot be found | Copy `Animation2D.dll` from `bin\Debug` to `Libraries` using the command above. |
| MonoGame package or build targets cannot be found | Keep the extracted `packages` folder beside the solution, or restore the package specified in `packages.config`. |
| Missing textures, fonts, or audio; content-loading exception | Restore the complete `bin\Debug\Content` folder from the ZIP and keep it beside the executable. |
| Missing `SDL2.dll` or `soft_oal.dll` | Restore the bundled `x86` and `x64` directories beside the executable. |
| Statistics fail to load or save | Launch from `bin\Debug`, keep `Stats.txt` there, and use a folder your account can write to. |
| Fight does not start the match | Fill both players' item queues first. |

## Verification

These instructions were checked against the supplied project configuration, source code, and bundled files. The application was not launched or built in this environment; Windows runtime behavior has not been tested here.
