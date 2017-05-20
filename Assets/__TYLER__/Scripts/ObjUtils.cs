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
        var children = obj.GetComponentsInChildren<GameObject>();
        List<GameObject> objArr = null;

        if (children != null) {
            objArr = new List<GameObject>(List.SizeOf(children));

            foreach (var child in children) {
                if (child) {
                    objArr.Add(child);
                }
            }
        }

        return objArr;
    }

    public static List<GameObject> GetAllComponentsInParent<T>(GameObject obj) {
        var parents = obj.GetComponentsInParent<GameObject>();
        List<GameObject> objArr = null;

        if (parents != null) {
            objArr = new List<GameObject>(List.SizeOf(parents));

            foreach (var parent in parents) {
                if (parent) {
                    objArr.Add(parent);
                }
            }
        }

        return objArr;
    }
    #endregion

    #region LIST UTILITIES
    public struct List {
        public static bool IsNullOrEmpty(List<GameObject> list) {
            return list == null || list.Count == 0;
        }

        public static bool IsNullOrEmpty(GameObject[] list) {
            return list == null || list.Length == 0;
        }

        public static bool ContainsObject(List<GameObject> list, GameObject obj) {
            if (obj && !IsNullOrEmpty(list)) {
                return list.Contains(obj);
            }

            return false;
        }

        public static bool ContainsObject(GameObject[] list, GameObject obj) {
            if (obj && !IsNullOrEmpty(list)) {
                foreach (var gameObject in list) {
                    if (gameObject.Equals(obj)) {
                        return true;
                    }
                }
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
            if (!IsNullOrEmpty(list) && objectName != null && objectName.Equals("")) {
                foreach (var gameObject in list) {
                    if (isTag) {
                        if (gameObject.tag.ToLower().Contains(objectName.ToLower())) {
                            return true;
                        }
                    } else {
                        if (gameObject.name.ToLower().Contains(objectName.ToLower())) {
                            return true;
                        }
                    }
                }
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
            if (listToAdd != null && listToAdd.Count > 0) {
                rootList = new List<GameObject>(listToAdd.Count);
                foreach (var listItem in listToAdd) {
                    if (listItem != null) {
                        rootList.Add(listItem);
                    }
                }
            }

            return rootList;
        }

        public static List<GameObject> AddAll(List<GameObject> rootList, GameObject[] listToAdd) {
            if (listToAdd != null && listToAdd.Length > 0) {
                rootList = new List<GameObject>(listToAdd.Length);
                foreach (var listItem in listToAdd) {
                    if (listItem != null) {
                        rootList.Add(listItem);
                    }
                }
            }

            return rootList;
        }

        public static List<GameObject> AddAll(GameObject[] rootList, List<GameObject> listToAdd) {
            List<GameObject> tmpList = null; ;
            if (listToAdd != null && listToAdd.Count > 0) {
                tmpList = new List<GameObject>(listToAdd.Count);
                foreach (var listItem in listToAdd) {
                    if (listItem != null) {
                        tmpList.Add(listItem);
                    }
                }
            }

            return tmpList;
        }

        public static List<GameObject> AddAll(GameObject[] rootList, GameObject[] listToAdd) {
            List<GameObject> tmpList = null; ;
            if (listToAdd != null && listToAdd.Length > 0) {
                tmpList = new List<GameObject>(listToAdd.Length);
                foreach (var listItem in listToAdd) {
                    if (listItem != null) {
                        tmpList.Add(listItem);
                    }
                }
            }

            return tmpList;
        }

    }
    #endregion



}
