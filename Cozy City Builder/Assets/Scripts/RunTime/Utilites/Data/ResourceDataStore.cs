using System.IO;

namespace MyUtilities.Data
{
    public class ResourceDataStore
    {
        private const string DataDirectory = "Assets/Resources/Data/";

        public static void StoreData(string filePath, string data)
        {
            //Debug.Log("Writing to file started. File path:" + filePath);
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }

            filePath = GetFilePath(filePath);

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, data);
            //Debug.Log("Writing to file completed. File path:" + filePath);
        }

        public static string GetData(string filePath)
        {
            //Debug.Log("Reading from file started. File path:" + filePath);
            filePath = GetFilePath(filePath);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var text = File.ReadAllText(filePath);
            //Debug.Log("Reading from file completed. File path:" + filePath);
            return string.IsNullOrEmpty(text) ? null : text;
        }

        private static string GetFilePath(string filePath) =>
            Path.Combine(DataDirectory, filePath);

        public static bool Exists(string file)
        {
            var path = GetFilePath(file);
            return File.Exists(path);
        }

        public static string GetDataDirectory(string filePath) => Path.GetDirectoryName(GetFilePath(filePath));
    }
}