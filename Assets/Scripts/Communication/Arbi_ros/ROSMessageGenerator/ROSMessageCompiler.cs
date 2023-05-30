using UnityEngine;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

public class ROSMessageCompiler
{
    private string dllPath = "";

    public ROSMessageCompiler()
    {
        bool isEditor = EnvironmentManager.instance.isEditor;
        string relativeFolderPath = "";
        
        if (isEditor)
        {
            relativeFolderPath = "./Library/ScriptAssemblies/";
        }
        else
        {
            relativeFolderPath = "./simulator_Data/Managed/";
        }

        //dllPath = System.IO.Path.GetFullPath(relativeFolderPath);
        dllPath = relativeFolderPath;
        //Debug.Log(dllPath);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceCode"></param>
    /// <returns>compiled assembly</returns>
    public Assembly compileROSMessage(string sourceCode)
    {
        //Debug.Log(sourceCode);
        CompilerParameters parameters = new CompilerParameters();
        parameters.GenerateInMemory = true;
        
        // 유니티 메시지 타입 등 사용할 추가적인 어셈블리를 추가해야 할 수 있음.
        parameters.ReferencedAssemblies.Add("System.dll");
        parameters.ReferencedAssemblies.Add("System.Core.dll");
        parameters.ReferencedAssemblies.Add(dllPath + "Unity.Robotics.ROSTCPConnector.dll");
        parameters.ReferencedAssemblies.Add(dllPath + "Unity.Robotics.ROSTCPConnector.Messages.dll");
        parameters.ReferencedAssemblies.Add(dllPath + "Unity.Robotics.ROSTCPConnector.MessageGeneration.dll");
        parameters.ReferencedAssemblies.Add("C:/Program Files/Unity/Editor/Data/Managed/UnityEngine.dll");
        

        // C# 코드 컴파일
        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerResults results = provider.CompileAssemblyFromSource(parameters, sourceCode);
        if (results.Errors.HasErrors)
        {
            foreach (CompilerError error in results.Errors)
            {
                Debug.LogError(error.ErrorText);
            }
            return null;
        }
        else
        {
            return results.CompiledAssembly;
        }
    }
    
}
