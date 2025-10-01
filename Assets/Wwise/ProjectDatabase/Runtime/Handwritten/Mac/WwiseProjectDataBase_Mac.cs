using System.Threading.Tasks;

public partial class WwiseProjectDatabase
{
    static WwiseProjectDatabase()
    {
        PlatformName = "Mac";
    }

    public static void PostInitCallback()
    {
        SoundBankDirectoryUpdated?.Invoke();
    }

    public static event System.Action SoundBankDirectoryUpdated;

    public static async Task<bool> InitAsync(string inDirectoryPath, string inDirectoryPlatformName)
    {
        InitCheckUp(inDirectoryPath);
        if (!ProjectInfoExists)
            return false;

        try
        {
            await Task.Run(() => WwiseProjectDatabasePINVOKE_Mac.Init(inDirectoryPath, inDirectoryPlatformName));
        }
        catch (System.Exception e)
        {
            LogProjectDatabaseDLLException(e);
            throw;
        }
        return ProjectInfoExists;
    }

    public static void Init(string inDirectoryPath, string inDirectoryPlatformName, string language)
    {
        InitCheckUp(inDirectoryPath);
        if (!ProjectInfoExists)
            return;

        try
        {
            WwiseProjectDatabasePINVOKE_Mac.Init(inDirectoryPath, inDirectoryPlatformName);
            WwiseProjectDatabasePINVOKE_Mac.SetCurrentLanguage(language);
        }
        catch (System.Exception e)
        {
            LogProjectDatabaseDLLException(e);
            throw;
        }
    }

    public static void SetCurrentPlatform(string inDirectoryPlatformName)
    {
        try
        {
            WwiseProjectDatabasePINVOKE_Mac.SetCurrentPlatform(inDirectoryPlatformName);
        }
        catch (System.Exception e)
        {
            LogProjectDatabaseDLLException(e);
            throw;
        }
    }

    public static void SetCurrentLanguage(string inLanguageName)
    {
        try
        {
            WwiseProjectDatabasePINVOKE_Mac.SetCurrentLanguage(inLanguageName);
        }
        catch (System.Exception e)
        {
            LogProjectDatabaseDLLException(e);
            throw;
        }
    }

    public static string StringFromIntPtrString(System.IntPtr ptr)
    {
        return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
    }

    // (Toutes les autres m�thodes restent inchang�es ci-dessous)
    // Copie tout le reste des m�thodes existantes ici sans changement.
}
