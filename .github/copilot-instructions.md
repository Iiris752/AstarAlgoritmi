# Guidance for AI coding agents (project-specific)

Short summary
- Unity project that demonstrates an A* pathfinding implementation in a small 3D grid.
- Key runtime scripts: `Assets/Scripts/AstrNode.cs`, `Assets/Scripts/PathfinderScript.cs`.
- Editor helpers live in `Assets/Scripts/Editor/AStarEditor.cs` (menu items used to generate and prepare the grid).

Big picture / architecture
- Grid of nodes represented by GameObjects with tag `AStarNode` (see `AStarEditor.GenerateGrid`).
- Each node is backed by `AstrNode` (public fields visible to the Inspector). `AstrNode` stores `g/h/f`, `parent`, `neighborNodes`, and a `nodeColours` array used by `SetColour(NodeType)`.
- Workflow: editor menu creates nodes → `AStarEditor.CheckBarriers` marks nodes unwalkable with an upward raycast → `AStarEditor.SearchNeighbors` populates `neighborNodes` (distance < 1.8) → in Play mode `PathfinderScript` finds start/end nodes and runs the A* coroutine.

Important conventions and examples
- Node discovery: GameObjects are collected with `GameObject.FindGameObjectsWithTag("AStarNode")` (PathfinderScript.StartCoroutine, AStarEditor).
- Start/End: a node becomes the start or end by toggling `startpoint` / `endpoint` bools on the `AstrNode` component in the Inspector.
- Neighbor detection: neighbors are detected if distance < 1.8 (see `AStarEditor.SearchNeighbors`). This threshold is an explicit project convention.
- Heuristic: Manhattan distance on X and Z axes is used: `h = Mathf.Abs(dx) + Mathf.Abs(dz)` (see `PathfinderScript.CheckCurrentNode`).
- Movement cost: `g` is incremented by 1 per step (`checkNode.g = ThisNode.g + 1`).
- Visualization and timing: `PathfinderScript` uses `WaitForSeconds` (1s) for visual step-by-step debugging; many waits exist to make changes visible in Editor Play mode.

Developer workflows (not discoverable by static build tools)
- Run / debug: open the project with the Unity Editor, press Play, then press the `p` key to start the pathfinding coroutine (see `PathfinderScript.Update`).
- Prepare grid in the Editor: use menu `AStar -> Generate Grid`, then `AStar -> Check Barriers`, then `AStar -> Search Neighbors` to set up node neighbors and walkability before Play.
- Prefabs / resources: the grid generator loads a `Node` prefab from `Resources` (`Resources.Load<GameObject>("Node")`). Ensure the `Node` prefab exists in `Assets/Resources` and has the `AstrNode` component and proper materials assigned.

Project-specific patterns to follow
- Inspector-driven: Many variables are intentionally public for quick experimentation in the Inspector (e.g., `nodeColours`, `startpoint`, `endpoint`). Prefer changing values in the Inspector rather than hardcoding new defaults.
- Editor menu helpers: utility code that modifies scene objects lives under `Assets/Scripts/Editor`. These classes use `MenuItem` and `DestroyImmediate`; operate on Editor-only workflows.
- Tag-based node collection: always assume nodes are found via the `AStarNode` tag; avoid adding alternative collection strategies unless updating Editor flows.

Integration points & external deps
- Unity Editor and runtime APIs only. The project references `Unity.VisualScripting` in some files but core A* code uses only UnityEngine APIs.
- No test harness or CI configuration present. Building and running is done through the Unity Editor.

When editing code — quick heuristics for agents
- If changing neighbor logic, update `AStarEditor.SearchNeighbors` and ensure the `neighborNodes` lists are rebuilt in the Editor before testing.
- If changing visualization colors, update `AstrNode.nodeColours` mapping and keep indices compatible with `NodeType` enum.
- Avoid removing `WaitForSeconds` calls when first iterating — they are used intentionally for visual debugging.

Files to inspect first (order matters)
- `Assets/Scripts/AstrNode.cs` — data model for nodes (f/g/h, parent, neighbors, SetColour).
- `Assets/Scripts/PathfinderScript.cs` — core A* algorithm and coroutine-driven visualization.
- `Assets/Scripts/Editor/AStarEditor.cs` — grid generation, barrier detection, neighbor search.
- `Assets/Resources/Node` (prefab) — expected to contain `AstrNode` and `nodeColours` materials.

If anything is unclear or you need example edits (safety checks, refactors, or improved neighbor search), tell me which area to change and I will propose a focused patch.
