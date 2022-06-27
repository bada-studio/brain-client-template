/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author Ha Sungmin <sm.ha@biscuitlabs.io>
* @created 2022/06/27
* @desc PersistenceUtil
*/
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class PersistenceUtil {
    public static string LoadTextFile(string path) {
#if !UNITY_WEBGL        
        try {
            StreamReader sr = new StreamReader(path);
            string text = sr.ReadToEnd();
            sr.Close();
            
            return text;
        }
        catch (Exception e) {
            //Debug.Log(e.Message);
            return "";
        }
#else    
        if (path.IndexOf(Application.persistentDataPath) == 0) {
            path = path.Substring(Application.persistentDataPath.Length + 1);
        }
        
        path = path.Replace("/", "_");
        return PlayerPrefs.GetString(path);
#endif
    }
    
    public static string LoadTextResource(string path) {
        TextAsset textAsset = (TextAsset) Resources.Load(path, typeof(TextAsset));
        
        if (textAsset == null) {
            Debug.Log("Can not load text resource : " + path);
            return "";
        }
        
        return textAsset.text;
    }

    public static bool SaveTextFile(string path, string text) {
#if !UNITY_WEBGL
        try {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(text);
            sw.Close();
            
            //Debug.Log("File save complete: "  + path);
            return true;
        }
        catch (Exception e) {
            Debug.LogError(e.Message);
            return false;
        }
#else
        if (path.IndexOf(Application.persistentDataPath) == 0) {
            path = path.Substring(Application.persistentDataPath.Length + 1);
        }

        path = path.Replace("/", "_");
        PlayerPrefs.SetString(path, text);
        return true;
#endif
    }
    
    public static bool DeleteFile(string path) {
#if !UNITY_WEBGL
        try {
            File.Delete(path);
            return true;
        } catch (Exception e) {
            Debug.LogError(e.Message);
            return false;
        }
#endif
        return true;
    }

    public static byte[] ReadBinaryResource(string path) {
        TextAsset textAsset = (TextAsset) Resources.Load(path, typeof(TextAsset));
        if (textAsset == null) {
            Debug.Log("Can not load binary resource : " + path);
            return null;
        }

        return textAsset.bytes;
    }

    public static byte[] ReadBinaryFile(string path) {
        try {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (fs == null) {
                return null;
            }

            BinaryReader reader = new BinaryReader(fs);
            byte[] result = reader.ReadBytes((int)reader.BaseStream.Length);
            reader.Close();
            fs.Close();

            return result;
        } catch (Exception e) {
            Debug.Log(e.Message);
            return null;
        }
    }

    public static bool WriteBinaryFile(string path, byte[] data) {
        try {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            if (fs == null) {
                return false;
            }

            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(data);
            writer.Close();
            fs.Close();
            return true;
        } catch (Exception e) {
            Debug.LogError(e.Message);
            return false;
        }
    }

    public static void CreateFolder(string path) {
#if !UNITY_WEBGL
        DirectoryInfo di = new DirectoryInfo(path);
        if (di.Exists == false) {
            di.Create();
        }
#endif
    }

    public static void DeleteFolder(string path) {
#if !UNITY_WEBGL
        DirectoryInfo di = new DirectoryInfo(path);
        try {
            if (di.Exists == true) {
                di.Delete(true);
            }
        }
        catch (Exception e) {
            Debug.LogError(e.ToString());
        }
#endif    
    }
    
    public static List<string> GetFileList(string folderPath, string extName, bool includeExt) {
        List<string> fileList = new List<string>();
#if !UNITY_WEBGL
        DirectoryInfo di = new DirectoryInfo(folderPath);
        try {
            if (di.Exists == true) {
                var list = di.GetFiles();
                for (int i=0; i < list.Length; i++) {
                    var fileInfo = list[i];
                    var fileName = fileInfo.Name;
                    if (fileInfo.Extension.Equals(extName)) {
                        if (!includeExt && !string.IsNullOrEmpty(fileInfo.Extension)) {
                            fileName = fileName.Replace(fileInfo.Extension, "");
                        }
                        
                        fileList.Add(fileName);
                    }
                }
            }
        }
        catch (Exception e) {
            Debug.LogError(e.ToString());
        }
#endif
        return fileList;
    }
}