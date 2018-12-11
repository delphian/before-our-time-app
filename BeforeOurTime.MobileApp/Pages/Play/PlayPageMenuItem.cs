using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Play
{

    public class PlayPageMenuItem
    {
        public PlayPageMenuItem()
        {
            TargetType = typeof(Explore.ExplorePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}