using UnityEngine;
using System.Collections.Generic;
using Lemmings.Util;

namespace Lemmings.Managers {
    /// <summary>
    /// Holds JSON data about the spawn point of lemmings.
    /// </summary>
    struct LemmingsInput {
        /// <summary> The starting position of the lemmings. </summary>
        internal Vector3 position;
        /// <summary> The starting rotation of the lemmings. </summary>
        internal Vector3 rotation;
        /// <summary> The number of lemmings to spawn.</summary>
        internal int amount;

        /// <summary>
        /// Initializes a lemming spawn point.
        /// </summary>
        /// <param name="json">JSON data for the lemming spawn point.</param>
        internal LemmingsInput(JSONObject json) {
            position = JSONUtil.MakeVectorFromJSON(json.GetField("position"));
            rotation = JSONUtil.MakeVectorFromJSON(json.GetField("rotation"));
            amount = (int)json.GetField("amount").i;
        }
    }

    /// <summary>
    /// Holds JSON data about the goal.
    /// </summary>
    struct GoalInput {
        /// <summary> The position of the goal. </summary>
        internal Vector3 position;

        /// <summary>
        /// Initializes a goal.
        /// </summary>
        /// <param name="json">JSON data for the goal./param>
        internal GoalInput(JSONObject json) {
            position = JSONUtil.MakeVectorFromJSON(json);
        }
    }

    /// <summary>
    /// Holds JSON data about surfaces.
    /// </summary>
    struct SurfaceInput {
        /// <summary> The platforms that make up the surface. </summary>
        internal List<PlatformInput> platforms;

        /// <summary>
        /// Initializes a surface.
        /// </summary>
        /// <param name="json">Json.</param>
        internal SurfaceInput(JSONObject json) {
            List<JSONObject> jsonList = json.list;
            platforms = new List<PlatformInput>(jsonList.Count);
            foreach (JSONObject platform in jsonList) {
                platforms.Add(new PlatformInput(platform));
            }
        }
    }

    /// <summary>
    /// Holds JSON data about platforms.
    /// </summary>
    struct PlatformInput {
        /// <summary> The positions of the corners of the platforms. </summary>
        internal List<Vector3> vertices;
        /// <summary> A custom height for the platform. </summary>
        internal float height;

        /// <summary>
        /// Initializes a platform.
        /// </summary>
        /// <param name="vertices">The positions of the corners of the platform.</param>
        internal PlatformInput(List<Vector3> vertices) {
            this.vertices = vertices;
            height = 0;
        }

        /// <summary>
        /// Initializes a platform.
        /// </summary>
        /// <param name="json">JSON data for the platform.</param>
        internal PlatformInput(JSONObject json) {
            List<JSONObject> jsonList = json.GetField("points").list;
            vertices = new List<Vector3>(jsonList.Count);
            foreach (JSONObject vertexJSON in jsonList) {
                vertices.Add(JSONUtil.MakeVectorFromJSON(vertexJSON));
            }
            if (json.HasField("height")) {
                height = json.GetField("height").f;
            } else {
                height = 0;
            }
        }
    }

    /// <summary>
    /// Holds JSON data about a wall.
    /// </summary>
    struct WallInput {
        /// <summary> The endpoints of the wall. </summary>
        internal Vector3[] endpoints;

        /// <summary>
        /// Initializes a wall.
        /// </summary>
        /// <param name="json">JSON data for the wall.</param>
        internal WallInput(JSONObject json) {
            endpoints = new Vector3[2];
            List<JSONObject> endpointList = json.list;
            for (int i = 0; i < endpoints.Length; i++) {
                endpoints[i] = JSONUtil.MakeVectorFromJSON(endpointList[i]);
            }
        }
    }
}

