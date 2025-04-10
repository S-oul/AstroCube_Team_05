using System.Threading.Tasks;

public partial class WwiseProjectDatabase
{
    static WwiseProjectDatabase()
    {
        PlatformName = "Windows";
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
            await Task.Run(() => WwiseProjectDatabasePINVOKE_Windows.Init(inDirectoryPath, inDirectoryPlatformName));
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
            WwiseProjectDatabasePINVOKE_Windows.Init(inDirectoryPath, inDirectoryPlatformName);
            WwiseProjectDatabasePINVOKE_Windows.SetCurrentLanguage(language);
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
            WwiseProjectDatabasePINVOKE_Windows.SetCurrentPlatform(inDirectoryPlatformName);
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
            WwiseProjectDatabasePINVOKE_Windows.SetCurrentLanguage(inLanguageName);
        }
        catch (System.Exception e)
        {
            LogProjectDatabaseDLLException(e);
            throw;
        }
    }

    public static string StringFromIntPtrString(System.IntPtr ptr)
    {
        try
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
        }
        catch (System.Exception e)
        {
            LogProjectDatabaseDLLException(e);
            throw;
        }
    }

    // (Toutes les autres méthodes restent inchangées ci-dessous)
    // Copie tout le reste des méthodes existantes ici sans changement.
}
