using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Time
    {
        public State cur_state;
        public State next_state;
        public Action cur_action;
        public double tVal;
        public int indicator;

        public Time() { indicator = -1; }

        public Time(State cur_state, State next_state, Action cur_action)
        {
            this.cur_state = cur_state;
            this.next_state = next_state;
            this.cur_action = cur_action;
            this.tVal = 0.00;
            this.indicator = 0;
            this.tVal = 1.00;
        }

        
    }
}
