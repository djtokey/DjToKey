using System.Diagnostics;
using System.Windows.Forms;
using WindowsInput;

namespace Ktos.DjToKey.Models
{
    public static class ScriptsHelper
    {       
        public static Document Document = new Document();
        public static Console Console = new Console();
        public static InputSimulator Simulator = new InputSimulator();
    }

    public class Document
    {
        public void Alert(string message)
        {
            MessageBox.Show(message);
        }
    }

    public class Console
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }

}
