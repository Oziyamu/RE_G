using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AutoSetup : EditorWindow
{
    [MenuItem("Tools/Сгенерировать игру (Magic Button)")]
    public static void SetupGame()
    {
        GameObject gmObj = new GameObject("_GameManager");
        gmObj.AddComponent<GameManager>();

        GameObject camObj = new GameObject("Main Camera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 3.5f;
        cam.backgroundColor = new Color(0.1f, 0.1f, 0.15f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        camObj.tag = "MainCamera";
        var camFollow = camObj.AddComponent<CameraFollow>();
        camObj.transform.position = new Vector3(0, 0, -10);

        GameObject player = new GameObject("PlayerPrinter");
        player.tag = "Player";
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        sr.color = Color.white;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        player.AddComponent<BoxCollider2D>();
        
        var printerCtrl = player.AddComponent<PrinterController>();
        var weaponSys = player.AddComponent<WeaponSystem>();
        
        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.SetParent(player.transform);
        firePoint.transform.localPosition = new Vector3(0, 0.8f, 0);
        weaponSys.firePoint = firePoint.transform;
        camFollow.target = player.transform;

        GameObject bullet = new GameObject("BulletPrefab");
        SpriteRenderer bSr = bullet.AddComponent<SpriteRenderer>();
        bSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        bSr.color = Color.yellow;
        bullet.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        Rigidbody2D bRb = bullet.AddComponent<Rigidbody2D>();
        bRb.bodyType = RigidbodyType2D.Kinematic;
        bullet.AddComponent<CircleCollider2D>().isTrigger = true;
        bullet.AddComponent<BulletController>();
        GameObject bulletPrefab = PrefabUtility.SaveAsPrefabAsset(bullet, "Assets/_Project/Prefabs/Bullet.prefab");
        DestroyImmediate(bullet);
        weaponSys.bulletPrefab = bulletPrefab;

        GameObject energy = new GameObject("EnergyPrefab");
        SpriteRenderer eSr = energy.AddComponent<SpriteRenderer>();
        eSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        eSr.color = Color.green;
        energy.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        energy.AddComponent<CircleCollider2D>().isTrigger = true;
        energy.AddComponent<EnergyPickup>();
        GameObject energyPrefab = PrefabUtility.SaveAsPrefabAsset(energy, "Assets/_Project/Prefabs/Energy.prefab");
        DestroyImmediate(energy);

        // СПАВН 3 ВРАГОВ (Добавлен Rigidbody2D!)
        for (int i = 0; i < 3; i++)
        {
            GameObject enemy = new GameObject("EnemyDummy_" + i);
            enemy.tag = "Enemy";
            SpriteRenderer enSr = enemy.AddComponent<SpriteRenderer>();
            enSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            enSr.color = Color.red;
            enemy.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            
            // ДОБАВЛЯЕМ ФИЗИКУ ВРАГУ
            Rigidbody2D eRb = enemy.AddComponent<Rigidbody2D>();
            eRb.bodyType = RigidbodyType2D.Dynamic;
            eRb.gravityScale = 0f;
            eRb.freezeRotation = true;

            float angle = i * Mathf.PI * 2f / 3f;
            enemy.transform.position = new Vector3(Mathf.Cos(angle) * 5f, Mathf.Sin(angle) * 5f, 0);
            enemy.AddComponent<BoxCollider2D>();
            var enemyCtrl = enemy.AddComponent<EnemyController>();
            enemyCtrl.energyPrefab = energyPrefab;
        }

        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();
        var uiManager = canvasObj.AddComponent<UIManager>();

        GameObject energyTextObj = new GameObject("EnergyText");
        energyTextObj.transform.SetParent(canvasObj.transform);
        Text eText = energyTextObj.AddComponent<Text>();
        eText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        eText.text = "ENERGY: 0";
        eText.color = Color.cyan;
        eText.fontSize = 30;
        eText.alignment = TextAnchor.MiddleLeft;
        RectTransform eRect = energyTextObj.GetComponent<RectTransform>();
        eRect.anchorMin = new Vector2(0, 1);
        eRect.anchorMax = new Vector2(0, 1);
        eRect.pivot = new Vector2(0, 1);
        eRect.anchoredPosition = new Vector2(20, -20);
        eRect.sizeDelta = new Vector2(300, 50);

        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.SetParent(canvasObj.transform);
        Text hText = healthTextObj.AddComponent<Text>();
        hText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        hText.text = "HP: 100";
        hText.color = Color.red;
        hText.fontSize = 30;
        hText.alignment = TextAnchor.MiddleLeft;
        RectTransform hRect = healthTextObj.GetComponent<RectTransform>();
        hRect.anchorMin = new Vector2(0, 1);
        hRect.anchorMax = new Vector2(0, 1);
        hRect.pivot = new Vector2(0, 1);
        hRect.anchoredPosition = new Vector2(20, -60);
        hRect.sizeDelta = new Vector2(300, 50);

        GameObject timerTextObj = new GameObject("TimerText");
        timerTextObj.transform.SetParent(canvasObj.transform);
        Text tText = timerTextObj.AddComponent<Text>();
        tText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        tText.text = "TIME: 30";
        tText.color = Color.white;
        tText.fontSize = 36;
        tText.alignment = TextAnchor.MiddleCenter;
        RectTransform tRect = timerTextObj.GetComponent<RectTransform>();
        tRect.anchorMin = new Vector2(0.5f, 1);
        tRect.anchorMax = new Vector2(0.5f, 1);
        tRect.pivot = new Vector2(0.5f, 1);
        tRect.anchoredPosition = new Vector2(0, -20);
        tRect.sizeDelta = new Vector2(200, 50);

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
        endText.fontSize = 72;
        endText.alignment = TextAnchor.MiddleCenter;
        RectTransform endRect = endTextObj.GetComponent<RectTransform>();
        endRect.anchorMin = Vector2.zero;
        endRect.anchorMax = Vector2.one;
        endRect.offsetMin = Vector2.zero;
        endRect.offsetMax = Vector2.zero;
        endScreenObj.SetActive(false);

        uiManager.energyText = eText;
        uiManager.timerText = tText;
        uiManager.healthText = hText;
        uiManager.endScreen = endScreenObj;

        GameObject cockpitPanel = new GameObject("CockpitPanel");
        cockpitPanel.transform.SetParent(canvasObj.transform);
        Image cockpitBg = cockpitPanel.AddComponent<Image>();
        cockpitBg.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);
        RectTransform cpRect = cockpitPanel.GetComponent<RectTransform>();
        cpRect.anchorMin = new Vector2(0, 0);
        cpRect.anchorMax = new Vector2(0, 0);
        cpRect.pivot = new Vector2(0, 0);
        cpRect.anchoredPosition = new Vector2(20, 20);
        cpRect.sizeDelta = new Vector2(500, 300);

        GameObject breech = new GameObject("Breech");
        breech.transform.SetParent(cockpitPanel.transform);
        Image breechImg = breech.AddComponent<Image>();
        breechImg.color = new Color(0.3f, 0.3f, 0.3f);
        RectTransform bRect = breech.GetComponent<RectTransform>();
        bRect.anchorMin = new Vector2(0.5f, 0.5f);
        bRect.anchorMax = new Vector2(0.5f, 0.5f);
        bRect.anchoredPosition = new Vector2(0, 30);
        bRect.sizeDelta = new Vector2(200, 80);

        GameObject lever = new GameObject("Lever");
        lever.transform.SetParent(cockpitPanel.transform);
        Image leverImg = lever.AddComponent<Image>();
        leverImg.color = Color.red;
        RectTransform lRect = lever.GetComponent<RectTransform>();
        lRect.anchorMin = new Vector2(0.8f, 0.5f);
        lRect.anchorMax = new Vector2(0.8f, 0.5f);
        lRect.anchoredPosition = new Vector2(0, 0);
        lRect.sizeDelta = new Vector2(40, 150);

        var cockpitScript = cockpitPanel.AddComponent<CockpitUIManager>();
        cockpitScript.leverTransform = lRect;

        Debug.Log("Игра v4 (Physics Fix) сгенерирована!");
    }
}