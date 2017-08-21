using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Event_bm
    {
        public int lp_idx;
        public int dp_idx;
        public int lu_idx;
        public int du_idx;
        public int beta_idx;

        //event state
        public Event_bm(int lp_idx, int dp_idx, int lu_idx, int du_idx, int beta_idx)
        {
            this.lp_idx = lp_idx;
            this.dp_idx = dp_idx;
            this.lu_idx = lu_idx;
            this.du_idx = du_idx;
            this.beta_idx = beta_idx;
        }
    }
}
