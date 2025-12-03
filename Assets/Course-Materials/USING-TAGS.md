# Using Git Tags for Course Navigation

## Overview
This document explains how to use Git tags to navigate between different stages of the WallBreak 3D course. Each class has a corresponding tag that represents the state of the project at that point.

## Available Tags

### Part 1: Foundational Unity & Core Setup
| Tag | Class | Description |
|-----|-------|-------------|
| `class-01` | Class 1 | Unity Editor Introduction & Basic Architecture |
| `class-02` | Class 2 | GameObjects, Transforms & Scene Setup |
| `class-03` | Class 3 | 3D Primitives & Materials |
| `class-04` | Class 4 | Prefabs & Project Organization |
| `class-05` | Class 5 | Introduction to C# Scripting I |
| `class-06` | Class 6 | Introduction to C# Scripting II |

### Part 2: Physics & Input Systems
| Tag | Class | Description |
|-----|-------|-------------|
| `class-07` | Class 7 | Physics System I: Rigidbody |
| `class-08` | Class 8 | Physics System II: Colliders & Collision |
| `class-09` | Class 9 | Input System & Mouse Handling |
| `class-10` | Class 10 | Raycasting & Object Detection |
| `class-11` | Class 11 | Forces & Physics Interactions |

### Part 3: Midterm Build-up
| Tag | Class | Description |
|-----|-------|-------------|
| `class-12` | Class 12 | Procedural Generation: Wall Generator |
| `class-13` | Class 13 | Events & Delegates |
| `class-14` | Class 14 | Game Management & Singleton Pattern |
| `class-15` | Class 15 | Coroutines & Timer Systems |
| `class-16` | Class 16 | MILESTONE: Midterm Project Sprint |

### Part 4: Polish & Complete Game
| Tag | Class | Description |
|-----|-------|-------------|
| `class-17` | Class 17 | UI System I: Canvas & Layout |
| `class-18` | Class 18 | UI System II: TextMeshPro & Buttons |
| `class-19` | Class 19 | Audio System |
| `class-20` | Class 20 | Visual Effects I: Particle Systems |
| `class-21` | Class 21 | Visual Effects II: Polish & Game Feel |
| `class-22` | Class 22 | Animation with DOTween |
| `class-23` | Class 23 | Advanced Features & Optimization |
| `class-24` | Class 24 | FINAL PROJECT SPRINT |

## How to Use Tags

### View All Tags
```bash
git tag
```

### Checkout a Specific Class
```bash
# Go to Class 10 state
git checkout class-10

# Go to Midterm state
git checkout class-16

# Go to Final state
git checkout class-24
```

### Return to Latest Version
```bash
git checkout main
# or
git checkout class
```

### View Tag Information
```bash
git show class-10
```

### Compare Two Classes
```bash
# See what changed between Class 10 and Class 11
git diff class-10 class-11
```

## Creating Tags (For Instructors)

### Create a Tag
```bash
# Lightweight tag
git tag class-01

# Annotated tag (recommended)
git tag -a class-01 -m "Class 1: Unity Editor Introduction"
```

### Push Tags to Remote
```bash
# Push single tag
git push origin class-01

# Push all tags
git push origin --tags
```

### Delete a Tag
```bash
# Local
git tag -d class-01

# Remote
git push origin --delete class-01
```

## Best Practices

### For Students
1. **Don't modify checked-out tags** - Create a branch if you want to experiment
2. **Use tags for reference** - Compare your work to the tagged version
3. **Return to main** - Always return to main/class branch for current work

### For Instructors
1. **Create tags at end of each class** - After demonstrating all concepts
2. **Use annotated tags** - Include description of what's covered
3. **Test tags** - Verify project works at each tag
4. **Document changes** - Note what's new in each tag

## Workflow Examples

### Student: Check Your Progress
```bash
# See what the project should look like after Class 8
git checkout class-08

# Compare to your current work
git diff class-08 HEAD

# Return to your work
git checkout class
```

### Student: Start Fresh from a Point
```bash
# Create branch from Class 10
git checkout class-10
git checkout -b my-class-10-attempt

# Now work from here
```

### Instructor: Create Class Tag
```bash
# After completing Class 12 demonstration
git add .
git commit -m "Class 12: Procedural Generation complete"
git tag -a class-12 -m "Procedural generation with WallGenerator3D"
git push origin class-12
```

## Troubleshooting

### "Detached HEAD" Warning
This is normal when checking out a tag. You're viewing a snapshot, not a branch.

To work from this point:
```bash
git checkout -b my-branch
```

### Can't Find Tag
```bash
# Fetch tags from remote
git fetch --tags
```

### Tag Already Exists
```bash
# Delete and recreate
git tag -d class-01
git tag -a class-01 -m "Updated description"
```

## Tag Naming Convention

Format: `class-XX`
- `class-01` through `class-24`
- Two digits for proper sorting
- Lowercase for consistency

Alternative tags (optional):
- `midterm` - Points to class-16
- `final` - Points to class-24
- `part-1-complete` - Points to class-06
- `part-2-complete` - Points to class-11

## Summary

Tags provide snapshots of the project at each class milestone. Use them to:
- Review what was covered in each class
- Compare your progress to expected state
- Start fresh from any point
- Navigate the course non-linearly

Remember: Tags are read-only snapshots. Create a branch if you want to modify!

---

## Quick Reference

```bash
# List tags
git tag

# Checkout tag
git checkout class-XX

# Return to main
git checkout main

# Create tag
git tag -a class-XX -m "Description"

# Push tag
git push origin class-XX

# Compare tags
git diff class-10 class-11
```

