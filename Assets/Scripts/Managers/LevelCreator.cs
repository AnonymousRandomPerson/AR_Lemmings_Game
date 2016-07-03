using UnityEngine;
using System.Collections.Generic;
using System.Net;
using Lemmings.Entities;
using Lemmings.Level;
using Lemmings.Util;

namespace Lemmings.Managers {
    /// <summary>
    /// Creates a ribbon path from input.
    /// </summary>
    class LevelCreator : MonoBehaviour {
        
        /// <summary> Goal resource to be instantiated from. </summary>
        [SerializeField]
        [Tooltip("Goal resource to be instantiated from.")]
        private Goal goalPrefab;
        /// <summary> Lemming spawner resource to be instantiated from. </summary>
        [SerializeField]
        [Tooltip("Lemming spawner resource to be instantiated from.")]
        private LemmingSpawner lemmingSpawnerPrefab;

        /// <summary> Texture for surfaces. </summary>
        [SerializeField]
        [Tooltip("Texture for surfaces.")]
        private Material surfaceMaterial;
        /// <summary> Texture for walls. </summary>
        [SerializeField]
        [Tooltip("Texture for walls.")]
        private Material wallMaterial;

        /// <summary> JSON file to load the level from. </summary>
        [SerializeField]
        [Tooltip("JSON file to load the level from.")]
        private TextAsset json;

        /// <summary> The height of surface platforms. </summary>
        [SerializeField]
        [Tooltip("The height of surface platforms.")]
        private float surfaceHeight;

        /// <summary> The height of walls. </summary>
        [SerializeField]
        [Tooltip("The height of walls.")]
        private float wallHeight;

        /// <summary> The thickness of walls. </summary>
        [SerializeField]
        [Tooltip("The thickness of walls.")]
        private float wallThickness;

        /// <summary>
        /// Creates the level from either the level AI server or a local JSON.
        /// </summary>
        private void Start() {
            if (json == null) {
                // Connect to the server to get a JSON file.
                NetworkingManager.instance.ProcessStringFromURL((jsonText) =>
                    {
                        CreateLevel(jsonText);
                    });
            } else {
                // Hard-coded JSON resource for testing.
                CreateLevel(json.text);
            }
        }

        /// <summary>
        /// Creates the level.
        /// </summary>
        /// <param name="jsonText">The JSON text to create the level with.</param>
        private void CreateLevel(string jsonText) {
            JSONObject input = new JSONObject(jsonText);

            JSONObject lemmingsJSON = input.GetField("lemmings");
            LemmingsInput lemmingsInput = new LemmingsInput(lemmingsJSON);

            JSONObject goalJSON = input.GetField("goal");
            GoalInput goalInput = new GoalInput(goalJSON);

            JSONObject surfaceJSONList = input.GetField("surfaces");
            List<SurfaceInput> surfaceInput = new List<SurfaceInput>(surfaceJSONList.Count);
            foreach (JSONObject surfaceJSON in surfaceJSONList.list) {
                surfaceInput.Add(new SurfaceInput(surfaceJSON));
            }
            JSONObject floorJSONList = input.GetField("floor");
            foreach (JSONObject floorJSON in floorJSONList.list) {
                surfaceInput.Add(new SurfaceInput(floorJSON, true));
            }

            JSONObject wallJSONList = input.GetField("walls");
            List<WallInput> wallInput = new List<WallInput>(wallJSONList.Count);
            foreach (JSONObject wallJSON in wallJSONList.list) {
                wallInput.Add(new WallInput(wallJSON));
            }

            CreateLemmings(lemmingsInput);
            CreateGoal(goalInput);
            CreateSurfaces(surfaceInput);
            CreateWalls(wallInput);

            LevelLogger.instance.json = jsonText;
        }

        /// <summary>
        /// Creates the lemmings in the level.
        /// </summary>
        /// <param name="lemmingsInput">JSON data for the starting points of the lemmings.</param>
        private void CreateLemmings(LemmingsInput lemmingsInput) {
            LemmingSpawner spawner = ObjectUtil.Instantiate(lemmingSpawnerPrefab, lemmingsInput.position, lemmingsInput.rotation) as LemmingSpawner;
            spawner.totalLemmings = lemmingsInput.amount;
            GameManager.instance.lemmingSpawner = spawner;
        }

        /// <summary>
        /// Creates the goal in the level.
        /// </summary>
        /// <param name="lemmingsInput">JSON data for the goal.</param>
        private void CreateGoal(GoalInput goalInput) {
            ObjectUtil.Instantiate(goalPrefab, goalInput.position);
        }

        /// <summary>
        /// Creates the surface bounding boxes in the level.
        /// </summary>
        /// <param name="surfaceInput">JSON data for the surface bounding boxes.</param>
        private void CreateSurfaces(List<SurfaceInput> surfaceInput) {
            GameObject surfaceContainer = ObjectUtil.CreateNewObject("Surfaces");

            foreach (SurfaceInput surface in surfaceInput) {
                GameObject surfaceObject = ObjectUtil.CreateNewObject("Surface", surfaceContainer);
                Surface surfaceComponent = surfaceObject.AddComponent<Surface>();
                SurfaceManager.instance.AddSurface(surfaceComponent);

                foreach (PlatformInput platform in surface.platforms) {
                    GameObject platformObject = CreatePlatform(platform, platform.height);
                    platformObject.name = "Platform";
                    platformObject.GetComponent<Renderer>().material = surfaceMaterial;
                    platformObject.transform.parent = surfaceObject.transform;

                    if (surface.isFloor) {
                        platformObject.AddComponent<Lava>();
                    }
                }
                surfaceComponent.isFloor = surface.isFloor;
            }
        }

        /// <summary>
        /// Creates the walls in the level.
        /// </summary>
        /// <param name="wallInput">JSON data for the walls in the level.</param>
        private void CreateWalls(List<WallInput> wallInput) {
            GameObject wallContainer = ObjectUtil.CreateNewObject("Walls");
            foreach (WallInput wall in wallInput) {
                Vector3 direction = wall.endpoints[1] - wall.endpoints[0];
                Vector3 ortho = VectorUtil.GetOrthonormal(direction) * wallThickness / 2;
                List<Vector3> wallPoints = new List<Vector3>(4);
                Vector3 heightOffset = Vector3.up * wallHeight / 2;
                wallPoints.Add(wall.endpoints[0] + ortho + heightOffset);
                wallPoints.Add(wall.endpoints[0] - ortho + heightOffset);
                wallPoints.Add(wall.endpoints[1] - ortho + heightOffset);
                wallPoints.Add(wall.endpoints[1] + ortho + heightOffset);
                PlatformInput wallSurface = new PlatformInput(wallPoints);
                GameObject wallObject = CreatePlatform(wallSurface, wallHeight);
                wallObject.layer = LayerMask.NameToLayer("Wall");
                wallObject.name = "Wall";
                wallObject.GetComponent<Renderer>().material = wallMaterial;
                wallObject.transform.parent = wallContainer.transform;
            }
        }

        /// <summary>
        /// Creates a platform from its top vertices.
        /// </summary>
        /// <returns>A new platform from the specified top vertices.</returns>
        /// <param name="input">The top vertices of the platform.</param>
        /// <param name="height">The thickness of the platform.</param>
        private GameObject CreatePlatform(PlatformInput input, float height = 0) {
            if (height == 0) {
                height = surfaceHeight;
            }
            List<Vector3> bottom = new List<Vector3>(input.vertices.Count);
            for (int i = 0; i < input.vertices.Count; i++) {
                bottom.Add(VectorUtil.SetY(input.vertices[i], input.vertices[i].y - height));
            }
            return CreatePlatform(input, new PlatformInput(bottom));
        }

        /// <summary>
        /// Creates a platform from both top and bottom vertices.
        /// </summary>
        /// <returns>A new platform from the specified vertices.</returns>
        /// <param name="top">The top vertices of the platform.</param>
        /// <param name="bottom">The bottom vertices of the platform.</param>
        private GameObject CreatePlatform(PlatformInput top, PlatformInput bottom) {
            GameObject virtualPlatform = new GameObject();
            virtualPlatform.layer = LayerMask.NameToLayer("Platform");
            virtualPlatform.AddComponent<MeshFilter>();
            virtualPlatform.AddComponent<MeshRenderer>();
            Mesh mesh = virtualPlatform.GetComponent<MeshFilter>().mesh;

            // Create the vertices of the platform.
            Vector3[] vertices = new Vector3[top.vertices.Count * 6];

            // Used to determine clockwise/counter-clockwise.
            float edgeSum = 0;
            for (int i = 0; i < top.vertices.Count; i++) {
                vertices[i] = top.vertices[i];
                vertices[i + top.vertices.Count] = bottom.vertices[i];
                if (i < top.vertices.Count - 1) {
                    edgeSum += (top.vertices[i + 1].x - top.vertices[i].x) * (top.vertices[i + 1].z + top.vertices[i].z);
                } else {
                    edgeSum += (top.vertices[0].x - top.vertices[i].x) * (top.vertices[0].z + top.vertices[i].z);
                }
            }
            bool clockwise = edgeSum > 0;

            // Find the triangles that can make up the top and bottom faces of the platform mesh.
            Triangulator triangulator = new Triangulator(top.vertices.ToArray());
            int[] topTriangles = triangulator.Triangulate();
            int[] triangles = new int[topTriangles.Length * 2 + top.vertices.Count * 6];
            for (int i = 0; i < topTriangles.Length; i += 3) {
                triangles[i] = topTriangles[i];
                triangles[i + 1] = topTriangles[i + 1];
                triangles[i + 2] = topTriangles[i + 2];
                triangles[topTriangles.Length + i] = topTriangles[i + 2] + top.vertices.Count;
                triangles[topTriangles.Length + i + 1] = topTriangles[i + 1] + top.vertices.Count;
                triangles[topTriangles.Length + i + 2] = topTriangles[i] + top.vertices.Count;
            }

            // Find the triangles for the sides of the platform.
            for (int i = 0; i < top.vertices.Count; i++) {
                int triangleOffset = topTriangles.Length * 2 + i * 6;
                int nextIndex = i < top.vertices.Count - 1 ? i + 1 : 0;

                int vertexOffset = top.vertices.Count * 2 + i * 4;
                vertices[vertexOffset] = vertices[i];
                vertices[vertexOffset + 1] = vertices[nextIndex];
                vertices[vertexOffset + 2] = vertices[top.vertices.Count + i];
                vertices[vertexOffset + 3] = vertices[top.vertices.Count + nextIndex];

                if (!clockwise) {
                    triangles[triangleOffset] = vertexOffset;
                    triangles[triangleOffset + 1] = vertexOffset + 1;
                    triangles[triangleOffset + 2] = vertexOffset + 2;
                    triangles[triangleOffset + 3] = vertexOffset + 3;
                    triangles[triangleOffset + 4] = vertexOffset + 2;
                    triangles[triangleOffset + 5] = vertexOffset + 1;
                } else {
                    triangles[triangleOffset + 5] = vertexOffset;
                    triangles[triangleOffset + 4] = vertexOffset + 1;
                    triangles[triangleOffset + 3] = vertexOffset + 2;
                    triangles[triangleOffset + 2] = vertexOffset + 3;
                    triangles[triangleOffset + 1] = vertexOffset + 2;
                    triangles[triangleOffset] = vertexOffset + 1;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            virtualPlatform.AddComponent<MeshCollider>();
            virtualPlatform.GetComponent<MeshCollider>().sharedMesh = mesh;

            virtualPlatform.transform.parent = transform;

            return virtualPlatform;
        }
    }
}