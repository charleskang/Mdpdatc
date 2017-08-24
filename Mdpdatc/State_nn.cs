using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class State_nn
    {

        public double b_idx; //1-st dimension, parcel capacity
        public double m_idx; //2-nd dimension, msd
        public double lp_idx;
        public double lu_idx;
        public double dp_idx;
        public double du_idx;
        public double beta_idx; //(event)3-rd dimension, incoming parcel's capacity 



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
        public State_nn(double b_idx, double m_idx,
            double lp_idx, double lu_idx, double dp_idx, double du_idx, double beta_idx)
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
