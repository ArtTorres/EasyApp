namespace EasyApp.Test.DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "-dir", "C:\\demo", "-out", "C:\\demo" };

            AppRunner.Execute<BasicApp>(args, new ExampleTask());
        }
    }
}
