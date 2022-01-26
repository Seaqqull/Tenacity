using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Cards
{
    [CustomEditor(typeof(Card), true)]
    public class EditCardMode : Editor
    {
        private Card card;
        private SerializedObject cardSO;
        private SerializedProperty properties;

        private readonly string FactoryName = Card.PropertyFactoryName;

        private void OnEnable()
        {
            card = target as Card;
            cardSO = new SerializedObject(card);
            properties = cardSO.FindProperty(FactoryName);
        }

        public override void OnInspectorGUI()
        {
            cardSO.Update();
            DrawPropertiesExcluding(cardSO, new string[] { FactoryName });
            EditorGUILayout.PropertyField(properties.FindPropertyRelative("Type"));

            SerializedProperty parentProperty = GetParentProperty();
            string parentPropertyPath = GetPropertyPath(parentProperty);
            while (parentProperty.NextVisible(true) && parentProperty.propertyPath.StartsWith(parentPropertyPath))
            {
                EditorGUILayout.PropertyField(parentProperty);
            }
            cardSO.ApplyModifiedProperties();

            if (GUI.changed)
            {
                card.UpdatePropertiesList();
            }
        }

        private Type GetChosenType()
        {
            CardFactory cardFactory = card.CardFactory;
            return cardFactory.GetClassByCardType(cardFactory.Type);
        }
        private SerializedProperty GetParentProperty()
        {
            string typeOfCardStr = GetChosenType().Name.ToString();
            return properties.FindPropertyRelative(typeOfCardStr).Copy();
        }
        private string GetPropertyPath(SerializedProperty property)
        {
            return property.propertyPath;
        }
    }
}