using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class WallGenerator3D : MonoBehaviour
{
    public Brick3D brickPrefab;
    public Transform wall;

    [Header("Wall Colors")]
    [SerializeField] private Color[] wallColors;
    [SerializeField] private float brickYOffset = 0.5f;

    [Space(10)]
    [Header("Weather Effects")]
    [SerializeField] private GameObject rainEffect;
    [SerializeField] private GameObject snowEffect;

    [Space(10)]
    [SerializeField] private float baseHeight = 0.4f;
    [SerializeField] private Transform bricksParent;

    private int rows = 8;
    private int columns = 12;
    private int totalBricksCreated = 0;
    private int wallIndex;

    private float spacing = 0.05f;
    private float verticalBrickChance = 0.2f;
    private float brickMass = 1f;  // Default mass for bricks

    public bool animateBricks = true; // Toggle for brick animation

    public Transform levelEndEffectsParent;

    private bool alternateRows = true;
    private bool useVerticalBricks = false;
    private bool useAdjacentVerticalBricks = false;
    private bool[,] occupiedSpaces;

    private WallPattern currentPattern;

    public static Action OnWallSetUp;

    private float animationDelay = 0.1f; // Delay between brick animations
    private float currentBrickDelay = 0f; // Current cumulative delay

    public void SetBrickMass(float mass)
    {
        this.brickMass = mass;
    }

    public void SetWallPattern(bool useVertical, bool useAdjacent, float chance)
    {
        useVerticalBricks = useVertical;
        useAdjacentVerticalBricks = useAdjacent;
        verticalBrickChance = chance;
    }

    public void SetWallAttributes(int rows, int columns, float spacing, bool alternateRows)
    {
        this.rows = rows;
        this.columns = columns;
        this.spacing = spacing;
        this.alternateRows = alternateRows;
    }

    public void Initialize(int index, WallPattern pattern)
    {
        wallIndex = index;
        currentPattern = pattern;

        if(levelEndEffectsParent != null && wallIndex != 0)
            foreach(Transform g in levelEndEffectsParent)
                g.gameObject.SetActive(true);

        GenerateWall();
        WeatherPhenomena();
    }

    void GenerateWall()
    {
        totalBricksCreated = 0;
        currentBrickDelay = 0f; // Reset delay counter when generating a new wall
        Vector3 brickSize = brickPrefab.transform.localScale;
        float totalWidth = columns * (brickSize.x + spacing); //1 * (1+0) = 1
        Vector3 startPos = transform.position - new Vector3((totalWidth + 1) / 2, 0, 0);
        startPos.x += 1; // Offset to center the wall in the scene

        // Initialize occupied spaces array
        occupiedSpaces = new bool[columns, rows];

        if (useVerticalBricks)
        {
            GenerateWallWithVerticals(startPos, brickSize);
        }
        else
        {
            GenerateWallHorizontalOnly(startPos, brickSize);
        }

        StartCoroutine(WaitForWallSetup());
    }

    void GenerateWallHorizontalOnly(Vector3 startPos, Vector3 brickSize)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (ShouldPlaceBrick(x, y, currentPattern) && !IsSpaceOccupied(x, y))
                {
                    Vector3 position = GetBrickPosition(x, y, startPos, brickSize, false);
                    Brick3D brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    SetBreakSpecs(brick, position, false);
                    
                    // Mark space as occupied
                    occupiedSpaces[x, y] = true;
                    
                    totalBricksCreated++;
                }
            }
        }
    }

    void GenerateWallWithVerticals(Vector3 startPos, Vector3 brickSize)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (ShouldPlaceBrick(x, y, currentPattern) && !IsSpaceOccupied(x, y))
                {
                    bool isVertical = ShouldBeVertical(x, y);
                    
                    // With alternating rows, we need to adjust vertical brick positions
                    // to prevent overlap, but not disable them entirely
                    
                    // Check if vertical brick can be placed (needs space above)
                    if (isVertical && (y >= rows - 1 || IsSpaceOccupied(x, y + 1)))
                    {
                        isVertical = false;
                    }

                    Vector3 position = GetBrickPosition(x, y, startPos, brickSize, isVertical);
                    
                    if (isVertical)
                    {
                        // When using alternating rows with vertical bricks
                        if (alternateRows)
                        {
                            // Adjust position to avoid overlaps in alternating rows
                            if (y % 2 == 0 && (y + 1) < rows) 
                            {
                                // Even row extending into odd row - shift position
                                position.x += (brickSize.x + spacing) / 4;
                            }
                            else if (y % 2 == 1 && (y + 1) < rows)
                            {
                                // Odd row extending into even row - shift position
                                position.x -= (brickSize.x + spacing) / 4;
                            }
                        }
                        
                        CreateVerticalBricks(position, isVertical);
                    }
                    else
                    {
                        CreateBrick(position, Quaternion.identity, isVertical);
                        totalBricksCreated++;
                    }
                    
                    // Mark spaces as occupied
                    MarkSpaceAsOccupied(x, y, isVertical);
                }
            }
        }
    }

    private void CreateVerticalBricks(Vector3 basePosition, bool isVertical)
    {
        Quaternion verticalRotation = Quaternion.Euler(0, 0, 90);

        if (useAdjacentVerticalBricks)
        {
            // Create pair of vertical bricks
            Vector3 rightPosition = new Vector3(basePosition.x + brickYOffset / 2, basePosition.y, basePosition.z);
            Vector3 leftPosition = new Vector3(basePosition.x - brickYOffset / 2, basePosition.y, basePosition.z);
            
            CreateBrick(rightPosition, verticalRotation, isVertical);
            CreateBrick(leftPosition, verticalRotation, isVertical);
            
            totalBricksCreated += 2;
        }
        else
        {
            // Create single vertical brick
            CreateBrick(basePosition, verticalRotation, isVertical);
            totalBricksCreated++;
        }
    }

    private Brick3D CreateBrick(Vector3 position, Quaternion rotation, bool isVertical)
    {
        if (animateBricks)
        {
            // Start position for the animation (below the screen/field)
            Vector3 startPosition = Camera.main.transform.position;
            
            // Create the brick at the starting position
            Brick3D brick = Instantiate(brickPrefab, startPosition, rotation);
            SetBreakSpecs(brick, position, isVertical);
            
            // Animate the brick to its final position with a delay
            // Each brick gets a progressively longer delay
            StartCoroutine(AnimateBrickToPosition(brick, position, currentBrickDelay));
            
            // Increment the delay for the next brick
            currentBrickDelay += animationDelay;
            
            return brick;
        }
        else
        {
            // Create the brick directly at its final position - no animation
            Brick3D brick = Instantiate(brickPrefab, position, rotation);
            SetBreakSpecs(brick, position, isVertical);
            return brick;
        }
    }
    
    private IEnumerator AnimateBrickToPosition(Brick3D brick, Vector3 targetPosition, float delay)
    {
        // Wait for the specified delay before starting animation
        yield return new WaitForSeconds(delay);
        
        float duration = UnityEngine.Random.Range(0.6f, 0.9f); // Tighter duration range for more consistency
        float height = UnityEngine.Random.Range(3.0f, 5.0f);   // Higher arc for more cartoony feel
        
        Vector3 startPosition = brick.transform.position;
        Vector3 originalScale = brick.transform.localScale;
        float elapsed = 0f;
        
        // Make the brick kinematic during animation to prevent physics
        Rigidbody rb = brick.GetComponent<Rigidbody>();
        bool wasKinematic = rb.isKinematic;
        rb.isKinematic = true;
        
        // Pre-animation "anticipation" - slight scale down
        float anticipationTime = 0.1f;
        float anticipationScale = 0.7f;
        
        brick.transform.localScale = originalScale * anticipationScale;
        yield return new WaitForSeconds(anticipationTime);
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Calculate position along the path
            float parabola = height * 4 * t * (1 - t); // Parabola peaks at t=0.5
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
            currentPos.y += parabola; // Add the arc height
            
            // Apply position
            brick.transform.position = currentPos;
            
            // Squash and stretch effect based on trajectory
            if (t < 0.5f) // Rising - stretch vertically
            {
                float stretchFactor = 1.0f + (t * 0.3f); // Max 30% taller
                brick.transform.localScale = new Vector3(
                    originalScale.x * (1 / Mathf.Sqrt(stretchFactor)),
                    originalScale.y * stretchFactor,
                    originalScale.z
                );
            }
            else // Falling - stretch horizontally, then squash on impact
            {
                float fallT = (t - 0.5f) * 2; // 0 to 1 during fall
                if (fallT < 0.8f) // During most of the fall
                {
                    float stretchFactor = 1.0f + (fallT * 0.2f); // Max 20% wider
                    brick.transform.localScale = new Vector3(
                        originalScale.x * stretchFactor,
                        originalScale.y * (1 / Mathf.Sqrt(stretchFactor)),
                        originalScale.z
                    );
                }
                else // Near landing - prepare for squash
                {
                    float landingT = (fallT - 0.8f) * 5; // 0 to 1 during landing
                    float squashFactor = 1.0f + landingT * 0.4f; // Max 40% wider
                    brick.transform.localScale = new Vector3(
                        originalScale.x * squashFactor,
                        originalScale.y * (1 / squashFactor),
                        originalScale.z
                    );
                }
            }
            
            yield return null;
        }
        
        // Final landing squash
        brick.transform.position = targetPosition;
        brick.transform.localScale = new Vector3(
            originalScale.x * 1.4f, // 40% wider
            originalScale.y * 0.7f, // 30% shorter
            originalScale.z
        );
        
        yield return new WaitForSeconds(0.05f);
        
        // Small bounce
        for (float bounceTime = 0; bounceTime < 0.2f; bounceTime += Time.deltaTime)
        {
            float t = bounceTime / 0.2f;
            float bounceHeight = 0.3f * (1 - t); // Small bounce height that decreases
            brick.transform.position = targetPosition + new Vector3(0, bounceHeight, 0);
            
            // Return to original scale during bounce
            brick.transform.localScale = Vector3.Lerp(
                new Vector3(originalScale.x * 1.4f, originalScale.y * 0.7f, originalScale.z),
                originalScale,
                t
            );
            
            yield return null;
        }
        
        // Ensure final position and scale are exact
        brick.transform.position = targetPosition;
        brick.transform.localScale = originalScale;
        
        // Restore original kinematic state
        rb.isKinematic = wasKinematic;
    }

    private bool IsSpaceOccupied(int x, int y)
    {
        return occupiedSpaces[x, y];
    }

    private void MarkSpaceAsOccupied(int x, int y, bool isVertical)
    {
        occupiedSpaces[x, y] = true;
        if (isVertical && y + 1 < rows)
        {
            occupiedSpaces[x, y + 1] = true;
        }
    }

    private bool ShouldBeVertical(int x, int y)
    {
        if (y == rows - 1) return false;
        return UnityEngine.Random.value < verticalBrickChance;
    }

    private void SetBreakSpecs(Brick3D brick, Vector3 position, bool isVertical)
    {
        brick.GetComponent<Rigidbody>().mass = brickMass;
        brick.transform.parent = bricksParent;
        brick.transform.SetLocalPositionAndRotation(new(position.x, position.y, 0), isVertical ? Quaternion.Euler(0, 0, 90) : Quaternion.identity);
        int randCol = UnityEngine.Random.Range(0, wallColors.Length);
        brick.SetColor(wallColors[randCol]);       
    }

    IEnumerator WaitForWallSetup()
    {
        if (animateBricks)
        {
            // Calculate total animation time based on the number of bricks
            // and the delay between them, plus some buffer for the animations themselves
            float totalAnimationTime = currentBrickDelay + 1.5f; // Last brick delay + animation duration + buffer
            
            yield return new WaitForSeconds(totalAnimationTime);
        }
        else
        {
            // Just a short delay if not animating
            yield return new WaitForSeconds(0.5f);
        }
        
        OnWallSetUp?.Invoke();
    }

    public int TotalBricksCreated()
    {
        return totalBricksCreated;
    }

    Vector3 GetBrickPosition(int x, int y, Vector3 startPos, Vector3 brickSize, bool isVertical)
    {
        // When using alternateRows, we need to properly calculate the offset
        // Vertical bricks need special handling with alternateRows
        float xOffset = 0;
        if (alternateRows)
        {
            // Only offset the current row, not the row it extends into (for vertical bricks)
            if (y % 2 == 1)
            {
                xOffset = (brickSize.x + spacing) / 2;
            }
        }

        // For vertical bricks, adjust the Y position
        float yOffset = isVertical ? brickYOffset * 0.5f : 0;

        Vector3 returnPos = startPos + new Vector3(
            x * (brickSize.x + spacing) + xOffset,
            (y * brickYOffset) + baseHeight + yOffset,
            -0.3f
        );
        return returnPos;
    }

    bool ShouldPlaceBrick(int x, int y, WallPattern pattern)
    {
        switch (pattern)
        {
            case WallPattern.Normal: return true;
            case WallPattern.Diagonal: return (x + y) % 2 == 0;
            case WallPattern.Checkerboard: return (x + y) % 3 != 0;
            case WallPattern.Pyramid: return Mathf.Abs(x - columns / 2) <= y;
            case WallPattern.Random: return UnityEngine.Random.value > 0.2f;
            case WallPattern.VerticalStripes: return x % 3 != 0;
            case WallPattern.HorizontalStripes: return y % 3 != 0;
            case WallPattern.CrossPattern: return (x % 3 != 0) || (y % 3 != 0);
            case WallPattern.DiamondPattern:
                return Mathf.Abs(x - columns / 2) + Mathf.Abs(y - rows / 2) < rows / 2;
            case WallPattern.CirclePattern:
                return Vector2.Distance(new Vector2(x, y), new Vector2(columns / 2, rows / 2)) <= rows / 2;
            case WallPattern.HeartPattern:
                float heartX = (x - columns / 2f) / (columns / 2f);
                float heartY = (rows - y) / (float)rows;
                float heartEq = Mathf.Pow(heartX, 2) + Mathf.Pow(heartY - Mathf.Sqrt(Mathf.Abs(heartX)), 2);
                return heartEq < 1.0f;
            case WallPattern.ZigZag: return (y + (x / 2)) % 3 != 0;
            case WallPattern.Spiral:
                int dx = x - columns / 2;
                int dy = y - rows / 2;
                return (Mathf.Atan2(dy, dx) + Mathf.PI + (Mathf.Sqrt(dx * dx + dy * dy) / 2)) % 1.0f > 0.5f;
            case WallPattern.TrianglePattern: return y <= (rows / 2) - Mathf.Abs(x - columns / 2);
            case WallPattern.HoneyComb: return ((x + y) % 3 != 0) && ((x - y) % 3 != 0);
            case WallPattern.StairsLeft: return x >= y;
            case WallPattern.StairsRight: return x + y <= columns;
            case WallPattern.WavePattern:
                return y >= Mathf.Sin((x * Mathf.PI * 2) / columns) * (rows / 4) + (rows / 2);
            case WallPattern.CenterHole:
                return Vector2.Distance(new Vector2(x, y), new Vector2(columns / 2, rows / 2)) > rows / 3;
            case WallPattern.BorderOnly:
                return x == 0 || x == columns - 1 || y == 0 || y == rows - 1;
            default: return true;
        }
    }

    public void SetAnimateBricks(bool animate)
    {
        animateBricks = animate;
    }
    
    public void DestroyThisWall()
    {
        Destroy(this.gameObject);
    }
    public void WeatherPhenomena()
    {
        rainEffect.SetActive(false);
        snowEffect.SetActive(false);

        rainEffect.SetActive(UnityEngine.Random.Range(0, 2) == 0);
        if (!rainEffect.activeSelf)
            snowEffect.SetActive(UnityEngine.Random.Range(0, 2) == 0);
    }
}

public enum WallPattern
{
    Normal,
    Diagonal,
    Checkerboard,
    Pyramid,
    Random,
    VerticalStripes,
    HorizontalStripes,
    CrossPattern,
    DiamondPattern,
    CirclePattern,
    HeartPattern,
    ZigZag,
    Spiral,
    TrianglePattern,
    HoneyComb,
    StairsLeft,
    StairsRight,
    WavePattern,
    CenterHole,
    BorderOnly
}
