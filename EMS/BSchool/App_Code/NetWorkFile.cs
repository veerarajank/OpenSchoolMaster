using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for NetWorkFile
/// </summary>
public static class NetworkFile
{
    private static String storageLocale = Convert.ToString(ConfigurationManager.AppSettings["storageLocale"]);
    private static String storagePath = Convert.ToString(ConfigurationManager.AppSettings["storagePath"]);
    private static String urlProductRoot = Convert.ToString(ConfigurationManager.AppSettings["urlProductRoot"]);
    private static String localUsername = Convert.ToString(ConfigurationManager.AppSettings["localUsername"]);
    private static String localPassword = Convert.ToString(ConfigurationManager.AppSettings["localPassword"]);
    private static String localDomain = Convert.ToString(ConfigurationManager.AppSettings["localDomain"]);
    private static WindowsImpersonationContext impersonationContext;
    public static int LOGON32_LOGON_INTERACTIVE = 2;
    public static int LOGON32_PROVIDER_DEFAULT = 0;
    [DllImport("advapi32.dll")]
    public static extern int LogonUserA(String lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int DuplicateToken(IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool RevertToSelf();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);

    private static bool impersonateValidUser(String userName, String domain, String password)
    {
        WindowsIdentity tempWindowsIdentity;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;

        if (RevertToSelf())
        {
            if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                    {
                        CloseHandle(token);
                        CloseHandle(tokenDuplicate);
                        return true;
                    }
                }
            }
        }
        if (token != IntPtr.Zero)
            CloseHandle(token);
        if (tokenDuplicate != IntPtr.Zero)
            CloseHandle(tokenDuplicate);
        return false;
    }

    private static void undoImpersonation()
    {
        if (impersonationContext != null)
            impersonationContext.Undo();
    }

    public static String ReadFile(String filePath)
    {
        StringBuilder fileContents = new StringBuilder();
        try
        {
            LoginForStorageLocale();
            filePath = ReconcilePath(filePath);
            StreamReader reader = new StreamReader(filePath);
            fileContents.Append(reader.ReadToEnd());
            reader.Close();
        }
        catch
        {
        }
        finally
        {
            undoImpersonation();
        }
        return fileContents.ToString();
    }
    public static Byte[] ReadFileContent(String filePath)
    {        
            LoginForStorageLocale();
            filePath = ReconcilePath(filePath);
            if (System.IO.File.Exists(filePath) != true)
            {
                filePath = ReconcilePath("Product/Sorryimagenotavailable.png");
            }
            
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();
            undoImpersonation();
            return bytes;
    }




    public static bool WriteToFile(String content, String filePath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            filePath = ReconcilePath(filePath);
            StreamWriter writer = File.CreateText(filePath);
            writer.WriteLine(content);
            writer.Close();
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool CopyToFile(String sourcepath, String destionationpath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();            
            destionationpath = ReconcilePath(destionationpath);
            System.IO.File.Copy(sourcepath, destionationpath, true);

            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }
    public static bool ServerCopyToFile(String sourcepath, String destionationpath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            sourcepath = ReconcilePath(sourcepath);
            System.IO.File.Copy(sourcepath, destionationpath, true);

            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }
    public static byte[] ReadDocumentFile(String filePath)
    {
        StringBuilder fileContents = new StringBuilder();
        byte[] file = new byte[10000]; ;
        try
        {
            LoginForStorageLocale();
            file = File.ReadAllBytes(filePath);           
        }
        catch
        {
        }
        finally
        {
            undoImpersonation();
        }
        //return fileContents.ToString();
        return file;
    }

    public static bool AppendToFile(String content, String filePath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            filePath = ReconcilePath(filePath);
            StreamWriter writer = File.AppendText(filePath);
            writer.WriteLine(content);
            writer.Close();
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool CreateFile(String filePath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            File.Create(ReconcilePath(filePath));
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool CreateDirectory(String directoryPath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            Directory.CreateDirectory(ReconcilePath(directoryPath));
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool DeleteFile(String filePath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            File.Delete(ReconcilePath(filePath));
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool DeleteDirectory(String directoryPath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            Directory.Delete(ReconcilePath(directoryPath), true);
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool FileExists(String filePath)
    {
        bool fileExists = false;
        try
        {
            fileExists = File.Exists(ReconcilePath(filePath));
        }
        catch
        {
            fileExists = false;
        }
        finally
        {
            undoImpersonation();
        }
        return fileExists;
    }

    public static bool DirectoryExists(String directoryPath)
    {
        bool directoryExists = false;
        try
        {
            directoryExists = Directory.Exists(ReconcilePath(directoryPath));
        }
        catch
        {
            directoryExists = false;
        }
        finally
        {
            undoImpersonation();
        }
        return directoryExists;
    }

    public static bool LoginForStorageLocale()
    {
        bool impersonated = false;
        if (storageLocale == "network")
        {
            impersonateValidUser(localUsername, localDomain, localPassword);
            impersonated = true;
        }
        return impersonated;
    }

    public static String ReconcileWebUrl(String urlPath)
    {
        urlPath = HttpContext.Current.Request.ApplicationPath + urlProductRoot + urlPath;
        return urlPath;
    }

    public static String ReconcilePath(String filePath)
    {
        filePath = storagePath + filePath;
        if (storageLocale == "local")
            filePath = System.Web.HttpContext.Current.Server.MapPath("~/" + filePath);
        return filePath;
    }

    public static bool DownloadFile(String sourcepath, String destionationpath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();            
            System.IO.File.Copy(sourcepath, destionationpath, true);

            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }

    public static bool Delete_DocumentFile(String sourcepath, String destionationpath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();
            sourcepath = ReconcilePath(sourcepath);
            destionationpath = ReconcilePath(destionationpath);
            System.IO.File.Copy(sourcepath, destionationpath);
            System.IO.File.Delete(sourcepath);
            
            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }
    public static bool Delete_DocumentWithoutcopyFile(String sourcepath)
    {
        bool success = false;
        try
        {
            LoginForStorageLocale();          
            System.IO.File.Delete(sourcepath);

            success = true;
        }
        catch
        {
            success = false;
        }
        finally
        {
            undoImpersonation();
        }
        return success;
    }
}