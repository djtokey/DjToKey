using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Ktos.DjToKey.Plugins.Scripts;
using Ktos.DjToKey.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ktos.DjToKey.Helpers
{
    public class CompletionData : ICompletionData
    {
        private static List<string> _instance;
        public static List<string> KnownMembers
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new List<string>();
                    _instance.AddRange(typeof(ConsoleImpl).GetMethods().Select(x => x.Name));
                    _instance.AddRange(typeof(IMouseSimulator).GetMethods().Select(x => x.Name));
                    _instance.AddRange(typeof(IKeyboardSimulator).GetMethods().Select(x => x.Name));
                    _instance.AddRange(typeof(GlobalDictionary).GetMethods().Select(x => x.Name));
                    _instance.AddRange(typeof(MessageImpl).GetMethods().Select(x => x.Name));

                    _instance.AddRange(Enum.GetNames(typeof(VirtualKeyCode)));

                    var forbidden = new string[] { "ToString", "GetType", "Equals", "GetHashCode" };

                    _instance.RemoveAll(x => forbidden.Contains(x));
                    _instance.RemoveAll(x => x.StartsWith("get_") || x.StartsWith("set_"));
                    _instance = _instance.Distinct().ToList();
                }

                return _instance;
            }
        }

        public static IList<CompletionData> GetCompletion(string text)
        {
            return KnownMembers.Select(x => new CompletionData(x)).ToList();
        }

        public CompletionData(string text)
        {
            this.Text = text;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return "Description for " + this.Text; }
        }

        public double Priority { get; private set; }

        public void Complete(
            TextArea textArea,
            ISegment completionSegment,
            EventArgs insertionRequestEventArgs
        )
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
