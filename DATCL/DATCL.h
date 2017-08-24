// DATCL.h

#pragma once

#include "D:\Dropbox\Mdpdatc_mac\Mdpdatc\DATCCP\MyDef.h"
#include "D:\Dropbox\Mdpdatc_mac\Mdpdatc\DATCCP\RLDATC.cpp"

using namespace System;

namespace DATCL {

	public ref class DATCWrapper
	{
	public:
		
		DATCWrapper();

		double test(double kk);
		void InitialRunDATC();
		void InterRunDATC(
			double demand,
			double P_minW,
			double P_maxW,
			double P_service_time,
			double P_x_pos,
			double P_y_pos,
			double D_minW,
			double D_maxW,
			double D_service_time,
			double D_x_pos,
			double D_y_pos,
			double current_time);
		
		int main_from_external();
		double GetCurrentCapacity(double current_time);
		double GetCurrentET(double current_time);
		void RejectNewOrder(double current_time);
		double GetCurrentMSD(double current_time);
	private:
		SimulDatc *myCppClass;
	};
}
