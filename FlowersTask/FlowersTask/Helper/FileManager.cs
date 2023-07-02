namespace FlowersTask.Helper
{
    public class FileManager
    {

        static public string AddFile(string path,string folder,IFormFile file)
        {
            var filename=Guid.NewGuid().ToString()+(file.FileName.Length<60?file.FileName:file.FileName.Substring(file.FileName.Length-60));
            string pathfile=Path.Combine(path,folder,filename);
            using (var st = new FileStream(pathfile, FileMode.Create))
            {
                file.CopyTo(st);
            }
            return filename;
        }
        static public void Delete(string path,string folder,string fileName)
        {
            var mainFile=Path.Combine(path,folder,fileName);
            if (File.Exists(mainFile))
            {
                File.Delete(mainFile);            
                    }
        }
        static public void DeleteFiles(string path, string folder, List<string> fileNames)
        {
            foreach (var item in fileNames)
            {
                var mainFile = Path.Combine(path, folder, item);
                if (File.Exists(mainFile))
                {
                    File.Delete(mainFile);
                }
            }
        }
    }
}
