using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShotController : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScreenShot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,false);

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();


        String fileName = "ScreenShoot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";


        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //PC 
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../" + fileName,bytes);
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            //Mobile
            NativeGallery.SaveImageToGallery(texture, "AllPrimum_pictures", fileName);
        }


        Destroy(texture);
        UI.SetActive(true);
    }

    public void TakeScreenShot()
    {
        StartCoroutine(ScreenShot());
        UI.SetActive(false);
    }
}
