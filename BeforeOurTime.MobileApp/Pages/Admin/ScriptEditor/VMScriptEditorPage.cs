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
            get { return _script; }
            set { _script = value; NotifyPropertyChanged("Script"); }
        }
        private string _script { set; get; }
        /// <summary>
        /// Code snippet names
        /// </summary>
        public List<string> CodeNames
        {
            get { return _codeNames; }
            set { _codeNames = value; NotifyPropertyChanged("CodeNames"); }
        }
        private List<string> _codeNames { set; get; } = new List<string>();
        /// <summary>
        /// Currently selected code snippet name;
        /// </summary>
        public string CodeName
        {
            get { return _codeName; }
            set { _codeName = value; NotifyPropertyChanged("CodeName"); }
        }
        private string _codeName { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public VMScriptEditorPage(IContainer container) : base(container)
        {
            CodeNames.Add("OnUse");
            CodeNames.Add("OnItemRead");
            CodeNames.Add("BotEmote");
            CodeNames.Add("BotLog");
            CodeNames.Add("BotReadItem");
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
            NotifyPropertyChanged("Script");
        }
        /// <summary>
        /// Get the script string
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            return Script;
        }
        /// <summary>
        /// Insert code snippet
        /// </summary>
        public void InsertSnippet(string snippetName)
        {
            if (snippetName == "OnUse")
            {
                Script = @"// Respond to a command
var onUse = function(item, itemCommand, origin) {
    botEmote(300, ""I like "" + itemCommand.data.like);
};" + $"\n{Script}";
            }
            if (snippetName == "OnItemRead")
            {
                Script = @"// Add command to item
var onItemRead = function(item) {
    botAddProperty(""BeforeOurTime.Models.Modules.Core.Models.Properties.CommandItemProperty"", {
        ""commands"": [{
            ""itemId"": item.Id,
            ""id"": ""c558c1f9-7d01-45f3-bc35-dcab52b5a37c"",
            ""data"": {
                ""like"": 5
            },
            ""name"": ""Go Now""
        }]
    });
};" + $"\n{Script}";
            }
            if (snippetName == "BotEmote")
            {
                Script = @"// Send out emote from current item
// 100 = Smile, 200 = Frown, 300 = Speak, 400 = Raw
botEmote(300, ""Hello World"");" + $"\n{Script}";
            }
            if (snippetName == "BotLog")
            {
                Script = @"// Write to log file
botLog(""Testing log file"");" + $"\n{Script}";
            }
            if (snippetName == "BotReadItem")
            {
                Script = @"// Read an item
var item = botReadItem(c558c1f9-7d01-45f3-bc35-dcab52b5a37c);
botEmote(300, ""I just read item "" + item.id);" + $"\n{Script}";
            }
        }
    }
}
