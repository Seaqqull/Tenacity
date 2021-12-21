using System;
using UnityEditor;

[CustomEditor(typeof(CardManager), true)]
public class EditCardMode : Editor
{
    private CardManager _manager;
    private SerializedObject _cardManagerSO;
    private SerializedProperty _properties;

    private readonly string FactoryName = CardManager.PropertyFactoryName;

    private void OnEnable()
    {
        _manager = target as CardManager;
        _cardManagerSO = new SerializedObject(_manager);
        _properties = _cardManagerSO.FindProperty(FactoryName);
    }

    public override void OnInspectorGUI()
    {
        _cardManagerSO.Update();
        DrawPropertiesExcluding(_cardManagerSO, new string[] { FactoryName });
        EditorGUILayout.PropertyField(_properties.FindPropertyRelative("Type"));

        SerializedProperty parentProperty = GetParentProperty();
        string parentPropertyPath = GetPropertyPath(parentProperty);
        while (parentProperty.NextVisible(true) && parentProperty.propertyPath.StartsWith(parentPropertyPath))
        {
            EditorGUILayout.PropertyField(parentProperty);
        }

        _cardManagerSO.ApplyModifiedProperties();
    }

    private Type GetChosenType()
    {
        CardFactory cardFactory = _manager.CardFactory;
        return cardFactory.GetClassByCardType(cardFactory.Type);
    }
    private SerializedProperty GetParentProperty()
    {
        string typeOfCardStr = GetChosenType().ToString();
        return _properties.FindPropertyRelative(typeOfCardStr).Copy();
    }
    private string GetPropertyPath(SerializedProperty property)
    {
        return property.propertyPath;
    }
}
