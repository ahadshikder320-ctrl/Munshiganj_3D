using System;

namespace Munshiganj.Core
{
    /// <summary>Raw mesh buffers produced by <see cref="PlaneMeshBuilder"/>.</summary>
    public class PlaneMeshData
    {
        public float[] Positions;
        public float[] Normals;
        public float[] Uvs;
        public int[] Indices;

        public int VertexCount
        {
            get { return Positions.Length / 3; }
        }

        public int TriangleCount
        {
            get { return Indices.Length / 3; }
        }
    }

    /// <summary>
    /// Builds the "new PlaneMesh" used as the ground of the Munshiganj_3D scene.
    /// The mesh is a subdivided grid: width runs along X, depth runs along Z and the
    /// surface normal points up (+Y).
    /// </summary>
    public static class PlaneMeshBuilder
    {
        /// <summary>Number of quads along the X axis.</summary>
        public const int MinSegments = 1;

        /// <summary>
        /// Builds the plane buffers.
        /// </summary>
        /// <param name="width">Size along the X axis (must be greater than zero).</param>
        /// <param name="depth">Size along the Z axis (must be greater than zero).</param>
        /// <param name="widthSegments">Quads along X (at least 1).</param>
        /// <param name="depthSegments">Quads along Z (at least 1).</param>
        public static PlaneMeshData Build(float width, float depth, int widthSegments, int depthSegments)
        {
            if (width <= 0f)
            {
                throw new ArgumentOutOfRangeException("width", "Plane width must be greater than zero.");
            }

            if (depth <= 0f)
            {
                throw new ArgumentOutOfRangeException("depth", "Plane depth must be greater than zero.");
            }

            if (widthSegments < MinSegments)
            {
                throw new ArgumentOutOfRangeException("widthSegments", "A plane needs at least one segment.");
            }

            if (depthSegments < MinSegments)
            {
                throw new ArgumentOutOfRangeException("depthSegments", "A plane needs at least one segment.");
            }

            int columns = widthSegments + 1;
            int rows = depthSegments + 1;
            int vertexCount = columns * rows;

            PlaneMeshData data = new PlaneMeshData();
            data.Positions = new float[vertexCount * 3];
            data.Normals = new float[vertexCount * 3];
            data.Uvs = new float[vertexCount * 2];
            data.Indices = new int[widthSegments * depthSegments * 6];

            float halfWidth = width * 0.5f;
            float halfDepth = depth * 0.5f;

            for (int row = 0; row < rows; row++)
            {
                float v = (float)row / depthSegments;
                float z = -halfDepth + (depth * v);

                for (int column = 0; column < columns; column++)
                {
                    float u = (float)column / widthSegments;
                    float x = -halfWidth + (width * u);

                    int vertex = row * columns + column;

                    data.Positions[vertex * 3 + 0] = x;
                    data.Positions[vertex * 3 + 1] = 0f;
                    data.Positions[vertex * 3 + 2] = z;

                    data.Normals[vertex * 3 + 0] = 0f;
                    data.Normals[vertex * 3 + 1] = 1f;
                    data.Normals[vertex * 3 + 2] = 0f;

                    data.Uvs[vertex * 2 + 0] = u;
                    data.Uvs[vertex * 2 + 1] = v;
                }
            }

            int index = 0;
            for (int row = 0; row < depthSegments; row++)
            {
                for (int column = 0; column < widthSegments; column++)
                {
                    int bottomLeft = (row * columns) + column;
                    int bottomRight = bottomLeft + 1;
                    int topLeft = ((row + 1) * columns) + column;
                    int topRight = topLeft + 1;

                    // Unity treats a triangle as front facing when its vertices are wound
                    // clockwise as seen from the camera, so the order below makes the
                    // ground face up (+Y).
                    data.Indices[index++] = bottomLeft;
                    data.Indices[index++] = bottomRight;
                    data.Indices[index++] = topLeft;

                    data.Indices[index++] = bottomRight;
                    data.Indices[index++] = topRight;
                    data.Indices[index++] = topLeft;
                }
            }

            return data;
        }

        /// <summary>
        /// Geometric normal of a triangle, computed with the right handed cross product.
        /// Used by the tests to confirm the generated plane faces upwards.
        /// </summary>
        public static Float3 TriangleNormal(PlaneMeshData data, int triangleIndex)
        {
            int i0 = data.Indices[triangleIndex * 3 + 0] * 3;
            int i1 = data.Indices[triangleIndex * 3 + 1] * 3;
            int i2 = data.Indices[triangleIndex * 3 + 2] * 3;

            Float3 a = new Float3(data.Positions[i0], data.Positions[i0 + 1], data.Positions[i0 + 2]);
            Float3 b = new Float3(data.Positions[i1], data.Positions[i1 + 1], data.Positions[i1 + 2]);
            Float3 c = new Float3(data.Positions[i2], data.Positions[i2 + 1], data.Positions[i2 + 2]);

            Float3 ab = b - a;
            Float3 ac = c - a;

            return new Float3(
                (ab.y * ac.z) - (ab.z * ac.y),
                (ab.z * ac.x) - (ab.x * ac.z),
                (ab.x * ac.y) - (ab.y * ac.x));
        }
    }
}
