using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Codice.Client.BaseCommands.BranchExplorer.Layout;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;


[CustomPropertyDrawer(typeof(PromptTimelineBehaviour))]
public class PromptTimelineDrawer : PropertyDrawer
{
    
    // int   selectedSize = 1;
    string[] startTexts;
    int[] startSizes;
    
    string[] endTexts;
    int[] endSizes;

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 0;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);
        // singleFieldRect.y += EditorGUIUtility.singleLineHeight*2;

        // var attr    = attribute as EnumElementUsageAttribute;
        SerializedProperty linesProps = property.FindPropertyRelative ("lines");
        SerializedProperty startIndexProps = property.FindPropertyRelative ("startIndex");
        SerializedProperty endIndexProps = property.FindPropertyRelative ("endIndex");
        SerializedProperty promptProps = property.FindPropertyRelative ("prompt");
        startTexts = new string[linesProps.arraySize];
        startSizes = new int[linesProps.arraySize];
        endTexts = new string[linesProps.arraySize];
        endSizes = new int[linesProps.arraySize];
        for (int i = 0; i < linesProps.arraySize; i++)
        {
            var text = linesProps.GetArrayElementAtIndex(i).stringValue;
            var firstLine = text.Split("\n")[0];
            startTexts[i] = $"{i}: {firstLine}...";
            startSizes[i] = i;
            
            endTexts[i] = $"{i}:  {firstLine}...";
            endSizes[i] = i;
        }
      
        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight+100);
        
        var startIndex = startIndexProps.intValue;
        var endIndex = endIndexProps.intValue;
               
        startIndexProps.intValue = EditorGUILayout.IntPopup(startIndexProps.intValue, startTexts, startSizes);
        endIndexProps.intValue = EditorGUILayout.IntPopup(endIndexProps.intValue, endTexts, endSizes);

        if (startIndexProps.intValue < endIndexProps.intValue)
        {
            
            promptProps.ClearArray();
            // var newPromptList = new List<string>();
            var sb = new StringBuilder();
            promptProps.arraySize = (endIndexProps.intValue - startIndexProps.intValue);
            for (int i = startIndexProps.intValue; i < endIndexProps.intValue+1; i++)
            {
                // Debug.Log(i);
                sb.AppendLine(linesProps.GetArrayElementAtIndex(i).stringValue);
            }
            promptProps.stringValue = sb.ToString();


        }
        singleFieldRect.y += EditorGUIUtility.singleLineHeight*10;
        EditorGUI.TextArea(singleFieldRect,promptProps.stringValue);
       
        EditorGUI.EndProperty();

    }
    
}
