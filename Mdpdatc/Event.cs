using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Event
    {
        public int ek_idx;// 1-st dimension, event, parcel category # arrival
        public int ed_idx;// 2-nd dimension, event, relative due date

        //event state
        public Event(int ek_idx, int ed_idx)
        {
            this.ek_idx = ek_idx;
            this.ed_idx = ed_idx;
        }
    }
}
