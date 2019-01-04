using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.ScriptEditor
{
    public class VMScriptEditorPage : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Script code
        /// </summary>
        public string Script
        {
            set { _script = value; NotifyPropertyChanged("Script"); }
            get { return _script; }
        }
        private string _script { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public VMScriptEditorPage(IContainer container) : base(container)
        {
        }
        /// <summary>
        /// Remove escape characters and set script string
        /// </summary>
        /// <param name="script"></param>
        public void SetScript(string script)
        {
            var beautifier = new Jsbeautifier.Beautifier(new Jsbeautifier.BeautifierOptions() {
                IndentSize = 4
            });
            Script = beautifier.Beautify(script);
        }
        /// <summary>
        /// Get the script string
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            return Script;
        }
    }
}
