using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mdpdatc
{
    class Reinforcement_SL0627
    {

        //public double[,] q;

        //public double[,] v;

        public Random random;

        public int seed = 1;

        public int truck_no = 2;
        public int pk_no = 4;
        public int d_no = 10;
        public int u_no = 10;
        public int n_no = 10;

        public List<State> S;
        public List<Action> A;
        public List<Qfactor> Q;
        public List<Reward> R;

        public State cur_state;// = S[0];
        public State next_state;
        public Action max_action;

        public double alpha;
        public double beta;

        public double exp_lamda = 5.0; //on average, arrivals occured every 5 minutes
        public double abs_time = 0.00;

        public Reinforcement_SL0627()
        {


            initialize();
            //this.q = new double[3, 2];

            //this.v = new double[3, 2];
            exec_algorithm();

        }

        public void initialize()
        {
            random = new Random();

            S = new List<State>();
            S.Clear();
            A = new List<Action>();
            Q = new List<Qfactor>();
            R = new List<Reward>();
            //generte & initialize state
            for (int i = 0; i < truck_no; i++)
                for (int k = 0; k < pk_no; k++)
                    for (int u = 0; u < u_no; k++)
                        for (int d = 0; d < d_no; d++)
                            for (int p = 0; p < n_no; p++)
                            {
                                //Console.Out.WriteLine(i + "" + k + ""+ u + "" + d + "" + p);
                                State temp = new State(i, k, u, d, p);

                                S.Add(temp);
                                //Console.Out.WriteLine(i + "" + k + "" + u + ""+ d + "" + p);

                            }

            //initialize current state
            cur_state = new State();

            //initialize next state
            next_state = S[0];

            //generate & initialize action  a0=accept, a1=reject
            for (int a = 0; a < 2; a++)
                A.Add(new Mdpdatc.Action(a));


            //generate & initialize qfactor
            foreach (State state in S)
                foreach (Action action in A)
                    Q.Add(new Qfactor(state, action));



            //generate & initialize reward
            foreach (State c_state in S)
                foreach (State n_state in S)
                    foreach (Action c_action in A)
                        R.Add(new Reward(c_state, n_state, c_action));

            //Console.Out.WriteLine("============");

            

        }

        public double randomGen()
        {
            return random.NextDouble();
        }

        public void sampleAction_egreedy(double epsilon, State state)
        {
            double rnd = randomGen();//Math.random();
            Action chosen_Action = new Action();

            List<Qfactor> qset =
                    Q.FindAll(x => (x.state.i_idx == state.i_idx) &&
                                    (x.state.k_idx == state.k_idx) &&
                                    (x.state.u_idx == state.u_idx) &&
                                    (x.state.d_idx == state.d_idx) &&
                                    (x.state.n_idx == state.n_idx));

            int idx_i = -1;
            int idx_k = -1;
            int idx_u = -1;
            int idx_d = -1;
            int idx_n = -1;

            if (rnd >= epsilon)
            {
                double temp = 0.00;
                foreach (Qfactor qelem in qset)
                {
                    if (qelem.qval >= temp)
                    {
                        chosen_Action = qelem.action;
                        temp = qelem.qval;
                        idx_i = qelem.state.i_idx;
                        idx_k = qelem.state.k_idx;
                        idx_u = qelem.state.u_idx;
                        idx_d = qelem.state.d_idx;
                        idx_n = qelem.state.n_idx;
                    }
                }

                /*
                 state.i_idx = idx_i;
                 state.k_idx = idx_k;
                 state.u_idx = idx_u;
                 state.d_idx = idx_d;
                 cur_state = state;

                 //simulate action
                 if(chosen_Action.i_idx==0)
                     next_state
                     */
            }
            else  //e-greedy
            {
                if (rnd <= (epsilon / 2.0))
                    chosen_Action = qset[0].action;
                else
                    chosen_Action = qset[1].action;
            }

            max_action = chosen_Action;
        }

        //This will be replaced with S.Lee's reward feedback part
        public Reward reward(State cst, State nst, Action cat)
        {
            Reward temp = new Reward();
            temp = R.Find(x => x.cur_state == cst && x.next_state == nst && x.cur_action == cat);
            if (temp.indicator == -1)
                throw new Exception();
            return temp;
        }

        public State sampleNextState(State state, Action chosen_action)
        {
            //double rnd = randomGen();
            //if a_idx=0, accept, and retrive state n+1 and return
            //
            //if a_idx=1, reject, and stay n
            //
            //
            State nextone = state;
            if (chosen_action.a_idx == 0)
            {
                if (state.n_idx != (n_no - 1))
                {
                    nextone = S.Find(x => x.i_idx == state.i_idx &&
                                              x.k_idx == state.k_idx &&
                                              x.u_idx == state.u_idx &&
                                              x.d_idx == state.d_idx &&
                                              x.n_idx == (state.n_idx + 1));
                }
            }
            return nextone;
        }


        //return the event
        public Event simulateArrival()
        {


            double rnd = randomGen();

            //1.Arrival time simulation
            double int_arr_time = -1.0 * exp_lamda * Math.Log(rnd);



            //2.ek, ed simulation
            rnd = randomGen();
            double[] sector = new double[pk_no];
            for (int i = 0; i < pk_no; i++)
                sector[i] = (double)i / (double)pk_no;

            int arrived_ek = 0;
            for (int i = pk_no - 1; i >= 0; i--)
            {
                if (rnd >= sector[i])
                {
                    arrived_ek = i;
                    break;
                }
            }

            // if (arrived_ek == 0) Console.Out.WriteLine("ekekekekekek");

            rnd = randomGen();

            sector = new double[d_no];
            for (int i = 0; i < d_no; i++)
                sector[i] = (double)i / (double)d_no;

            int arrived_ed = 0;
            for (int i = d_no - 1; i >= 0; i--)
            {
                if (rnd >= sector[i])
                {
                    arrived_ed = i;
                    break;
                }
            }
            //Console.Out.WriteLine(arrived_ek+"=="+arrived_ed);
            return new Event(arrived_ek, arrived_ed);

        }

        public void exec_algorithm()
        {


            //Simulate Poisson Arrival


            //Setting alpha and beta
            alpha = 0.2;
            beta = 0.2;
            double epsilon = 0.01;

            //Setting initial state


            //learning
            for (int loop = 1; loop < 200000; loop++)
            {
                /*
                int count = 0;
                foreach (State ss in S)
                    if (ss.d_idx == 0)
                        count++;

                Console.Out.WriteLine(count);
                */

                int i_idx = 0;
                int k_idx = 0;
                int u_idx = 0;
                int d_idx = 0;
                int n_idx = 0;

                //2. simulate arrival process.. Event has ek and ed
                Event event_occurred = this.simulateArrival();
                k_idx = event_occurred.ek_idx;
                d_idx = event_occurred.ed_idx;


                //3. check RC to see if there are any departures since the last decision epoch.
                //In case of departure events, update current capacity information. 
                if (cur_state.n_idx > 0)
                {
                    double rnd = randomGen();
                    if (rnd > 0.6)
                        cur_state.n_idx = cur_state.n_idx - 1;
                    else if (cur_state.n_idx > 1 && rnd > 0.8)
                        cur_state.n_idx = cur_state.n_idx - 2;
                }

                //4. OAC checks capacity constraints for all trucks. 
                //If any available, then go to 5 otherwise reject the request and go to 9


                //5. OAC sends truck information(i), relative due-date (d) and 
                //the number of parcels (n) information to RC


                //6. Given the information from 4, 
                //RC performs continuous variable feedback control 
                //to come up with the best possible routing schedule, 
                //which can minimize mean square due-deviation (MSD). 
                //After computation, RC generates which truck will be assigned for the request 
                //and what would be the reward by doing so.

                //7.OAC receives truck information(i) and reward(r) from RC

                //8.OAC runs R-SMART with the information i, k, d, n , reward and update Q function.
                State found_cur_state;

                found_cur_state = S.Find(x => x.i_idx == cur_state.i_idx &&
                                              x.k_idx == k_idx &&
                                              x.u_idx == u_idx &&
                                              x.d_idx == d_idx &&
                                              x.n_idx == cur_state.n_idx);


                if (found_cur_state == null) throw new Exception();

                this.sampleAction_egreedy(epsilon, found_cur_state);
                next_state = this.sampleNextState(found_cur_state, max_action);

                Qfactor cur_q = Q.Find(x => x.state == found_cur_state && x.action == max_action);

                //Just regular MDP with Relative Q-Learning
                cur_q.qval = (1 - alpha) * cur_q.qval + alpha * (reward(found_cur_state, next_state, max_action).rVal + maxQ(found_cur_state) - Q[0].qval);



                cur_state.i_idx = next_state.i_idx;
                cur_state.k_idx = next_state.k_idx;
                cur_state.u_idx = next_state.u_idx;
                cur_state.d_idx = next_state.d_idx;
                cur_state.n_idx = next_state.n_idx;

                //Console.Out.WriteLine(next_state.k_idx + "==" + next_state.d_idx);
                //9. OAC updates capacity information and go to 2 until loop limit is reached

                if (loop % 1000 == 0)
                    Console.Out.WriteLine(loop);
            }

            int zeroval_count = 0;
            int non_zeroval_count = 0;
            foreach (Qfactor qelem in Q)
            {
                //if (qelem.qval != 0)
                Console.Out.WriteLine(qelem.state.i_idx + "," +
                                        qelem.state.k_idx + "," +
                                        qelem.state.u_idx + "," +
                                        qelem.state.d_idx + "," +
                                        qelem.state.n_idx + ":" +
                                        qelem.action.a_idx + "=" + qelem.qval);

                if (qelem.qval == 0)
                    zeroval_count++;
                else
                    non_zeroval_count++;

            }

            Console.Out.WriteLine("zero:" + zeroval_count + "non_zero:" + non_zeroval_count);
        }

        public double maxQ(State state)
        {
            List<Qfactor> qlist = Q.FindAll(x => x.state.i_idx == state.i_idx &&
                                                 x.state.k_idx == state.k_idx &&
                                                 x.state.u_idx == state.u_idx &&
                                                 x.state.d_idx == state.d_idx &&
                                                 x.state.n_idx == state.n_idx);
            double retVal = -1000000.00;
            if (qlist.Count == 0) throw new Exception();
            foreach (Qfactor qelem in qlist)
            {
                if (retVal < qelem.qval)
                    retVal = qelem.qval;
            }

            return retVal;
        }


    }
}
