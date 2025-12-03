# Commit Strategy for 24-Class Course

## Overview
This document outlines the progressive commit strategy for teaching Unity game development through 24 classes. Since the complete project already exists, we use a combination of documentation and tagged reference points to guide students through the development process.

## Teaching Approach

### Method 1: Build-Along (Recommended)
Students create a NEW project and build alongside the instructor:
1. Instructor demonstrates each step
2. Students replicate in their own project
3. Reference this complete project for troubleshooting
4. Presentations guide the concepts
5. Hands-on exercises reinforce learning

### Method 2: Deconstruction
Students explore the complete project:
1. Study each component individually
2. Understand how pieces fit together
3. Modify and experiment
4. Rebuild sections from scratch
5. Use as reference for own projects

---

## Part 1: Foundational Unity & Core Setup (Classes 1-6)

### Class 1: Unity Editor Introduction & Basic Architecture
**Starting Point:** Empty Unity 3D URP Project
**Steps:**
1. Create new 3D URP project in Unity Hub
2. Explore Unity interface
3. Practice Scene view navigation
4. Create new scene: "Gameplay"
5. Save scene

**Files Created:**
- Gameplay.unity

---

### Class 2: GameObjects, Transforms & Scene Setup
**Build On:** Class 1
**Steps:**
1. Ensure Main Camera exists (default)
2. Ensure Directional Light exists (default)
3. Position camera: (-0.2, 1.97, -6.62), Rotation: (-5, 0, 0)
4. Position light: (0, 3, 0), Rotation: (50, -30, 0)
5. Create empty GameObject "GameController"
6. Save scene

**Files Modified:**
- Gameplay.unity

---

### Class 3: 3D Primitives & Materials
**Build On:** Class 2
**Steps:**
1. Create folder: Assets/Materials
2. Create Plane: GameObject > 3D Object > Plane
3. Scale plane: (5, 1, 5)
4. Position plane: (0, -1, 0)
5. Create material: "Ground"
6. Set color: Brown/Gray
7. Apply to plane
8. Tag plane as "Ground"
9. Create material: "Brick"
10. Set color: Red/Orange
11. Save scene

**Files Created:**
- Materials/Ground.mat
- Materials/Brick.mat

---

### Class 4: Prefabs & Project Organization
**Build On:** Class 3
**Steps:**
1. Create folder: Assets/Prefabs/3D
2. Create Cube in scene
3. Scale: (2, 1, 1) - brick shape
4. Apply Brick material
5. Add Box Collider (auto-added)
6. Drag to Prefabs folder → "Brick_Prefab"
7. Delete instance from scene
8. Create folder structure for project
9. Save scene

**Files Created:**
- Prefabs/3D/Brick_Prefab.prefab

---

### Class 5: Introduction to C# Scripting I
**Build On:** Class 4
**Steps:**
1. Create folder: Assets/Scripts/3D
2. Create script: "BrickTest.cs"
3. Add code with variables and Debug.Log
4. Attach to Brick prefab
5. Test in Play mode
6. Observe Console output

**Files Created:**
- Scripts/3D/BrickTest.cs

---

### Class 6: Introduction to C# Scripting II
**Build On:** Class 5
**Steps:**
1. Expand BrickTest.cs with:
   - Methods with parameters
   - Conditionals (if/else)
   - Loops
2. Practice different control flow
3. Test various scenarios
4. Prepare for physics

**Files Modified:**
- Scripts/3D/BrickTest.cs

---

## Part 2: Physics & Input Systems (Classes 7-11)

### Class 7: Physics System I - Rigidbody
**Build On:** Class 6
**Steps:**
1. Open Brick prefab
2. Add Component: Rigidbody
3. Set Mass: 50
4. Enable "Use Gravity"
5. Enable "Is Kinematic" (initially)
6. Update script with Rigidbody reference
7. Test brick falling with Space key

**Files Modified:**
- Brick_Prefab.prefab (added Rigidbody)
- BrickTest.cs (updated)

---

### Class 8: Physics System II - Colliders & Collision
**Build On:** Class 7
**Steps:**
1. Create folder: Assets/PhysicsMaterials
2. Create Physic Material: "BrickMaterial"
3. Set Dynamic Friction: 0.6
4. Set Bounciness: 0.3
5. Apply to Brick's Box Collider
6. Add OnCollisionEnter to script
7. Test collision with ground

**Files Created:**
- PhysicsMaterials/BrickMaterial.physicMaterial

**Files Modified:**
- BrickTest.cs (added collision detection)

---

### Class 9: Input System & Mouse Handling
**Build On:** Class 8
**Steps:**
1. Install Input System package
2. Create script: "RayCastManager.cs"
3. Set up InputAction for mouse click
4. Implement OnEnable/OnDisable
5. Log mouse position on click
6. Add to scene

**Files Created:**
- Scripts/3D/RayCastManager.cs

---

### Class 10: Raycasting & Object Detection
**Build On:** Class 9
**Steps:**
1. Create interface: "IOption.cs"
2. Complete RayCastManager with raycast
3. Update Brick script to implement IOption
4. Test clicking on bricks
5. Log hit information

**Files Created:**
- Scripts/3D/IOption.cs

**Files Modified:**
- RayCastManager.cs (added raycast)
- Brick script (implements IOption)

---

### Class 11: Forces & Physics Interactions
**Build On:** Class 10
**Steps:**
1. Create full Brick3D.cs script with:
   - Impact force application
   - Torque for rotation
   - Physics.OverlapSphere for area effect
   - DetachBrick method
2. Add public variables for tuning
3. Test clicking bricks with forces
4. Adjust force values

**Files Created:**
- Scripts/3D/Brick3D.cs (replaces BrickTest.cs)

---

## Part 3: Midterm Build-up (Classes 12-16)

### Class 12: Procedural Generation - Wall Generator
**Build On:** Class 11
**Steps:**
1. Create script: "WallGenerator3D.cs"
2. Add grid generation logic
3. Add public variables (rows, columns, spacing)
4. Create WallGenerator GameObject
5. Assign brick prefab
6. Test wall generation

**Files Created:**
- Scripts/3D/WallGenerator3D.cs

---

### Class 13: Events & Delegates
**Build On:** Class 12
**Steps:**
1. Add events to Brick3D.cs:
   - OnAnyBrickDetached
   - OnScoredPoints
2. Create script: "WallManager.cs"
3. Subscribe to brick events
4. Track remaining bricks
5. Implement wall progression

**Files Created:**
- Scripts/3D/WallManager.cs

**Files Modified:**
- Brick3D.cs (added events)

---

### Class 14: Game Management & Singleton Pattern
**Build On:** Class 13
**Steps:**
1. Create script: "GameManager.cs"
2. Implement singleton pattern
3. Add score tracking
4. Add game state enum
5. Implement StartGame/EndGame
6. Connect to WallManager

**Files Created:**
- Scripts/3D/GameManager.cs

---

### Class 15: Coroutines & Timer Systems
**Build On:** Class 14
**Steps:**
1. Add timer coroutine to GameManager
2. Implement countdown
3. Update UI placeholder
4. Handle timer completion
5. Test game flow with timer

**Files Modified:**
- GameManager.cs (added timer)

---

### Class 16: MIDTERM PROJECT SPRINT
**Build On:** Class 15
**Steps:**
1. Integrate all systems
2. Test complete game flow
3. Fix any bugs
4. Tune physics values
5. Verify core loop works

**Deliverable:** Working core game with:
- Clickable bricks with physics
- Generated walls
- Score tracking
- Timer countdown
- Wall progression

---

## Part 4: Polish & Complete Game (Classes 17-24)

### Class 17: UI System I - Canvas & Layout
**Build On:** Class 16
**Steps:**
1. Import TextMeshPro Essentials
2. Create Canvas (Screen Space - Overlay)
3. Set Canvas Scaler: Scale With Screen Size (1920x1080)
4. Create UI panels:
   - MainMenuPanel
   - TimeSelectPanel
   - GameplayPanel
   - PausePanel
   - GameOverPanel
5. Set up anchors and layout

**Files Created:**
- UI panels in scene

---

### Class 18: UI System II - TextMeshPro & Buttons
**Build On:** Class 17
**Steps:**
1. Add TMP_Text elements to panels
2. Add Buttons with events
3. Create script: "UIManager.cs"
4. Wire up button events
5. Implement panel switching
6. Connect to GameManager

**Files Created:**
- Scripts/3D/UIManager.cs

---

### Class 19: Audio System
**Build On:** Class 18
**Steps:**
1. Import audio files to Assets/SFX
2. Create script: "SoundManager.cs"
3. Add AudioSource components
4. Implement music and SFX methods
5. Connect to events
6. Test all audio

**Files Created:**
- Scripts/3D/SoundManager.cs

---

### Class 20: Visual Effects I - Particle Systems
**Build On:** Class 19
**Steps:**
1. Import/use Cartoon FX package
2. Create custom impact effect
3. Add effect references to Brick3D
4. Instantiate effects on:
   - Brick hit
   - Ground collision
5. Test all effects

**Files Modified:**
- Brick3D.cs (added effects)

---

### Class 21: Visual Effects II - Polish & Game Feel
**Build On:** Class 20
**Steps:**
1. Add camera shake script
2. Implement screen flash
3. Add celebration effects
4. Layer feedback systems
5. Polish all interactions

**Files Created:**
- Scripts/3D/CameraShake.cs (optional)

---

### Class 22: Animation with DOTween
**Build On:** Class 21
**Steps:**
1. Setup DOTween (already in project)
2. Add camera movement with DOTween
3. Implement brick spawn animation
4. Add squash and stretch
5. Animate UI panels

**Files Modified:**
- WallManager.cs (DOTween camera)
- WallGenerator3D.cs (animations)
- UIManager.cs (panel animations)

---

### Class 23: Advanced Features & Optimization
**Build On:** Class 22
**Steps:**
1. Implement wall patterns (enum + switch)
2. Add weather effects
3. Implement progressive difficulty
4. Basic object pooling (optional)
5. Profile and optimize
6. Prepare for final build

**Files Modified:**
- WallGenerator3D.cs (patterns)
- Various optimization

---

### Class 24: FINAL PROJECT SPRINT
**Build On:** Class 23
**Steps:**
1. Final integration check
2. Complete polish pass
3. Build the game
4. Test built version
5. Present project

**Deliverable:** Complete, polished, playable game!

---

## File Structure by Part

### After Part 1 (Class 6)
```
Assets/
├── Materials/
│   ├── Ground.mat
│   └── Brick.mat
├── Prefabs/3D/
│   └── Brick_Prefab.prefab
├── Scripts/3D/
│   └── BrickTest.cs
└── Scenes/
    └── Gameplay.unity
```

### After Part 2 (Class 11)
```
Assets/
├── Materials/
├── PhysicsMaterials/
│   └── BrickMaterial.physicMaterial
├── Prefabs/3D/
├── Scripts/3D/
│   ├── Brick3D.cs
│   ├── IOption.cs
│   └── RayCastManager.cs
└── Scenes/
```

### After Part 3 (Class 16)
```
Assets/
├── Materials/
├── PhysicsMaterials/
├── Prefabs/3D/
├── Scripts/3D/
│   ├── Brick3D.cs
│   ├── GameManager.cs
│   ├── IOption.cs
│   ├── RayCastManager.cs
│   ├── WallGenerator3D.cs
│   └── WallManager.cs
└── Scenes/
```

### After Part 4 (Class 24)
```
Assets/
├── Materials/
├── PhysicsMaterials/
├── Prefabs/3D/
├── Scripts/3D/
│   ├── Brick3D.cs
│   ├── GameManager.cs
│   ├── IOption.cs
│   ├── RayCastManager.cs
│   ├── SoundManager.cs
│   ├── UIManager.cs
│   ├── WallGenerator3D.cs
│   └── WallManager.cs
├── SFX/
├── UI Assets/
├── JMO Assets/
├── Plugins/Demigiant/
└── Scenes/
```

---

## Teaching Tips

1. **Live Coding:** Type code with students, explain as you go
2. **Frequent Testing:** Test after each major change
3. **Error Handling:** Show how to read and fix errors
4. **Experimentation:** Encourage changing values
5. **Questions:** Pause for questions regularly
6. **Recap:** Start each class with previous class recap
7. **Homework:** Assign practice exercises

## Assessment Ideas

### Part 1 Assessment
- Create custom prefab with materials
- Write script with variables and Debug.Log

### Part 2 Assessment
- Implement physics object that responds to click
- Create collision detection system

### Part 3 Assessment (Midterm)
- Complete core game loop working
- All systems integrated

### Part 4 Assessment (Final)
- Complete polished game
- All features working
- Built and playable

## Common Student Issues

1. **Forgetting to save:** Remind frequently
2. **Wrong component:** Check Inspector carefully
3. **Typos in code:** Case-sensitive!
4. **Missing references:** Assign in Inspector
5. **Not testing:** Test after each change
6. **Giving up:** Encourage persistence

---

This strategy provides a clear path from empty project to complete game while using this repository as a reference and troubleshooting resource.

