using SoulControl;
using SoulControl.Environment;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.JSInterop;
using System.Text;
using Microsoft.AspNetCore.Mvc.Razor;

using ASPTest.ModelAPI;

namespace ASPTest.razor
{
    public class IndexModel : PageModel
    {
        public List<CallBack> EntityTick { get; set; }
        public List<CallBack> PlayerTick { get; set; }
        public List<CallBack> Logs { get; set; }

        public static string _ent { get; set; }
        public static string _pl { get; set; }
        public static string _log { get; set; }
        public static string _code { get; set; } = "execute(`scan`)";
        private static Task ModelRun = null;

        private readonly IJSRuntime js;

        public IndexModel(IJSRuntime js)
        {
            this.js = js;
        }


        public string CallBackToHTML(CallBack callBack)
        {
            string res = "<div class=\"game-box\">";
            if (callBack.Type != "html")
                res += "<pre>";
            if (callBack.Type == "function")
                res += "<script>" + callBack.Source + "</script>";
            res += callBack.Value;
            if (callBack.Type != "html")
                res += "</pre>";
            res += "</div>";
            return res;
        }
        public string CallBackCompilation(IEnumerable<CallBack> callbacks)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (CallBack callBack in callbacks)
                stringBuilder.Append(CallBackToHTML(callBack));
            return stringBuilder.ToString();
        }
        public void _OnRequest(string text)
        {
            try
            {
                ModelAPI.ModelAPI.Model.Request(text);
            }
            catch (Exception ex) { ModelAPI.ModelAPI.Model.Log(ex.ToString()); }


            try
            {
                var EntityTick = ModelAPI.ModelAPI.Model.EntityTick().ToList();
                var PlayerTick = ModelAPI.ModelAPI.Model.Tick().ToList();
                var Logs = ModelAPI.ModelAPI.Model.GetCallBacks().ToList();

                _ent = CallBackCompilation(EntityTick);
                _pl = CallBackCompilation(PlayerTick);
                _log = CallBackCompilation(Logs);
            }
            catch(Exception ex) { ModelAPI.ModelAPI.Model.Log(ex.ToString()); }
        }
        public void _OnTick()
        {
            
        }

        public void OnPost(string req)
        {
            _code = req;
            _OnRequest(req);
        }

        public void OnGet()
        {
            /*if(ModelRun == null)
            {
                ModelRun = Task.Run(() => { while (true) { Thread.Sleep(1000); _OnTick(); } });
            }*/
        }
    }
}
