using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace EZInstall{
  public struct Application{
    public string name;
    public string description;
    public string company;
    public string exe;
    public byte[] zipData;
    public bool autoStart;
    public bool createStartMenuShortcut;
  }

  [Serializable]
  public class InvalidApplicationException : Exception{
    public InvalidApplicationException(string message) : base(message){

    }
  }

  public class EZInstall{
    public EZInstall(Application application){
      if(string.IsNullOrWhiteSpace(application.name) || string.IsNullOrWhiteSpace(application.company)){
        throw new InvalidApplicationException("application name and company must not be null or whitespace");
      }

      if(application.zipData.Length <= 0){
        throw new InvalidApplicationException("zipData must have a length greater then 0");
      }

      if(string.IsNullOrWhiteSpace(application.exe)){
        application.exe = application.name + ".exe";
      }

      if(!application.exe.EndsWith(".exe")){
        application.exe += ".exe";
      }

      string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), application.company, application.name);
      string zipPath = Path.Combine(outputPath, $"{application.name}_install.zip");
      string exePath = Path.Combine(outputPath, application.exe);

      // if the desired directory exists, delete it
      if(Directory.Exists(outputPath)){
        Directory.Delete(outputPath, true);
      }

      // create the output directory
      Directory.CreateDirectory(outputPath);

      // get the ZIP file
      File.WriteAllBytes(zipPath, application.zipData);

      // extract the ZIP file
      ZipFile.ExtractToDirectory(zipPath, outputPath);

      // start the program if wanted
      if(application.autoStart){
        Process.Start(new ProcessStartInfo(exePath){
          WorkingDirectory = outputPath
        });
      }

      // create the start menu shortcut if wanted
      if(application.createStartMenuShortcut){
        string startMenuShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), application.company);
        Directory.CreateDirectory(startMenuShortcutPath);

        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(Path.Combine(startMenuShortcutPath, application.name + ".lnk"));
        shortcut.TargetPath = exePath;
        shortcut.IconLocation = exePath;
        shortcut.Description = application.description;
        shortcut.WorkingDirectory = outputPath;
        shortcut.Save();
      }

      // delete the temporary ZIP
      File.Delete(zipPath);
    }
  }
}
