using System.Diagnostics;
using System.Windows.Forms;

namespace Ktos.DjToKey.Models
{
    public static class ScriptsHelper
    {       
        public static Document Document = new Document();
        public static Console Console = new Console();
    }

    public class Document
    {
        public static void Alert(string message)
        {
            MessageBox.Show(message);
        }
    }

    public class Console
    {
        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }

}
