using System.IO;
using System.Reflection;

namespace Globe3DLight
{
    public static class EmbeddedResources
    {
        public static string GetText(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }
    }
}
