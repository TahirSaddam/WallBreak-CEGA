# Using Git Tags for Course Navigation

## Overview
This repository uses git tags to mark reference points for each of the 15 classes. While the complete project exists in one state, these tags serve as conceptual milestones for what should be completed at each stage.

## Available Tags

```
class-01  →  Unity Basics & Project Setup
class-02  →  3D Primitives & Materials
class-03  →  Prefabs & Object Instantiation
class-04  →  Introduction to C# Scripting
class-05  →  Physics System - Rigidbody
class-06  →  Colliders & Collision Detection
class-07  →  Input System & Raycasting
class-08  →  Forces & Physics Interactions
class-09  →  Procedural Generation
class-10  →  Events & Delegates
class-11  →  Game Management & State
class-12  →  UI System with TextMeshPro
class-13  →  Audio System
class-14  →  Visual Effects & Polish
class-15  →  Advanced Features & Optimization
```

## Viewing Tags

### List All Tags
```bash
git tag
```

### View Tag Details
```bash
git show class-01
```

### View Tag Message
```bash
git tag -n class-01
```

### List All Tags with Messages
```bash
git tag -n
```

## Using Tags as Reference Points

### Recommended Workflow for Instructors

1. **Before Each Class:**
   ```bash
   # Review what should be completed by this class
   git show class-05
   ```

2. **During Class:**
   - Use presentation materials from `Course-Materials/`
   - Follow build steps from `COMMIT-STRATEGY.md`
   - Reference complete code in project

3. **After Class:**
   - Students should have functionality matching the tag description
   - Use complete project for troubleshooting

### Recommended Workflow for Students

1. **Start New Project:**
   - Create fresh Unity 3D URP project
   - Build alongside instructor

2. **Reference Complete Project:**
   - When stuck, check corresponding files
   - Compare your code to complete version
   - Understand differences

3. **Check Progress:**
   - At end of each class, verify you have features described in tag
   - Example: After Class 5, bricks should fall with physics

## Tag Descriptions

### class-01: Unity Basics & Project Setup
**What Should Exist:**
- Empty Unity 3D URP project
- Scene with Main Camera
- Directional Light
- Basic understanding of Unity interface

### class-02: 3D Primitives & Materials
**What Should Exist:**
- Ground plane with material
- Brick material created
- Understanding of URP materials
- Tagged ground object

### class-03: Prefabs & Object Instantiation
**What Should Exist:**
- Brick prefab with collider
- Organized folder structure
- Understanding of prefab workflow

### class-04: Introduction to C# Scripting
**What Should Exist:**
- First script attached to brick
- Debug.Log messages working
- Understanding of MonoBehaviour lifecycle

### class-05: Physics System - Rigidbody
**What Should Exist:**
- Brick with Rigidbody component
- Can toggle kinematic mode
- Brick falls with gravity
- Understanding of physics simulation

### class-06: Colliders & Collision Detection
**What Should Exist:**
- Collision detection working
- OnCollisionEnter implemented
- Physics material applied
- Ground collision detected

### class-07: Input System & Raycasting
**What Should Exist:**
- Input System package installed
- RayCastManager script
- IOption interface
- Can click on bricks
- Raycast hit detection working

### class-08: Forces & Physics Interactions
**What Should Exist:**
- Brick3D script with full impact system
- Forces applied on click
- Torque for rotation
- Area-of-effect destruction
- Multiple bricks affected

### class-09: Procedural Generation
**What Should Exist:**
- WallGenerator3D script
- Walls generated in grids
- Position calculation working
- Multiple bricks instantiated
- Parent-child organization

### class-10: Events & Delegates
**What Should Exist:**
- Event system implemented
- OnAnyBrickDetached event
- OnScoredPoints event
- WallManager subscribing to events
- Brick count tracking

### class-11: Game Management & State
**What Should Exist:**
- GameManager with singleton
- Score system working
- Timer countdown
- Coroutines for timing
- Game flow (start/end)
- SoundManager singleton

### class-12: UI System with TextMeshPro
**What Should Exist:**
- Canvas with proper scaling
- All UI panels (Menu, Gameplay, End, Pause)
- TextMeshPro text elements
- Buttons with events
- UIManager script
- Panel switching working

### class-13: Audio System
**What Should Exist:**
- Audio files imported
- SoundManager fully implemented
- Background music playing
- Sound effects on events
- Audio transitions working

### class-14: Visual Effects & Polish
**What Should Exist:**
- Particle effects package imported
- Impact effects on brick hit
- Ground collision effects
- Crack texture system
- Celebration effects
- All effects instantiating correctly

### class-15: Advanced Features & Optimization
**What Should Exist:**
- DOTween package installed
- Camera movement with tweening
- Brick spawn animations
- Squash and stretch effects
- Wall patterns implemented
- Weather effects (rain/snow)
- Complete, polished game
- Performance optimized

## Checking Your Progress

After each class, ask yourself:

1. **Does my project have the features described in the tag?**
2. **Can I demonstrate the functionality?**
3. **Do I understand how it works?**
4. **Can I explain it to someone else?**

If you answer "no" to any of these:
- Review the presentation materials
- Check COMMIT-STRATEGY.md for steps
- Compare your code to the complete project
- Ask your instructor for help

## Troubleshooting with Tags

### If You're Behind
1. Identify which class you're on
2. Check the tag description for that class
3. Review what should exist
4. Focus on completing those features first
5. Don't skip ahead until current class is solid

### If You're Ahead
1. Great! You're doing well
2. Experiment with extensions
3. Help classmates who are behind
4. Try the homework challenges
5. Start thinking about your own game ideas

### If Something Doesn't Work
1. Check which class introduced that feature
2. Review that class's presentation
3. Compare your code to complete project
4. Check COMMIT-STRATEGY.md for detailed steps
5. Verify all prerequisites from previous classes

## Best Practices

### Do:
- ✅ Use tags as checkpoints
- ✅ Verify functionality at each stage
- ✅ Reference complete project when stuck
- ✅ Build your own project alongside
- ✅ Test frequently

### Don't:
- ❌ Skip classes/tags
- ❌ Copy code without understanding
- ❌ Move ahead if current class isn't working
- ❌ Ignore errors
- ❌ Forget to save your work

## For Self-Learners

If you're learning independently:

1. **Start at class-01**
2. **Read the presentation** in Course-Materials/
3. **Follow COMMIT-STRATEGY.md** for that class
4. **Build the features** described in the tag
5. **Test thoroughly**
6. **Move to next class** only when current is complete

Estimated time per class: 2-3 hours

## Quick Reference Commands

```bash
# View all tags
git tag

# See tag details
git show class-05

# See all tag messages
git tag -n

# Check current branch
git branch

# View commit history
git log --oneline

# See what files changed
git status
```

## Additional Resources

- **Presentations:** `Course-Materials/Class-XX-Presentation.txt`
- **Build Guide:** `COMMIT-STRATEGY.md`
- **Course Overview:** `COURSE-GUIDE.md`
- **Main README:** `README.md`

## Summary

Tags in this repository serve as:
- ✅ Progress checkpoints
- ✅ Feature milestones
- ✅ Reference points
- ✅ Learning goals

They help you:
- Track your progress
- Verify completeness
- Stay organized
- Learn systematically

Remember: The goal is understanding, not just completion. Take your time, experiment, and enjoy the learning process!

---

**Happy Learning! 🎓**

