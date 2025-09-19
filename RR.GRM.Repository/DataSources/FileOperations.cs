namespace RR.GRM.Repository.DataSources
{
    public class FileOperations: IFileOperations
    {
        public string[] GetFileLines(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
