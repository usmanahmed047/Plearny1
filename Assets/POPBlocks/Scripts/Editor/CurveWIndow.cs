// // ©2015 - 2021 Candy Smith
// // All rights reserved
// // Redistribution of this software is strictly not allowed.
// // Copy of this software can be obtained from unity asset store only.
// 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// // THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using POPBlocks.Scripts.Gameplay.LevelHandle;
using Syrus.Plugins.ChartEditor;
using UnityEditor;
using UnityEngine;

namespace POPBlocks.Scripts.Editor
{
    public class CurveWIndow : EditorWindow
    {
        public List<Function[]> functions = new List<Function[]>();

        [Serializable]
        public struct Function
        {
            public Color funColor;
            public Vector2[] points;
            public string xField;
            public string yField;
        }
        private AnimationCurve scoreColors;
        private AnimationCurve starsScore;
        private AnimationCurve scoreMoves;
        Material mat;

        public void PlotCurve(List<TSVParseObject> objects)
        {
            AddCurve(objects, "moves", "score");
            AddCurve(objects, "colors", "score");
        }

        private void AddCurve(List<TSVParseObject> objects, string xAxis, string yAxis)
        {
            var function = new Function[2];
            var shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            Keyframe[] keyframes;
            // if(yAxis == "count")
            //     keyframes = objects.Where(i => !i.fail).Where(i=>i.fail)
            // else
                keyframes = GetCurve(objects.Where(i => !i.fail).OrderBy(i => i.moves), xAxis, yAxis).keys;
            function[0] = new Function
            {
                funColor = Color.green,
                points = keyframes.Select(i => new Vector2(i.time, i.value)).ToArray(),
                xField = xAxis,
                yField = yAxis
            };
            function[1] = new Function
                {funColor = Color.red, points = GetCurve(objects.Where(i => i.fail).OrderBy(i => i.moves), xAxis, yAxis).keys.Select(i => new Vector2(i.time, i.value)).ToArray()};
            functions.Add(function);
        }

        void OnGUI()
        {
            foreach (var function in functions)
            {
                DrawDiagram(function);
            }
        }

        private void DrawDiagram(Function[] functions1)
        {
            if (!functions1.Any()) return;
            EditorGUILayout.LabelField(functions1[0].xField + " / " + functions1[0].yField);
            GUILayout.BeginHorizontal( EditorStyles.helpBox );
            // var firstPoint = functions1.points.First();
            // var lastPoint = functions1.points.Last();
            GUIChartEditor.BeginChart(0, 50, 100, 200, Color.black,
                GUIChartEditorOptions.ChartBounds(0, functions1[0].points.Max(i => i.x) * 1.1f, 0, functions1[0].points.Max(i => i.y)*1.1f),
                GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft),
               // GUIChartEditorOptions.ShowAxes(Color.white),
                GUIChartEditorOptions.ShowGrid(2f, 100, new Color(36f / 255f, 36 / 255f, 36 / 255f), true)
                /*GUIChartEditorOptions.ShowLabels("0.##", 100f, firstPoint.x, firstPoint.y, 1f, lastPoint.x, lastPoint.y)*/);

            // Draws lines
            
            foreach (var t in functions1)
            {
                Vector2[] points = t.points;
                Color functionColor = t.funColor;
               
                GUIChartEditor.PushLineChart( points, functionColor );
            }
            GUIChartEditor.EndChart();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private AnimationCurve GetCurve(IOrderedEnumerable<TSVParseObject> orderedList, string xAxis, string yAxis)
        {
            var curve = new AnimationCurve();
            foreach (var tsvParseObject in orderedList)
            {
                curve.AddKey(GetValue(tsvParseObject, xAxis), GetValue(tsvParseObject, yAxis));
            }
            return curve;
        }

        int GetValue(object myobject, string propertyname)
        {
            var propertyInfo = myobject.GetType().GetField(propertyname);
            return propertyInfo != null ? (int) propertyInfo.GetValue(myobject) : 0;
        }
    }
}