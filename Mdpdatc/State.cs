using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class State
    {
        
        public int i_idx; //1-st dimension, truck #
        public int k_idx; //2-nd dimension, parcel capacity
        public int u_idx; //3-rd dimension, expected time until picking up 
        public int d_idx; //4-th dimension, relative due date
        public int n_idx; //5-th dimension, # of parcel which has the same i, k and d

        

        public State()
        {
            this.i_idx = 0;
            this.k_idx = 0;
            this.u_idx = 0;
            this.d_idx = 0;
            this.n_idx = 0;
        }

        

        //general state (generic + event)
        public State(int i_idx,int k_idx, int u_idx,int d_idx,int n_idx)
        {
            this.i_idx = i_idx;
            this.k_idx = k_idx;
            this.u_idx = u_idx;
            this.d_idx = d_idx;
            this.n_idx = n_idx;
        }

        public State(int i_idx, int k_idx, int d_idx, int n_idx)
        {
            this.i_idx = i_idx;
            this.k_idx = k_idx;
            this.u_idx = -999;
            this.d_idx = d_idx;
            this.n_idx = n_idx;
        }

    }
}
