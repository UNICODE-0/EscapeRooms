using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SpellSinger.BookmarksAndSelections
{
    public class BookmarksWindow : EditorWindow
    {
        private readonly string AddTip =
            "To add asset object to bookmarks right click on asset and choose 'Add to Bookmarks' in context menu";

        private Vector2 scrollPos;
        private Bookmarks so;
        private bool editMode;
        private Vector2 scrollPosition;

        private void OnEnable()
        {
            so = SoStorage.GetStorage<Bookmarks>();
        }

        private void OnGUI()
        {
            if (so == null)
            {
                return;
            }

            if (so.List.Count == 0 || editMode)
            {
                var textStyle = EditorStyles.label;
                textStyle.wordWrap = true;
                EditorGUILayout.LabelField(AddTip, textStyle);
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            var buttonWidth = GUILayout.Width(30);
            var dragHandleWidth = GUILayout.Width(20);


            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            var list = so.List;
            for (var i = 0; i < list.Count; i++)
            {
                var bookmark = list[i];

                EditorGUILayout.BeginHorizontal();

                if (editMode)
                {
                    if (i == 0)
                    {
                        GUI.enabled = false;
                    }

                    if (GUILayout.Button("▲", buttonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i - 1, bookmark);
                        SaveSo();
                    }

                    if (i == 0)
                    {
                        GUI.enabled = true;
                    }

                    if (i == list.Count - 1)
                    {
                        GUI.enabled = false;
                    }

                    if (GUILayout.Button("▼", buttonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i + 1, bookmark);
                        SaveSo();
                    }

                    if (i == list.Count - 1)
                    {
                        GUI.enabled = true;
                    }

                    if (GUILayout.Button("X", buttonWidth))
                    {
                        list.RemoveAt(i);
                        SaveSo();
                    }
                }


                EditorGUILayout.ObjectField(bookmark.bookmarkObject, typeof(Object), true);
                EditorGUI.BeginDisabledGroup(!editMode);
                bookmark.descriprion = GUILayout.TextArea(bookmark.descriprion, GUILayout.Width(150));
                EditorGUI.EndDisabledGroup();

                var e = Event.current;
                if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (e.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            AddToBookmarks(draggedObject);
                        }
                    }

                    Event.current.Use();
                }


                if (!editMode)
                {
                    GUILayout.Label(" ≡ ", dragHandleWidth);

                    if (GUILayoutUtility.GetLastRect().Contains(e.mousePosition) && e.type == EventType.MouseDrag)
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.objectReferences = new[] { bookmark.bookmarkObject };
                        DragAndDrop.StartDrag("drag");
                        Event.current.Use();
                    }

                    if (Selection.activeObject == bookmark.bookmarkObject)
                    {
                        GUI.enabled = true;
                    }
                }


                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();


            if (list.Count > 0)
            {
                if (editMode)
                {
                    if (GUILayout.Button("Exit Edit Mode"))
                    {
                        editMode = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Edit"))
                    {
                        editMode = true;
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void SaveSo()
        {
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssetIfDirty(so);
        }

        [MenuItem("Window/SpellSinger/Bookmarks")]
        private static void ShowWindow()
        {
            var window = GetWindow<BookmarksWindow>();
            window.titleContent = new GUIContent("Bookmarks");
            window.Show();
        }

        [MenuItem("Assets/Add to Bookmarks", false, 30)]
        private static void MENU_AddToBookmarks(MenuCommand menuCommand)
        {
            AddToBookmarks(Selection.activeObject);
        }

        private static void AddToBookmarks(Object o)
        {
            if (o == null)
            {
                return;
            }

            var bookmarks = SoStorage.GetStorage<Bookmarks>();
            if(bookmarks.List.Any((Bookmark Bookmark) => Bookmark.bookmarkObject.Equals(o))) return;

            bookmarks.List.Add(new Bookmark(o, ""));
            EditorUtility.SetDirty(bookmarks);
            AssetDatabase.SaveAssetIfDirty(bookmarks);
        }
    }
}