using System.IO;

public class FileManager
{
    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            UnityEngine.Debug.Log("File Deleted");
            File.Delete(filePath);
        }
    }

    public bool isFileExist(string filePath)
    {
        return File.Exists(filePath);
    }

    public string GetProjectDirectoryPath()
    {
        return Directory.GetParent(UnityEngine.Application.dataPath).FullName;
    }
}
