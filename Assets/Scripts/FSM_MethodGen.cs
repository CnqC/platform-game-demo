using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CnqC.PlatformGame;

public static class FSM_MethodGen
{
    private static string m_savePath = Application.dataPath + "/FSM_Methods.txt";

    public static void Gen<T>(bool hasFixedUpdate = false, bool hasFinally = false)
    {
        System.Array A = System.Enum.GetNames(typeof(T));

        if (A != null && A.Length > 0)
        {
            string methods = "";

            for (int i = 0; i < A.Length; i++)
            {
                string stateName = A.GetValue(i).ToString();

                 methods +=
                    "protected void " + stateName + "_Enter() { } \n" +
                    "protected void " + stateName + "_Update() { } \n";

                if(hasFixedUpdate)
                    methods += "protected void " + stateName + "_FixedUpdate() { } \n";

                methods += "protected void " + stateName + "_Exit() { } \n";

                if(hasFinally)
                    methods += "protected  void " + stateName + "_Finally() { } \n";
            }

            File.WriteAllText(m_savePath, methods);
        }
    }
}
