namespace RR.GRM.Repository.DataSources
{
    public interface IFileOperations
    {
        string[] GetFileLines(string path);
    }
}
