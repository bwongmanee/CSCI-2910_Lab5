using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WongmaneeB_ConsumingAPIsII
{
    public class TaleGOI
    {
        // ======================== TALE PROPERTIES ======================== //
        public DateTime Created_At { get; set; }
        public string Creator { get; set; }
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<string> Hubs { get; set; }
        public int Rating { get; set; }

        public override string ToString()
        {
            string item = "";
            item += $"========== {Title} ==========\n";

            item += $"Author: {Creator}\n";
            item += $"Date Contrived: {Created_At}\n\n";
            item += $"Rating: {Rating}\n\n";

            item += $"\n===== HUBS =====\n";
            item += $"{String.Join('\n', Hubs)}\n\n";

            item += $"===== TAGS =====\n";
            item += $"{String.Join('\n', Tags)}\n\n";

            item += $"\n===== LINK TO ENTRY ======\n";
            item += $"{Url}\n\n";
            return item;
        }
    }
}
