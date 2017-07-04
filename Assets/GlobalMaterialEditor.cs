
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(GlobalMaterialSettings))]
public class GlobalMaterialEditor : Editor
{
    enum PropertyType { TypeColor, TypeFloat, TypeVector4 };

    GlobalMaterialSettings  m_GMS;
    SerializedObject        m_SerObj;
	SerializedProperty      m_Colors;
    SerializedProperty      m_Floats;
    SerializedProperty      m_Vectors;
    ReorderableList         ColorsList;
    ReorderableList         FloatsList;
    ReorderableList         VectorsList;

    void OnEnable()
    {
        if(target == null) return;
        m_GMS       = (GlobalMaterialSettings)target;
        m_SerObj	= new SerializedObject (m_GMS);
        m_Colors    = m_SerObj.FindProperty ("globalColors");
        m_Floats    = m_SerObj.FindProperty ("globalFloats");
        m_Vectors   = m_SerObj.FindProperty ("globalVectors");

        ColorsList  = new ReorderableList(serializedObject, m_Colors, true, true, true, true );
        FloatsList  = new ReorderableList(serializedObject, m_Floats, true, true, true, true );
        VectorsList = new ReorderableList(serializedObject, m_Vectors, true, true, true, true);

        DrawList(ColorsList , PropertyType.TypeColor , "Colors");
        DrawList(FloatsList , PropertyType.TypeFloat , "Floats");
        DrawList(VectorsList, PropertyType.TypeVector4,"Vectors");
    }
        
    void DrawList (ReorderableList list, PropertyType type, string Header = "")
    {
        // Header
        list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, Header ); };

        // Element
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
			SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty globalName  = element.FindPropertyRelative("globalName" );
            SerializedProperty globalValue = element.FindPropertyRelative("globalValue");
            rect.y += 1;
            float offset = (type == PropertyType.TypeVector4) ? 200: 70;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - offset - 4, EditorGUIUtility.singleLineHeight + 1), globalName, GUIContent.none);

            switch (type)
            {
                case PropertyType.TypeFloat:
                    // The float value can be changed via horizon slide.
                    EditorGUIUtility.labelWidth = 8;
                    globalValue.floatValue = EditorGUI.FloatField(new Rect(rect.x + rect.width - offset, rect.y, offset, EditorGUIUtility.singleLineHeight), " ", globalValue.floatValue);
                    break;
                case PropertyType.TypeVector4:
                    globalValue.vector4Value = EditorGUI.Vector4Field(new Rect(rect.x + rect.width - offset, rect.y, offset, EditorGUIUtility.singleLineHeight), "", globalValue.vector4Value);
                    break;
                default:
                    EditorGUI.PropertyField(new Rect(rect.x + rect.width - offset, rect.y, offset, EditorGUIUtility.singleLineHeight + 1), globalValue, GUIContent.none);
                    break;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        if (m_SerObj == null) return;

        EditorGUI.BeginChangeCheck();
        m_SerObj.Update();

        ColorsList.DoLayoutList();
        EditorGUILayout.Space();
        FloatsList.DoLayoutList();
        EditorGUILayout.Space();
        VectorsList.DoLayoutList();

        //if (GUILayout.Button("Apply to Materials", GUILayout.MinHeight(25)))
        //    m_GMS.UpdateGlobalUniform();

        m_SerObj.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            m_GMS.UpdateGlobalUniform();
        }
    }
}
