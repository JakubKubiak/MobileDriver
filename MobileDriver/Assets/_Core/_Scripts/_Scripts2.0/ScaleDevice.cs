using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScaleDevice : MonoBehaviour
{

    public float resoulutionFactor;
    float _height = Screen.height;
    float _weight = Screen.width;
    float scaleFactorW;
    float scaleFactorH;
    void Awake()
    {

        float _height = Screen.height;
        float _weight = Screen.width;
        float scaleFactorW = _weight / 1024; //1136
        float scaleFactorH = _height / 768;
        resoulutionFactor = scaleFactorW / scaleFactorH;

#if UNITY_IOS
 
     if(iPhone.generation == iPhoneGeneration.iPhone4 || iPhone.generation == iPhoneGeneration.iPhone4S || iPhone.generation == iPhoneGeneration.iPhone5S || iPhone.generation == iPhoneGeneration.iPhone5 || iPhone.generation == iPhoneGeneration.iPhone6 || iPhone.generation == iPhoneGeneration.iPhone6S) 
         {
         if (scaleFactorW > scaleFactorH) {                        
             this.transform.localScale = new Vector3 (scaleFactorW / scaleFactorH, 1, 1);
         }
     }
#endif
#if UNITY_ANDROID || UNITY_EDITOR
 
         if (scaleFactorW > scaleFactorH) {                        
         this.transform.localScale = new Vector3 (scaleFactorW / scaleFactorH, 1, scaleFactorW / scaleFactorH);
         }
#endif
    }
}