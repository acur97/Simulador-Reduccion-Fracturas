using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
 
 // Screen Recorder will save individual images of active scene in any resolution and of a specific image format
 // including raw, jpg, png, and ppm.  Raw and PPM are the fastest image formats for saving.
 //
 // You can compile these images into a video using ffmpeg:
 // ffmpeg -i screen_3840x2160_%d.ppm -y test.avi
 
    [RequireComponent(typeof(Camera))]
 public class TakePicVideo : MonoBehaviour
{
    public Camera CameraXray;

    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PNG;

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int counter = 0; // image #
    private string ultimaIMG;

    // commands
    private bool captureScreenshot = false;
    private bool captureVideo = false;

	//public RawImage m_RawImage;

    [Space]
    public Text countTomas;
    public Text countPic;
    public GameObject prefabRadiografias;
    public Transform content;

    private RawImage[] radiografiaImg;
    private Text[] radiografiaTex;
    private Text countRadiografia;
    private int counter2 = 0;
    private int counter3 = 0;

    private void Awake()
    {
        countTomas.text = counter3.ToString("000");
        countPic.text = counter3.ToString("000");
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void TomarRadiografia()
    {
        GameObject radiografiaPrefab = Instantiate(prefabRadiografias, content);
        radiografiaPrefab.transform.SetAsFirstSibling();
        radiografiaImg = radiografiaPrefab.GetComponentsInChildren<RawImage>();
        radiografiaImg[0].texture = null;
        counter2 += 1;
        counter3 += 1;
        countTomas.text = counter3.ToString("000");
        countPic.text = counter3.ToString("000");
        TakePic();
        StartCoroutine(DelayMostrarRadiografia());
    }

    private IEnumerator DelayMostrarRadiografia()
    {
        yield return new WaitForSeconds(1);
        showImage();
    }

    //agregar que el boton borrar solo este activo si hay que borrar
    /*public void BorrarUltimaRadiografia()
    {
        Transform[] hijos = content.GetComponentsInChildren<Transform>();
        if (hijos.Length > 1)
        {
            counter2 -= 1;
            Destroy(hijos[1].gameObject);
        }
    }*/

    // create a unique filename using a one-up variable
    private string uniqueFilename(int width, int height)
    {
        // if folder not specified by now use a good default
        if (folder == null || folder.Length == 0)
        {
            folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = folder + "/..";
                folder = Path.GetFullPath(stringPath);
            }
            folder += "/screenshots";

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(folder);

            // count number of files of specified format in folder
            string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
            counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
        }

        // use width, height, and counter for unique file name
        var tiempo = System.DateTime.Now;
      //var filename = string.Format("{0}\\screen_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());
        var filename = string.Format("{0}\\Xray_F({1}-{2}-{3})_T({4}-{5}-{6})_{7}.{8}", folder, tiempo.Year, tiempo.Month.ToString("00"), tiempo.Day.ToString("00"), tiempo.Hour.ToString("00"), tiempo.Minute.ToString("00"), tiempo.Second.ToString("00"), counter, format.ToString().ToLower());

        ultimaIMG = filename;

        // up counter for next call
        ++counter;

        // return unique filename
        return filename;
    }

    public void CaptureScreenshot()
    {
        captureScreenshot = true;
    }

    public void TakePic()
    {
        // check keyboard 'k' for one time screenshot capture and holding down 'v' for continious screenshots
        //captureScreenshot |= Input.GetKeyDown("k");
        //captureVideo = Input.GetKey("v");

        //if (captureScreenshot || captureVideo)
        //{
            captureScreenshot = false;

            // hide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(false);

            // create screenshot objects if needed
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }

            // get main camera and manually render scene into rt
            //Camera camera = GetComponent<Camera>(); // NOTE: added because there was no reference to camera in original script; must add this script to Camera
            Camera camera = CameraXray;
            camera.targetTexture = renderTexture;
            camera.Render();

            // read pixels will read from the currently active render texture so make our offscreen 
            // render texture active and then read the pixels
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            // reset active camera texture and render texture
            camera.targetTexture = null;
            RenderTexture.active = null;

            // get our unique filename
            string filename = uniqueFilename((int)rect.width, (int)rect.height);
			//string filename = ""; 
			//print(filename);
            // pull in our file header/data bytes for the specified image format (has to be done from main thread)
            byte[] fileHeader = null;
            byte[] fileData = null;
            if (format == Format.RAW)
            {
                fileData = screenShot.GetRawTextureData();
            }
            else if (format == Format.JPG)
            {
                fileData = screenShot.EncodeToJPG();
            }
            //else(format == Format.PNG)
            else
            {
                fileData = screenShot.EncodeToPNG();
            }
            /*else // ppm
            {
                // create a file header for ppm formatted file
                string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                fileData = screenShot.GetRawTextureData();
            }*/

			//print ("Estamos ahi pasando por print");
            // create new thread to save the image to file (only operation that can be done in background)
            new System.Threading.Thread(() =>
            {
                // create file and write optional header with image bytes
                var f = System.IO.File.Create(filename);
                if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                f.Write(fileData, 0, fileData.Length);
                f.Close();
                Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
            }).Start();

            // unhide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(true);

            // cleanup if needed
            if (optimizeForManyScreenshots == false)
            {
                Destroy(renderTexture);
                renderTexture = null;
                screenShot = null;
            }
        //}
    }

    public void showImage()
    {
        Texture2D tex = null;
        byte[] fileData;
        //string filePath = "C:\\Users\\LabRV\\Documents\\LaboratorioRV\\Reduction_Fracture_Simulator\\TestingCArmCameraMovementV2\\Assets\\screen_1920x1080_2.PNG";
        string filePath = ultimaIMG;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            //print("Everything is ok righ now");

            Texture tex2 = (Texture)tex;
            radiografiaImg[0].texture = tex2;
            //m_RawImage.texture = tex2;
            //m_RawImage.IsActive ();
            //m_RawImage.enabled = !m_RawImage.enabled;
        }
    }

    void Update()
    {
        // check keyboard 'k' for one time screenshot capture and holding down 'v' for continious screenshots
        /*captureScreenshot |= Input.GetKeyDown("k");
        captureVideo = Input.GetKey("v");

        if (captureScreenshot || captureVideo)
        {
            captureScreenshot = false;

            // hide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(false);

            // create screenshot objects if needed
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }

            // get main camera and manually render scene into rt
            Camera camera = this.GetComponent<Camera>(); // NOTE: added because there was no reference to camera in original script; must add this script to Camera
            camera.targetTexture = renderTexture;
            camera.Render();

            // read pixels will read from the currently active render texture so make our offscreen 
            // render texture active and then read the pixels
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            // reset active camera texture and render texture
            camera.targetTexture = null;
            RenderTexture.active = null;

            // get our unique filename
            string filename = uniqueFilename((int)rect.width, (int)rect.height);

            // pull in our file header/data bytes for the specified image format (has to be done from main thread)
            byte[] fileHeader = null;
            byte[] fileData = null;
            if (format == Format.RAW)
            {
                fileData = screenShot.GetRawTextureData();
            }
            else if (format == Format.PNG)
            {
                fileData = screenShot.EncodeToPNG();
            }
            else if (format == Format.JPG)
            {
                fileData = screenShot.EncodeToJPG();
            }
            else // ppm
            {
                // create a file header for ppm formatted file
                string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                fileData = screenShot.GetRawTextureData();
            }

            // create new thread to save the image to file (only operation that can be done in background)
            new System.Threading.Thread(() =>
            {
                // create file and write optional header with image bytes
                var f = System.IO.File.Create(filename);
                if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                f.Write(fileData, 0, fileData.Length);
                f.Close();
                Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
            }).Start();

            // unhide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(true);

            // cleanup if needed
            if (optimizeForManyScreenshots == false)
            {
                Destroy(renderTexture);
                renderTexture = null;
                screenShot = null;
            }
        }*/
    }
}



