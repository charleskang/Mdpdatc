using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Qfactor
    {
        public State state;
        public Action action;
        public double qval;

        public State_bm state_bm;

        public Qfactor() { }

        public Qfactor(State state, Action action)
        {
            this.state = state;
            this.action = action;
            this.qval = 0.00;
        }

        public Qfactor(State_bm state, Action action)
        {
            this.state_bm = state;
            this.action = action;
            this.qval = 0.00;
        }


        public void setQfactor(double val)
        {
            this.qval = val;
        }

        public double getQfactor()
        {
            return this.qval;
        }
    }
}
