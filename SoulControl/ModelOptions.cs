using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.ModelOptions
{
    public class Options
    {
        public Detailed.RequestOptions Request { get; set; } = new Detailed.RequestOptions();
        public Detailed.CallBackOptions CallBack { get; set; } = new Detailed.CallBackOptions();
        public Detailed.InteropOptions Interop { get; set; } = new Detailed.InteropOptions();

        public Options() { }
    }
    namespace Detailed
    {
        public class RequestOptions
        {
            public bool ClearQueueOnRequest { get; set; } = false;
            public bool ClearQueueOnTick { get; set; } = false;

            public RequestOptions() { }
        }
        public class CallBackOptions
        {
            public bool FilterPrivate { get; set; } = true;
            public string PrivateFlag { get; set; } = "private";

            public CallBackOptions() { }
        }
        public class InteropOptions
        {
            public bool AllowEval { get; set; } = true;

            public InteropOptions() { }
        }
    }
}
