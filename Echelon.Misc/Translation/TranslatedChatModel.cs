using System.Collections.Generic;

namespace Echelon.Misc.Translation
{
    public class TranslatedChatModel
    {
        public int Code { get; set; }
        public string Lang { get; set; }
        public List<string> Text { get; set; }
    }
}