using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preset
{
    public string presetName;
    Vector3 size;
    float[] reverbProperties;

    public Preset(string n){
        presetName = n;
    }
    public void setSize(float a, float b, float c){
        size = new Vector3(a, b, c);
    }

    public void setMaterials(float[] m){
        reverbProperties = m;
    }

    public void setPreset(GameObject obj){
        obj.GetComponent<ResonanceAudioRoom>().leftWall = (ResonanceAudioRoomManager.SurfaceMaterial)(int)reverbProperties[0];
        obj.GetComponent<ResonanceAudioRoom>().rightWall = (ResonanceAudioRoomManager.SurfaceMaterial)(int)reverbProperties[1];        
        obj.GetComponent<ResonanceAudioRoom>().floor = (ResonanceAudioRoomManager.SurfaceMaterial)(int)reverbProperties[2];
        obj.GetComponent<ResonanceAudioRoom>().ceiling = (ResonanceAudioRoomManager.SurfaceMaterial)(int)reverbProperties[3];
        obj.GetComponent<ResonanceAudioRoom>().frontWall = (ResonanceAudioRoomManager.SurfaceMaterial)(int)reverbProperties[4];
        obj.GetComponent<ResonanceAudioRoom>().backWall = (ResonanceAudioRoomManager.SurfaceMaterial)(int)reverbProperties[5];
        obj.GetComponent<ResonanceAudioRoom>().reflectivity = reverbProperties[6];
        obj.GetComponent<ResonanceAudioRoom>().reverbGainDb = reverbProperties[7];
        obj.GetComponent<ResonanceAudioRoom>().reverbBrightness = reverbProperties[8];
        obj.GetComponent<ResonanceAudioRoom>().reverbTime = reverbProperties[9];
        obj.GetComponentInParent<Transform>().parent.localScale = size;
    }
}
