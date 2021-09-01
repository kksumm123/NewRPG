using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class EditorOptionConfig : EditorWindow
{
    [MenuItem("Tools/Option")]
    static void SetNormalTexture_command()
    {
        Debug.Log("노멀 텍스쳐 적용하자");
        SetNormalTextures();
    }

    private static void SetNormalTextures()
    {
        Debug.Log("SetNormalTexture 실행");
        foreach (var item in Selection.objects)
        {
            Material mat = (Material)item;
            if (mat != null)
                SetNormalTexture(mat);
        }
    }

    private static void SetNormalTexture(Material mat)
    {
        string mainTexturePath = AssetDatabase.GetAssetPath(mat.mainTexture);
        if (mainTexturePath != null)
        {
            string normalTexturePath = mainTexturePath.Replace(".png", "_n.png");

            // AssetDatabase.LoadAssetAtPath 에디터상에서만 쓸 수 있는
            // Resources 같은 기능, 위치 구애 없이 로드 가능
            Texture normalTexture = AssetDatabase.LoadAssetAtPath<Texture>(normalTexturePath);
            if (normalTexture != null)
                mat.SetTexture("_BumpMap", normalTexture);
            else
                Debug.Log($"selected MainTex : {mainTexturePath}\nNotFound normalTex : {normalTexturePath} 경로에 파일이 없음");
        }
    }

    [MenuItem("Tools/Option")]
    static void Init()
    {
        GetWindow(typeof(EditorOptionConfig));
    }

    Vector2 mPos = Vector2.zero;
    void OnGUI()
    {
        mPos = GUILayout.BeginScrollView(mPos);
        if (GUILayout.Button("테스트 버튼"))
        {
            Debug.Log("테스트 버튼 누름");
        }
        if (GUILayout.Button("텍스쳐에 노멀맵 달아버리기"))
        {
            SetNormalTexture_command();
        }

        for (OptionType i = OptionType.StartIndex + 1; i < OptionType.LastIndex; i++)
        {
            GUILayout.BeginHorizontal();
            {
                bool tempBool = EditorOption.Options[i];

                EditorOption.Options[i] = GUILayout.Toggle(EditorOption.Options[i], i.ToString());

                if (tempBool != EditorOption.Options[i])
                {
                    string key = "DevOption_" + i;
                    EditorPrefs.SetInt(key, EditorOption.Options[i] == true ? 1 : 0);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
}