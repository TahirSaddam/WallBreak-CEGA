# Commit Strategy for 15-Class Course

## Overview
This document outlines the progressive commit strategy for teaching Unity game development through 15 classes. Since the complete project already exists, we use a combination of documentation and tagged reference points to guide students through the development process.

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

## Progressive Build Guide

### Class 1: Unity Basics & Project Setup
**Starting Point:** Empty Unity 3D URP Project
**Steps:**
1. Create new 3D URP project in Unity Hub
2. Create new scene: "Gameplay"
3. Ensure Main Camera exists (default)
4. Ensure Directional Light exists (default)
5. Position camera: (-0.2, 1.97, -6.62), Rotation: (-5, 0, 0)
6. Position light: (0, 3, 0), Rotation: (50, -30, 0)
7. Save scene

**Files Created:**
- Gameplay.unity

---

### Class 2: 3D Primitives & Materials
**Build On:** Class 1
**Steps:**
1. Create folder: Assets/Materials
2. Create Plane: GameObject > 3D Object > Plane
3. Scale plane: (5, 1, 5)
4. Position plane: (0, -1, 0)
5. Create material: "Ground"
6. Set color: Brown/Gray
7. Apply to plane
8. Tag plane as "Ground"
9. Create Cube for testing
10. Create material: "Brick"
11. Set color: Red/Orange
12. Save scene

**Files Created:**
- Materials/Ground.mat
- Materials/Brick.mat

---

### Class 3: Prefabs & Object Instantiation
**Build On:** Class 2
**Steps:**
1. Create folder: Assets/Prefabs
2. Create Cube in scene
3. Scale: (2, 1, 1) - brick shape
4. Apply Brick material
5. Add Box Collider (auto-added)
6. Drag to Prefabs folder → "Brick_Prefab"
7. Delete instance from scene
8. Test: Drag prefab to scene multiple times
9. Save scene

**Files Created:**
- Prefabs/Brick_Prefab.prefab

---

### Class 4: Introduction to C# Scripting
**Build On:** Class 3
**Steps:**
1. Create folder: Assets/Scripts
2. Create script: "BrickTest.cs"
3. Add code:
```csharp
using UnityEngine;

public class BrickTest : MonoBehaviour
{
    public Color brickColor = Color.red;
    private int clickCount = 0;
    
    void Start()
    {
        Debug.Log("Brick initialized at: " + transform.position);
    }
    
    void Update()
    {
        // Log position every second
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"Brick position: {transform.position}");
        }
    }
}
```
4. Attach to Brick prefab
5. Test in Play mode
6. Observe Console output

**Files Created:**
- Scripts/BrickTest.cs

---

### Class 5: Physics System - Rigidbody
**Build On:** Class 4
**Steps:**
1. Open Brick prefab
2. Add Component: Rigidbody
3. Set Mass: 50
4. Enable "Use Gravity"
5. Enable "Is Kinematic" (initially)
6. Update BrickTest.cs:
```csharp
private Rigidbody rb;

void Awake()
{
    rb = GetComponent<Rigidbody>();
}

void Update()
{
    // Press Space to make brick fall
    if (Input.GetKeyDown(KeyCode.Space))
    {
        rb.isKinematic = false;
        Debug.Log("Brick released!");
    }
}
```
7. Test: Place brick above ground, press Space
8. Save

**Files Modified:**
- Brick_Prefab.prefab (added Rigidbody)
- BrickTest.cs (updated)

---

### Class 6: Colliders & Collision Detection
**Build On:** Class 5
**Steps:**
1. Create folder: Assets/PhysicsMaterials
2. Create Physic Material: "BrickMaterial"
3. Set Dynamic Friction: 0.6
4. Set Bounciness: 0.3
5. Apply to Brick's Box Collider
6. Update BrickTest.cs:
```csharp
void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        Debug.Log("Brick hit ground!");
        float force = collision.relativeVelocity.magnitude;
        Debug.Log($"Impact force: {force}");
    }
}
```
7. Ensure Ground has tag "Ground"
8. Test collision detection

**Files Created:**
- PhysicsMaterials/BrickMaterial.physicMaterial

**Files Modified:**
- BrickTest.cs (added collision detection)

---

### Class 7: Input System & Raycasting
**Build On:** Class 6
**Steps:**
1. Install Input System package
2. Create interface: "IOption.cs"
```csharp
using UnityEngine;

public interface IOption
{
    void OnClicked(Vector3 impactPoint);
}
```
3. Create script: "RayCastManager.cs"
```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class RayCastManager : MonoBehaviour
{
    private Camera playerCamera;
    private InputAction clickAction;
    
    void Start()
    {
        playerCamera = Camera.main;
    }
    
    void Awake()
    {
        clickAction = new InputAction();
        clickAction.AddBinding("<Mouse>/leftButton");
        clickAction.performed += OnMouseClick;
    }
    
    void OnEnable()
    {
        clickAction.Enable();
    }
    
    void OnDisable()
    {
        clickAction.Disable();
    }
    
    void OnMouseClick(CallbackContext ctx)
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IOption clickable = hit.collider.GetComponent<IOption>();
            if (clickable != null)
            {
                clickable.OnClicked(hit.point);
            }
        }
    }
}
```
4. Update Brick script to implement IOption
5. Add RayCastManager to scene
6. Test clicking on bricks

**Files Created:**
- Scripts/IOption.cs
- Scripts/RayCastManager.cs

---

### Class 8: Forces & Physics Interactions
**Build On:** Class 7
**Steps:**
1. Create full Brick3D.cs script with:
   - Impact force application
   - Torque for rotation
   - Physics.OverlapSphere for area effect
   - DetachBrick method
2. Replace BrickTest with Brick3D
3. Add public variables for tuning:
   - impactRadius = 1.5f
   - impactForce = 400f
   - fallForce = 300f
4. Test clicking bricks
5. Adjust force values

**Files Created:**
- Scripts/Brick3D.cs (replaces BrickTest.cs)

---

### Class 9: Procedural Generation - Wall Generator
**Build On:** Class 8
**Steps:**
1. Create script: "WallGenerator3D.cs"
2. Add grid generation logic:
   - Nested loops for rows/columns
   - Position calculation
   - Instantiation
   - Parent organization
3. Add public variables:
   - rows = 8
   - columns = 12
   - spacing = 0.05f
4. Create empty GameObject: "WallGenerator"
5. Attach script
6. Assign brick prefab
7. Test wall generation

**Files Created:**
- Scripts/WallGenerator3D.cs

---

### Class 10: Events & Delegates
**Build On:** Class 9
**Steps:**
1. Add to Brick3D.cs:
```csharp
public static Action<Brick3D> OnAnyBrickDetached;
public static Action<int> OnScoredPoints;
```
2. Invoke events in appropriate methods
3. Create script: "WallManager.cs"
4. Subscribe to brick events
5. Track remaining bricks
6. Implement wall progression
7. Test event system

**Files Created:**
- Scripts/WallManager.cs

**Files Modified:**
- Brick3D.cs (added events)

---

### Class 11: Game Management & State
**Build On:** Class 10
**Steps:**
1. Create script: "GameManager.cs"
2. Implement singleton pattern
3. Add score tracking
4. Add timer coroutine
5. Implement StartGame/EndGame
6. Create script: "SoundManager.cs" (singleton)
7. Test game flow

**Files Created:**
- Scripts/GameManager.cs
- Scripts/SoundManager.cs

---

### Class 12: UI System with TextMeshPro
**Build On:** Class 11
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
5. Add TMP_Text elements
6. Add Buttons
7. Create script: "UIManager.cs"
8. Wire up button events
9. Test panel switching

**Files Created:**
- Scripts/UIManager.cs
- UI prefabs and scene objects

---

### Class 13: Audio System
**Build On:** Class 12
**Steps:**
1. Import audio files to Assets/Audio
2. Update SoundManager.cs:
   - Add AudioSource component
   - Add audio clip references
   - Implement PlayBGM, PlaySFX methods
3. Call sounds from events:
   - Brick hit
   - Ground collision
   - Button clicks
   - Music transitions
4. Test all audio

**Files Modified:**
- SoundManager.cs (full implementation)

---

### Class 14: Visual Effects & Polish
**Build On:** Class 13
**Steps:**
1. Import Cartoon FX package
2. Add effect prefab references to Brick3D
3. Instantiate effects on:
   - Brick hit
   - Ground collision
   - Last brick destroyed
4. Add crack texture system
5. Test all effects

**Files Modified:**
- Brick3D.cs (added effects)

---

### Class 15: Advanced Features & Optimization
**Build On:** Class 14
**Steps:**
1. Import DOTween package
2. Add camera movement with DOTween
3. Implement brick spawn animation
4. Add wall patterns (enum + switch)
5. Add weather effects
6. Optimize performance
7. Build the game!

**Files Modified:**
- WallManager.cs (DOTween camera)
- WallGenerator3D.cs (animations, patterns)

---

## File Structure by Class

### Class 1
```
Assets/
└── Scenes/
    └── Gameplay.unity
```

### Class 2
```
Assets/
├── Materials/
│   ├── Ground.mat
│   └── Brick.mat
└── Scenes/
    └── Gameplay.unity
```

### Class 3
```
Assets/
├── Materials/
├── Prefabs/
│   └── Brick_Prefab.prefab
└── Scenes/
```

### Class 4
```
Assets/
├── Materials/
├── Prefabs/
├── Scripts/
│   └── BrickTest.cs
└── Scenes/
```

### Class 5-15
(Progressive additions to Scripts folder and other assets)

## Teaching Tips

1. **Live Coding:** Type code with students, explain as you go
2. **Frequent Testing:** Test after each major change
3. **Error Handling:** Show how to read and fix errors
4. **Experimentation:** Encourage changing values
5. **Questions:** Pause for questions regularly
6. **Recap:** Start each class with previous class recap
7. **Homework:** Assign practice exercises

## Assessment Ideas

- **Class 5:** Create object that falls when clicked
- **Class 8:** Implement explosion effect
- **Class 9:** Generate different grid patterns
- **Class 11:** Add pause functionality
- **Class 12:** Create custom UI panel
- **Class 15:** Add new feature to game

## Common Student Issues

1. **Forgetting to save:** Remind frequently
2. **Wrong component:** Check Inspector carefully
3. **Typos in code:** Case-sensitive!
4. **Missing references:** Assign in Inspector
5. **Not testing:** Test after each change
6. **Giving up:** Encourage persistence

---

This strategy provides a clear path from empty project to complete game while using this repository as a reference and troubleshooting resource.

