using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Editor.Package.Tool
{
    internal class ExportSceneToJSON : UnityEditor.Editor
    {
        // 将当前选择的GAMEOBJECT与下面的子GAMEOBJECT全导出为JSON格式
        [MenuItem("Tools/ExportJSON")]
        static void ExportJSON()
        {
            if (!Selection.activeGameObject)
            {
                EditorUtility.DisplayDialog("提示", "请选择一个GAME OBJECT，再选择导出.", "确定");
                return;
            }

            var go = Selection.activeGameObject;
            var strTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var strList = Application.dataPath.Split(new char[] {'\\', '/'});
            strList[strList.Length-1] = $"{strTime}-{go.name}.json";
            string filepath = string.Join("/", strList);
            FileInfo t = new FileInfo(filepath);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            StreamWriter sw = t.CreateText();

            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);

                WriteTransform(Selection.activeGameObject, writer);

            sw.WriteLine(sb.ToString());
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
        }

        private static void WriteTransform(GameObject go, JsonWriter writer)
        {
            if (go)
            {
                var tran = go.GetComponent<RectTransform>();
                if (!tran)
                {
                    return;
                }

                writer.WriteObjectStart();

                writer.WritePropertyName("name");
                writer.Write(go.name);

                writer.WritePropertyName("position");
                writer.WriteObjectStart();
                    writer.WritePropertyName("x");
                    writer.Write(tran.anchoredPosition.x.ToString());
                    writer.WritePropertyName("y");
                    writer.Write(tran.anchoredPosition.y.ToString());
                    writer.WritePropertyName("w");
                    writer.Write(tran.rect.width.ToString());
                    writer.WritePropertyName("h");
                    writer.Write(tran.rect.height.ToString());
                writer.WriteObjectEnd();

                var img = go.GetComponent<Image>();
                if (img && img.sprite)
                {
                    writer.WritePropertyName("image");
                    writer.Write(img.sprite.name);
                }

                if (tran.childCount > 0)
                {
                    writer.WritePropertyName("children");
                    writer.WriteArrayStart();

                    for (int i = 0; i < tran.childCount; ++i)
                    {
                        WriteTransform(tran.GetChild(i).gameObject, writer);
                    }

                    writer.WriteArrayEnd();
                }

                writer.WriteObjectEnd();
            }
        }


        //将所有游戏场景导出为JSON格式
        static void ExportJSON2()
        {
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
            {
                if (S.enabled)
                {
                    string name = S.path;
                    EditorApplication.OpenScene(name);

                    foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
                    {
                        if (obj.transform.parent == null)
                        {
                        }
                    }
                }
            }
        }
    }

}