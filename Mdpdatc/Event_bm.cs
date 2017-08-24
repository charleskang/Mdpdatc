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

        public double db_lp_idx;
        public double db_dp_idx;
        public double db_lu_idx;
        public double db_du_idx;
        public double db_beta_idx;

        //event state
        public Event_bm(int lp_idx, int dp_idx, int lu_idx, int du_idx, int beta_idx)
        {
            this.lp_idx = lp_idx;
            this.dp_idx = dp_idx;
            this.lu_idx = lu_idx;
            this.du_idx = du_idx;
            this.beta_idx = beta_idx;
        }

        public Event_bm(double lp_idx, double dp_idx, double lu_idx, double du_idx, double beta_idx)
        {
            this.db_lp_idx = lp_idx;
            this.db_dp_idx = dp_idx;
            this.db_lu_idx = lu_idx;
            this.db_du_idx = du_idx;
            this.db_beta_idx = beta_idx;
        }
    }
}
