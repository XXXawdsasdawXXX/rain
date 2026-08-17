using UnityEditor;
using UnityEngine;

namespace Essential.Data.BoolMatrix.Editor
{
    [CustomPropertyDrawer(typeof(BoolMatrix), true)]
    public class BoolMatrixDrawer : PropertyDrawer
    {
        private const float CellSize = 18f;
        private const float LabelWidth = 22f;
        private const float Padding = 4f;
        private const float SizeFieldWidth = 40f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            int width = Mathf.Max(0, property.FindPropertyRelative("_width").intValue);
            int height = Mathf.Max(0, property.FindPropertyRelative("_height").intValue);

            return EditorGUIUtility.singleLineHeight * 2f
                   + Padding
                   + (height + 1) * CellSize
                   + Padding
                   + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty widthProp = property.FindPropertyRelative("_width");
            SerializedProperty heightProp = property.FindPropertyRelative("_height");
            SerializedProperty valuesProp = property.FindPropertyRelative("_values");

            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            Rect sizeRect = new Rect(
                position.x,
                foldoutRect.yMax + Padding,
                position.width,
                EditorGUIUtility.singleLineHeight);

            float labelW = 50f;
            Rect wLabel = new Rect(sizeRect.x, sizeRect.y, labelW, sizeRect.height);
            Rect wField = new Rect(wLabel.xMax, sizeRect.y, SizeFieldWidth, sizeRect.height);
            Rect hLabel = new Rect(wField.xMax + 8f, sizeRect.y, labelW, sizeRect.height);
            Rect hField = new Rect(hLabel.xMax, sizeRect.y, SizeFieldWidth, sizeRect.height);

            EditorGUI.LabelField(wLabel, "Width");
            EditorGUI.BeginChangeCheck();
            int width = EditorGUI.IntField(wField, Mathf.Max(0, widthProp.intValue));
            EditorGUI.LabelField(hLabel, "Height");
            int height = EditorGUI.IntField(hField, Mathf.Max(0, heightProp.intValue));

            if (EditorGUI.EndChangeCheck())
            {
                ResizeValues(valuesProp, widthProp.intValue, heightProp.intValue, width, height);
                widthProp.intValue = width;
                heightProp.intValue = height;
            }

            width = widthProp.intValue;
            height = heightProp.intValue;
            EnsureValuesSize(valuesProp, width, height);

            Rect gridOrigin = new Rect(
                position.x,
                sizeRect.yMax + Padding,
                LabelWidth + width * CellSize,
                (height + 1) * CellSize);

            // Column indices
            for (int x = 0; x < width; x++)
            {
                Rect header = new Rect(
                    gridOrigin.x + LabelWidth + x * CellSize,
                    gridOrigin.y,
                    CellSize,
                    CellSize);
                GUI.Label(header, x.ToString(), EditorStyles.centeredGreyMiniLabel);
            }

            // Rows: draw top-to-bottom as y = height-1 .. 0 so (0,0) is bottom-left visually.
            for (int drawRow = 0; drawRow < height; drawRow++)
            {
                int y = height - 1 - drawRow;
                float rowY = gridOrigin.y + (drawRow + 1) * CellSize;

                Rect rowLabel = new Rect(gridOrigin.x, rowY, LabelWidth, CellSize);
                GUI.Label(rowLabel, y.ToString(), EditorStyles.centeredGreyMiniLabel);

                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (index < 0 || index >= valuesProp.arraySize)
                    {
                        continue;
                    }

                    SerializedProperty cell = valuesProp.GetArrayElementAtIndex(index);
                    Rect cellRect = new Rect(
                        gridOrigin.x + LabelWidth + x * CellSize,
                        rowY,
                        CellSize,
                        CellSize);

                    cell.boolValue = EditorGUI.Toggle(cellRect, cell.boolValue);
                }
            }

            Rect footer = new Rect(
                position.x,
                gridOrigin.yMax + Padding,
                position.width,
                EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(footer, $"{width}×{height}  (0,0 = bottom-left)", EditorStyles.miniLabel);

            EditorGUI.EndProperty();
        }

        private static void EnsureValuesSize(SerializedProperty valuesProp, int width, int height)
        {
            int needed = Mathf.Max(0, width) * Mathf.Max(0, height);
            if (valuesProp.arraySize != needed)
            {
                valuesProp.arraySize = needed;
            }
        }

        private static void ResizeValues(
            SerializedProperty valuesProp,
            int oldWidth,
            int oldHeight,
            int newWidth,
            int newHeight)
        {
            newWidth = Mathf.Max(0, newWidth);
            newHeight = Mathf.Max(0, newHeight);

            bool[] oldValues = new bool[valuesProp.arraySize];
            for (int i = 0; i < valuesProp.arraySize; i++)
            {
                oldValues[i] = valuesProp.GetArrayElementAtIndex(i).boolValue;
            }

            valuesProp.arraySize = newWidth * newHeight;
            int copyW = Mathf.Min(oldWidth, newWidth);
            int copyH = Mathf.Min(oldHeight, newHeight);

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    int newIndex = y * newWidth + x;
                    bool value = false;

                    if (x < copyW && y < copyH)
                    {
                        int oldIndex = y * oldWidth + x;
                        if (oldIndex < oldValues.Length)
                        {
                            value = oldValues[oldIndex];
                        }
                    }

                    valuesProp.GetArrayElementAtIndex(newIndex).boolValue = value;
                }
            }
        }
    }
}
