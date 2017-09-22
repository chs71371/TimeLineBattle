// ===============================================================================
// 简述 : 常用函数
// ===============================================================================
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.Events;
//using MovementEffects;
public class Util : MonoSingleton<Util>
{
 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 清除携程,以免拿住一些对象的引用
    /// </summary>
    public static void Clear()
    {
        Instance.StopAllCoroutines();
    }

    public static String GetReadableByteSize(double size)
    {
        String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
        double mod = 1024.0;
        int i = 0;
        while (size >= mod)
        {
            size /= mod;
            i++;
        }
        return Math.Round(size) + units[i];
    }

    public static void ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, new Vector2(screenPoint.x, screenPoint.y), cam, out localPoint);
    }

    /// <summary>
    /// 修改粒子物体的大小
    /// </summary>
    public static GameObject ChangeParticlesScale(GameObject obj, float scale, Transform parent = null)
    {
        ScaleParticles script = obj.GetComponent<ScaleParticles>();
        if (script == null)
        {
            GameObject scaleObj = new GameObject();
            scaleObj.name = obj.name;
            SetParent(obj, scaleObj);

            script = scaleObj.AddComponent<ScaleParticles>();
            if (parent != null)
            {
                scaleObj.transform.SetParent(parent);
                scaleObj.transform.localPosition = Vector3.zero;
                scaleObj.transform.localEulerAngles = Vector3.zero;
            }
            scaleObj.transform.localScale = new Vector3(scale, scale, scale);
            obj = scaleObj;
        }
        script.ScaleSize = scale;
        script.Run();
        return obj;
    }

    /// <summary>
    /// 延时功能
    /// </summary>
    public static void DelayCall(float rTime, UnityEngine.Events.UnityAction rFunc)
    {
        Util.Instance.StartCoroutine(OnDelayCall(rTime, rFunc));
    }

    /// <summary>
    /// 回调协程
    /// </summary>
    private static IEnumerator  OnDelayCall(float time, UnityEngine.Events.UnityAction rFunc)
    {
        yield return new WaitForSeconds(time);
        if (rFunc != null) rFunc();
    }

 
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string MD5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string MD5String(string str)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        string ret = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)), 4, 8);
        return ret.Replace("-", "");
    }

    /// <summary>
    /// 修正RectTransform
    /// </summary>
    public static void FixInstantiated(Component source, Component instance)
    {
        FixInstantiated(source.gameObject, instance.gameObject);
    }

    /// <summary>
    /// 修正RectTransform
    /// </summary>
    public static void FixInstantiated(GameObject source, GameObject instance)
    {
        var defaultRectTransform = source.GetComponent<RectTransform>();
        var rectTransform = instance.GetComponent<RectTransform>();

        rectTransform.localPosition = defaultRectTransform.localPosition;
        rectTransform.localRotation = defaultRectTransform.localRotation;
        rectTransform.localScale = defaultRectTransform.localScale;
        rectTransform.anchoredPosition = defaultRectTransform.anchoredPosition;
    }

    /// <summary>
    /// 向上搜索Canvas
    /// </summary>
    public static Transform FindCanvas(Transform currentObject)
    {
        var canvas = currentObject.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            return null;
        }
        return canvas.transform;
    }

    /// <summary>
    /// 获取所有包含组件T的子物体
    /// </summary>
    public static Dictionary<string, GameObject> GetAllCom<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            var coms = go.GetComponentsInChildren<T>(true);
            Dictionary<string, GameObject> objs = new Dictionary<string, GameObject>();
            foreach (var com in coms)
            {
                var comObj = com.gameObject;
                if (!objs.ContainsKey(comObj.name))
                {
                    objs.Add(comObj.name, com.gameObject);
                }
            }
            return objs;
        }
        return new Dictionary<string, GameObject>();
    }
    public static List<GameObject> GetAllComList<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            var coms = go.GetComponentsInChildren<T>(true);
            List<GameObject> objs = new List<GameObject>();
            for (int i = 0; i < coms.Length; i++)
            {
                objs.Add(coms[i].gameObject);
            }
            return objs;
        }

        return new List<GameObject>();
    }

    /// <summary>
    /// 在整个Scene中搜索T
    /// </summary>
    public static T GetGameOject<T>(string subnode) where T : UnityEngine.Object
    {
        GameObject sub = GameObject.Find(subnode);
        if (sub != null)
        {
            return sub as T;
        }
        return null;
    }

    /// <summary>
    /// 搜索子物体组件-GameObject版
    /// </summary>
    public static T Get<T>(GameObject go, string subnode) where T : Component
    {
        if (go != null)
        {
            Transform sub = go.transform.Find(subnode);
            if (sub != null) return sub.GetComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 搜索子物体组件-Transform版
    /// </summary>
    public static T Get<T>(Transform go, string subnode) where T : Component
    {
        if (go != null)
        {
            Transform sub = go.Find(subnode);
            if (sub != null) return sub.GetComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 搜索子物体组件-Component版
    /// </summary>
    public static T Get<T>(Component go, string subnode) where T : Component
    {
        return go.transform.Find(subnode).GetComponent<T>();
    }


    public static List<GameObject> FindGameObjectAllChild(GameObject obj)
    {
        List<GameObject> objList = new List<GameObject>();

        List<GameObject> findObjList = new List<GameObject>();

        findObjList.Add(obj);

        while (findObjList.Count > 0)
        {
            for (int i = 0; i < findObjList[0].transform.childCount; i++)
            {
                objList.Add(findObjList[0].transform.GetChild(i).gameObject);
                findObjList.Add(findObjList[0].transform.GetChild(i).gameObject);
            }

            findObjList.RemoveAt(0);
        }


        return objList;
    }



    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(GameObject go, string subnode)
    {
        return Child(go.transform, subnode);
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(Transform go, string subnode)
    {
        Transform tran = go.Find(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    public static GameObject FindChildObj(Transform rParent, string rPath)
    {
        if (rParent.name == rPath)
        {
            return rParent.gameObject;
        }

        var obj = rParent.Find(rPath);

        if (obj != null)
        {
            return obj.gameObject;
        }

        return null;
    }

    /// <summary>
    /// 查找子对象路径
    /// </summary>
    public static void FindChildPath(Transform parent, string name, ref string path, string mPath = null)
    {
        if (parent.name == name)
        {
            path = name;
            return;
        }

        Transform getTrans = parent.Find(name);
        if (getTrans == null)
        {

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform getChild = parent.GetChild(i);
                string childPath = mPath + getChild.name + "/";
                if (getChild.childCount > 0)
                {
                    FindChildPath(getChild, name, ref path, childPath);
                }
            }
        }
        else
        {
            path = mPath + name;
        }
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(GameObject go, string subnode)
    {
        return Peer(go.transform, subnode);
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(Transform go, string subnode)
    {
        Transform tran = go.parent.Find(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 设置物体和子物体的层级
    /// </summary>
    public static void SetGameObjectLayer(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SetGameObjectLayer(obj.transform.GetChild(i).gameObject, layerName);
        }
    }

	/// <summary>
	/// 设置物体和子物体的层级
	/// </summary>
	public static void SetParent(GameObject child,GameObject parent)
	{
        if (parent == null)
        {
            child.transform.parent = null;
        }
        else
        {
            child.transform.SetParent(parent.transform);
        }
		child.transform.localScale = Vector3.one;
		child.transform.localPosition = Vector3.zero;
		child.transform.localEulerAngles =Vector3.zero;
	}

    /// <summary>
    /// 检测是否有触控到UI
    /// </summary>
    /// <returns></returns>
    public static bool CheckTouchUI()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount>0)
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return true;
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }

        return false;
    }

  

 
 

    /// <summary>
    /// 清理内存
    /// </summary>
    public static void ClearMemory()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }


    /// <summary>
    /// 是否为数字
    /// </summary>
    public static bool IsNumber(string strNumber)
    {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }

    public static int Random(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float Random(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static string Uid(string uid)
    {
        int position = uid.LastIndexOf('_');
        return uid.Remove(0, position + 1);
    }
    /// <summary>
    /// 得到Path文件夹下所有文件 并放入allFilePath List中
    /// </summary>
    public static void RecursiveDir(string path, ref List<string> allFilePath, bool isFirstRun = true)
    {

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        if (isFirstRun && allFilePath.Count > 0)
        {
            allFilePath.TrimExcess();
            allFilePath.Clear();
        }

        if (!Directory.Exists(path))
        {
            return;
        }

        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);

        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;

            allFilePath.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            RecursiveDir(dir, ref allFilePath, false);
        }

    }

    #region Local Informations 
    /// <summary>
    /// 生成一个Key名
    /// </summary>
    public static string GetKey(string key)
    {
        return "Player" + "00" + "_" + key;
    }

    /// <summary>
    /// 取得整型
    /// </summary>
    public static int GetInt(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.GetInt(name);
    }

    /// <summary>
    /// 有没有值
    /// </summary>
    public static bool HasKey(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.HasKey(name);
    }

    /// <summary>
    /// 保存整型
    /// </summary>
    public static void SetInt(string key, int value)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetInt(name, value);
    }
    public static void SetBool(string key, bool value)
    {
        int num = value ? 1 : 0;
        SetInt(key, num);
    }
    public static bool GetBool(string key)
    {
        return GetInt(key) == 1;
    }
    /// <summary>
    /// 取得数据
    /// </summary>
    public static string GetString(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.GetString(name);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public static void SetString(string key, string value)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetString(name, value);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public static void RemoveData(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }
    #endregion

    #region Scroll
    public static Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }
    public static Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included

        var pivotOffset = new Vector3(

            (0.5f - target.pivot.x) * target.rect.size.x,

            (0.5f - target.pivot.y) * target.rect.size.y,

            0f);

        var localPosition = target.localPosition + pivotOffset;

        return target.parent.TransformPoint(localPosition);
    }
    #endregion

    /// <summary>
    /// 时间戳转时间_秒
    /// </summary>
    public static DateTime GetTime(UInt32 timeStamp)
    {
        DateTime start = new DateTime(1970, 1, 1, 8, 0, 0);
        return start.AddSeconds(timeStamp);
    }
    public static long GetTime(DateTime time)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        return (long)(time - startTime).TotalSeconds;
    }
    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }
    /// <summary>
    /// 获得该时间所属星期
    /// </summary>
    public static int GetWeek(DateTime nowTime)
    {
        return int.Parse(nowTime.DayOfWeek.ToString("D"));
    }

    /// <summary>
    /// 获取去掉\0的byte数组
    /// </summary>
    public static byte[] BytesExceptDiveZero(byte[] sBytes)
    {
        List<byte> bytes = new List<byte>();
        for (int i = 0; i < sBytes.Length; i++)
        {
            if (sBytes[i] != 0)
            {
                bytes.Add(sBytes[i]);
            }
        }
        return bytes.ToArray();
    }
    /// <summary>
    /// byte数组去掉\0并转成String
    /// </summary>
    public static string GetString(byte[] sBytes)
    {
        return System.Text.Encoding.UTF8.GetString(BytesExceptDiveZero(sBytes));
    }
}