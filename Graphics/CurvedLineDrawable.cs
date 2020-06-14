using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics
{
    public class CurvedLineDrawable : MaskableGraphic {

        private float radius = 5f;
        private float curveAngle = 4 * Mathf.Deg2Rad;
        private bool smoothEnds = false;

        private UIVertex vertex = new UIVertex();

        /// <summary>
        /// The list of lines to be drawn on renderer.
        /// </summary>
        private List<Line> lines = new List<Line>();

        [SerializeField]
        private Texture texture;


        public Texture Texture
        {
            get => texture;
            set
            {
                if(texture == value)
                    return;
                texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        public override Texture mainTexture => texture;

        /// <summary>
        /// The radius of the curve path.
        /// </summary>
        public float CurveRadius
        {
            get => radius;
            set
            {
                if(radius == value)
                    return;
                radius = Mathf.Clamp(value, 0.1f, value);
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// The angle interval of the curve joint path in degrees.
        /// </summary>
        public float CurveAngle
        {
            get => curveAngle;
            set
            {
                value *= Mathf.Deg2Rad;
                if(curveAngle == value)
                    return;
                curveAngle = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// Whether the curve ends should be smooth.
        /// </summary>
        public bool UseSmoothEnds
        {
            get => smoothEnds;
            set
            {
                if(smoothEnds == value)
                    return;
                smoothEnds = value;
                SetVerticesDirty();
            }
        }


        /// <summary>
        /// Adds the specified line to the renderer.
        /// </summary>
        public void AddLine(Line line)
        {
            // If the specified line is positioned at the end of last line and has equal angle, simply extend it instead.
            if (lines.Count > 0)
            {
                var lastLine = lines[lines.Count - 1];
                if (line.Theta == lastLine.Theta && line.StartPoint == lastLine.EndPoint)
                {
                    lastLine.SetLength(lastLine.Length + line.Length);
                    SetVerticesDirty();
                    return;
                }
            }

            lines.Add(line);
            SetVerticesDirty();
        }

        /// <summary>
        /// Clears all lines.
        /// </summary>
        public void ClearLines()
        {
            lines.Clear();
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            if (lines.Count > 0)
            {
                Line prevLine = null;
                // Draw all the lines first.
                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    Vector2 right = line.Right * radius;
                    AddQuad(
                        vh,
                        right + line.StartPoint,
                        -right + line.StartPoint,
                        -right + line.EndPoint,
                        right + line.EndPoint
                    );

                    // Create a smooth curve between the previous line and this line.
                    if (prevLine != null)
                    {
                        DrawJoint(vh, prevLine.GetAngleDiff(line), prevLine, prevLine.EndPoint);
                    }
                    prevLine = line;
                }

                if (smoothEnds)
                {
                    Line first = lines[0];
                    Line last = lines[lines.Count - 1];
                    DrawJoint(vh, Mathf.PI, first, first.StartPoint, true);
                    DrawJoint(vh, Mathf.PI, last, last.EndPoint);
                }
            }
        }

        /// <summary>
        /// Draws the joint using smooth curve.
        /// </summary>
        private void DrawJoint(VertexHelper vh, float turnAmount, Line startLine, Vector2 joint, bool isStartPoint = false)
        {
            if (turnAmount != 0f)
            {
                float turnDir = Mathf.Sign(turnAmount);
                float angle = startLine.Theta + 90f * -turnDir * (isStartPoint ? -1f : 1f) * Mathf.Deg2Rad;
                int loopCount = Mathf.CeilToInt(Mathf.Abs(turnAmount / curveAngle)) + 1;
                Vector2 prevFill = startLine.Right * -turnDir * radius + joint;
                for (int t = 0; t < loopCount; t++)
                {
                    Vector2 newFill = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius) + joint;
                    angle += curveAngle * turnDir;
                    AddTriangle(
                        vh,
                        joint,
                        prevFill,
                        newFill
                    );
                    prevFill = newFill;
                }
            }
        }

        /// <summary>
        /// Draws a new triangle using specified values.
        /// </summary>
        private void AddTriangle(VertexHelper vh, Vector2 corner1, Vector2 corner2, Vector2 corner3)
        {
            int vertCount = vh.currentVertCount;
            vertex.color = this.color;

            vertex.position = corner1;
            vertex.uv0 = Vector2.zero;
            vh.AddVert(vertex);

            vertex.position = corner2;
            vertex.uv0 = new Vector2(1f, 0f);
            vh.AddVert(vertex);

            vertex.position = corner3;
            vertex.uv0 = Vector2.one;
            vh.AddVert(vertex);

            vh.AddTriangle(vertCount, vertCount + 2, vertCount +1);

        }

        /// <summary>
        /// Draws a new quad using specified values.
        /// </summary>
        private void AddQuad(
            VertexHelper vh,
            Vector2 corner1, Vector2 corner2, Vector2 corner3, Vector2 corner4
        )
        {
            int vertCount = vh.currentVertCount;
            vertex.color = this.color;

            vertex.position = corner1;
            vertex.uv0 = Vector2.zero;
            vh.AddVert(vertex);

            vertex.position = corner2;
            vertex.uv0 = new Vector2(1f, 0f);
            vh.AddVert(vertex);

            vertex.position = corner3;
            vertex.uv0 = Vector2.one;
            vh.AddVert(vertex);

            vertex.position = corner4;
            vertex.uv0 = new Vector2(0f, 1f);
            vh.AddVert(vertex);

            vh.AddTriangle(vertCount, vertCount + 2, vertCount + 1);
            vh.AddTriangle(vertCount + 3, vertCount + 2, vertCount);
        }
    }
}