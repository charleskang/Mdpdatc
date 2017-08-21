using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class State_nn
    {

        public int b_idx; //1-st dimension, parcel capacity
        public int m_idx; //2-nd dimension, msd
        public int lp_idx;
        public int lu_idx;
        public int dp_idx;
        public int du_idx;
        public int beta_idx; //(event)3-rd dimension, incoming parcel's capacity 



        public State_nn()
        {
            this.b_idx = 0;
            this.m_idx = 0;
            this.lp_idx = 0;
            this.lu_idx = 0;
            this.dp_idx = 0;
            this.du_idx = 0;
            this.beta_idx = 0;

        }



        //general state (generic + event)
        public State_nn(int b_idx, int m_idx, int lp_idx, int lu_idx, int dp_idx, int du_idx, int beta_idx)
        {
            this.b_idx = b_idx;
            this.m_idx = m_idx;
            this.lp_idx = lp_idx;
            this.lu_idx = lu_idx;
            this.dp_idx = dp_idx;
            this.du_idx = du_idx;
            this.beta_idx = beta_idx;

        }

    }
}
