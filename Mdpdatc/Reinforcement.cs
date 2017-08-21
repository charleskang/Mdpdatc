//Reinforcement.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mdpdatc
{
	class Reinforcement
	{
		public double[,] tp_a1;
		public double[,] tp_a2;
		public double[,] r_a1;
		public double[,] r_a2;

		public double[,] q;

		public double[,] v;

		public State_eg s1, s2, cur_st, next_st;
		public Action_eg a1, a2, cur_act;

		public double big_a = 150;
		public double big_b = 300;
		public double lamda = 0.8;

		public int count_11 = 0;
		public int count_12 = 0;
		public int count_21 = 0;
		public int count_22 = 0;

		public Random random;

		public Reinforcement()
		{

			s1 = new State_eg("s1");
			s2 = new State_eg("s2");
			a1 = new Action_eg("a1");
			a2 = new Action_eg("a2");

			this.tp_a1 = new double[2, 2];
			this.tp_a2 = new double[2, 2];

			this.r_a1 = new double[2, 2];
			this.r_a2 = new double[2, 2];

			this.q = new double[2, 2];

			this.v = new double[2, 2];
		}

		public void populateTps()
		{
			this.tp_a1[0, 0] = 0.7;
			this.tp_a1[0, 1] = 0.3;
			this.tp_a1[1, 0] = 0.4;
			this.tp_a1[1, 1] = 0.6;

			this.tp_a2[0, 0] = 0.9;
			this.tp_a2[0, 1] = 0.1;
			this.tp_a2[1, 0] = 0.2;
			this.tp_a2[1, 1] = 0.8;

		}

		public void populateRwds()
		{
			this.r_a1[0, 0] = 6;
			this.r_a1[0, 1] = -5;
			this.r_a1[1, 0] = 7;
			this.r_a1[1, 1] = 12; //12;

			this.r_a2[0, 0] = 10; //10;
			this.r_a2[0, 1] = 17; //17;
			this.r_a2[1, 0] = -14; //-14;
			this.r_a2[1, 1] = 13; //13;
		}

		//Simulator
		public State_eg sampleNextState(State_eg curr_state, Action_eg exe_action)
		{
			State_eg next_state = null;

			double rnd = random.NextDouble();//Math.random();

			if (exe_action.idx == 0)
			{
				if (tp_a1[curr_state.idx, s1.idx] >= rnd)
					next_state = s1;
				else if (tp_a1[curr_state.idx, s1.idx] + tp_a1[curr_state.idx, s2.idx] >= rnd)
					next_state = s2;
				else
					Console.WriteLine("Something Wrong 85");
			}
			else
			{
				if (tp_a2[curr_state.idx, s1.idx] >= rnd)
					next_state = s1;
				else if (tp_a2[curr_state.idx, s1.idx] + tp_a2[curr_state.idx, s2.idx] >= rnd)
					next_state = s2;
				else
					Console.WriteLine("Something Wrong 94");
			}

			return next_state;
		}

		public void sampleAction_eqprob()
		{

			double rnd = random.NextDouble();//Math.random();

			if (rnd < 0.5) cur_act = this.a1;
			else cur_act = this.a2;


		}

		public void sampleAction_egreedy(double epsilon)
		{
			//int act_num = 2;

			double rnd = random.NextDouble();//Math.random();

			// if(rnd > epsilon) cur_act = 
			if (q[cur_st.idx, 0] > q[cur_st.idx, 1])
			{
				if (rnd > epsilon) cur_act = this.a1;
				else cur_act = this.a2;
			}
			else
			{
				if (rnd > epsilon) cur_act = this.a2;
				else cur_act = this.a1;
			}


		}


		public double reward(State_eg cst, Action_eg cat, State_eg nst)
		{
			if (cat.idx == 0)
				return this.r_a1[cst.idx, nst.idx];
			else
				return this.r_a2[cst.idx, nst.idx];

		}
		/**
         * @param args the command line arguments
         */

		public void entry_algorithm()
		{
			random = new Random();
			for (int i = 0; i < 10; i++)
			{
				exec_algorithm();

				this.q = new double[3, 2];
				this.v = new double[3, 2];
			}

			Console.WriteLine("11:" + this.count_11);
			Console.WriteLine("12:" + this.count_12);
			Console.WriteLine("21:" + this.count_21);
			Console.WriteLine("22:" + this.count_22);
		}

		public void exec_algorithm()
		{

			this.populateTps();
			this.populateRwds();


			this.cur_st = s1;
			//start learning
			for (int loop = 1; loop < 2000000; loop++)
			{
				//{
				//select an action (0.5 for each action)
				//dbeug(1208)
				//this.cur_act = a1;
				this.sampleAction_eqprob();
				//this.sampleAction_egreedy(0.001);
				//sample next state
				//debug(1208)
				//next_st = s2;
				next_st = this.sampleNextState(cur_st, cur_act);
				//update q-factor and v-factor
				double maxq = -10000.00;
				for (int i = 0; i < 2; i++)
					if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

				v[cur_st.idx, cur_act.idx]++;
				double step_alpha = 0.00;
				//step_alpha = big_a / (big_b + loop); //v[cur_st.idx, cur_act.idx]);
				//step_alpha = Math.Log(loop) / loop;
				step_alpha = 1 / (double)loop;

				calculateq(cur_st, cur_act, next_st, step_alpha, maxq);

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



			for (int k = 0; k < 2; k++)
                for (int t = 0; t < 2; t++)
					Console.WriteLine((k+1) + "," + (t+1) + ":" + q[k, t]);
            
			string resultaction = "";
			if (q[0, 0] >= q[0, 1])
				resultaction += "1";
			else
				resultaction += "2";

			if (q[1, 0] >= q[1, 1])
				resultaction += "1";
			else
				resultaction += "2";

			switch (resultaction)
			{
				case ("11"):
					this.count_11++;
					break;
				case ("12"):
					this.count_12++;
					break;
				case ("21"):
					this.count_21++;
					break;
				case ("22"):
					this.count_22++;
					break;
				default:
					throw new Exception();

			}


		}

		public void calculateq(State_eg cst, Action_eg cat, State_eg nst, Double step_alpha, Double maxq)
		{
			//double before = q[cst.idx, cat.idx];
			q[cst.idx, cat.idx] = (1 - step_alpha) * q[cst.idx, cat.idx] + step_alpha * (reward(cst, cat, nst) + lamda * maxq);
			//Console.WriteLine((cst.idx+1)+","+(cat.idx+1)+"="+q[cst.idx, cat.idx]);
			//double after = q[cst.idx, cat.idx];
			//if (cst.idx == 0 && cat.idx == 1)
			//	Console.WriteLine(after - before);
		}

	}
}
