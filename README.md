# Optimal Patrol Path Around Machines

A C# solution to find the shortest closed patrol path that encloses all axis-aligned rectangular machines on a factory floor.

## Problem Summary

Given **N** axis-aligned rectangles, find the shortest simple closed polygonal path that:
- Does not pass through the interior of any rectangle
- Encloses all rectangles entirely

## Approach

The optimal path is the **perimeter of the convex hull** of all rectangle corner points.

### Algorithm: Andrew's Monotone Chain

1. Extract all **4N corner points** from the N rectangles
2. **Sort** points lexicographically (by X, then Y)
3. Build the **lower hull** (left → right sweep)
4. Build the **upper hull** (right → left sweep)
5. Compute the **perimeter** by summing Euclidean distances along hull edges

**Time complexity:** O(N log N)  
**Space complexity:** O(N)

### Key Formulas

**Cross product** (orientation test for three points O, A, B):
```
Cross(O, A, B) = (Ax - Ox)(By - Oy) - (Ay - Oy)(Bx - Ox)

 > 0  →  left turn  (counter-clockwise) — keep
 = 0  →  collinear                      — keep
 < 0  →  right turn (clockwise)         — pop from hull
```

**Euclidean distance:**
```
dist(A, B) = sqrt((Bx - Ax)^2 + (By - Ay)^2)
```

**Perimeter:**
```
P = sum of dist(hull[i], hull[(i+1) mod n])  for i = 0..n-1
```

## Input Format

```
N
x1 y1 x2 y2
x1 y1 x2 y2
...
```

- First line: integer N (1 ≤ N ≤ 2 × 10^5)
- Next N lines: four numbers per line representing opposite corners of each rectangle
- Coordinates: real numbers or integers with absolute value up to 10^6

## Output Format

A single real number — the minimum patrol path length — with error at most 10^-6.

## Example

**Input:**
```
2
0 0 1 1
1 0 2 1
```

**Output:**
```
6.00000000
```

Two adjacent unit squares form a 2×1 rectangle; the convex hull perimeter is 6.

## Test Cases

| Input | Expected Output | Description |
|---|---|---|
| 1 rect: `0 0 1 1` | `4.0` | Single unit square |
| 2 rects: `0 0 1 1`, `1 0 2 1` | `6.0` | Two adjacent squares |
| 3 rects in a row | `8.0` | Collinear hull points |
| 1 rect: `5 5 5 5` | `0.0` | Degenerate point |
| 1 rect: `0 0 5 0` | `10.0` | Degenerate line |
| 1 rect: `-1000000 -1000000 1000000 1000000` | `8000000.0` | Max coordinates |

## File Structure

```
OptimalPatrolPath/
├── PatrolPath.cs      # Main solution
└── README.md          # This file
```

## How to Run

**Compile:**
```bash
csc PatrolPath.cs -out:PatrolPath.exe
```

**Run:**
```bash
mono PatrolPath.exe < input.txt
```

Or with .NET:
```bash
dotnet run
```

## Implementation Notes

- Uses `CultureInfo.InvariantCulture` for locale-safe parsing (`.` as decimal separator on all systems)
- Output format `G9` guarantees 9 significant digits, satisfying the 10^-6 error bound at all coordinate magnitudes
- Collinear hull points are **kept** (strict `< 0` test, not `<= 0`) so shared rectangle edges contribute their exact length
- Compatible with C# 7.3+ and .NET Framework — no modern syntax used
