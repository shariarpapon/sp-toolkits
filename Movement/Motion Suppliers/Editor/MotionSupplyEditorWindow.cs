using System.IO;
using UnityEditor;
using UnityEngine;

namespace SPToolkits.Movement.Editor
{
    public sealed class MotionSupplyEditorWindow : EditorWindow
    {
        private const string EDITOR_WINDOW_MENU_PATH = "Assets/Create/Motion Supply";
        private const string SAVE_DIR_KEY = "motionSupplySavePath";
        private const string MS_MENU_PATH_KEY = "motionSupplierMenuPath";
        private const string DEFAULT_SAVE_DIR = "Scripts/SPToolkits/Movement/Motion Suppliers/";
        private const string DEFAULT_MS_MENU_PATH = "SPToolkits/Motion Suppliers/";
        private const string DEFAULT_MS_NAMESPACE = "SPToolkits.Movement";
        private const string MS_NAMESPACE_KEY = "msNamespace";
        private static MotionSupplyEditorWindow Instance;

        //Fields
        private static string MSName = "NewMotionSupply";
        private static string MSSaveDir = DEFAULT_SAVE_DIR;
        private static string MSMenuPath = DEFAULT_MS_MENU_PATH;
        private static string MSNamespace = DEFAULT_MS_NAMESPACE;
        private static bool ReplaceFile = false;

        private static readonly Vector2 MAX_WINDOW_SIZE = new Vector2(500, 250);
        private static readonly Vector2 MIN_WINDOW_SIZE = new Vector2(500, 250);

        [MenuItem(EDITOR_WINDOW_MENU_PATH, priority = 1)]
        public static void Open()
        {
            if (Instance == null)
            {
                Instance = GetWindow<MotionSupplyEditorWindow>("Create Motion Supply", true);
                Instance.maxSize = MAX_WINDOW_SIZE;
                Instance.minSize = MIN_WINDOW_SIZE;
                Instance.ShowAuxWindow();

                if (EditorPrefs.HasKey(SAVE_DIR_KEY))
                    MSSaveDir = EditorPrefs.GetString(SAVE_DIR_KEY);
                if (EditorPrefs.HasKey(MS_MENU_PATH_KEY))
                    MSMenuPath = EditorPrefs.GetString(MS_MENU_PATH_KEY);
                if (EditorPrefs.HasKey(MS_NAMESPACE_KEY))
                    MSNamespace = EditorPrefs.GetString(MS_NAMESPACE_KEY);
            }
            else return;
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(SAVE_DIR_KEY, string.IsNullOrEmpty(MSSaveDir) ? DEFAULT_SAVE_DIR : MSSaveDir);
            EditorPrefs.SetString(MS_MENU_PATH_KEY, string.IsNullOrEmpty(MSMenuPath) ? DEFAULT_MS_MENU_PATH : MSMenuPath);
            EditorPrefs.SetString(MS_NAMESPACE_KEY, string.IsNullOrEmpty(MSNamespace) ? DEFAULT_MS_NAMESPACE : MSNamespace);
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, MAX_WINDOW_SIZE.x, MAX_WINDOW_SIZE.y), HexToColor("#3E3D3B"));

            EditorGUILayout.BeginVertical();
            MSName = EditorGUILayout.TextField("Name", MSName);
            MSNamespace = EditorGUILayout.TextField("Namespace", MSNamespace);
            MSSaveDir = EditorGUILayout.TextField("Save Directory", MSSaveDir);
            MSMenuPath = EditorGUILayout.TextField("Menu Path", MSMenuPath);
            ReplaceFile = EditorGUILayout.Toggle("Replace File", ReplaceFile);
            EditorGUILayout.Space();

            if (GUILayout.Button("Create"))
                CreateMSFile();

            EditorGUILayout.EndVertical();
        }

        private void CreateMSFile() 
        {
            try
            {
                if (!DirectoryExists(MSSaveDir))
                    CreateDirectory(MSSaveDir);

                string msLocalPath = Path.Combine(MSSaveDir, MSName + ".cs");
                if (FileExists(msLocalPath))
                {
                    if (!ReplaceFile)
                    {
                        Debug.LogError($"Motion Supply class with name <color=yellow>{MSName}.cs</color> already exists.");
                        return;
                    }
                    else DeleteFile(msLocalPath);
                }

                CreateTextFile(msLocalPath, GetScriptContent());
            }
            catch (System.Exception ex) 
            {
                Debug.LogError(ex.Message);
                return;
            }
            Instance.Close();
            Debug.Log($"<color=lime>Motion Supplier <<color=yellow>{MSName}</color>> created.</color>");

            AssetDatabase.Refresh();
        }

        private static string GetScriptContent() 
        {
            string scriptContent =
            $@"
using UnityEngine;

namespace {MSNamespace}
{{
    [CreateAssetMenu(fileName = ""{MSName}"", menuName = ""{MSMenuPath}""+ nameof({MSName}))]
    public class {MSName}: MotionSupplier
    {{
        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {{
        }}
    }}
}}
            ";
            return scriptContent;
        }

        public static void DeleteFile(string localPath)
            => File.Delete(LocalToSystemPath(localPath));

        public static void CreateFile(string localPath) =>
            File.Create(LocalToSystemPath(localPath));

        public static void CreateTextFile(string localPath, string content) =>
         File.WriteAllText(LocalToSystemPath(localPath), content);

        public static void CreateDirectory(string localPath) =>
            Directory.CreateDirectory(LocalToSystemPath(localPath));


        public static bool FileExists(string localPath) =>
            File.Exists(LocalToSystemPath(localPath));


        public static bool DirectoryExists(string localPath) =>
            Directory.Exists(LocalToSystemPath(localPath));


        public static string LocalToSystemPath(string path) =>
            Path.Combine(Application.dataPath, path);

        public static Color HexToColor(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color output);
            return output;
        }

    }   
}