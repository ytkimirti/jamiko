using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

// [CustomPropertyDrawer(typeof(Vector3Spring))]
// public class SpringPropertyDrawer : PropertyDrawer
// {
//     public override VisualElement CreatePropertyGUI(SerializedProperty property)
//     {
//         var root = new VisualElement();
//         
//         var amountField = new PropertyField(property.FindPropertyRelative("mass"));
//         var unitField = new PropertyField(property.FindPropertyRelative("tension"));
//         var nameField = new PropertyField(property.FindPropertyRelative("friction"), "Fancy Name");
//
//         // Add fields to the container.
//         root.Add(amountField);
//         root.Add(unitField);
//         root.Add(nameField);
//         
//         root.Add(new Label("Heyoo"));
//
//         return root;
//     }
// }