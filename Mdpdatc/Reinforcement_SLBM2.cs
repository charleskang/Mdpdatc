using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.Neural.Feedforward;
using Encog.Neural.Feedforward.Train;
using Encog.Neural.Feedforward.Train.Backpropagation;
using Encog.Util;
using DATCL;

namespace Mdpdatc
{
    //Complete NN
    class Reinforcement_SLBM2
    {

        //public double[,] q;

        //public double[,] v;

        public Random random;

        public int seed = 1;

        public double b_no = 10;
        public double m_no = 10;
        public double lp_no = 5;
        public double lu_no = 5;
        public double dp_no = 5;
        public double du_no = 5;
        public double beta_no = 5;

        //public int d_no = 10;
        //public int n_no = 10;

        //public List<State_bm> S;
        public List<Action> A;
        //public List<Qfactor> Q;
        //public List<Reward> R;

        public State_nn cur_state;// = S[0];
        public State_nn next_state;
        public Action max_action;

        public Qfactor_nn cur_q_accept;
        public Qfactor_nn cur_q_reject;

        public double alpha;
        public double betas;

        public double exp_lamda = 5.0; //on average, arrivals occured every 5 minutes
        public double abs_time = 0.00;

        public double eta = 0.99;
        public double total_time = 0.00;
        public double total_reward = 0.00;
        public double rho = 0.00;
        public double cur_int_arr_time = 0.00;

        public double init_pos_x = 50.0;
        public double init_pos_y = 50.0;
        public double curr_pos_x;
        public double curr_pos_y;
        public double p_pos_x;
        public double p_pos_y;
        public double d_pos_x;
        public double d_pos_y;

        public double curr_time=9.00;
      

        public FeedforwardNetwork network_accept;
        public FeedforwardNetwork network_reject;

        public Train train_accept;
        public Train train_reject;

        public DATCWrapper datc_ctrl;

        public Reinforcement_SLBM2()
        {
            datc_ctrl = new DATCWrapper();
            //double kk = datc_ctrl.test(0.1);
            //datc_ctrl.main_from_external();
            datc_ctrl.InitialRunDATC();
            // double tt = datc_ctrl.GetCurrentCapacity(1);

            initialize();
            //this.q = new double[3, 2];

            //this.v = new double[3, 2];
            exec_algorithm();

        }

        public void initialize()
        {
            random = new Random();

            //S = new List<State_bm>();
            //S.Clear();
            cur_state = new State_nn();
            A = new List<Action>();
            //Q = new List<Qfactor>();
            //R = new List<Reward>();
            //generte & initialize state


            //initialize next state
            next_state = new State_nn();

            //generate & initialize action  a0=accept, a1=reject
            for (int a = 0; a < 2; a++)
                A.Add(new Mdpdatc.Action(a));

            max_action = new Action();

            //generate & initialize qfactor

            cur_q_accept = new Qfactor_nn(cur_state, A[0]);
            cur_q_reject = new Qfactor_nn(cur_state, A[1]);
           

            //generate & initialize reward
            /*
            foreach (State_bm c_state in S)
                foreach (State_bm n_state in S)
                    foreach (Action c_action in A)
                        R.Add(new Reward(c_state, n_state, c_action));
                        */
            //Console.Out.WriteLine("============");

            NN_Setup();
            
        }

        public double randomGen()
        {
            return random.NextDouble();
        }

        public void sampleAction_egreedyNN(double epsilon, State_nn state) //this state means current state
        {

            double rnd = randomGen();//Math.random();
            Action chosen_Action = new Action();

            
            //retrieve Q for the current state
            cur_q_accept.setQfactor_nn(state, A[0], comp_NN(state, A[0]));

            cur_q_reject.setQfactor_nn(state, A[1], comp_NN(state, A[1]));

            if (cur_q_accept.qval < cur_q_reject.qval)
                Console.Write("****rej_pref*********");
            Console.Write("acc_q=" + cur_q_accept.qval + "rej_q=" + cur_q_reject.qval);
            
            /*
            List<Qfactor> qset =
                    Q.FindAll(x => (x.state_bm.b_idx == state.b_idx) &&
                                    (x.state_bm.m_idx == state.m_idx) &&
                                    (x.state_bm.lp_idx == state.lp_idx) &&
                                    (x.state_bm.lu_idx == state.lu_idx) &&
                                    (x.state_bm.dp_idx == state.dp_idx) &&
                                    (x.state_bm.du_idx == state.du_idx) &&
                                    (x.state_bm.beta_idx == state.beta_idx));
            */


            if (rnd >= epsilon)
            {
                if(cur_q_accept.qval >= cur_q_reject.qval)
                    chosen_Action = A[0];
                else
                    chosen_Action = A[1];
            }
            else  //e-greedy
            {
                Console.Write("[EG]");
                if (rnd <= (1.0 / 2.0)) //if (rnd <= (epsilon / 2.0)) -- this was original one.. but weird
                    chosen_Action = A[0];
                else
                    chosen_Action = A[1];

            }

           // Console.Write("a[1]=" + A[1].a_idx);


            //max_action.a_idx = chosen_Action.a_idx;
            max_action = chosen_Action;
            //Console.Write("a[1]=" + A[1].a_idx);


        }

        //This will be replaced with S.Lee's reward feedback part
        public Reward rewardNN(State_nn cst, State_nn nst, Action cat)
        {

            Reward temp = new Reward();
            double unit_price = 10.00;
            double price = (nst.b_idx - cst.b_idx) * unit_price;
            double cost_func = (nst.m_idx - cst.m_idx) * 10.00;
            
            temp.rVal = price - cost_func;
            //temp.rVal = 0.1;
            //if (cat.a_idx == 0)
            //    temp.rVal = 0.2;
            /*
            temp = R.Find(x => x.cur_state_bm == cst && x.next_state_bm == nst && x.cur_action == cat);
            if (temp.indicator == -1)
                throw new Exception();
            */
            return temp;
        }

        /*
        public State_nn sampleNextStateNN(State_nn imsi_next_state, State_nn cur_state, Action chosen_action)
        {

            State_nn nextone = null;

            return new State_nn(imsi_next_state.b_idx,
                imsi_next_state.m_idx
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
        */

        //return the event
        public Event_bm simulateArrival()
        {
            double rnd = randomGen();

            //1.Arrival time simulation
            double int_arr_time = -1.0 * exp_lamda * Math.Log(rnd);
            total_time += int_arr_time;
            cur_int_arr_time = int_arr_time;

            curr_time += int_arr_time;

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

            //lp: length pickup 
            //dp: due pickup
            //lu: length delivery 
            //du: due delivery
            //generating pick-up and delivery position
            rnd = randomGen();
            p_pos_x = rnd*100;
            rnd = randomGen();
            p_pos_y = rnd * 100;
            rnd = randomGen();
            d_pos_x = rnd * 100;
            rnd = randomGen();
            d_pos_y = rnd * 100;

            double arrived_lp = 0.00;
            double arrived_lu = 0.00;
            arrived_lp = Math.Sqrt(Math.Pow(curr_pos_x - p_pos_x, 2) + Math.Pow(curr_pos_y - p_pos_y, 2));
            arrived_lu = Math.Sqrt(Math.Pow(d_pos_x - p_pos_x, 2) + Math.Pow(d_pos_y - p_pos_y, 2));

            double arrived_dp = 0.00;
            double arrived_du = 0.00;
            rnd = randomGen();
            arrived_dp = curr_time + rnd;
            rnd = randomGen();
            arrived_du = arrived_dp + rnd;

            //(replace)this should be also replaced
            /*
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
            */
            
            Console.Out.WriteLine(arrived_lp + ","+
                                    arrived_dp+","+
                                    arrived_lu+","+
                                    arrived_du+","+
                                    arrived_beta);
            
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

        public void NN_train(State_nn in_s, Action in_a, Qfactor_nn target_val)
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
                Console.Write(INPUTS[0][i].ToString() + ",");
            Console.Write("IDEAL:" + IDEAL[0][0]);
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
                } while ((epoch < 10) && (train_accept.Error > 0.001));
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
                } while ((epoch < 10) && (train_reject.Error > 0.001));
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

        public double comp_NN(State_nn state, Action action)
        {
            double retval = 0.00;

            double[] compose = new double[7] {Convert.ToDouble(state.b_idx)/Convert.ToDouble(b_no-1), Convert.ToDouble(state.m_idx)/Convert.ToDouble(m_no-1),
                                Convert.ToDouble(state.lp_idx)/Convert.ToDouble(lp_no-1), Convert.ToDouble(state.lu_idx)/Convert.ToDouble(lu_no-1),
                                Convert.ToDouble(state.dp_idx)/Convert.ToDouble(dp_no-1), Convert.ToDouble(state.du_idx)/Convert.ToDouble(du_no-1),
                                Convert.ToDouble(state.beta_idx)/Convert.ToDouble(beta_no-1)};
            if (action.a_idx == 0)
                retval = network_accept.ComputeOutputs(compose)[0];
            else if (action.a_idx == 1)
                retval = network_reject.ComputeOutputs(compose)[0];
            else
                throw new Exception();

            return retval;
        }

        public void exec_algorithm()
        {

            List<State_nn> state_history = new List<State_nn>();
            //Simulate Poisson Arrival


            //Setting alpha and beta(wrote as betas for avoiding confusions)
            alpha = 0.2;
            betas = 0.2;
            double epsilon = 0.01;

            //Setting initial state

            int b_idx = 0;
            int m_idx = 0;
            double lp_idx = 0.00;
            double lu_idx = 0.00;
            double dp_idx = 0.00;
            double du_idx = 0.00;
            double beta_idx = 0.00;

            //bool departure_flag = false;
            //learning
            for (int loop = 1; loop < 10000; loop++)
            {
                //1. simulate arrival process.. 
                //(replace)this part should be replaced
                //inter arrival time should be accumulated and stored somewhere!!
                Event_bm event_occurred = this.simulateArrival();
                lp_idx = event_occurred.db_lp_idx;
                lu_idx = event_occurred.db_lu_idx;
                dp_idx = event_occurred.db_dp_idx;
                du_idx = event_occurred.db_du_idx;
                beta_idx = event_occurred.db_beta_idx;


                

                //2. check RC to see if there are any departures since the last decision epoch.
                //Regardless of whether there was departure or not, update current capacity(b) and msd(m) information. 
                //cur_state represents the current loading information + event
                //(replace) code below will be replaced

                double cur_capa = datc_ctrl.GetCurrentCapacity(curr_time);
                double cur_msd = datc_ctrl.GetCurrentMSD(curr_time);

                cur_state.b_idx = cur_capa;
                cur_state.m_idx = cur_msd;

                this.sampleAction_egreedyNN(epsilon, cur_state);

                //if exceeding capa, just reject the reqeust
                //Console.Write("chk1=" + cur_state.b_idx +" "+ beta_idx);
                //Console.Write("a1="+max_action.a_idx);
                if ((cur_state.b_idx + beta_idx) >= b_no)
                    max_action = A[1];

                //if (cur_state.m_idx == (m_no - 1) && max_action.a_idx == 0)
                //    max_action = A[1];

                //Console.Write("chk2=" + cur_state.b_idx + " " + beta_idx);
                //Console.Write("a2=" + max_action.a_idx);

                Qfactor_nn cur_q;
                if (max_action.a_idx == 0)
                    cur_q = cur_q_accept;
                else
                    cur_q = cur_q_reject;

                State_nn imsi_next_state = new State_nn();
                if (max_action.a_idx == 0)
                {
                    //4. With ‘Accept’ flag, OAC sends RC the following information:  
                    //weight, due date(pickup, delivery), traveling distance(pickup, delivery)
                    //(replace)the below 2 should be replaced!!

                    datc_ctrl.InterRunDATC(beta_idx, dp_idx, dp_idx + 0.1, 0.1,
                        p_pos_x, p_pos_y, du_idx, du_idx + 0.1, 0.1,
                        d_pos_x, d_pos_y, curr_time);

                    double ret_b = datc_ctrl.GetCurrentCapacity(curr_time);
                    double ret_m = datc_ctrl.GetCurrentMSD(curr_time);
                    //(need to implement) also need to set the reward value
                   
                    imsi_next_state = new State_nn(ret_b, ret_m, lp_idx, lu_idx, dp_idx, du_idx, beta_idx);
                }
                else
                {
                    double ret_b = cur_state.b_idx;
                    double ret_m = cur_state.m_idx;
                    //(need to implement) also need to set the reward value

                    imsi_next_state = new State_nn(ret_b, ret_m, lp_idx, lu_idx, dp_idx, du_idx, beta_idx);
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



                //next_state = this.sampleNextStateNN(imsi_next_state, cur_state, max_action);

                next_state = imsi_next_state;

                    

                /*
                Qfactor cur_q = Q.Find(x => x.state_bm.b_idx == cur_state.b_idx &&
                                            x.state_bm.m_idx == cur_state.m_idx &&
                                            x.state_bm.lp_idx == cur_state.lp_idx &&
                                            x.state_bm.lu_idx == cur_state.lu_idx &&
                                            x.state_bm.dp_idx == cur_state.dp_idx &&
                                            x.state_bm.du_idx == cur_state.du_idx &&
                                            x.state_bm.beta_idx == cur_state.beta_idx &&
                                            x.action.a_idx == max_action.a_idx);
                */
                total_reward += rewardNN(cur_state, next_state, max_action).rVal;
                rho = (1.00 - betas) * rho + betas * total_reward / total_time;
                //Just regular MDP with Relative Q-Learning
                cur_q.qval = (1 - alpha) * cur_q.qval + alpha * (rewardNN(cur_state, next_state, max_action).rVal - rho * cur_int_arr_time + eta * maxQ(next_state));

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

                //state_history.Add(cur_state);
                
            }

            //int zeroval_count = 0;
            int non_zeroval_count = 0;

            //**************************************************
            //**************************************************
            /*
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

                    if (qelem.action.a_idx == 0)
                        pred_nn = (network_accept.ComputeOutputs(new double[7] { qelem.state_bm.b_idx, qelem.state_bm.m_idx,
                                                              qelem.state_bm.lp_idx, qelem.state_bm.lu_idx,
                                                              qelem.state_bm.dp_idx, qelem.state_bm.du_idx,
                                                              qelem.state_bm.beta_idx}))[0];
                    else
                        pred_nn = (network_reject.ComputeOutputs(new double[7] { qelem.state_bm.b_idx, qelem.state_bm.m_idx,
                                                              qelem.state_bm.lp_idx, qelem.state_bm.lu_idx,
                                                              qelem.state_bm.dp_idx, qelem.state_bm.du_idx,
                                                              qelem.state_bm.beta_idx}))[0];
                    Console.Out.WriteLine(" error=" + (qelem.qval - pred_nn) + " error percent=" + (qelem.qval - pred_nn) / qelem.qval);
                }

                if (qelem.qval == 0)
                    zeroval_count++;
                else
                    non_zeroval_count++;

            }
            */

            Console.Out.WriteLine("non_zero:" + non_zeroval_count);

            SaveTrainedNN(network_accept, "accept" );
            SaveTrainedNN(network_reject, "reject" );

            //FeedforwardNetwork temp = (FeedforwardNetwork)SerializeObject.Load("accept");
        }

        public void SaveTrainedNN(FeedforwardNetwork network, string filename)
        {
            SerializeObject.Save(filename, network);
        }

        public double maxQ(State_nn state)  //next states
        {
            double retVal = -1000000.00;
            double accept_qval = comp_NN(state, A[0]);
            double reject_qval = comp_NN(state, A[1]);
            if (accept_qval >= reject_qval)
                retVal = accept_qval;
            else
                retVal = reject_qval;

            /*
                List<Qfactor> qlist = Q.FindAll(x => x.state_bm.b_idx == state.b_idx &&
                                                 x.state_bm.m_idx == state.m_idx &&
                                                 x.state_bm.lp_idx == state.lp_idx &&
                                                 x.state_bm.lu_idx == state.lu_idx &&
                                                 x.state_bm.dp_idx == state.dp_idx &&
                                                 x.state_bm.du_idx == state.du_idx &&
                                                 x.state_bm.beta_idx == state.beta_idx);
            
            if (qlist.Count == 0) throw new Exception();
            foreach (Qfactor qelem in qlist)
            {
                if (retVal < qelem.qval)
                    retVal = qelem.qval;
            }
            */
            return retVal;
        }


    }
}
