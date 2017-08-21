using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.Neural.Feedforward;
using Encog.Neural.Feedforward.Train;
using Encog.Neural.Feedforward.Train.Backpropagation;

namespace Mdpdatc
{
    class Reinforcement_SLBM
    {
        
        //public double[,] q;

        //public double[,] v;

        public Random random;

        public int seed = 1;

        public int b_no = 10;
        public int m_no = 10;
        public int lp_no = 5;
        public int lu_no = 5;
        public int dp_no = 5;
        public int du_no = 5;
        public int beta_no = 10;
        
        //public int d_no = 10;
        //public int n_no = 10;

        public List<State_bm> S;
        public List<Action> A;
        public List<Qfactor> Q;
        public List<Reward> R;

        public State_bm cur_state;// = S[0];
        public State_bm next_state;
        public Action max_action;

        public double alpha;
        public double betas;

        public double exp_lamda = 5.0; //on average, arrivals occured every 5 minutes
        public double abs_time = 0.00;

        public double eta = 0.99;
        public double total_time = 0.00;
        public double total_reward = 0.00;
        public double rho = 0.00;
        public double cur_int_arr_time = 0.00;

        public FeedforwardNetwork network_accept;
        public FeedforwardNetwork network_reject;

        public Train train_accept;
        public Train train_reject;
        public Reinforcement_SLBM()
        {


            initialize();
            //this.q = new double[3, 2];

            //this.v = new double[3, 2];
            exec_algorithm();

        }

        public void initialize()
        {
            random = new Random();

            S = new List<State_bm>();
            S.Clear();
            A = new List<Action>();
            Q = new List<Qfactor>();
            R = new List<Reward>();
            //generte & initialize state
            for (int b = 0; b < b_no; b++)
                for (int m = 0; m < m_no; m++)
                    for (int lp = 0; lp < lp_no; lp++)
                        for (int lu = 0; lu < lu_no; lu++)
                            for (int dp = 0; dp < dp_no; dp++)
                                for (int du = 0; du < du_no; du++)
                                    for (int beta = 0; beta < beta_no; beta++)
                                    {
                                        State_bm temp = new State_bm(b, m, lp, lu, dp, du, beta);
                                        S.Add(temp);
                                    }
                            //Console.Out.WriteLine(i + "" + k + "" + d + "" + p);

                        

            //initialize current state
            cur_state = new State_bm();

            //initialize next state
            next_state = S[0];

            //generate & initialize action  a0=accept, a1=reject
            for (int a = 0; a < 2; a++)
                A.Add(new Mdpdatc.Action(a));


            //generate & initialize qfactor
            foreach (State_bm state in S)
                foreach (Action action in A)
                    Q.Add(new Qfactor(state, action));



            //generate & initialize reward
            /*
            foreach (State_bm c_state in S)
                foreach (State_bm n_state in S)
                    foreach (Action c_action in A)
                        R.Add(new Reward(c_state, n_state, c_action));
                        */
            //Console.Out.WriteLine("============");

            NN_Setup();
            max_action = new Action();
        }

        public double randomGen()
        {
            return random.NextDouble();
        }

        public void sampleAction_egreedyBM(double epsilon, State_bm state)
        {
            /*
            if (max_action != null)
            {
                if (max_action.a_idx == 2)
                    Console.WriteLine("debugging2");
               // if (max_action.a_idx == -1)
               //     Console.WriteLine("debugging-1");
            }
            */

            double rnd = randomGen();//Math.random();
            Action chosen_Action = new Action();

            List<Qfactor> qset =
                    Q.FindAll(x => (x.state_bm.b_idx == state.b_idx) &&
                                    (x.state_bm.m_idx == state.m_idx) &&
                                    (x.state_bm.lp_idx == state.lp_idx) &&
                                    (x.state_bm.lu_idx == state.lu_idx) &&
                                    (x.state_bm.dp_idx == state.dp_idx) &&
                                    (x.state_bm.du_idx == state.du_idx) &&
                                    (x.state_bm.beta_idx == state.beta_idx));

            int idx_b = -1;
            int idx_m = -1;
            int idx_lp = -1;
            int idx_lu = -1;
            int idx_dp = -1;
            int idx_du = -1;
            int idx_beta = -1;

            if (rnd >= epsilon)
            {
                double temp = -999999.99;
                foreach (Qfactor qelem in qset)
                {
                    if (qelem.qval > temp)
                    {
                        chosen_Action = qelem.action;
                        
                        temp = qelem.qval;
                        idx_b = qelem.state_bm.b_idx;
                        idx_m = qelem.state_bm.m_idx;
                        idx_lp = qelem.state_bm.lp_idx;
                        idx_lu = qelem.state_bm.lu_idx;
                        idx_dp = qelem.state_bm.dp_idx;
                        idx_du = qelem.state_bm.du_idx;
                        idx_beta = qelem.state_bm.beta_idx;
                    }
                    
                }
              
                /*
                 state.i_idx = idx_i;
                 state.k_idx = idx_k;
                 state.d_idx = idx_d;
                 cur_state = state;

                 //simulate action
                 if(chosen_Action.i_idx==0)
                     next_state
                     */
            }
            else  //e-greedy
            {
                if (rnd <= (1.0 / 2.0)) //if (rnd <= (epsilon / 2.0)) -- this was original one.. but weird
                    chosen_Action = qset[0].action;
                else
                    chosen_Action = qset[1].action;

               
            }

           
            //if exceeding capa, just reject the reqeust
            if (state.b_idx+state.beta_idx >0 && state.b_idx + state.beta_idx >= b_no)
                chosen_Action= qset[1].action;

            if (state.m_idx == m_no-1 && chosen_Action.a_idx==0)
                chosen_Action = qset[1].action;

            max_action.a_idx = chosen_Action.a_idx;

           
            
        }

        //This will be replaced with S.Lee's reward feedback part
        public Reward rewardBM(State_bm cst, State_bm nst, Action cat)
        {

            Reward temp = new Reward();
            temp.rVal = 0.1;
            /*
            temp = R.Find(x => x.cur_state_bm == cst && x.next_state_bm == nst && x.cur_action == cat);
            if (temp.indicator == -1)
                throw new Exception();
            */
            return temp;
        }

        public State_bm sampleNextStateBM(State_bm imsi_next_state, State_bm cur_state, Action chosen_action)
        {
            
            State_bm nextone = null;
            //Accept
            if (chosen_action.a_idx == 0)
            {
                //if (state.n_idx != (n_no - 1))
                //{
                    nextone = S.Find(x => x.b_idx == imsi_next_state.b_idx &&
                                              x.m_idx == imsi_next_state.m_idx &&
                                              x.lp_idx == imsi_next_state.lp_idx &&
                                              x.lu_idx == imsi_next_state.lu_idx &&
                                              x.dp_idx == imsi_next_state.dp_idx &&
                                              x.du_idx == imsi_next_state.du_idx &&
                                              x.beta_idx == imsi_next_state.beta_idx);
                //}
            }
            //reject
            else
            {
                //nextone = cur_state;
                nextone = S.Find(x => x.b_idx == imsi_next_state.b_idx && //cur_state.b_idx &&
                                              x.m_idx == imsi_next_state.m_idx && //cur_state.m_idx &&
                                              x.lp_idx == imsi_next_state.lp_idx &&
                                              x.lu_idx == imsi_next_state.lu_idx &&
                                              x.dp_idx == imsi_next_state.dp_idx &&
                                              x.du_idx == imsi_next_state.du_idx &&
                                              x.beta_idx == imsi_next_state.beta_idx);
            }
            return nextone;
        }


        //return the event
        public Event_bm simulateArrival()
        {
            double rnd = randomGen();

            //1.Arrival time simulation
            double int_arr_time = -1.0 * exp_lamda * Math.Log(rnd);
            total_time += int_arr_time;
            cur_int_arr_time = int_arr_time;

            //2.arrived_beta, arrived_del_m simulation
            //(replace)this should be replaced by the incoming parameter after the simul is implemented
            rnd = randomGen();
            double[] sector = new double[beta_no];
            for (int beta = 0; beta < beta_no; beta++)
                sector[beta] = (double)beta / (double)beta_no;

            int arrived_beta = 0;
            for (int beta = beta_no - 1; beta >= 0; beta--)
            {
                if (rnd >= sector[beta])
                {
                    arrived_beta = beta;
                    break;
                }
            }
            
            //(replace)this should be also replaced
            rnd = randomGen();

            sector = new double[lp_no];
            for (int lp = 0; lp < lp_no; lp++)
                sector[lp] = (double)lp / (double)lp_no;

            int arrived_lp = 0;
            for (int lp = lp_no - 1; lp >= 0; lp--)
            {
                if (rnd >= sector[lp])
                {
                    arrived_lp = lp;
                    break;
                }
            }

            //(replace)this should be also replaced
            rnd = randomGen();

            sector = new double[lu_no];
            for (int lu = 0; lu < lu_no; lu++)
                sector[lu] = (double)lu / (double)lu_no;

            int arrived_lu = 0;
            for (int lu = lu_no - 1; lu >= 0; lu--)
            {
                if (rnd >= sector[lu])
                {
                    arrived_lu = lu;
                    break;
                }
            }

            //(replace)this should be also replaced
            rnd = randomGen();

            sector = new double[dp_no];
            for (int dp = 0; dp < dp_no; dp++)
                sector[dp] = (double)dp / (double)dp_no;

            int arrived_dp = 0;
            for (int dp = dp_no - 1; dp >= 0; dp--)
            {
                if (rnd >= sector[dp])
                {
                    arrived_dp = dp;
                    break;
                }
            }

            //(replace)this should be also replaced
            rnd = randomGen();

            sector = new double[du_no];
            for (int du = 0; du < du_no; du++)
                sector[du] = (double)du / (double)du_no;

            int arrived_du = 0;
            for (int du = du_no - 1; du >= 0; du--)
            {
                if (rnd >= sector[du])
                {
                    arrived_du = du;
                    break;
                }
            }
            /*
            Console.Out.WriteLine(arrived_lp + ","+
                                    arrived_dp+","+
                                    arrived_lu+","+
                                    arrived_du+","+
                                    arrived_beta);
            */
            return new Event_bm(arrived_lp, arrived_dp, arrived_lu, arrived_du, arrived_beta);

        }

        public void NN_Setup()
        {
            network_accept = new FeedforwardNetwork();
            network_accept.AddLayer(new FeedforwardLayer(7));
            network_accept.AddLayer(new FeedforwardLayer(3));
            network_accept.AddLayer(new FeedforwardLayer(1));
            network_accept.Reset();

            network_reject = new FeedforwardNetwork();
            network_reject.AddLayer(new FeedforwardLayer(7));
            network_reject.AddLayer(new FeedforwardLayer(3));
            network_reject.AddLayer(new FeedforwardLayer(1));
            network_reject.Reset();

            train_accept = new Backpropagation(network_accept, 0.7, 0.9);
            train_reject = new Backpropagation(network_reject, 0.7, 0.9);
        }

        public void NN_train(State_bm in_s, Action in_a, Qfactor target_val)
        {
            //Composing input and ideal
            
            double[][] INPUTS =
            {
                new double[7] { Convert.ToDouble(in_s.b_idx), Convert.ToDouble(in_s.m_idx),
                                Convert.ToDouble(in_s.lp_idx), Convert.ToDouble(in_s.lu_idx),
                                Convert.ToDouble(in_s.dp_idx), Convert.ToDouble(in_s.du_idx),
                                Convert.ToDouble(in_s.beta_idx)}
            };
           
            double[][] IDEAL =
            {
                new double[1] {target_val.qval}
            };

            // train the neural network
            
            Console.Write("INPUTS:");
            for (int i = 0; i < 7; i++)
                Console.Write(INPUTS[0][i].ToString()+",");
            Console.Write("IDEAL:"+IDEAL[0][0]);
            Console.WriteLine("Action:" + in_a.a_idx);
            

            int epoch = 1;
            if (in_a.a_idx == 0)
            {
                train_accept.setInputVar(INPUTS);
                train_accept.setIdealVar(IDEAL);
                do
                {
                    train_accept.Iteration();
                    //Console.WriteLine("Epoch #" + epoch + " Error:" + train_accept.Error);
                    epoch++;
                } while ((epoch < 5000) && (train_accept.Error > 0.001));
            }
            else
            {
                train_reject.setInputVar(INPUTS);
                train_reject.setIdealVar(IDEAL);
                do
                {
                    train_reject.Iteration();
                    //Console.WriteLine("Epoch #" + epoch + " Error:" + train_reject.Error);
                    epoch++;
                } while ((epoch < 5000) && (train_reject.Error > 0.001)) ;
            }
            /*
            Console.WriteLine("Neural Network Results:");
            for (int i = 0; i < IDEAL.Length; i++)
            {
                double[] actual_accept = network_accept.ComputeOutputs(INPUTS[i]);
                Console.WriteLine("acc:"+INPUTS[i][0] + "," + INPUTS[i][1]
                        + ", actual=" + actual_accept[0] + ",ideal=" + IDEAL[i][0]);
            }

            for (int i = 0; i < IDEAL.Length; i++)
            {
                double[] actual_reject = network_reject.ComputeOutputs(INPUTS[i]);
                Console.WriteLine("rej:" + INPUTS[i][0] + "," + INPUTS[i][1]
                        + ", actual=" + actual_reject[0] + ",ideal=" + IDEAL[i][0]);
            }
            */
        }

        public void exec_algorithm()
        {


            //Simulate Poisson Arrival


            //Setting alpha and beta(wrote as betas for avoiding confusions)
            alpha = 0.2;
            betas = 0.2;
            double epsilon = 0.01;

            //Setting initial state
            
            int b_idx = 0;
            int m_idx = 0;
            int lp_idx = 0;
            int lu_idx = 0;
            int dp_idx = 0;
            int du_idx = 0;
            int beta_idx = 0;

            bool departure_flag = false;
            //learning
            for (int loop = 1; loop < 10000; loop++)
            {
                //1. simulate arrival process.. 
                //(replace)this part should be replaced
                //inter arrival time should be accumulated and stored somewhere!!
                Event_bm event_occurred = this.simulateArrival();
                lp_idx = event_occurred.lp_idx;
                lu_idx = event_occurred.lu_idx;
                dp_idx = event_occurred.dp_idx;
                du_idx = event_occurred.du_idx;
                beta_idx = event_occurred.beta_idx;

                departure_flag = false;
                //2. check RC to see if there are any departures since the last decision epoch.
                //3. In case of departure events, update current capacity(b) and msd(m) information. 
                //cur_state represents the current loading information + event
                //(replace) code below will be replaced
                if (cur_state.b_idx > 0)
                {   
                    double rnd = randomGen();
                    if (rnd > 0.6)
                    {
                        departure_flag = true;
                        Console.WriteLine("departed");                       
                        cur_state.b_idx = cur_state.b_idx - 1;
                        if (cur_state.m_idx > 0)
                            cur_state.m_idx = cur_state.m_idx - 1;
                    }
                    
                }

                if (!departure_flag)
                {
                    //if (Q[1].action.a_idx != 0 && Q[1].action.a_idx != 1)
                   //     Console.WriteLine("something changed");

                    //max_action will be determined
                    //max_action.a_idx = -100;

                   // if (Q[1].action.a_idx != 0 && Q[1].action.a_idx != 1)
                   //     Console.WriteLine("something changed");

                    this.sampleAction_egreedyBM(epsilon, cur_state);

                    

                    State_bm imsi_next_state = new State_bm();
                    if (max_action.a_idx == 0)
                    {
                        //4. With ‘Accept’ flag, OAC sends RC the following information:  
                        //weight, due date(pickup, delivery), traveling distance(pickup, delivery)
                        //(replace)the below 2 should be replaced!!
                        int ret_b = cur_state.b_idx + cur_state.beta_idx;
                        int ret_m = cur_state.m_idx + 1;
                        //(need to implement) also need to set the reward value

                        imsi_next_state = new State_bm(ret_b, ret_m, lp_idx, lu_idx, dp_idx, du_idx, beta_idx);
                    }else
                    {
                        int ret_b = cur_state.b_idx;
                        int ret_m = cur_state.m_idx;
                        //(need to implement) also need to set the reward value

                        imsi_next_state = new State_bm(ret_b, ret_m, lp_idx, lu_idx, dp_idx, du_idx, beta_idx);
                    }

                    //5. With all information provided by RC, 
                    //OAC calculates Q-value for the following state-action pair: 
                    //Q(S, accept), Q(S, reject)

                    /*
                    State_bm found_cur_imsi_state;

                    cur_state = S.Find(x => x.b_idx == imsi_state.b_idx &&
                                                  x.m_idx == imsi_state.m_idx &&
                                                  x.lp_idx == imsi_state.lp_idx &&
                                                  x.lu_idx == imsi_state.lu_idx &&
                                                  x.dp_idx == imsi_state.dp_idx &&
                                                  x.du_idx == imsi_state.du_idx &&
                                                  x.beta_idx == imsi_state.beta_idx);


                    if (cur_state == null) throw new Exception();
                    */



                    next_state = this.sampleNextStateBM(imsi_next_state, cur_state, max_action);

                    

                    Qfactor cur_q = Q.Find(x => x.state_bm.b_idx == cur_state.b_idx &&
                                                x.state_bm.m_idx == cur_state.m_idx &&
                                                x.state_bm.lp_idx == cur_state.lp_idx &&
                                                x.state_bm.lu_idx == cur_state.lu_idx &&
                                                x.state_bm.dp_idx == cur_state.dp_idx &&
                                                x.state_bm.du_idx == cur_state.du_idx &&
                                                x.state_bm.beta_idx == cur_state.beta_idx &&
                                                x.action.a_idx == max_action.a_idx);

                    total_reward += rewardBM(cur_state, next_state, max_action).rVal;
                    rho = (1.00 - betas) * rho + betas * total_reward / total_time;
                    //Just regular MDP with Relative Q-Learning
                    cur_q.qval = (1 - alpha) * cur_q.qval + alpha * (rewardBM(cur_state, next_state, max_action).rVal - rho * cur_int_arr_time + eta * maxQ(cur_state));

                    NN_train(cur_state, max_action, cur_q);

                   

                    cur_state.b_idx = next_state.b_idx;
                    cur_state.m_idx = next_state.m_idx;
                    cur_state.lp_idx = next_state.lp_idx;
                    cur_state.lu_idx = next_state.lu_idx;
                    cur_state.dp_idx = next_state.dp_idx;
                    cur_state.du_idx = next_state.du_idx;
                    cur_state.beta_idx = next_state.beta_idx;

                    //Console.Out.WriteLine(next_state.k_idx + "==" + next_state.d_idx);
                    //9. OAC updates capacity information and go to 2 until loop limit is reached

                    if (loop % 1000 == 0)
                        Console.Out.WriteLine(loop);
                }
            }

            int zeroval_count = 0;
            int non_zeroval_count = 0;


            foreach (Qfactor qelem in Q)
            {
                double pred_nn = 0.00;
                if (qelem.qval != 0)
                {
                    Console.Out.Write(qelem.state_bm.b_idx + "," +
                                            qelem.state_bm.m_idx + "," +
                                            qelem.state_bm.lp_idx + "," +
                                            qelem.state_bm.lu_idx + "," +
                                            qelem.state_bm.dp_idx + "," +
                                            qelem.state_bm.du_idx + "," +
                                            qelem.state_bm.beta_idx + "," +
                                            qelem.action.a_idx + "=" + qelem.qval);

                    if(qelem.action.a_idx == 0)
                        pred_nn = (network_accept.ComputeOutputs(new double[7] { qelem.state_bm.b_idx, qelem.state_bm.m_idx,
                                                              qelem.state_bm.lp_idx, qelem.state_bm.lu_idx,
                                                              qelem.state_bm.dp_idx, qelem.state_bm.du_idx,
                                                              qelem.state_bm.beta_idx}))[0];
                    else
                        pred_nn = (network_reject.ComputeOutputs(new double[7] { qelem.state_bm.b_idx, qelem.state_bm.m_idx,
                                                              qelem.state_bm.lp_idx, qelem.state_bm.lu_idx,
                                                              qelem.state_bm.dp_idx, qelem.state_bm.du_idx,
                                                              qelem.state_bm.beta_idx}))[0];
                    Console.Out.WriteLine(" error="+ (qelem.qval - pred_nn) + " error percent=" + (qelem.qval - pred_nn)/ qelem.qval);
                }

                if (qelem.qval == 0)
                    zeroval_count++;
                else
                    non_zeroval_count++;

            }

            Console.Out.WriteLine("zero:" + zeroval_count + "non_zero:" + non_zeroval_count);
        }

        public double maxQ(State_bm state)
        {
            List<Qfactor> qlist = Q.FindAll(x => x.state_bm.b_idx == state.b_idx &&
                                                 x.state_bm.m_idx == state.m_idx &&
                                                 x.state_bm.lp_idx == state.lp_idx &&
                                                 x.state_bm.lu_idx == state.lu_idx &&
                                                 x.state_bm.dp_idx == state.dp_idx &&
                                                 x.state_bm.du_idx == state.du_idx &&
                                                 x.state_bm.beta_idx == state.beta_idx);
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
