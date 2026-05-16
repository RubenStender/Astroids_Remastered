using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MovementPresetSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class MovementPreset
    {
        public string presetName = "Preset";

        [Tooltip("Hoe snel het schip versnelt")]
        public float thrustForce = 5f;

        [Tooltip("Maximale snelheid")]
        public float maxSpeed = 8f;

        [Tooltip("Luchtweerstand (0 = geen, 1 = direct stop)")]
        [Range(0f, 1f)]
        public float drag = 0.02f;

        [Tooltip("Rotatiesnelheid in graden per seconde")]
        public float rotationSpeed = 180f;

        [Tooltip("Wrap padding buiten het scherm")]
        public float wrapPadding = 0.74f;
    }

    [Header("Presets")]
    [SerializeField]
    private MovementPreset preset1 = new MovementPreset
    {
        presetName = "Klassiek Asteroids",
        thrustForce = 5f,
        maxSpeed = 8f,
        drag = 0.001f,
        rotationSpeed = 180f,
        wrapPadding = 0.74f
    };

    [SerializeField]
    private MovementPreset preset2 = new MovementPreset
    {
        presetName = "Zwaar & Traag",
        thrustForce = 3f,
        maxSpeed = 5f,
        drag = 0.005f,
        rotationSpeed = 100f,
        wrapPadding = 0.74f
    };

    [SerializeField]
    private MovementPreset preset3 = new MovementPreset
    {
        presetName = "Snel & Wendbaar",
        thrustForce = 10f,
        maxSpeed = 15f,
        drag = 0.04f,
        rotationSpeed = 300f,
        wrapPadding = 0.74f
    };

    [Header("Target")]
    [SerializeField] private AsteroidsMovement movementScript;

    [HideInInspector] public string activePresetName = "—";

    private void Awake()
    {
        if (movementScript == null)
            movementScript = GetComponent<AsteroidsMovement>();
    }

    public void ApplyPreset(int index)
    {
        if (movementScript == null)
        {
            Debug.LogWarning("MovementPresetSwitcher: geen AsteroidsMovement gevonden!");
            return;
        }

        MovementPreset p = index switch
        {
            1 => preset1,
            2 => preset2,
            3 => preset3,
            _ => preset1
        };

        movementScript.thrustForce = p.thrustForce;
        movementScript.maxSpeed = p.maxSpeed;
        movementScript.drag = p.drag;
        movementScript.rotationSpeed = p.rotationSpeed;
        movementScript.wrapPadding = p.wrapPadding;

        activePresetName = $"[{index}] {p.presetName}";
        Debug.Log($"<color=cyan>Preset {index} actief:</color> {p.presetName}");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MovementPresetSwitcher))]
public class MovementPresetSwitcherEditor : Editor
{
    private SerializedProperty activePresetName;

    private void OnEnable()
    {
        activePresetName = serializedObject.FindProperty("activePresetName");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovementPresetSwitcher script = (MovementPresetSwitcher)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("─── Preset Switcher ───────────────", EditorStyles.boldLabel);

        GUI.enabled = false;
        EditorGUILayout.TextField("Actief", activePresetName.stringValue);
        GUI.enabled = true;

        EditorGUILayout.Space(4);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("1  Klassiek", GUILayout.Height(32))) script.ApplyPreset(1);
        if (GUILayout.Button("2  Zwaar", GUILayout.Height(32))) script.ApplyPreset(2);
        if (GUILayout.Button("3  Snel", GUILayout.Height(32))) script.ApplyPreset(3);
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif