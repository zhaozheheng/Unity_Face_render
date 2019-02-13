using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class GetImage : MonoBehaviour
{
    //timer
    float timer = 0f;
    int index = 0;

    public Camera mainCam;
    RenderTexture rt;
    Texture2D t2d;
    int num = 0;
    GameObject obj;
    Rigidbody rb;
    Material mat;
    MeshRenderer meshRenderer;
    GameObject head;


    Matrix4x4 modelview = new Matrix4x4();
    Matrix4x4 project = new Matrix4x4();

    public static List<string> meshArray = new List<string>();

    public static List<string> textureArray = new List<string>();

    public static List<string> matrixArray = new List<string>();

    //Matrix4x4 project = new Matrix4x4 ( new Vector4(0.0015677f, 0.0f, 0.0f, 0.0f), 
    //                                   new Vector4(0.0f, 0.00278703f, 0.0f, 0.0f), 
    //                                   new Vector4(0.0f, 0.0f, -0.001f, 0.0f),
    //                                   new Vector4(-1.0f, -1.0f, 0.0f, 1.0f) );

    //Matrix4x4 modelview = new Matrix4x4(new Vector4(0.882177f, 0.0297954f, 0.469975f, 0.0f),
    //                                    new Vector4(0.0581432f, 0.983469f, -0.171489f, 0.0f),
    //                                    new Vector4(-0.467315f, 0.178609f, 0.865861f, 0.0f),
    //                                    new Vector4(657.56f, 426.622f, 0.0f, 1.0f));

    // Use this for initialization
    void Start () {
        meshArray = GetObjectNameToArray<string>("Mesh/male", "obj");
        //foreach (string str in meshArray)
        //{
        //    Debug.Log(str);
        //}
        //Debug.Log(meshArray.Count);
        textureArray = GetObjectNameToArray<string>("Textures/male", "png");
        //foreach (string str in meshArray)
        //{
        //    Debug.Log(str);
        //}
        //Debug.Log(textureArray.Count);
        matrixArray = GetObjectNameToArray<string>("Matrix/male", "txt");
        //foreach (string str in meshArray)
        //{
        //    Debug.Log(str);
        //}
        //Debug.Log(matrixArray.Count);


        //obj = CreatePrefabAtPath("Assets/Mesh/male/0000.obj");
        //mat = AssetDatabase.LoadMainAssetAtPath("Assets/Materials/head3d.mat") as Material;
        //rb = obj.AddComponent<Rigidbody>();
        //obj.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

        //add texture
        //head = GameObject.Find(obj.name + "/default");
        //meshRenderer = head.GetComponent<MeshRenderer>();
        //meshRenderer.material = mat;

        //rb.mass = 0.0f;
        //rb.useGravity = false;
        //AssetDatabase.ImportAsset("Assets/Mesh/head3d.obj", ImportAssetOptions.Default);

        //portrait
        t2d = new Texture2D(1080, 1920, TextureFormat.RGB24, false);
        rt = new RenderTexture(1080, 1920, 24);
        mainCam.targetTexture = rt;

        //landscape
        //t2d = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        //rt = new RenderTexture(1920, 1080, 24);
        //mainCam.targetTexture = rt;

        //texture related
        //MeshFilter meshFilter = head.GetComponent<MeshFilter>();
        //Mesh mesh = meshFilter.mesh;

        //Debug.Log(mainCam.transform.localToWorldMatrix);
        //Debug.Log(mainCam.worldToCameraMatrix);

        //mainCam.worldToCameraMatrix = modelview * Matrix4x4.Scale(new Vector3(1, 1, -1));
        //Debug.Log(mainCam.worldToCameraMatrix);

        //Debug.Log(mainCam.projectionMatrix);
        //mainCam.projectionMatrix = project * Matrix4x4.Scale(new Vector3(1, 1, -1));
        //Debug.Log(mainCam.projectionMatrix);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam.orthographic)
        {
            mainCam.Render();
        }

        if (index < meshArray.Count)
        {
            if (timer >= 0.033f) //timer controller
            {
                Destroy(obj);
                //tmpPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Mesh/" + nameArray[index] + ".obj");
                obj = CreatePrefabAtPath("Assets/Mesh/male/" + meshArray[index] + ".obj");

                //update texture
                mat = AssetDatabase.LoadMainAssetAtPath("Assets/Materials/head3d.mat") as Material;
                Texture tex = AssetDatabase.LoadMainAssetAtPath("Assets/Textures/male/" + textureArray[index] + ".isomap.png") as Texture;
                mat.SetTexture("_MainTex", tex);

                //add texture
                head = GameObject.Find(obj.name + "/default");
                meshRenderer = head.GetComponent<MeshRenderer>();
                meshRenderer.material = mat;

                //texture related
                MeshFilter meshFilter = head.GetComponent<MeshFilter>();
                Mesh mesh = meshFilter.mesh;

                List<Matrix4x4> tmp = StringExtention.readMatrix("Matrix/male/" + matrixArray[index] + ".txt");
                modelview = tmp[0];
                project = tmp[1];
                //Debug.Log(modelview);
                //Debug.Log(project);

                //mat = AssetDatabase.LoadMainAssetAtPath("Assets/Materials/head3d.mat") as Material;
                rb = obj.AddComponent<Rigidbody>();
                obj.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

                rb.mass = 0.0f;
                rb.useGravity = false;

                mainCam.worldToCameraMatrix = modelview * Matrix4x4.Scale(new Vector3(1, 1, -1));
                //Debug.Log(mainCam.worldToCameraMatrix);

                //Debug.Log(mainCam.projectionMatrix);
                mainCam.projectionMatrix = project * Matrix4x4.Scale(new Vector3(1, 1, -1));
                //Debug.Log(mainCam.projectionMatrix);

                Debug.Log(index);
                index++;
                timer = 0;
            }
            timer += Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            RenderTexture.active = rt;
            t2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            t2d.Apply();
            RenderTexture.active = null;

            byte[] byt = t2d.EncodeToJPG();
            File.WriteAllBytes(Application.dataPath + "//" + num.ToString() + ".png", byt);

            Debug.Log("Current cut's number is " + num.ToString());
            num++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.transform.Translate(Vector3.up * 1.0f, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            obj.SetActive(false);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 160, 80), "Back to Menu"))
        {
            SceneManager.LoadScene("MenuPage");
        }
    }

    GameObject CreatePrefabAtPath(string path){
        GameObject gameObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        return PrefabUtility.InstantiatePrefab(gameObj) as GameObject;
    }

    List<string> GetObjectNameToArray<T>(string path, string pattern)
    {
        List<string> nameArray = new List<string>();
        string objPath = Application.dataPath + "/" + path;
        string[] directoryEntries;
        try
        {
            //返回指定的目录中文件和子目录的名称的数组或空数组
            directoryEntries = System.IO.Directory.GetFileSystemEntries(objPath);

            for (int i = 0; i < directoryEntries.Length; i++)
            {
                string p = directoryEntries[i];
                //得到要求目录下的文件或者文件夹（一级的）//
                string[] tempPaths = StringExtention.SplitWithString(p, "/Assets/" + path + "\\");

                //tempPaths 分割后的不可能为空,只要directoryEntries不为空//
                if (tempPaths[1].EndsWith(".meta"))
                    continue;
                string[] pathSplit = StringExtention.SplitWithString(tempPaths[1], ".");
                //文件
                if (pathSplit.Length > 1)
                {
                    nameArray.Add(pathSplit[0]);
                }
                //遍历子目录下 递归吧！
                else
                {
                    GetObjectNameToArray<T>(path + "/" + pathSplit[0], "pattern");
                    continue;
                }
            }
        }
        catch (System.IO.DirectoryNotFoundException)
        {
            Debug.Log("The path encapsulated in the " + objPath + "Directory object does not exist.");
        }
        return nameArray;
    }

    
}
