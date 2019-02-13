using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class StringExtention {

    public static string[] SplitWithString(string sourceString, string splitString)
    {
        string tempSourceString = sourceString;
        List<string> arrayList = new List<string>();
        string s = string.Empty;
        while (sourceString.IndexOf(splitString) > -1)  //分割
        {
            s = sourceString.Substring(0, sourceString.IndexOf(splitString));
            sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length);
            arrayList.Add(s);
        }
        arrayList.Add(sourceString);
        return arrayList.ToArray();
    }

    //load camera parameters, 1st is modelview matrix, 2nd is projection matrix
    public static List<Matrix4x4> readMatrix(string path)
    {
        List<Matrix4x4> cameraParam = new List<Matrix4x4>();
        //count line
        int sum = 0;

        List<Vector4> cols = new List<Vector4>();

        FileInfo fi = new FileInfo(Application.dataPath + "/" + path);
        if (!fi.Exists)
        {
            Debug.Log("read failed");
            //sr = fi.AppendText()
            //sw.WriteLine ("this is a line.");
        }
        else
        {
            Debug.Log("read file");
        }

        StreamReader inp_stm = new StreamReader(Application.dataPath + "/" + path);

        string[] tmp;


        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();

            // Do Something with the input.
            if (sum > 0 && sum < 5)
            {
                tmp = inp_ln.Split(' ');
                cols.Add(new Vector4(float.Parse(tmp[0]), float.Parse(tmp[1]), float.Parse(tmp[2]), float.Parse(tmp[3])));
                //Debug.Log(tmp);
            }

            if (sum > 6 && sum < 11)
            {
                if (sum == 9)
                {
                    tmp = inp_ln.Split(' ');
                    cols.Add(new Vector4(float.Parse(tmp[0]), float.Parse(tmp[1]), -0.001f, float.Parse(tmp[3])));
                }
                else
                {
                    tmp = inp_ln.Split(' ');
                    cols.Add(new Vector4(float.Parse(tmp[0]), float.Parse(tmp[1]), float.Parse(tmp[2]), float.Parse(tmp[3])));
                }
                //Debug.Log(inp_ln);
            }
            sum++;
        }
        cameraParam.Add(new Matrix4x4(cols[0], cols[1], cols[2], cols[3]));
        //Debug.Log(modelview);

        cameraParam.Add(new Matrix4x4(cols[4], cols[5], cols[6], cols[7]));
        //Debug.Log(projection);

        inp_stm.Close();

        return cameraParam;
    }
}
