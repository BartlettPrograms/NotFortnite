using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditorInternal;
using UnityEngine;


namespace PlayerScripts.Events
{
    [CustomEditor(typeof(SMB_Event))]
    public class Editor_SMB_Event : Editor
    {
        private SerializedProperty m_totalFrames;
        private SerializedProperty m_currentFrame;
        private SerializedProperty m_normalizedTime;
        private SerializedProperty m_normalizedTimeUncapped;
        private SerializedProperty m_motionTime;
        private SerializedProperty m_events;
        private ReorderableList m_eventsList;

        private void OnEnable()
        {
            m_totalFrames = serializedObject.FindProperty("m_totalFrames");
            m_currentFrame = serializedObject.FindProperty("m_currentFrame");
            m_normalizedTime = serializedObject.FindProperty("m_normalized Time");
            m_normalizedTimeUncapped = serializedObject.FindProperty("m_normalizedTimeUncapped");
            m_motionTime = serializedObject.FindProperty("m_motionTime");
            m_events = serializedObject.FindProperty("Events");
            m_eventsList = new ReorderableList(serializedObject, m_events, true, true, true, true);

            m_eventsList.drawHeaderCallback = DrawHeaderCallback;
            m_eventsList.drawElementCallback = DrawElementCallback;
            m_eventsList.elementHeightCallback = ElementHeightCallback;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUI.IndentLevelScope(1))
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((SMB_Event)target), typeof(SMB_Event), false);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_totalFrames);
                    EditorGUILayout.PropertyField(m_currentFrame);
                    EditorGUILayout.PropertyField(m_normalizedTime);
                    EditorGUILayout.PropertyField(m_normalizedTimeUncapped);
                }
                EditorGUILayout.PropertyField(m_motionTime);
                m_eventsList.DoLayoutList();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Events");
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty _element = m_eventsList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty _eventName = _element.FindPropertyRelative("eventName");
            SerializedProperty _timing = _element.FindPropertyRelative("timing");

            string _elementTitle;
            int _timingIndex = _timing.enumValueIndex;
            _elementTitle = string.IsNullOrEmpty(_eventName.stringValue) ? 
                $"Event *Name* ({_timing.enumDisplayNames[_timingIndex]})" : 
                $"Event {_eventName.stringValue} ({_timing.enumDisplayNames[_timingIndex]})";

            EditorGUI.PropertyField(rect, _element, new GUIContent(_elementTitle), true);
        }

        private float ElementHeightCallback(int index)
        {
            SerializedProperty _element = m_eventsList.serializedProperty.GetArrayElementAtIndex(index);
            float _propertyHieght = EditorGUI.GetPropertyHeight(_element, true);
            return _propertyHieght;
        }
    }
}