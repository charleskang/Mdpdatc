#pragma once

#include <fstream>
#include <vector>
using namespace std;

static struct VEHICLE
{
	int v_id;
	int curr_loc_id;
	int avail_vehicle;// 0 available, 1 unavailable

	double max_capa;
	double avail_capa;

	double arv_time;
	double comp_time;

	double remain_time;
	double check_tag;
	int temp_tag;
	double time_diff;

	double start_time;
	double weight;

	double xPos;
	double yPos;
};

static struct CUSTOMER
{
	int c_id;
	int loc_id;
	int prev_c_id;
	int prev_loc_id;
	int v_id;

	double demand;
	double minW;
	double maxW;

	double arv_time;
	double comp_time;
	double service_time;

	double et;

	double x_pos;
	double y_pos;

	int customer_tag; // 0 customer, 1 depot
	int complete_tag; // 1: complete delivery, 0: incomplete

	double v_trv_time;
	double v_trv_distance;

	int order_type; // 0: pickup, 1: delivery
};

// algorithm parameters:
double c_gain = 0.01;
int num_of_iter = 1000;
double v_speed = 60.0;
double max_work_time = 20.0;


// transportation env:
int num_of_customer = 0;
int num_of_vehicle = 0;
double depot_xPos = 50.0;
double depot_yPos = 50.0;
double scale_for_dist = 50; // 0.5 is 50 km, diameter = 1 (100 km), radius = 0.5 (50 km) 
int best_iter = 0;

// file io

// data
VEHICLE* vehicle = NULL;
CUSTOMER* customer = NULL;
CUSTOMER* customer_selected = NULL;
double** m_arrDistance = NULL;
double** m_arrArvTime = NULL;
double** m_arrCompTime = NULL;
double* m_arrTravlDist = NULL;
double* m_arrTravlTime = NULL;
double* m_arrET = NULL;
double* m_arrJitCost = NULL;
double* m_arrMSD = NULL;
int max_c_id = 0;

vector< CUSTOMER> vCurrRoutes;
vector< CUSTOMER> vCurrRoutes_Cpy;
vector< CUSTOMER> vBestRoutes;
vector< CUSTOMER> vBestRoutes_Cpy;
vector< vector <CUSTOMER>> vRouteByVehicle;
vector< vector <CUSTOMER>> vRouteByVehicle_Cpy;


// functions
//bool SortCustomer(CUSTOMER*& p_data);
bool SortCustomerV(std::vector<CUSTOMER> &vRoutes);
void CreateDistanceMatrix();
void InitData();
int GetVehicle(int c_id, int loc_id, double minW, double maxW);
void InitVehicle();
double GetAbsoluteET(double comp_time, double minW, double maxW);
void PrintDATCResult();
double GetTrvDistance(int fromCustID, int toCustID);
//void InitialRunDATC();
double GetCurrX(double x1, double y1, double x2, double y2, double trv_time);
double GetCurrY(double x1, double y1, double x2, double y2, double trv_time);
void Print_Pajek_Result(double current_time);
// Intermediate run of DATC for new demand <- OAC should call this function
// xPos, yPos: position of customer
// demand: customer demand
// remaining_deliverytime: remaining delivery time reuquested by customer at current time
// current_time: time at which order is requested
/*
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
	*/
//double GetCurrentCapacity(double current_time);
//double GetCurrentET(double current_time);
void UpdateVehicleLocation(double current_time);
void RemoveCompleteCustomer(double current_time);


// added 8/17/2017
//void RejectNewOrder(double current_time);


 class SimulDatc
 {
	 

	 // algorithm parameters:
 public:
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
	 double test(double kk);
	 int main_from_external();
	 // Intermediate run of DATC for new demand <- OAC should call this function
	 // xPos, yPos: position of customer
	 // demand: customer demand
	 // remaining_deliverytime: remaining delivery time reuquested by customer at current time
	 // current_time: time at which order is requested
	 /*
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
		 */
	 //void InterRunDATC(double demand, double D_minW, double D_maxW, double D_service_time, double D_x_pos, double D_y_pos, double P_x_pos, double P_y_pos, double remaining_pickuptime, double current_time, double P_service_time);
	 double GetCurrentMSD(double current_time);
	 double GetCurrentCapacity(double current_time);
	 double GetCurrentET(double current_time);
	 void RejectNewOrder(double current_time);

 };
