using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSensor : MonoBehaviour
{

    public int resWidth = 2550;
    public int resHeight = 3300;
    public GameObject drone;
    private Vector3 offset;

    private bool takeHiResShot = false;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - drone.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

   

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
        transform.position = drone.transform.position + offset;
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
        }

    }
}
