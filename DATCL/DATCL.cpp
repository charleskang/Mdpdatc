// This is the main DLL file.

#include "stdafx.h"

#include "DATCL.h"

#include "D:\Dropbox\Mdpdatc_mac\Mdpdatc\DATCCP\MyDef.h"
#include "D:\Dropbox\Mdpdatc_mac\Mdpdatc\DATCCP\RLDATC.cpp"

DATCL::DATCWrapper::DATCWrapper()
{
	myCppClass = new SimulDatc();
}
double DATCL::DATCWrapper::test(double kk)
{
	return myCppClass->test(kk);
}

void DATCL::DATCWrapper::InitialRunDATC()
{
	myCppClass->InitialRunDATC();
}

void DATCL::DATCWrapper::InterRunDATC(
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
	double current_time)
{
	myCppClass->InterRunDATC(demand,
		P_minW,
		P_maxW,
		P_service_time,
		P_x_pos,
		P_y_pos,
		D_minW,
		D_maxW,
		D_service_time,
		D_x_pos,
		D_y_pos,
		current_time);
}

int DATCL::DATCWrapper::main_from_external()
{
	return myCppClass->main_from_external();
}

double DATCL::DATCWrapper::GetCurrentCapacity(double current_time)
{
	return myCppClass->GetCurrentCapacity(current_time);
}

double DATCL::DATCWrapper::GetCurrentET(double current_time)
{
	return myCppClass->GetCurrentCapacity(current_time);
}

void DATCL::DATCWrapper::RejectNewOrder(double current_time)
{
	myCppClass->RejectNewOrder(current_time);
}

double DATCL::DATCWrapper::GetCurrentMSD(double current_time)
{
	return myCppClass->GetCurrentMSD(current_time);
}

/*
void 	DATCL::DATCWrapper::InterRunDATC(
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
	double current_time)
{
	return myCppClass->InterRunDATC(demand,
		P_minW,
		P_maxW,
		P_service_time,
		P_x_pos,
		P_y_pos,
		D_minW,
		D_maxW,
		D_service_time,
		D_x_pos,
		D_y_pos,
		current_time);
}
*/
/*
bool 	DATCL::DATCWrapper::SortCustomerV(std::vector<CUSTOMER> &vRoutes)
{	return myCppClass->SortCustomerV(vRoutes); }
void 	DATCL::DATCWrapper::CreateDistanceMatrix()
{
	return myCppClass->CreateDistanceMatrix();
}
*/
/*
void 	DATCL::DATCWrapper::InitData()

int 	DATCL::DATCWrapper::GetVehicle(int c_id, int loc_id, double minW, double maxW);
void 	DATCL::DATCWrapper::InitVehicle();
double 	DATCL::DATCWrapper::GetAbsoluteET(double comp_time, double minW, double maxW);
void 	DATCL::DATCWrapper::PrintDATCResult();
double 	DATCL::DATCWrapper::GetTrvDistance(int fromCustID, int toCustID);
void 	DATCL::DATCWrapper::InitialRunDATC();
void 	DATCL::DATCWrapper::DATC_By_Vehicle(vector<CUSTOMER> currVec, CUSTOMER& newcustomer);
double 	DATCL::DATCWrapper::GetCurrX(double x1, double y1, double x2, double y2, double trv_time);
double 	DATCL::DATCWrapper::GetCurrY(double x1, double y1, double x2, double y2, double trv_time);
void 	DATCL::DATCWrapper::Print_Pajek_Result(double current_time);
void 	DATCL::DATCWrapper::InterRunDATC(
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

double 	DATCL::DATCWrapper::GetCurrentCapacity(double current_time);
double 	DATCL::DATCWrapper::GetCurrentET(double current_time);
void 	DATCL::DATCWrapper::UpdateVehicleLocation(double current_time);


//missing methods: added by yck
void 	DATCL::DATCWrapper::InitSpecificVehicle(int v_id);
void 	DATCL::DATCWrapper::RunDATC_for_SelectedVehicle(vector<CUSTOMER> SelVec, int v_id);
double 	DATCL::DATCWrapper::GetCurrentMSD(double current_time);
int 	DATCL::DATCWrapper::_tmain(int argc, _TCHAR* argv[]);SortCustomerV(std::vector<CUSTOMER> &vRoutes);
void 	CreateDistanceMatrix();
void 	InitData();
int 	GetVehicle(int c_id, int loc_id, double minW, double maxW);
void 	InitVehicle();
double 	DATCL::DATCWrapper::GetAbsoluteET(double comp_time, double minW, double maxW);
void 	DATCL::DATCWrapper::PrintDATCResult();
double 	DATCL::DATCWrapper::GetTrvDistance(int fromCustID, int toCustID);
void 	DATCL::DATCWrapper::InitialRunDATC();
void 	DATCL::DATCWrapper::DATC_By_Vehicle(vector<CUSTOMER> currVec, CUSTOMER& newcustomer);
double 	DATCL::DATCWrapper::GetCurrX(double x1, double y1, double x2, double y2, double trv_time);
double 	DATCL::DATCWrapper::GetCurrY(double x1, double y1, double x2, double y2, double trv_time);
void 	DATCL::DATCWrapper::Print_Pajek_Result(double current_time);
void 	DATCL::DATCWrapper::InterRunDATC(
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

double 	DATCL::DATCWrapper::GetCurrentCapacity(double current_time);
double 	DATCL::DATCWrapper::GetCurrentET(double current_time);
void 	DATCL::DATCWrapper::UpdateVehicleLocation(double current_time);


//missing methods: added by yck
void 	DATCL::DATCWrapper::InitSpecificVehicle(int v_id);
void 	DATCL::DATCWrapper::RunDATC_for_SelectedVehicle(vector<CUSTOMER> SelVec, int v_id);
double 	DATCL::DATCWrapper::GetCurrentMSD(double current_time);
int 	DATCL::DATCWrapper::_tmain(int argc, _TCHAR* argv[]);
*/