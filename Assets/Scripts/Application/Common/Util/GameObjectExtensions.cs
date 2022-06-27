/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author Ha Sungmin <sm.ha@biscuitlabs.io>
* @created 2022/06/27
* @desc GameObjectExtensions
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class GameObjectExtensions {
    public static GameObject FirstDescendant(this MonoBehaviour v, string name) {
        return GetGameObject(v.gameObject, name);
    }

    public static GameObject FirstDescendant(this GameObject v, string name) {
        return GetGameObject(v, name);
    }

    private static GameObject GetGameObject(this GameObject v, string name) {
        Queue<Transform> list = new Queue<Transform>();

        for (int i=0; i<v.transform.childCount; i++) {
            list.Enqueue(v.transform.GetChild(i));
        }

        while (list.Count > 0) {
            Transform trans = list.Dequeue();

            if (trans.gameObject.name == name){
                return trans.gameObject;
            }
            else {
                for (int j=0;j<trans.childCount;j++) {
                    list.Enqueue(trans.GetChild(j));
                }
            }
        }

        return null;
    }
    
    public static UnityWebRequest Post(this MonoBehaviour v, string url, string body) {
        var www = UnityWebRequest.Post(url, "");
        www.timeout = 10;
        www.SetRequestHeader("Content-Type", "application/json");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
        return www;
    }

    public static UnityWebRequest Get(this MonoBehaviour v, string url, params object[] values) {
        var www = UnityWebRequest.Get(string.Format(url, values));
        www.timeout = 10;
        www.SetRequestHeader("Content-Type", "application/json");
        return www;
    }    
    
    public static bool IsSuccess(this UnityWebRequest www) {
        var code = (int)www.responseCode;
        return (code / 100) == 2 || code == 304;
    }    
    
    public static T FirstDescendantComponent<T>(this MonoBehaviour v, string name) where T : Component {
        GameObject obj = v.FirstDescendant(name);
        if (obj == null) {
            return null;
        }
     
        return obj.GetComponent<T>();
    }

    public static T FirstDescendantComponent<T>(this GameObject v, string name) where T : Component {
        GameObject obj = v.FirstDescendant(name);
        if (obj == null) {
            return null;
        }

        return obj.GetComponent<T>();
    }

    public static T GetGameObjectComponent<T>(this MonoBehaviour v, string name) where T : Component {
        GameObject obj = GameObject.Find(name);
        if (obj == null) {
            return null;
        }
     
        return obj.GetComponent<T>();
    }

    public static T InstantiateUI<T>(this MonoBehaviour v, string path, GameObject parent) where T : Component {
        path = "Prefabs/UI/" + path;
        GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
        if (parent != null) {
            obj.transform.SetParent(parent.transform);
        }
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<T>();
    }
    
    public static T InstantiateUI<T>(this MonoBehaviour v, GameObject prefab, GameObject parent) where T : Component {
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        if (parent != null) {
            obj.transform.SetParent(parent.transform);
        }
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<T>();
    }    

    public static T Instantiate<T>(this MonoBehaviour v, string path, GameObject parent) where T : Component {
        path = "Prefabs/" + path;
        GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
        if (parent != null) {
            obj.transform.SetParent(parent.transform);
        }
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<T>();
    }

    public static void ClearChildren(this MonoBehaviour v) {
        for (int i=0; i<v.transform.childCount; i++) {
            Object.Destroy(v.transform.GetChild(i).gameObject);
        }
    }    

    public static void ClearChildren(this GameObject v) {
        for (int i=0; i<v.transform.childCount; i++) {
            Object.Destroy(v.transform.GetChild(i).gameObject);
        }
    }    

    public static Color HexToColor(this Color v, string hex) {
        byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r,g,b, 255);
    }

    public static bool HasTouch(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount >= 1;
        }

        return Input.GetMouseButton(0);
    }

    public static bool HasTouch1(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount == 1;
        }

        return Input.GetMouseButton(0);
    }

    public static bool HasTouch2(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount == 2;
        }

        return false;
    }

    public static bool HasTouch1Began(this MonoBehaviour v) {
#if !UNITY_EDITOR
        if ( Application.platform == RuntimePlatform.IPhonePlayer || 
             Application.platform == RuntimePlatform.Android) {
            return Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
#endif
        return Input.GetMouseButtonDown(0);
    }

    public static bool HasTouch1Doing(this MonoBehaviour v) {
#if !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount == 1 && 
                (Input.GetTouch(0).phase == TouchPhase.Moved || 
                Input.GetTouch(0).phase == TouchPhase.Stationary);
        }
#endif

        return Input.GetMouseButton(0);
    }

    public static Vector3 GetTouchPosition1(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.GetTouch(0).position;
        }

        return Input.mousePosition;
    }

#if BOOT_NGUI_SUPPORT
    public static Vector3 GetUITouchPosition(this UIRoot root, Vector3 pt) {
        float scale = root.GetScreenUIScale();
        float width = Screen.width * scale;
        float height = Screen.height * scale;

        Vector3 res = pt * scale;
        res.x -= width * 0.5f;
        res.y -= height * 0.5f;
        return res;
    }    

    public static Vector3 GetUITouchPosition1(this UIRoot root) {
        float scale = root.GetScreenUIScale();
        float width = Screen.width * scale;
        float height = Screen.height * scale;

        Vector3 pt = root.GetTouchPosition1() * scale;
        pt.x -= width * 0.5f;
        pt.y -= height * 0.5f;
        return pt;
    }    

    public static float GetUIWidth(this UIRoot root) {
        float scale = root.GetScreenUIScale();
        return Screen.width * scale;
    }

    public static float GetUIHeight(this UIRoot root) {
        float scale = root.GetScreenUIScale();
        return Screen.height * scale;
    }

    public static float GetScreenUIScale(this UIRoot root) {
        if (root.fitWidth) {
            return root.manualWidth / (float)Screen.width;
        }
        else {
            return root.manualHeight / (float)Screen.height;
        }
    }

    public static Vector3 ScreenToUIPosition(this UIRoot v, Vector3 pt) {
        float screenScale = 1f;
        if (v.fitWidth) {
            screenScale = v.manualWidth / (float)Screen.width;
        }
        else {
            screenScale = v.manualHeight / (float)Screen.height;
        }

        Vector3 position = pt;
        position.x -= Screen.width / 2f;
        position.y -= Screen.height / 2f;
        position.x *= screenScale;
        position.y *= screenScale;
        position.z = 0;
        return position;
    }

    public static void SetDepthRecursive(this UIPanel v, int depth) {
        int offset = depth - v.depth;

        UIPanel []panels = v.GetComponentsInChildren<UIPanel>();
        foreach (UIPanel panel in panels) {
            panel.depth += offset;
        }
    }    
#endif

    public static void StopCoroutineSafe(this MonoBehaviour v, Coroutine coroutine) {
        if (coroutine != null) {
            v.StopCoroutine(coroutine);
        }
    }

    public static string Indent(this string v, int indent) {
        if (indent > 0) {
            string space = "  ";
            for (int i=0; i<indent; i++) {
                space += "  ";
            }

            v = space + v.Replace("\n", "\n" + space);
        }
        return v;
    }

    public static void ChangeLayers(this GameObject go, string name) {
        ChangeLayers(go, LayerMask.NameToLayer(name));
    }

    public static void ChangeLayers(this GameObject go, int layer) {
        go.layer = layer;
        foreach (Transform child in go.transform) {
            ChangeLayers(child.gameObject, layer);
        }
    }
    
    public static void Shuffle<T>(this IList<T> list) {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            
            int k = Random.Range(0, n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
    
    public static void SetActive(this MonoBehaviour go, Component component, bool active) {
        if (component != null) {
            component.gameObject.SetActive(active);
        }
    }

    public static void SetActive(this MonoBehaviour go, GameObject target, bool active) {
        if (target != null) {
            target.SetActive(active);
        }
    }    

    public static void SetText(this MonoBehaviour go, Text text, string value) {
        if (text != null) {
            text.text = value;
        }
    }
    
    public static async void RunNextFrame(this MonoBehaviour go, System.Action action) {
        //await new WaitForEndOfFrame();
        action();
    }

    public static Vector2 ToLocalPosition(this RectTransform rt, Vector2 worldPos) {
        Vector2 localPos;
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenP, null, out localPos);
        return localPos;
    }

    public static Vector2 ScreenToLocalPosition(this RectTransform rt, Vector2 screenPos) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPos, Camera.main, out Vector2 pos);
        return pos;
    }

    public static void SetSprite(this RawImage image, string path) {
        var bytes = PersistenceUtil.ReadBinaryResource(path);
		var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        if (texture.LoadImage(bytes)) {
            texture.wrapMode = TextureWrapMode.Clamp;
            image.texture = texture;
        }
    }

    public static void CopyToClipboard(this string str) {
        var textEditor = new TextEditor();
        textEditor.text = str;
        textEditor.SelectAll();
        textEditor.Copy();
    }
}