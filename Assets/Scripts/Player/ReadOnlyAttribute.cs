using UnityEngine;
//Unityエディタ上のみで動かすための条件
#if UNITY_EDITOR
using UnityEditor;
#endif

// 属性を定義するためのクラス
//[ReadOnly]というラベルを定義し、PropertyAttributeを継承することで変数にこの属性を付与できるようにする
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
// インスペクター上での見た目を制御するクラス
//ReadOnlyAttributeが付与された変数をどう表示するかを指定
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    //GUI上での描画方法を定義するメソッド
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //ここから下の描画を無効化することで、インスペクター上で変数を読み取り専用にする
        GUI.enabled = false; // 書き換えを無効化
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;  // 有効化に戻す
    }
}
#endif