using System.IO;

public partial class WwiseProjectDatabase
{
    public static bool ProjectInfoExists = false;
    public static void InitCheckUp(string inDirectoryPath)
    {
        var jsonFilename = Path.Combine(inDirectoryPath, "ProjectInfo.json");
        ProjectInfoExists = File.Exists(jsonFilename);
    }

    public static string PlatformName = "";

    public static void LogProjectDatabaseNotFound(string errorMessage)
    {
        int index = errorMessage.IndexOf(' ');
        string libraryName = index != -1 ? errorMessage.Substring(0, index) : errorMessage;
        string directory = "Assets/Wwise/ProjectDatabase/Runtime/Plugins";
        if (PlatformName == "Windows")
        {
            bool x86Architecture = System.IntPtr.Size == 4;
            directory += "/Windows/" + (x86Architecture ? "x86" : "x86_64");
        }
        else if (PlatformName == "Mac")
        {
            directory += "/Mac/DSP";
        }

        UnityEngine.Debug.LogError($"WwiseUnity: {libraryName} could not be found. Please check the parent folder {directory}. If the {Path.GetExtension(libraryName)} is missing, try 1. Modifying the Wwise Project or 2. Copying the {libraryName} directly from the SDK\\platform_architecture\\Profile\\bin folder of your Wwise installation into {directory}.");
    }

    public static void LogProjectDatabaseDLLException(System.Exception e)
    {
        if (e is System.DllNotFoundException)
        {
            LogProjectDatabaseNotFound(e.Message);
        }
        else
        {
            UnityEngine.Debug.LogError($"WwiseUnity: The project database dll encountered the following error: {e.Message}");
        }
    }
}
