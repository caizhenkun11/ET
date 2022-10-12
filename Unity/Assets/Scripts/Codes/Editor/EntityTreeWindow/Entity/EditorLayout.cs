using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 样式
    /// </summary>
    public static class Styles
    {
        private static GUIStyle _sectionHeader;
        private static GUIStyle _sectionContent;

        public static GUIStyle sectionHeader
        {
            get
            {
                if (Styles._sectionHeader == null)
                    Styles._sectionHeader = new GUIStyle((GUIStyle)"OL Title");
                return Styles._sectionHeader;
            }
        }

        public static GUIStyle sectionContent
        {
            get
            {
                if (Styles._sectionContent == null)
                {
                    Styles._sectionContent = new GUIStyle((GUIStyle)"OL Box");
                    Styles._sectionContent.stretchHeight = false;
                }

                return Styles._sectionContent;
            }
        }
    }
    /// <summary>
    /// 编辑模式布局组件
    /// </summary>
    public static class EditorLayout
    {
        private const int DEFAULT_FOLDOUT_MARGIN = 11;

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="size"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetWindow<T>(string title, Vector2 size) where T : EditorWindow
        {
            T window = EditorWindow.GetWindow<T>(true, title);
            window.minSize = window.maxSize = size;
            return window;
        }
        /// <summary>
        /// 加载纹理
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string label)
        {
            string[] assets = AssetDatabase.FindAssets(label);
            if (assets.Length != 0)
            {
                string guid = assets[0];
                if (guid != null)
                    return AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
            }

            return (Texture2D)null;
        }
        /// <summary>
        /// 回执纹理
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Rect DrawTexture(Texture2D texture)
        {
            if (!((UnityEngine.Object)texture != (UnityEngine.Object)null))
                return new Rect();
            Rect aspectRect = GUILayoutUtility.GetAspectRect((float)((double)texture.width / (double)texture.height),
                new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
            GUI.DrawTexture(aspectRect, (Texture)texture, ScaleMode.ScaleAndCrop);
            return aspectRect;
        }
        /// <summary>
        /// 对象区域按钮
        /// </summary>
        /// <param name="label"></param>
        /// <param name="buttonText"></param>
        /// <returns></returns>
        public static bool ObjectFieldButton(string label, string buttonText)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, new GUILayoutOption[1] { GUILayout.Width(146f) });
            if (buttonText.Length > 24)
                buttonText = "..." + buttonText.Substring(buttonText.Length - 24);
            int num = GUILayout.Button(buttonText, EditorStyles.objectField, new GUILayoutOption[0]) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            return num != 0;
        }
        /// <summary>
        /// 对象区域文件夹面板
        /// </summary>
        /// <param name="label"></param>
        /// <param name="buttonText"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static string ObjectFieldOpenFolderPanel(string label, string buttonText, string defaultPath)
        {
            if (!EditorLayout.ObjectFieldButton(label, buttonText))
                return (string)null;
            string str = defaultPath ?? "Assets/";
            if (!Directory.Exists(str))
                str = "Assets/";
            return EditorUtility.OpenFolderPanel(label, str, string.Empty).Replace(Directory.GetCurrentDirectory() + "/", string.Empty);
        }
        /// <summary>
        /// 对象区域文件面板
        /// </summary>
        /// <param name="label"></param>
        /// <param name="buttonText"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static string ObjectFieldOpenFilePanel(string label, string buttonText, string defaultPath)
        {
            if (!EditorLayout.ObjectFieldButton(label, buttonText))
                return (string)null;
            string str = defaultPath ?? "Assets/";
            if (!File.Exists(str))
                str = "Assets/";
            return EditorUtility.OpenFilePanel(label, str, "dll").Replace(Directory.GetCurrentDirectory() + "/", string.Empty);
        }
        /// <summary>
        /// 最小按钮
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool MiniButton(string c)
        {
            return EditorLayout.miniButton(c, EditorStyles.miniButton);
        }
        /// <summary>
        /// 最小按钮左
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool MiniButtonLeft(string c)
        {
            return EditorLayout.miniButton(c, EditorStyles.miniButtonLeft);
        }

        public static bool MiniButtonMid(string c)
        {
            return EditorLayout.miniButton(c, EditorStyles.miniButtonMid);
        }

        public static bool MiniButtonRight(string c)
        {
            return EditorLayout.miniButton(c, EditorStyles.miniButtonRight);
        }
        /// <summary>
        /// 最小按钮
        /// </summary>
        /// <param name="c"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        private static bool miniButton(string c, GUIStyle style)
        {
            GUILayoutOption[] guiLayoutOptionArray1;
            if (c.Length != 1)
                guiLayoutOptionArray1 = new GUILayoutOption[0];
            else
                guiLayoutOptionArray1 = new GUILayoutOption[1] { GUILayout.Width(19f) };
            GUILayoutOption[] guiLayoutOptionArray2 = guiLayoutOptionArray1;
            int num = GUILayout.Button(c, style, guiLayoutOptionArray2) ? 1 : 0;
            if (num == 0)
                return num != 0;
            GUI.FocusControl((string)null);
            return num != 0;
        }
        /// <summary>
        /// 折页
        /// </summary>
        /// <param name="foldout"></param>
        /// <param name="content"></param>
        /// <param name="leftMargin"></param>
        /// <returns></returns>
        public static bool Foldout(bool foldout, string content, int leftMargin = 11)
        {
            return EditorLayout.Foldout(foldout, content, EditorStyles.foldout, leftMargin);
        }

        public static bool Foldout(bool foldout, string content, GUIStyle style, int leftMargin = 11)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space((float)leftMargin);
            foldout = EditorGUILayout.Foldout(foldout, content, style);
            EditorGUILayout.EndHorizontal();
            return foldout;
        }
        /// <summary>
        /// 搜索文本区域
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static string SearchTextField(string searchString)
        {
            bool changed = GUI.changed;
            GUILayout.BeginHorizontal();
            searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"), new GUILayoutOption[0]);
            if (GUILayout.Button(string.Empty, GUI.skin.FindStyle("ToolbarSeachCancelButton"), new GUILayoutOption[0]))
                searchString = string.Empty;
            GUILayout.EndHorizontal();
            GUI.changed = changed;
            return searchString;
        }
        /// <summary>
        /// 匹配搜索文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static bool MatchesSearchString(string str, string search)
        {
            string[] strArray = search.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 0)
                return ((IEnumerable<string>)strArray).Any<string>(new Func<string, bool>(str.Contains));
            return true;
        }
        /// <summary>
        /// 绘制栏头开关
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool DrawSectionHeaderToggle(string header, bool value)
        {
            return GUILayout.Toggle(value, header, Styles.sectionHeader, new GUILayoutOption[0]);
        }
        /// <summary>
        /// 开始栏内容
        /// </summary>
        public static void BeginSectionContent()
        {
            EditorGUILayout.BeginVertical(Styles.sectionContent, new GUILayoutOption[0]);
        }
        /// <summary>
        /// 解锁栏内容
        /// </summary>
        public static void EndSectionContent()
        {
            EditorGUILayout.EndVertical();
        }
        /// <summary>
        /// 开始垂直box
        /// </summary>
        /// <returns></returns>
        public static Rect BeginVerticalBox()
        {
            return EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
        }
        /// <summary>
        /// 结束垂直box
        /// </summary>
        public static void EndVerticalBox()
        {
            EditorGUILayout.EndVertical();
        }
    }
}