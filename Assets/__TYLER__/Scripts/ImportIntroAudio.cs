using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportIntroAudio : MonoBehaviour {

    private AudioSource AudioSource;

    private void Awake() {

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {


    }

    private void OnDestroy() {
        //Log.d("Finding Object \'Intro Music\'...");
        //if (GameObject.FindWithTag("IntroMusic")) { //IntroMusicSingleton.Instance.AudioSource ?? new AudioSource();
        //this.AudioSource = //GameObject.FindWithTag("IntroMusic");
        //    GameObject musicObj = GameObject.FindWithTag("IntroMusic");

        //    if (musicObj) {
        //        Log.d("Object found");
        //    } else {
        //        Log.w("Object not found. Skipping procedure.");
        //        return;
        //    }

        //    Log.d("Locating \'AudioSource\' object...");
        //    if (musicObj.GetComponent<AudioSource>()) {
        //        Log.d("\'AudioSource\' found");
        //        Log.d("Stopping music...");
        //        musicObj.GetComponent<AudioSource>().Stop();
        //        Log.d("Music stopped");
        //    }
        //}

        //Log.d("Destroying music object(s)...");
        //List<GameObject> musicObjects;
        //var numMusicObjs = 0;
        //var objects = GameObject.FindWithTag("IntroMusic");
        //foreach (var obj in objects) {
        //    if (obj.tag.Equals("IntroMusic")) {
        //Log.d("Music object detected");
        //numMusicObjs++;
        //    }
        //}

        //if (numMusicObjs > 0) {
        //    musicObjects = new List<GameObject>(numMusicObjs);
        //    foreach (var mObj in musicObjects) {
        //        Log.d("Destroying music object \'" + mObj.name + "\'...");
        //        Destroy(mObj);
        //        Log.d("Object destroyed");
        //    }
        //}
    }
}
