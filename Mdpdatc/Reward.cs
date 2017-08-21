using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Reward
    {
        public State cur_state;
        public State next_state;
        public Action cur_action;
        public double rVal;
        public int indicator;

        public State_bm cur_state_bm;
        public State_bm next_state_bm;


        public Reward()
        {
            indicator = -1;
        }

        //reward value part will be replaced later
        public Reward(State cur_state, State next_state, Action cur_action)
        {
            this.cur_state = cur_state;
            this.next_state = next_state;
            this.cur_action = cur_action;
            this.rVal = 0.00;
            this.indicator = 0;

            
            if (cur_action.a_idx == 0)
                this.rVal = (double)(cur_state.k_idx + next_state.k_idx + 1); 
            else
                this.rVal = 0.0; 

            // +
            //cur_action.a_idx);
            /*
            String imsistr = cur_state.i_idx.ToString() +
                                cur_state.k_idx.ToString() +
                                next_state.i_idx.ToString() +
                                next_state.k_idx.ToString() +
                                cur_action.i_idx.ToString();
                                */
            //if(imsistr==00000)

        }

        public Reward(State_bm cur_state, State_bm next_state, Action cur_action)
        {
            this.cur_state_bm = cur_state;
            this.next_state_bm = next_state;
            this.cur_action = cur_action;
            this.rVal = 0.00;
            this.indicator = 0;


            //(replace)this should be replaced by simulator (I think)
            if (cur_action.a_idx == 0)
                this.rVal = (double)(cur_state.b_idx + next_state.m_idx + 1);
            else
                this.rVal = 0.0;

            // +
            //cur_action.a_idx);
            /*
            String imsistr = cur_state.i_idx.ToString() +
                                cur_state.k_idx.ToString() +
                                next_state.i_idx.ToString() +
                                next_state.k_idx.ToString() +
                                cur_action.i_idx.ToString();
                                */
            //if(imsistr==00000)

        }

        public double getReward() { return this.rVal; }
        public void setReward(double rVal) { this.rVal=rVal;}

    }
}
