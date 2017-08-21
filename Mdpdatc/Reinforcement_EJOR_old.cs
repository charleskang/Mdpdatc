using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mdpdatc
{
	class Reinforcement_EJOR_old
	{
		public double[,] tp_a1;
		public double[,] tp_a2;
		public double[,] r_a1;
		public double[,] r_a2;

		public double[,] q;

		public double[,] v;

		public State_eg s1, s2, cur_st, next_st;
		public Action_eg a1, a2, cur_act;

		public double big_a = 0.1;
		public double lamda = 0.8;

		public Reinforcement_EJOR_old()
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

		public State_eg sampleNextState(State_eg curr_state, Action_eg exe_action)
		{
			State_eg next_state = null;

			Random random = new Random();
			double rnd = random.NextDouble();//Math.random();

			if (exe_action.idx == 0)
			{
				if (tp_a1[curr_state.idx, s1.idx] >= rnd)
					next_state = s1;
				else if (tp_a1[curr_state.idx, s1.idx] + tp_a1[curr_state.idx, s2.idx] >= rnd)
					next_state = s2;
			}
			else
			{
				if (tp_a2[curr_state.idx, s1.idx] >= rnd)
					next_state = s1;
				else if (tp_a2[curr_state.idx, s1.idx] + tp_a2[curr_state.idx, s2.idx] >= rnd)
					next_state = s2;

			}

			return next_state;
		}

		public void sampleAction_eqprob()
		{

			Random random = new Random();
			double rnd = random.NextDouble();//Math.random();

			if (rnd < 0.5) cur_act = this.a1;
			else cur_act = this.a2;

		}

		public void sampleAction_egreedy(double epsilon)
		{
			//int act_num = 2;
			Random random = new Random();
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


		public void exec_algorithm()
		{
			this.populateTps();
			this.populateRwds();


			this.cur_st = s1;
			//start learning
			for (int loop = 0; loop < 100; loop++)
			{
				//select an action (0.5 for each action)
				this.sampleAction_eqprob();
				// this.sampleAction_egreedy(0.001);
				//sample next state
				next_st = this.sampleNextState(cur_st, cur_act);
				//update q-factor and v-factor
				double maxq = 0.00;
				for (int i = 0; i < 2; i++)
					if (maxq < q[next_st.idx, i]) maxq = q[next_st.idx, i];

				v[cur_st.idx, cur_act.idx]++;
				double step_alpha = 0.00;
				step_alpha = big_a / v[cur_st.idx, cur_act.idx];

				calculateq(cur_st, cur_act, next_st, step_alpha, maxq);

				// System.out.println(cur_st.idx+":"+next_st.idx+":"+cur_act.idx+"="+reward(cur_st,cur_act,next_st)+"q:="+q[cur_st.idx][cur_act.idx]);
				// if(cur_st.idx==0 && cur_act.idx==0) 
				//     System.out.println(q[cur_st.idx][cur_act.idx]);

				this.cur_st = next_st;
			}


			for (int k = 0; k < 2; k++)
				for (int t = 0; t < 2; t++)
					Console.WriteLine(k + "," + t + ":" + q[k, t]);

		}

		public void calculateq(State_eg cst, Action_eg cat, State_eg nst, Double step_alpha, Double maxq)
		{
			q[cst.idx, cat.idx] = (1 - step_alpha) * q[cst.idx, cat.idx] + step_alpha * (reward(cst, cat, nst) + lamda * maxq);

		}
	}
}
