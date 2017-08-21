//State_eg.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mdpdatc
{
	class State_eg
	{
		public String state;
		public int idx;

		public State_eg()
		{
			//idx=-1;
		}

		public State_eg(String state_nm)
		{
			this.state = state_nm;
			switch (state_nm)
			{
				case "s1":
					idx = 0;
					break;
				case "s2":
					idx = 1;
					break;
				case "s3":
					idx = 2;
					break;
				default:
					break;
			}
		}
	}
}
