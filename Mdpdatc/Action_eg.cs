//Action_eg.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mdpdatc
{
	class Action_eg
	{
		public String action;
		public int idx;

		public Action_eg()
		{
			//idx=-1;
		}

		public Action_eg(String action_nm)
		{
			this.action = action_nm;
			switch (action_nm)
			{
				case "a1":
					idx = 0;
					break;
				case "a2":
					idx = 1;
					break;
				default:
					break;
			}

		}
	}
}