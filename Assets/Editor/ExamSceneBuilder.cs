using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class ExamSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/MainScene.unity";

    [MenuItem("Examen/Generar escena del examen")]
    public static void Build()
    {
        Directory.CreateDirectory("Assets/Scenes");
        Sprite playerSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Player.png");
        Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Square.png");
        if (playerSprite == null || squareSprite == null)
        {
            AssetDatabase.Refresh();
            playerSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Player.png");
            squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Square.png");
        }

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "MainScene";
        Camera camera = new GameObject("Main Camera").AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.orthographic = true;
        camera.orthographicSize = 5.2f;
        camera.transform.position = new Vector3(0f, 0f, -10f);
        camera.backgroundColor = new Color(0.96f, 0.92f, 0.92f);
        camera.clearFlags = CameraClearFlags.SolidColor;

        int ground = LayerMask.NameToLayer("Default");
        CreatePlatform("Piso inicial", new Vector2(-6.1f, -4.35f), new Vector2(3.8f, 0.45f), squareSprite, Color.black, false, ground);
        CreatePlatform("Plataforma 1", new Vector2(-1.8f, -2.9f), new Vector2(2.0f, 0.45f), squareSprite, Color.black, false, ground);
        CreatePlatform("Plataforma 2", new Vector2(1.2f, -1.0f), new Vector2(2.1f, 0.45f), squareSprite, Color.black, false, ground);
        CreatePlatform("Plataforma 3", new Vector2(4.4f, 1.1f), new Vector2(2.2f, 0.45f), squareSprite, Color.black, false, ground);
        CreatePlatform("Plataforma superior", new Vector2(6.2f, 3.0f), new Vector2(4.0f, 0.45f), squareSprite, Color.black, false, ground);
        CreatePlatform("Muro lateral", new Vector2(7.8f, -1.0f), new Vector2(0.45f, 3.5f), squareSprite, Color.black, false, ground);

        GameObject player = new GameObject("PLAYER");
        player.transform.position = new Vector3(-7.0f, -3.85f, 0f);
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>(); sr.sprite = playerSprite; sr.color = Color.white; sr.sortingOrder = 5;
        CircleCollider2D circle = player.AddComponent<CircleCollider2D>(); circle.radius = 0.48f;
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>(); rb.freezeRotation = true; rb.gravityScale = 3f; rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        player.AddComponent<PlayerController>();
        CreatePlatform("ZONA DE REBOTE", new Vector2(-0.8f, 2.7f), new Vector2(1.5f, 5.2f), squareSprite, Color.blue, true, ground).AddComponent<BounceZone>();
        CreatePlatform("ZONA DE DAÑO", new Vector2(2.0f, -4.65f), new Vector2(8.0f, 0.35f), squareSprite, new Color(1f, 0.48f, 0f), true, ground).AddComponent<DamageZone>();

        CreateText("MOVIMIENTO: A/D y FLECHAS | SALTO: W / ↑ / ESPACIO", new Vector2(-5.7f, 4.25f), 0.28f, Color.black);
        CreateText("SALTO DE PLATAFORMAS", new Vector2(3.4f, 4.25f), 0.28f, Color.black);
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        Debug.Log("EVAL 2 V4: escena creada correctamente en " + ScenePath);
    }

    private static GameObject CreatePlatform(string name, Vector2 pos, Vector2 size, Sprite sprite, Color color, bool trigger, int layer)
    {
        GameObject obj = new GameObject(name); obj.transform.position = pos; obj.transform.localScale = new Vector3(size.x, size.y, 1f); obj.layer = layer;
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>(); renderer.sprite = sprite; renderer.color = color;
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>(); collider.isTrigger = trigger;
        return obj;
    }

    private static void CreateText(string value, Vector2 pos, float size, Color color)
    {
        GameObject obj = new GameObject("Instrucciones"); obj.transform.position = pos;
        TextMesh text = obj.AddComponent<TextMesh>(); text.text = value; text.fontSize = 48; text.characterSize = size / 2f; text.color = color; text.anchor = TextAnchor.MiddleCenter;
    }
}
