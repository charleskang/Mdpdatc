using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Qfactor_nn
    {
        public State_nn state;
        public Action action;
        public double qval;

        

        public Qfactor_nn() { }

       

        public Qfactor_nn(State_nn state, Action action)
        {
            this.state = state;
            this.action = action;
            this.qval = 0.00;
        }


        public void setQfactor_nn(double b, double m,
            double lp, double lu, double dp, double du, double beta, Action action, double val)
        {
            State_nn temp = new State_nn(b, m, lp, lu, dp, du, beta);
            this.state = temp;
            this.action = action;
            this.qval = val;
        }

        public void setQfactor_nn(State_nn state, Action action, double val)
        {
            this.state = state;
            this.action = action;
            this.qval = val;
        }

        public double getQfactor_nn()
        {
            return this.qval;
        }
    }
}
