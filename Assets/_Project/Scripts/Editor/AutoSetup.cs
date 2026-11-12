using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AutoSetup : EditorWindow
{
    [MenuItem("Tools/Сгенерировать игру (Magic Button)")]
    public static void SetupGame()
    {
        // 1. Создаем GameManager
        GameObject gmObj = new GameObject("_GameManager");
        gmObj.AddComponent<GameManager>();

        // 2. Создаем Камеру
        GameObject camObj = new GameObject("Main Camera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5;
        cam.backgroundColor = new Color(0.1f, 0.1f, 0.15f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        camObj.tag = "MainCamera";
        var camFollow = camObj.AddComponent<CameraFollow>();
        camObj.transform.position = new Vector3(0, 0, -10);

        // 3. Создаем Игрока (Принтер)
        GameObject player = new GameObject("PlayerPrinter");
        player.tag = "Player";
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        sr.color = Color.white;
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        player.AddComponent<BoxCollider2D>();
        
        var printerCtrl = player.AddComponent<PrinterController>();
        var weaponSys = player.AddComponent<WeaponSystem>();
        
        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.SetParent(player.transform);
        firePoint.transform.localPosition = new Vector3(0, 1f, 0);
        weaponSys.firePoint = firePoint.transform;
        camFollow.target = player.transform;

        // 4. Создаем Пулю (Префаб)
        GameObject bullet = new GameObject("BulletPrefab");
        SpriteRenderer bSr = bullet.AddComponent<SpriteRenderer>();
        bSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        bSr.color = Color.yellow;
        bullet.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        Rigidbody2D bRb = bullet.AddComponent<Rigidbody2D>();
        bRb.bodyType = RigidbodyType2D.Kinematic;
        CircleCollider2D bCol = bullet.AddComponent<CircleCollider2D>();
        bCol.isTrigger = true;
        bullet.AddComponent<BulletController>();
        
        // Сохраняем пулю как префаб
        GameObject bulletPrefab = PrefabUtility.SaveAsPrefabAsset(bullet, "Assets/_Project/Prefabs/Bullet.prefab");
        DestroyImmediate(bullet); // Удаляем со сцены
        weaponSys.bulletPrefab = bulletPrefab;

        // 5. Создаем Энергию (Префаб)
        GameObject energy = new GameObject("EnergyPrefab");
        SpriteRenderer eSr = energy.AddComponent<SpriteRenderer>();
        eSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        eSr.color = Color.green;
        energy.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        CircleCollider2D eCol = energy.AddComponent<CircleCollider2D>();
        eCol.isTrigger = true;
        energy.AddComponent<EnergyPickup>();

        // Сохраняем энергию как префаб
        GameObject energyPrefab = PrefabUtility.SaveAsPrefabAsset(energy, "Assets/_Project/Prefabs/Energy.prefab");
        DestroyImmediate(energy);

        // 6. Создаем Врага на сцене
        GameObject enemy = new GameObject("EnemyDummy");
        SpriteRenderer enSr = enemy.AddComponent<SpriteRenderer>();
        enSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        enSr.color = Color.red;
        enemy.transform.position = new Vector3(3, 3, 0);
        enemy.AddComponent<BoxCollider2D>();
        var enemyCtrl = enemy.AddComponent<EnemyController>();
        enemyCtrl.energyPrefab = energyPrefab;

        // 7. Создаем UI (Интерфейс)
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        var uiManager = canvasObj.AddComponent<UIManager>();

        // Текст Энергии
        GameObject energyTextObj = new GameObject("EnergyText");
        energyTextObj.transform.SetParent(canvasObj.transform);
        Text eText = energyTextObj.AddComponent<Text>();
        eText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        eText.text = "ENERGY: 0";
        eText.color = Color.cyan;
        eText.fontSize = 24;
        RectTransform eRect = energyTextObj.GetComponent<RectTransform>();
        eRect.anchorMin = new Vector2(0, 1);
        eRect.anchorMax = new Vector2(0, 1);
        eRect.anchoredPosition = new Vector2(100, -30);

        // Текст Таймера
        GameObject timerTextObj = new GameObject("TimerText");
        timerTextObj.transform.SetParent(canvasObj.transform);
        Text tText = timerTextObj.AddComponent<Text>();
        tText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        tText.text = "TIME: 30";
        tText.color = Color.white;
        tText.fontSize = 24;
        RectTransform tRect = timerTextObj.GetComponent<RectTransform>();
        tRect.anchorMin = new Vector2(0.5f, 1);
        tRect.anchorMax = new Vector2(0.5f, 1);
        tRect.anchoredPosition = new Vector2(0, -30);

        // Экран итогов
        GameObject endScreenObj = new GameObject("EndScreen");
        endScreenObj.transform.SetParent(canvasObj.transform);
        Image bg = endScreenObj.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);
        RectTransform esRect = endScreenObj.GetComponent<RectTransform>();
        esRect.anchorMin = Vector2.zero;
        esRect.anchorMax = Vector2.one;
        esRect.offsetMin = Vector2.zero;
        esRect.offsetMax = Vector2.zero;
        
        GameObject endTextObj = new GameObject("EndText");
        endTextObj.transform.SetParent(endScreenObj.transform);
        Text endText = endTextObj.AddComponent<Text>();
        endText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        endText.text = "ЦИКЛ ЗАВЕРШЕН";
        endText.color = Color.yellow;
        endText.fontSize = 36;
        endText.alignment = TextAnchor.MiddleCenter;
        RectTransform endRect = endTextObj.GetComponent<RectTransform>();
        endRect.anchorMin = Vector2.zero;
        endRect.anchorMax = Vector2.one;
        endRect.offsetMin = Vector2.zero;
        endRect.offsetMax = Vector2.zero;

        endScreenObj.SetActive(false); // Скрываем до конца игры

        // Связываем UI со скриптом
        uiManager.energyText = eText;
        uiManager.timerText = tText;
        uiManager.endScreen = endScreenObj;

        // Ура!
        Debug.Log("Игра успешно сгенерирована! Нажимай Play.");
    }
}