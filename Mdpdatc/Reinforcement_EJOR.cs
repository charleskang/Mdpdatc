//Reinforcement_EJOR.cs
//1.25.2017 modifying step_alpha
//2.15.2017 compared with original c-code(in Macbook) and confirmed this code is working correctly

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mdpdatc
{
	class Reinforcement_EJOR
	{
		public double[,] tp_a1;
		public double[,] tp_a2;
		public double[,] r_a1;
		public double[,] r_a2;

		public double[,] q;

		public double[,] v;

		public State_eg s1, s2, s3, cur_st, next_st;
		public Action_eg a1, a2, cur_act;

		public double big_a = 10.00;
		public double big_b = 100.00;
		public double lamda = 0.99;

		//resulting action

		public int count_111 = 0;
		public int count_112 = 0;
		public int count_121 = 0;
		public int count_122 = 0;
		public int count_211 = 0;
		public int count_212 = 0;
		public int count_221 = 0;
		public int count_222 = 0;

		public int count_s11 = 0;
		public int count_s12 = 0;
		public int count_s13 = 0;
		public int count_s21 = 0;
		public int count_s22 = 0;
		public int count_s23 = 0;
		public int count_s31 = 0;
		public int count_s32 = 0;
		public int count_s33 = 0;

		public Random random;

		public int seed = 1;
		public Reinforcement_EJOR()
		{
			//random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
			//random = new Random();

			initialize();
			this.q = new double[3, 2];

			this.v = new double[3, 2];

		}

		public void initialize()
		{
			s1 = new State_eg("s1");
			s2 = new State_eg("s2");
			s3 = new State_eg("s3");

			a1 = new Action_eg("a1");
			a2 = new Action_eg("a2");

			this.tp_a1 = new double[3, 3];
			this.tp_a2 = new double[3, 3];

			this.r_a1 = new double[3, 3];
			this.r_a2 = new double[3, 3];



			//random = new Random(Guid.NewGuid().GetHashCode());
			//seed++;
		}

		public void populateTps()
		{
			this.tp_a1[0, 0] = 0.7208;
			this.tp_a1[0, 1] = 0.0719;
			this.tp_a1[0, 2] = 0.2073;
			this.tp_a1[1, 0] = 0.2955;
			this.tp_a1[1, 1] = 0.1420;
			this.tp_a1[1, 2] = 0.5625;
			this.tp_a1[2, 0] = 0.1144;
			this.tp_a1[2, 1] = 0.0578;
			this.tp_a1[2, 2] = 0.8278;

			this.tp_a2[0, 0] = 0.0420;// 0.001465;//0.586055;//
			this.tp_a2[0, 1] = 0.1496; // 0.355905;//0.0719;//
			this.tp_a2[0, 2] = 0.8085;// 0.64263;//0.342045;//
			this.tp_a2[1, 0] = 0.0002;// 0.001344;//0.2955;//
			this.tp_a2[1, 1] = 0.0002;// 0.001344;//0.007255;//
			this.tp_a2[1, 2] = 0.9996;// 0.997313;//0.697245;//
			this.tp_a2[2, 0] = 0.00;//0.1144;//
			this.tp_a2[2, 1] = 0.00;//0.0578;//
			this.tp_a2[2, 2] = 1.00;//0.8278;//
								 /*
								 this.tp_a2[0, 0] = 0.001465;
								 this.tp_a2[0, 1] = 0.355905;
								 this.tp_a2[0, 2] = 0.64263;
								 this.tp_a2[1, 0] = 0.001344;
								 this.tp_a2[1, 1] = 0.001344;
								 this.tp_a2[1, 2] = 0.997313;
								 this.tp_a2[2, 0] = 0;
								 this.tp_a2[2, 1] = 0;
								 this.tp_a2[2, 2] = 1;
								  * */


		}

		public void populateRwds()
		{


			this.r_a1[0, 0] = 101.38;
			this.r_a1[0, 1] = 101.38;
			this.r_a1[0, 2] = 109.6;
			this.r_a1[1, 0] = 101.38;
			this.r_a1[1, 1] = 101.38; //12;
			this.r_a1[1, 2] = 109.6;
			this.r_a1[2, 0] = 101.38;
			this.r_a1[2, 1] = 101.38; //12;
			this.r_a1[2, 2] = 109.6;

			this.r_a2[0, 0] = 88.28;
			this.r_a2[0, 1] = 88.28;
			this.r_a2[0, 2] = 96.5;
			this.r_a2[1, 0] = 88.28;
			this.r_a2[1, 1] = 88.28; //12;
			this.r_a2[1, 2] = 96.5;
			this.r_a2[2, 0] = 88.28;
			this.r_a2[2, 1] = 88.28; //12;
			this.r_a2[2, 2] = 96.5;

		}

		public double randomGen()
		{
			//random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
			//random = new Random(Guid.NewGuid().GetHashCode());
			double ret = random.NextDouble();
			// Console.WriteLine(ret);
			return ret;
		}

		//Simulator
		public State_eg sampleNextState(State_eg curr_state, Action_eg exe_action)
		{
			State_eg next_state = null;

			double rnd = randomGen();//Math.random();

			if (exe_action.idx == 0)
			{
				if (rnd <= tp_a1[curr_state.idx, s1.idx])
					next_state = s1;
				else if (rnd <= (tp_a1[curr_state.idx, s1.idx] + tp_a1[curr_state.idx, s2.idx]))
					next_state = s2;
				else if (rnd <= (tp_a1[curr_state.idx, s1.idx] + tp_a1[curr_state.idx, s2.idx] + tp_a1[curr_state.idx, s3.idx]))
					next_state = s3;
				else
					Console.WriteLine("Something Wrong 85");
			}
			else if (exe_action.idx == 1)
			{
				if (rnd <= (tp_a2[curr_state.idx, s1.idx]))
					next_state = s1;
				else if (rnd <= (tp_a2[curr_state.idx, s1.idx] + tp_a2[curr_state.idx, s2.idx]))
					next_state = s2;
				else if (rnd <= (tp_a2[curr_state.idx, s1.idx] + tp_a2[curr_state.idx, s2.idx] + tp_a2[curr_state.idx, s3.idx]))
					next_state = s3;
				else
					Console.WriteLine("Something Wrong 94");
			}
			else
				throw new Exception();
			/*
            if (exe_action.idx == 1)
            {
                int cur_st_idx = curr_state.idx;
                int next_st_idx = next_state.idx;
                string st_combi = (cur_st_idx + 1).ToString() + (next_st_idx + 1).ToString();
                switch (st_combi)
                {
                    case ("11"):
                        count_s11++;
                        break;
                    case ("12"):
                        count_s12++;
                        break;
                    case ("13"):
                        count_s13++;
                        break;
                    case ("21"):
                        count_s21++;
                        break;
                    case ("22"):
                        count_s22++;
                        break;
                    case ("23"):
                        count_s23++;
                        break;
                    case ("31"):
                        count_s31++;
                        break;
                    case ("32"):
                        count_s32++;
                        break;
                    case ("33"):
                        count_s33++;
                        break;
                    default:
                        throw new Exception();
                        break;
                }
            }
             * */


			return next_state;
		}

		public void sampleAction_eqprob()
		{

			double rnd = randomGen();//Math.random();

			if (rnd < 0.5) cur_act = this.a1;
			else if (rnd >= 0.5 && rnd <= 1)
				cur_act = this.a2;
			else
				Console.WriteLine("eqprob error");


		}


		public void sampleAction_egreedy(double epsilon)
		{
			//int act_num = 2;

			double rnd = randomGen();//Math.random();

			// if(rnd > epsilon) cur_act = 
			if (q[cur_st.idx, 0] > q[cur_st.idx, 1])
			{
				if (rnd > epsilon) cur_act = this.a1;
				else cur_act = this.a2;
			}
			else if (q[cur_st.idx, 0] <= q[cur_st.idx, 1])
			{
				if (rnd > epsilon) cur_act = this.a2;
				else cur_act = this.a1;
			}
			else
				Console.WriteLine("greedy error");


		}



		public double reward(State_eg cst, Action_eg cat, State_eg nst)
		{
			if (cat.idx == 0)
				return this.r_a1[cst.idx, nst.idx];
			else if (cat.idx == 1)
				return this.r_a2[cst.idx, nst.idx];
			else
				throw new Exception();

		}
		/**
         * @param args the command line arguments
         */

		public void entry_algorithm()
		{
			random = new Random();

			for (int t = 0; t < 1; t++)
			{
				Console.WriteLine(t + "th iteration");
				for (int k = 0; k < 10; k++)
				{

					exec_algorithm();
					//initialize();
                                        

					for (int i = 0; i < 3; i++)
						for (int j = 0; j < 2; j++)
							Console.WriteLine(i + "," + j + "=" + q[i, j]);
					for (int i = 0; i < 3; i++)
						Console.WriteLine(i + " diff=" + (q[i, 0] - q[i, 1]));

					this.q = new double[3, 2];
					this.v = new double[3, 2];
				}

				Console.WriteLine("111:" + count_111);
				Console.WriteLine("112:" + count_112);
				Console.WriteLine("121:" + count_121);
				Console.WriteLine("122:" + count_122);
				Console.WriteLine("211:" + count_211);
				Console.WriteLine("212:" + count_212);
				Console.WriteLine("221:" + count_221);
				Console.WriteLine("222:" + count_222);

				/*
                Console.WriteLine("s11:" + count_s11);
                Console.WriteLine("s12:" + count_s12);
                Console.WriteLine("s13:" + count_s13);
                Console.WriteLine("s21:" + count_s21);
                Console.WriteLine("s22:" + count_s22);
                Console.WriteLine("s23:" + count_s23);
                Console.WriteLine("s31:" + count_s31);
                Console.WriteLine("s32:" + count_s32);
                Console.WriteLine("s33:" + count_s33);
                 * */
			}
		}

		public State_eg initialState()
		{
			double rnd = randomGen();
			State_eg ret = null;
			if (rnd <= 0.333)
				ret = new State_eg("s1");
			else if (rnd <= 0.667)
				ret = new State_eg("s2");
			else if (rnd <= 1)
				ret = new State_eg("s3");
			else
				Console.WriteLine("initialstate error");

			return ret;

		}


		public void exec_algorithm()
		{

			this.populateTps();
			this.populateRwds();


			this.cur_st = initialState();
			//start learning
			for (int loop = 1; loop < 2000000; loop++)
			{
				//{
				//select an action (0.5 for each action)
				//dbeug(1208)
				//this.cur_act = a1;
				this.sampleAction_eqprob();
				//this.sampleAction_egreedy(0.001); not working well when state # is 3
				//seems like depending on the actions at the initial stages....
				//sample next state
				//debug(1208)
				//next_st = s2;
				next_st = this.sampleNextState(cur_st, cur_act);
				//update q-factor and v-factor (off-policy)
				double maxq = -1000000.00;
				for (int i = 0; i < 2; i++)
					if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

				v[cur_st.idx, cur_act.idx]++;
				double step_alpha = 0.00;
                //step_alpha = (double)(big_a / (big_b + (double)loop)); //v[cur_st.idx, cur_act.idx]);
                //step_alpha = (big_a / (big_b + loop));
                //step_alpha = (double)(Math.Log(v[cur_st.idx, cur_act.idx])/(double)v[cur_st.idx, cur_act.idx]);
                step_alpha = Math.Log(loop) / (double)loop;

                calculateq_discount(cur_st, cur_act, next_st, step_alpha, maxq);

				// System.out.println(cur_st.idx+":"+next_st.idx+":"+cur_act.idx+"="+reward(cur_st,cur_act,next_st)+"q:="+q[cur_st.idx][cur_act.idx]);
				// if(cur_st.idx==0 && cur_act.idx==0) 
				//     System.out.println(q[cur_st.idx][cur_act.idx]);

				this.cur_st = next_st;
			}
			/*
			#region
			//phase 2
			this.cur_act = a2;
			this.next_st = s2;
			 maxq = 0.00;
			for (int i = 0; i < 2; i++)
				if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

			v[cur_st.idx, cur_act.idx]++;
			 step_alpha = 0.00;
			step_alpha = big_a / v[cur_st.idx, cur_act.idx];
			calculateq(cur_st, cur_act, next_st, step_alpha, maxq);
			this.cur_st = next_st;

			//phase 3
			this.cur_act = a1;
			this.next_st = s1;
			 maxq = 0.00;
			for (int i = 0; i < 2; i++)
				if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

			v[cur_st.idx, cur_act.idx]++;
			step_alpha = 0.00;
			step_alpha = big_a / v[cur_st.idx, cur_act.idx];
			calculateq(cur_st, cur_act, next_st, step_alpha, maxq);
			this.cur_st = next_st;

			//phase 4
			this.cur_act = a2;
			this.next_st = s2;
			maxq = 0.00;
			for (int i = 0; i < 2; i++)
				if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

			v[cur_st.idx, cur_act.idx]++;
			//step_alpha = 0.00;
			step_alpha = big_a / v[cur_st.idx, cur_act.idx];
			calculateq(cur_st, cur_act, next_st, step_alpha, maxq);
			this.cur_st = next_st;

			//phase 5
			this.cur_act = a2;
			this.next_st = s1;
			maxq = 0.00;
			for (int i = 0; i < 2; i++)
				if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

			v[cur_st.idx, cur_act.idx]++;
			step_alpha = 0.00;
			step_alpha = big_a / v[cur_st.idx, cur_act.idx];
			calculateq(cur_st, cur_act, next_st, step_alpha, maxq);
			this.cur_st = next_st;


			#endregion
*/


			/*
            
			for (int k = 0; k < 3; k++)
				for (int t = 0; t < 2; t++)
					Console.WriteLine((k + 1) + "," + (t + 1) + ":" + q[k, t]);
            */
			/*
            for (int k = 0; k < 3; k++)
                for (int t = 0; t < 2; t++)
                    Console.WriteLine((k + 1) + "," + (t + 1) + ":" + v[k, t]);
            */
			string resultaction = "";

			if (q[0, 0] >= q[0, 1])
				resultaction += "1";
			else
				resultaction += "2";

			if (q[1, 0] >= q[1, 1])
				resultaction += "1";
			else
				resultaction += "2";

			if (q[2, 0] >= q[2, 1])
				resultaction += "1";
			else
				resultaction += "2";

			switch (resultaction)
			{
				case ("111"):
					this.count_111++;
					break;
				case ("112"):
					this.count_112++;
					break;
				case ("121"):
					this.count_121++;
					break;
				case ("122"):
					this.count_122++;
					break;
				case ("211"):
					this.count_211++;
					break;
				case ("212"):
					this.count_212++;
					break;
				case ("221"):
					this.count_221++;
					break;
				case ("222"):
					this.count_222++;
					break;
				default:
					throw new Exception();
			}



		}

		public void calculateq_discount(State_eg cst, Action_eg cat, State_eg nst, Double step_alpha, Double maxq)
		{
			// double before = q[cst.idx, cat.idx];
			q[cst.idx, cat.idx] = (1 - step_alpha) * q[cst.idx, cat.idx] + step_alpha * (reward(cst, cat, nst) + lamda * maxq);
			//Console.WriteLine((cst.idx+1)+","+(cat.idx+1)+"="+q[cst.idx, cat.idx]);
			//double after = q[cst.idx, cat.idx];
			//if(cst.idx == 0 && cat.idx ==0)
			//Console.WriteLine(after - before);
		}

	}
}
