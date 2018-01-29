using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General utilities class for static functions pertaining to various
/// GameObject types
/// 
/// Author: Tyler Hostager
/// Version: 5/19/17
/// </summary>
public static class ObjUtils {

    #region GAMEOBJECT UTILITIES
    //public static GameObject GetChildComponent(GameObject obj, Object typeClass) {
    //    return obj.GetComponentInChildren <> ();
    //}

    public static List<GameObject> GetAllComponentsInChild<T>(GameObject obj) {
        return new List<GameObject>(obj.GetComponentsInChildren<GameObject>());
    }

    public static List<GameObject> GetAllComponentsInParent<T>(GameObject obj) {
        return new List<GameObject>(obj.GetComponentsInParent<GameObject>());
    }

    public static bool IsNullOrEmpty(UnityEngine.Object obj = default(GameObject)) {
        return obj == null;
    }

    public static bool IsNullOrEmpty(GameObject obj) {
        return obj == null;
    }

    public static bool IsNullOrEmpty(List<UnityEngine.Object> list) {
        return List.IsNullOrEmpty(list);
    }

    public static bool IsNullOrEmpty<T>(List<T> list) {
        return List.IsNullOrEmpty<T>(list);
    }

    public static bool IsNullOrEmpty(List<GameObject> list) {
        return List.IsNullOrEmpty(list);
    }

    public static bool IsNullOrEmpty(UnityEngine.Object[] list) {
        return List.IsNullOrEmpty(list);
    }

    public static bool IsNullOrEmpty(GameObject[] list) {
        return List.IsNullOrEmpty(list);
    }

    public static bool IsNullOrEmpty<T>(T[] list) {
        return List.IsNullOrEmpty<T>(list);
    }

    public static bool IsNullOrEmpty(string str) {
        return String.IsNullOrEmpty(str);
    }
    #endregion

    #region STRING UTILITIES
    public struct String {
        public static bool IsNullOrEmpty(string str) {
            return str == null || str.Equals("");
        }
    }
    #endregion

    #region LIST UTILITIES
    public struct List {
        public static bool IsNullOrEmpty(List<UnityEngine.Object> list) {
            return List.IsNullOrEmpty(list.ToArray());
        }

        public static bool IsNullOrEmpty<T>(List<UnityEngine.Object> list) {
            return List.IsNullOrEmpty<T>(list);
        }

        public static bool IsNullOrEmpty(List<GameObject> list) {
            return List.IsNullOrEmpty<GameObject>(list);
        }

        public static bool IsNullOrEmpty<T>(List<T> list) {
            return List.IsNullOrEmpty<T>(list.ToArray());
        }

        public static bool IsNullOrEmpty<T>(T[] list) {
            return list == null || list.Length == 0;
        }

        public static bool IsNullOrEmpty(UnityEngine.Object[] list = default(GameObject[])) {
            return IsNullOrEmpty<UnityEngine.Object>(list);
        }

        public static bool ContainsObject(List<GameObject> list, GameObject obj) {
            if (obj && !IsNullOrEmpty(list)) {
                return list.Contains(obj);
            }

            return false;
        }

        public static bool ContainsObject(GameObject[] list, GameObject obj) {
            if (obj && !IsNullOrEmpty(list)) {
                return list.Contains(obj);
            }

            return false;
        }

        public static bool ContainsObject(List<GameObject> list, string objectName) {
            if (!IsNullOrEmpty(list) && objectName != null && objectName.Equals("")) {
                return ContainsObject(list, objectName, false);
            }

            return false;
        }

        public static bool ContainsObject(GameObject[] list, string objectName) {
            if (!IsNullOrEmpty(list) && objectName != null && objectName.Equals("")) {
                return ContainsObject(list, objectName, false);
            }

            return false;
        }

        public static bool ContainsObject(List<GameObject> list, string objectName, bool isTag) {
            if (!IsNullOrEmpty(list) && objectName != null && objectName.Equals("")) {
                return ContainsObject(list.ToArray(), objectName, isTag);
            }

            return false;
        }

        public static bool ContainsObject(GameObject[] list, string objectName, bool isTag) {
            if (!IsNullOrEmpty(list) && !String.IsNullOrEmpty(objectName)) {
                return list.FirstOrDefault(
                    (obj) => (isTag ? obj.tag : obj.name).ToLower().Contains(objectName));
            }

            return false;
        }

        public static GameObject ObjectAtIndex(List<GameObject> list, int index) {
            return list[index];
        }

        public static GameObject ObjectAtIndex(GameObject[] list, int index) {
            return list[index];
        }

        public static int SizeOf(List<GameObject> list) {
            if (list != null) {
                return list.Count;
            }

            return 0;
        }

        public static int SizeOf(GameObject[] list) {
            if (list != null) {
                return list.Length;
            }

            return 0;
        }

        public static List<GameObject> AddAll(List<GameObject> rootList, List<GameObject> listToAdd) {
            rootList.AddRange(listToAdd);
            return rootList;
        }

        public static List<GameObject> AddAll(List<GameObject> rootList, GameObject[] listToAdd) {
            rootList.AddRange(listToAdd.ToList());
            return rootList;
        }

        public static List<GameObject> AddAll(GameObject[] rootList, List<GameObject> listToAdd) {
            return AddAll(
                rootList: rootList.ToList(),
                listToAdd: listToAdd
            );
        }

        public static List<GameObject> AddAll(GameObject[] rootList, GameObject[] listToAdd) {
            return AddAll(rootList, listToAdd.ToList());
        }

    }
    #endregion


}
