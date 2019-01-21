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
            CodeNames.Add("OnTick()");
            CodeNames.Add("OnUse(Item item, ItemCommand itemCommand, Item origin)");
            CodeNames.Add("OnItemRead");
            CodeNames.Add("void BotEmote(string message, int? level)");
            CodeNames.Add("int BotListCount(IList list)");
            CodeNames.Add("void BotLog(string message, int level)");
            CodeNames.Add("string BotStringify(object obj)");
            CodeNames.Add("Item BotReadItem(Guid id)");
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
            if (snippetName == "OnUse(Item item, ItemCommand itemCommand, Item origin)")
            {
                Script = @"// Respond to a javascript callback command
var onUse = function(item, itemCommand, origin) {
    botEmote(300, ""I like "" + itemCommand.data.like);
};" + $"\n{Script}";
            }
            if (snippetName == "OnItemRead")
            {
                Script = @"// Add javascript (onUse) command to item
var onItemRead = function(item) {
    botAddProperty(""BeforeOurTime.Models.Modules.Core.Models.Properties.CommandItemProperty"", {
        ""commands"": [{
            ""itemId"": item.Id,
            ""id"": ""22a73822-6655-4b7b-aa2d-100b5c4a00a7"",
            ""data"": {
                ""like"": 5
            },
            ""name"": ""Run Javascript Callback""
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
            if (snippetName == "void BotLog(string message, int level)")
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
            if (snippetName == "string BotStringify(object obj)")
            {
                Script = @"
var json = botStringify(object obj);
" + $"\n{Script}";
            }
            if (snippetName == "int BotListCount(IList list)")
            {
                Script = @"// Count the number of items in a list
var count = BotListCount(item.children);" + $"\n{Script}";
            }
            if (snippetName == "OnTick()")
            {
                Script = @"// Opportunity to execute code at regular interval
var onTick = function() {
};" + $"\n{Script}";
            }
        }
    }
}
