using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace vvctiger.com_WebSite.ViewModels
{
    [Serializable]
    public class GhostGameSettingVM
    {
        public string HumanWord { get; set; }
        public int HumanCount { get; set; }
        public string FoolWord { get; set; }
        public int FoolCount { get; set; }
        public string GhostWord { get; set; }
        public int GhostCount { get; set; }

        public string GameMode { get; set; }
    }
}