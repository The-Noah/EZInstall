using System;

namespace SampleInstall{
  public class Program{
    public static void Main(string[] args){
      Console.WriteLine("Installing sample app...");

      EZInstall.Application application = new EZInstall.Application{
        name = "SampleApp",
        company = "The Noah",
        zipData = Properties.Resources.SampleApp,
        autoStart = true
      };

      new EZInstall.EZInstall(application);

      Console.WriteLine("Done!");
      Console.ReadKey();
    }
  }
}
