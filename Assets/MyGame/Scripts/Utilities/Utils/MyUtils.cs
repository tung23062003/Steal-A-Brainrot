using System;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using UnityEngine.Events;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

public static class MyUtils
{
    public static int HowMuchDots(float num)
    {
        /* Convert num to string, split it with dot into an array, and take the second cell. Then get the length of the string */
        return num.ToString().Split(".")[1].Length;
    }
    
    public static bool IsContainDots(float num)
    {
        var str = num.ToString();
        if (str.Contains("."))
        {
            var arr = str.Split(".");
            if (int.Parse(arr[1]) == 0)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Converts a string into a SHA-256 Hash Vaue
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string GetSHA256(string text)
    {
        byte[] textToBytes = Encoding.UTF8.GetBytes(text);

        SHA256Managed mySHA256 = new();

        byte[] hashValue = mySHA256.ComputeHash(textToBytes);

        return GetHexStringFromHash(hashValue);
    }

    /// <summary>
    /// Converts an array of bytes into a hexadecimal string 
    /// </summary>
    /// <param name="hashValue"></param>
    /// <returns></returns>
    private static string GetHexStringFromHash(byte[] hashValue)
    {
        string hexString = string.Empty;

        foreach (byte b in hashValue)
        {
            hexString += b.ToString("x2");
        }

        return hexString;
    }

    public static int GetRelativeKeyboardHeight(RectTransform rectTransform, bool includeInput = false)
    {
        int keyboardHeight = GetKeyboardHeight(includeInput);
        float screenToRectRatio = Screen.height / rectTransform.rect.height;
        float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;

        return (int)keyboardHeightRelativeToRect;
    }

    public static int GetKeyboardHeight(bool includeInput = false)
    {
#if UNITY_ANDROID
        using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");
            AndroidJavaObject dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
            if (view == null || dialog == null)
                return 0;
            var decorHeight = 0;
            if (includeInput)
            {
                AndroidJavaObject decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
                if (decorView != null)
                    decorHeight = decorView.Call<int>("getHeight");
            }
            using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return Screen.height - rect.Call<int>("height") + decorHeight;
            }
        }
#elif UNITY_IOS
        return (int)TouchScreenKeyboard.area.height;
#else
        return 0;
#endif
    }

    public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
    {
        return potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
    }

    public static int GetSDKInt()
    {
#if UNITY_EDITOR
        return -1;
#elif UNITY_ANDROID
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
#else
        return -1;
#endif
    }

    public static void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //Logging.Log(child.name);
            child.gameObject.layer = layer;
        }
    }

    public static void ApplyLayout(GameObject createdModel, GameObject modelLayout)
    {
        createdModel.transform.localEulerAngles = modelLayout.transform.localEulerAngles;
        createdModel.transform.localPosition = modelLayout.transform.localPosition;
        createdModel.transform.localScale = modelLayout.transform.localScale;
        SetLayerAllChildren(createdModel.transform, modelLayout.layer);
    }

    public static string GenerateID()
    {
        return Guid.NewGuid().ToString("N");
    }

    public static void ResetUnityEvent<T>()
    {
        // Get all static members of MyClass
        FieldInfo[] staticFields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var field in staticFields)
        {
            var value = field.GetValue(null) as UnityEventBase;
            value?.RemoveAllListeners();
        }
    }

    public static int PointToIndex(Vector2 point, int col)
    {
        return (int)point.y * col + (int)point.x;
    }

    public static Vector2 IndexToPoint(int index, int col)
    {
        return new Vector2(index % col, index / col);
    }

    public static bool IsArrayContainAllElements<T>(T[] source, T[] arrElements)
    {
        foreach (T element in arrElements)
        {
            if (!source.Contains(element))
            {
                return false;
            }
        }
        return true;
    }

    public static void NeverUse(object obj)
    {
    }

    public static string RemoveWhiteSpaceAndBreak(string text)
    {
        return Regex.Replace(text, @"\s", "");
    }

    public static float StringToRoundFloat(string text)
    {
        if (text.Contains('.'))
        {
            text = text.TrimEnd('0').TrimEnd('.');
        }
        return float.Parse(text, CultureInfo.InvariantCulture);
    }

    public static string GetRandomString()
    {
        // Creating object of random class 
        System.Random rand = new();

        // Choosing the size of string using Next() string 
        int stringlen = rand.Next(4, 10);
        int randValue;
        string str = "";
        char letter;
        for (int i = 0; i < stringlen; i++)
        {
            // Generating a random number. 
            randValue = rand.Next(0, 26);

            // Generating random character by converting 
            // the random number into character. 
            letter = Convert.ToChar(randValue + 65);

            // Appending the letter to string. 
            str += letter;
        }
        return str;
    }

    public static int SubAsterisk(string index)
    {
        int petIndex;
        if (index.Contains("*"))
        {
            petIndex = int.Parse(index[0..^1]);
        }
        else
        {
            petIndex = int.Parse(index);
        }

        return petIndex;
    }

    public static int SubK(string index)
    {
        int petIndex;
        if (index.Contains("K"))
        {
            petIndex = int.Parse(index[0..^1]);
        }
        else
        {
            petIndex = int.Parse(index);
        }

        return petIndex;
    }
}
