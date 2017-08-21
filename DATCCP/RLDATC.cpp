// RLDATC.cpp : Defines the entry point for the console application.
//
#pragma once
// RLDATC.cpp : Defines the entry point for the console application.
//


#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <math.h>
#include <time.h>
#include <ctime>
#include <stdlib.h>
#include <stdio.h>
#include "MyDef.h"
#include <cstring>
#include <vector>

using namespace std;

#pragma warning(disable:4996)

double SimulDatc::test(double kk)
{
	return 0.12345;
}

bool SortCustomerV(std::vector<CUSTOMER> &vRoutes)
{
	int temp1 = 0;
	int temp2 = 0;
	int temp3 = 0;
	int temp4 = 0;
	int temp5 = 0;

	double temp6 = 0.0;
	double temp7 = 0.0;
	double temp8 = 0.0;
	double temp9 = 0.0;
	double temp10 = 0.0;
	double temp11 = 0.0;
	double temp12 = 0.0;
	double temp13 = 0.0;
	double temp14 = 0.0;

	int temp15 = 0;
	int temp16 = 0;

	double temp17 = 0.0;
	double temp18 = 0.0;

	int temp19 = 0;


	int last, latest, i;
	latest = num_of_customer - 1;

	do
	{
		last = latest;
		//for (i = 1; i <= last - 1; i++)
		for (i = 0; i <= last - 1; i++)
		{
			if (vRoutes[i].arv_time > vRoutes[i + 1].arv_time)
			{
				latest = i;

				temp1 = vRoutes[i].c_id;
				temp2 = vRoutes[i].loc_id;
				temp3 = vRoutes[i].prev_c_id;
				temp4 = vRoutes[i].prev_loc_id;
				temp5 = vRoutes[i].v_id;
				temp6 = vRoutes[i].demand;
				temp7 = vRoutes[i].minW;
				temp8 = vRoutes[i].maxW;
				temp9 = vRoutes[i].arv_time;
				temp10 = vRoutes[i].comp_time;
				temp11 = vRoutes[i].service_time;
				temp12 = vRoutes[i].et;
				temp13 = vRoutes[i].x_pos;
				temp14 = vRoutes[i].y_pos;
				temp15 = vRoutes[i].customer_tag;
				temp16 = vRoutes[i].complete_tag;
				temp17 = vRoutes[i].v_trv_time;
				temp18 = vRoutes[i].v_trv_distance;
				temp19 = vRoutes[i].order_type;

				vRoutes[i].c_id = vRoutes[i + 1].c_id;
				vRoutes[i].loc_id = vRoutes[i + 1].loc_id;
				vRoutes[i].prev_c_id = vRoutes[i + 1].prev_c_id;
				vRoutes[i].prev_loc_id = vRoutes[i + 1].prev_loc_id;
				vRoutes[i].v_id = vRoutes[i + 1].v_id;
				vRoutes[i].demand = vRoutes[i + 1].demand;
				vRoutes[i].minW = vRoutes[i + 1].minW;
				vRoutes[i].maxW = vRoutes[i + 1].maxW;
				vRoutes[i].arv_time = vRoutes[i + 1].arv_time;
				vRoutes[i].comp_time = vRoutes[i + 1].comp_time;
				vRoutes[i].service_time = vRoutes[i + 1].service_time;
				vRoutes[i].et = vRoutes[i + 1].et;
				vRoutes[i].x_pos = vRoutes[i + 1].x_pos;
				vRoutes[i].y_pos = vRoutes[i + 1].y_pos;
				vRoutes[i].customer_tag = vRoutes[i + 1].customer_tag;
				vRoutes[i].complete_tag = vRoutes[i + 1].complete_tag;
				vRoutes[i].v_trv_time = vRoutes[i + 1].v_trv_time;
				vRoutes[i].v_trv_distance = vRoutes[i + 1].v_trv_distance;
				vRoutes[i].order_type = vRoutes[i + 1].order_type;

				vRoutes[i + 1].c_id = temp1;
				vRoutes[i + 1].loc_id = temp2;
				vRoutes[i + 1].prev_c_id = temp3;
				vRoutes[i + 1].prev_loc_id = temp4;
				vRoutes[i + 1].v_id = temp5;
				vRoutes[i + 1].demand = temp6;
				vRoutes[i + 1].minW = temp7;
				vRoutes[i + 1].maxW = temp8;
				vRoutes[i + 1].arv_time = temp9;
				vRoutes[i + 1].comp_time = temp10;
				vRoutes[i + 1].service_time = temp11;
				vRoutes[i + 1].et = temp12;
				vRoutes[i + 1].x_pos = temp13;
				vRoutes[i + 1].y_pos = temp14;
				vRoutes[i + 1].customer_tag = temp15;
				vRoutes[i + 1].complete_tag = temp16;
				vRoutes[i + 1].v_trv_time = temp17;
				vRoutes[i + 1].v_trv_distance = temp18;
				vRoutes[i + 1].order_type = temp19;
			}
		}
	} while (latest != last && latest != 0);

	return true;
}

void CreateDistanceMatrix()
{
	ofstream ofs_distance;
	ofs_distance.open("distance.txt");

	double depot_x_pos = 0.0;
	double depot_y_pos = 0.0;

	for (int k = 0; k <= num_of_customer; k++)
	{
		if (customer[k].customer_tag == 1)
		{
			depot_x_pos = customer[k].x_pos;
			depot_y_pos = customer[k].y_pos;
			break;
		}
	}


	for (int i = 0; i <= num_of_customer; i++)
	{
		for (int j = 0; j <= num_of_customer; j++)
		{
			if (i == j)
				ofs_distance << 9999 << "\t";
			else
			{
				if (i == 0 && j != 0)
					ofs_distance << sqrt((depot_x_pos - customer[j].x_pos)*(depot_x_pos - customer[j].x_pos) + (depot_y_pos - customer[j].y_pos)*(depot_y_pos - customer[j].y_pos)) << "\t";
				else if (j == 0 && i != 0)
					ofs_distance << sqrt((customer[i].x_pos - depot_x_pos)*(customer[i].x_pos - depot_x_pos) + (customer[i].y_pos - depot_y_pos)*(customer[i].y_pos - depot_y_pos)) << "\t";
				else
					ofs_distance << sqrt((customer[i].x_pos - customer[j].x_pos)*(customer[i].x_pos - customer[j].x_pos) + (customer[i].y_pos - customer[j].y_pos)*(customer[i].y_pos - customer[j].y_pos)) << "\t";
			}
		}
		ofs_distance << "\n";
	}

	ofs_distance << endl;

	ifstream ifs_distance;
	ifs_distance.open("distance.txt");
	for (int i = 0; i <= num_of_customer; i++)
		for (int j = 0; j <= num_of_customer; j++)
			ifs_distance >> m_arrDistance[i][j];


	// save travel time matrix
	ofstream ofs_travel_distance;
	ofs_travel_distance.open("travel_time.txt");
	for (int i = 0; i <= num_of_customer; i++)
	{
		for (int j = 0; j <= num_of_customer; j++)
			ofs_travel_distance << m_arrDistance[i][j] / v_speed << "\t";

		ofs_travel_distance << endl;
	}


}



void InitData()
{
	ifstream ifs_customer;
	ifstream ifs_vehicle;

	ifs_customer.open("customer_5_vehicle_2.txt");
	ifs_customer >> num_of_customer;
	max_c_id = num_of_customer;
	ifs_vehicle.open("vehicle_2_customer 5.txt");
	ifs_vehicle >> num_of_vehicle;

	vehicle = new VEHICLE[num_of_vehicle + 1];
	customer = new CUSTOMER[num_of_customer + 1];
	customer_selected = new CUSTOMER[num_of_customer + 1];
	m_arrET = new double[num_of_iter + 1];
	m_arrJitCost = new double[num_of_iter + 1];
	m_arrMSD = new double[num_of_iter + 1];
	m_arrDistance = new double*[num_of_customer + 1];
	m_arrTravlTime = new double[num_of_iter + 1];

	// Customer
	for (int i = 1; i <= num_of_customer; i++)
	{
		ifs_customer >> customer[i].c_id >> customer[i].demand >> customer[i].minW >> customer[i].maxW >> customer[i].service_time >> customer[i].x_pos >> customer[i].y_pos >> customer[i].customer_tag >> customer[i].v_id;
		customer[i].arv_time = num_of_customer - i;
		customer[i].comp_time = 0.0;
		customer[i].complete_tag = 0;
		customer[i].et = 0.0;
		customer[i].loc_id = i;
		customer[i].prev_c_id = 0;
		customer[i].prev_loc_id = 0;
		customer[i].v_trv_time = 0.0;
		customer[i].v_trv_distance = 0.0;
		customer[i].order_type = 1;
	}

	for (int i = 1; i <= num_of_customer; i++)
		vCurrRoutes.push_back(customer[i]);

	// Distance matrix
	m_arrDistance = new double*[num_of_customer + 1];
	for (int i = 0; i <= num_of_customer; i++)
		m_arrDistance[i] = new double[num_of_customer + 1];


	// Vehicle
	for (int i = 1; i <= num_of_vehicle; i++)
	{
		ifs_vehicle >> vehicle[i].v_id >> vehicle[i].curr_loc_id >> vehicle[i].max_capa >> vehicle[i].avail_capa >> vehicle[i].comp_time >> vehicle[i].check_tag >> vehicle[i].weight;
		vehicle[i].time_diff = 0.0;
		vehicle[i].start_time = 0.0;
		vehicle[i].remain_time = max_work_time;
		vehicle[i].avail_vehicle = 0;
		vehicle[i].temp_tag = 0;
		vehicle[i].max_capa = vehicle[i].max_capa;
		vehicle[i].avail_capa = vehicle[i].avail_capa;
		vehicle[i].xPos = depot_xPos;
		vehicle[i].yPos = depot_yPos;
	}

	m_arrArvTime = new double*[num_of_iter + 1];
	for (int i = 1; i <= num_of_iter; i++)
		m_arrArvTime[i] = new double[num_of_customer + 1];

	m_arrCompTime = new double*[num_of_iter + 1];
	for (int i = 1; i <= num_of_iter; i++)
		m_arrCompTime[i] = new double[num_of_customer + 1];

}

int GetVehicle(int c_id, int loc_id, double minW, double maxW)
{
	int v_index = 0;
	int cnt = 0;
	double trvtime = 0.0;
	int best_vcode = 0;
	double temp_dist = 1000;
	double temp_time = 1000;
	double temp = 1000;


	// check temporal condition first
	for (v_index = 1; v_index <= num_of_vehicle; v_index++)
	{
		//trvtime = m_arrDistance[vehicle[v_index].curr_loc_id][loc_id] / v_speed;
		trvtime = GetTrvDistance(vehicle[v_index].curr_loc_id, loc_id) / v_speed;

		if (trvtime <= vehicle[v_index].remain_time)
		{
			vehicle[v_index].avail_vehicle = 0;

			if (vehicle[v_index].comp_time + trvtime <= maxW)
			{
				vehicle[v_index].check_tag = 1; // JIT available
				vehicle[v_index].time_diff = maxW - (vehicle[v_index].comp_time + trvtime);
				cnt++;
			}
			else
			{
				vehicle[v_index].check_tag = 0; // JIT not available
				vehicle[v_index].time_diff = (vehicle[v_index].comp_time + trvtime) - maxW;
			}
		}
		else
			vehicle[v_index].avail_vehicle = 1;
	}

	if (cnt > 0)
	{
		// find best vehicle considering spatial condition (distance)
		for (v_index = 1; v_index <= num_of_vehicle; v_index++)
		{
			if (vehicle[v_index].avail_vehicle == 0) // checking availability
			{
				if (vehicle[v_index].check_tag == 1) // JIT available
				{
					if (m_arrDistance[vehicle[v_index].curr_loc_id][loc_id] < temp_dist)
					{
						best_vcode = vehicle[v_index].v_id;
						temp_dist = m_arrDistance[vehicle[v_index].curr_loc_id][loc_id];
					}
				}
			}
		}
	}
	else
	{
		// find the second best vehicle, only considering temporal condition
		for (v_index = 1; v_index <= num_of_vehicle; v_index++)
		{
			if (vehicle[v_index].avail_vehicle == 0) // checking availability
			{
				if (vehicle[v_index].time_diff < temp)
				{
					best_vcode = vehicle[v_index].v_id;
					temp = vehicle[v_index].time_diff;
				}
			}
		}
	}

	// initialize
	for (v_index = 1; v_index <= num_of_vehicle; v_index++)
	{
		vehicle[v_index].check_tag = 0;
		vehicle[v_index].time_diff = 0.0;
		vehicle[v_index].avail_vehicle = 0;
	}

	return best_vcode;
}

void InitSpecificVehicle(int v_id)
{
	vehicle[v_id].avail_capa = vehicle[v_id].max_capa;
	vehicle[v_id].curr_loc_id = 0;
	vehicle[v_id].time_diff = 0.0;
	vehicle[v_id].comp_time = 0.0;
	vehicle[v_id].start_time = 0.0;
	vehicle[v_id].avail_vehicle = 0;
	vehicle[v_id].check_tag = 0;
	vehicle[v_id].temp_tag = 0;
	vehicle[v_id].remain_time = max_work_time;
}

void InitVehicle()
{
	for (int i = 1; i <= num_of_vehicle; i++)
	{
		vehicle[i].avail_capa = vehicle[i].max_capa;
		vehicle[i].curr_loc_id = 0;
		vehicle[i].time_diff = 0.0;
		vehicle[i].comp_time = 0.0;
		vehicle[i].start_time = 0.0;
		vehicle[i].avail_vehicle = 0;
		vehicle[i].check_tag = 0;
		vehicle[i].temp_tag = 0;
		vehicle[i].remain_time = max_work_time;

	}
}

double GetAbsoluteET(double comp_time, double minW, double maxW)
{
	double return_val = 0.0;

	if (comp_time < minW)
		return_val = minW - comp_time;
	else if (comp_time > maxW)
		return_val = comp_time - maxW;
	else if (minW <= comp_time && comp_time <= maxW)
		return_val = 0.0;
	else
		return_val = 0.0;

	return return_val;
}

void PrintDATCResult()
{
	ofstream ofs_init_routingresult;
	time_t t = time(0);   // get time now
	char buff[20];
	time_t now = time(NULL);
	strftime(buff, 20, "%Y-%m-%d %H-%M-%S", localtime(&now));
	cout << buff << endl;

	char* array1 = buff;
	char array2[] = ".txt";
	char * newArray = new char[strlen(array1) + strlen(array2) + 1];
	strcpy(newArray, array1);
	strcat(newArray, array2);
	cout << newArray << endl;

	ofs_init_routingresult.open(newArray);

	ofs_init_routingresult << "c_id" << "\t";
	ofs_init_routingresult << "v_id" << "\t";
	ofs_init_routingresult << "demand" << "\t";
	ofs_init_routingresult << "arv_time" << "\t";
	ofs_init_routingresult << "comp_time" << "\t";
	ofs_init_routingresult << "minW" << "\t";
	ofs_init_routingresult << "maxW" << "\t";
	ofs_init_routingresult << "et" << "\t";
	ofs_init_routingresult << "v_trv_distance" << "\t";
	ofs_init_routingresult << "v_trv_time" << endl;

	for (int j = 0; j <= vRouteByVehicle.size() - 1; j++)
	{
		for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
		{
			//if (vBestRoutes[i].v_id == j + 1)
			//{
			ofs_init_routingresult << vRouteByVehicle[j][i].c_id << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].v_id << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].demand << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].arv_time << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].comp_time << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].minW << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].maxW << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].et << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].v_trv_distance << "\t";
			ofs_init_routingresult << vRouteByVehicle[j][i].v_trv_time << endl;
			//}
		}
	}

	/*for (int i = 0; i <= num_of_customer - 1; i++)
	{
	ofs_init_routingresult << vBestRoutes[i].c_id << "\t";
	ofs_init_routingresult << vBestRoutes[i].v_id << "\t";
	ofs_init_routingresult << vBestRoutes[i].demand << "\t";
	ofs_init_routingresult << vBestRoutes[i].arv_time << "\t";
	ofs_init_routingresult << vBestRoutes[i].comp_time << "\t";
	ofs_init_routingresult << vBestRoutes[i].minW << "\t";
	ofs_init_routingresult << vBestRoutes[i].maxW << "\t";
	ofs_init_routingresult << vBestRoutes[i].et << "\t";
	ofs_init_routingresult << vBestRoutes[i].v_trv_distance << "\t";
	ofs_init_routingresult << vBestRoutes[i].v_trv_time << endl;
	}*/
}

double GetTrvDistance(int fromCustID, int toCustID)
{
	double trv_time = 0.0;

	double xFrom, yFrom = 0.0;
	double xTo, yTo = 0.0;

	if (fromCustID == 0)
	{
		xFrom = depot_xPos;
		yFrom = depot_yPos;
	}
	else
	{

		for (int i = 0; i <= num_of_customer - 1; i++)
		{
			if (vCurrRoutes[i].c_id == fromCustID)
			{
				xFrom = vCurrRoutes[i].x_pos;
				yFrom = vCurrRoutes[i].y_pos;
				break;
			}
		}
	}

	if (toCustID == 0)
	{
		xTo = depot_xPos;
		yTo = depot_yPos;
	}
	else
	{
		for (int i = 0; i <= num_of_customer - 1; i++)
		{
			if (vCurrRoutes[i].c_id == toCustID)
			{
				xTo = vCurrRoutes[i].x_pos;
				yTo = vCurrRoutes[i].y_pos;
				break;
			}
		}
	}

	trv_time = sqrt((xFrom - xTo)*(xFrom - xTo) + (yFrom - yTo)*(yFrom - yTo));

	return trv_time;
}

void SimulDatc::InitialRunDATC()
{
	int iter = 0;
	int sel_vehicle = 0;

	double trv_time = 0.0;
	double et = 0.0;
	double msd = 0.0;
	double cum_et = 0.0;
	double cum_travl_time = 0.0;
	double cum_travl_dist = 0.0;
	double total_jitcost = 0.0;
	double best_cost = 9999999.0;

	// Run Algorithm
	for (int iter = 1; iter <= num_of_iter; iter++)
	{
		if (iter == 998)
			cout << "xx" << endl;

		InitVehicle();

		//for (int i = 1; i <= num_of_customer; i++)
		for (int i = 0; i <= num_of_customer - 1; i++)
		{
			// Assign vehicle
			if (vCurrRoutes[i].customer_tag == 0)
				sel_vehicle = GetVehicle(vCurrRoutes[i].c_id, vCurrRoutes[i].loc_id, vCurrRoutes[i].minW, vCurrRoutes[i].maxW);
			else
				sel_vehicle = vCurrRoutes[i].v_id;

			if (sel_vehicle == 0)
				sel_vehicle = 1;

			// Update vehicle and travel information by selected vehicle
			trv_time = GetTrvDistance(vehicle[sel_vehicle].curr_loc_id, vCurrRoutes[i].c_id) / v_speed;

			vCurrRoutes[i].v_trv_time = trv_time;
			vCurrRoutes[i].v_trv_distance = GetTrvDistance(vehicle[sel_vehicle].curr_loc_id, vCurrRoutes[i].c_id);
			vCurrRoutes[i].v_id = sel_vehicle;
			vehicle[sel_vehicle].remain_time = vehicle[sel_vehicle].remain_time - trv_time;

			// Arv time control
			if (vCurrRoutes[i].comp_time < vCurrRoutes[i].minW) // earliness
				vCurrRoutes[i].arv_time = vCurrRoutes[i].arv_time + c_gain*(vCurrRoutes[i].minW - vCurrRoutes[i].comp_time);
			else if (vCurrRoutes[i].maxW < vCurrRoutes[i].comp_time) // tardiness
				vCurrRoutes[i].arv_time = vCurrRoutes[i].arv_time + c_gain*(vCurrRoutes[i].maxW - vCurrRoutes[i].comp_time);
			else if (vCurrRoutes[i].minW < vCurrRoutes[i].comp_time && vCurrRoutes[i].comp_time <= vCurrRoutes[i].maxW) // in time
				vCurrRoutes[i].arv_time = vCurrRoutes[i].arv_time - c_gain*(vCurrRoutes[i].comp_time - vCurrRoutes[i].minW);

			vehicle[sel_vehicle].curr_loc_id = vCurrRoutes[i].c_id;
			vehicle[sel_vehicle].avail_capa = vehicle[sel_vehicle].max_capa - vCurrRoutes[i].demand;
		}

		// Sort customer by FCFS
		SortCustomerV(vCurrRoutes);

		// update delivery completion time
		for (int i = 0; i <= num_of_customer - 1; i++)
		{
			if (vehicle[vCurrRoutes[i].v_id].temp_tag == 0)
			{
				vCurrRoutes[i].comp_time = vCurrRoutes[i].arv_time + vCurrRoutes[i].v_trv_time;
				vehicle[vCurrRoutes[i].v_id].comp_time = vCurrRoutes[i].comp_time;
				vehicle[vCurrRoutes[i].v_id].temp_tag = 1;
			}
			else
			{
				if (vCurrRoutes[i].arv_time < vehicle[vCurrRoutes[i].v_id].comp_time)
				{
					vCurrRoutes[i].comp_time = vehicle[vCurrRoutes[i].v_id].comp_time + vCurrRoutes[i].v_trv_time;
					vehicle[vCurrRoutes[i].v_id].comp_time = vCurrRoutes[i].comp_time;
				}
				else
				{
					vCurrRoutes[i].comp_time = vCurrRoutes[i].arv_time + vCurrRoutes[i].v_trv_time;
					vehicle[vCurrRoutes[i].v_id].comp_time = vCurrRoutes[i].comp_time;
				}
				vehicle[vCurrRoutes[i].v_id].temp_tag = 1;
			}

			et = GetAbsoluteET(vCurrRoutes[i].comp_time, vCurrRoutes[i].minW, vCurrRoutes[i].maxW);

			if (vCurrRoutes[i].customer_tag == 0)
				msd += et*et;

			cum_et += et;
			cum_travl_time += trv_time;

			vCurrRoutes[i].v_trv_time = trv_time;
			vCurrRoutes[i].et = et;


			m_arrArvTime[iter][vCurrRoutes[i].c_id] = vCurrRoutes[i].arv_time;
			m_arrCompTime[iter][vCurrRoutes[i].c_id] = vCurrRoutes[i].comp_time;
		}

		m_arrTravlTime[iter] = cum_travl_time;
		total_jitcost = sqrt(msd / (num_of_customer - num_of_vehicle));

		// Save best performance
		if (total_jitcost < best_cost) // if performance improved
		{
			best_iter = iter;
			best_cost = total_jitcost;

			m_arrET[iter] = cum_et;
			m_arrJitCost[iter] = best_cost;
			m_arrMSD[iter] = msd / (num_of_customer - num_of_vehicle);

			vBestRoutes = vCurrRoutes;
		}
		else // if performance NOT improved, save previous performance
		{
			m_arrET[iter] = m_arrET[iter - 1];
			m_arrJitCost[iter] = m_arrJitCost[iter - 1];
			m_arrMSD[iter] = m_arrMSD[iter - 1];
		}

		msd = 0.0;
		cum_et = 0.0;
		cum_travl_time = 0.0;
		cum_travl_dist = 0.0;

	} // end of iter


	vRouteByVehicle.resize(num_of_vehicle);
	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		for (int i = 0; i <= vBestRoutes.size() - 1; i++)
		{
			if (vBestRoutes[i].v_id == j + 1)
			{
				vRouteByVehicle[j].push_back(vBestRoutes[i]);
			}
		}
	}

	//vRouteByVehicle_Cpy = vRouteByVehicle;

	//PrintDATCResult();

}

void RunDATC_for_SelectedVehicle(int v_id)
{
	int iter = 0;
	int sel_vehicle = 0;

	double trv_time = 0.0;
	double et = 0.0;
	double msd = 0.0;
	double cum_et = 0.0;
	double cum_travl_time = 0.0;
	double cum_travl_dist = 0.0;
	double total_jitcost = 0.0;
	double best_cost = 9999999.0;

	// Run Algorithm
	for (int iter = 1; iter <= num_of_iter; iter++)
	{
		InitSpecificVehicle(v_id);

		for (int i = 0; i <= vCurrRoutes.size() - 1; i++)
		{
			sel_vehicle = v_id;

			// Update vehicle and travel information by selected vehicle
			trv_time = GetTrvDistance(vehicle[sel_vehicle].curr_loc_id, vCurrRoutes[i].c_id) / v_speed;

			vCurrRoutes[i].v_trv_time = trv_time;
			vCurrRoutes[i].v_trv_distance = GetTrvDistance(vehicle[sel_vehicle].curr_loc_id, vCurrRoutes[i].c_id);
			vCurrRoutes[i].v_id = sel_vehicle;
			vehicle[sel_vehicle].remain_time = vehicle[sel_vehicle].remain_time - trv_time;

			// Arv time control
			if (vCurrRoutes[i].comp_time < vCurrRoutes[i].minW) // earliness
				vCurrRoutes[i].arv_time = vCurrRoutes[i].arv_time + c_gain*(vCurrRoutes[i].minW - vCurrRoutes[i].comp_time);
			else if (vCurrRoutes[i].maxW < vCurrRoutes[i].comp_time) // tardiness
				vCurrRoutes[i].arv_time = vCurrRoutes[i].arv_time + c_gain*(vCurrRoutes[i].maxW - vCurrRoutes[i].comp_time);
			else if (vCurrRoutes[i].minW < vCurrRoutes[i].comp_time && vCurrRoutes[i].comp_time <= vCurrRoutes[i].maxW) // in time
				vCurrRoutes[i].arv_time = vCurrRoutes[i].arv_time - c_gain*(vCurrRoutes[i].comp_time - vCurrRoutes[i].minW);

			vehicle[sel_vehicle].curr_loc_id = vCurrRoutes[i].c_id;
			vehicle[sel_vehicle].avail_capa = vehicle[sel_vehicle].max_capa - vCurrRoutes[i].demand;
		}

		// Sort customer by FCFS
		SortCustomerV(vCurrRoutes);

		// update delivery completion time
		for (int i = 0; i <= vCurrRoutes.size() - 1; i++)
		{
			if (vehicle[vCurrRoutes[i].v_id].temp_tag == 0)
			{
				vCurrRoutes[i].comp_time = vCurrRoutes[i].arv_time + vCurrRoutes[i].v_trv_time;
				vehicle[vCurrRoutes[i].v_id].comp_time = vCurrRoutes[i].comp_time;
				vehicle[vCurrRoutes[i].v_id].temp_tag = 1;
			}
			else
			{
				if (vCurrRoutes[i].arv_time < vehicle[vCurrRoutes[i].v_id].comp_time)
				{
					vCurrRoutes[i].comp_time = vehicle[vCurrRoutes[i].v_id].comp_time + vCurrRoutes[i].v_trv_time;
					vehicle[vCurrRoutes[i].v_id].comp_time = vCurrRoutes[i].comp_time;
				}
				else
				{
					vCurrRoutes[i].comp_time = vCurrRoutes[i].arv_time + vCurrRoutes[i].v_trv_time;
					vehicle[vCurrRoutes[i].v_id].comp_time = vCurrRoutes[i].comp_time;
				}
				vehicle[vCurrRoutes[i].v_id].temp_tag = 1;
			}

			et = GetAbsoluteET(vCurrRoutes[i].comp_time, vCurrRoutes[i].minW, vCurrRoutes[i].maxW);

			if (vCurrRoutes[i].customer_tag == 0)
				msd += et*et;

			cum_et += et;
			cum_travl_time += trv_time;

			vCurrRoutes[i].v_trv_time = trv_time;
			vCurrRoutes[i].et = et;


			m_arrArvTime[iter][vCurrRoutes[i].c_id] = vCurrRoutes[i].arv_time;
			m_arrCompTime[iter][vCurrRoutes[i].c_id] = vCurrRoutes[i].comp_time;
		}

		m_arrTravlTime[iter] = cum_travl_time;
		total_jitcost = sqrt(msd / vCurrRoutes.size());

		// Save best performance
		if (total_jitcost < best_cost) // if performance improved
		{
			best_iter = iter;
			best_cost = total_jitcost;

			m_arrET[iter] = cum_et;
			m_arrJitCost[iter] = best_cost;
			m_arrMSD[iter] = msd / vCurrRoutes.size();

			vBestRoutes = vCurrRoutes;
		}
		else // if performance NOT improved, save previous performance
		{
			m_arrET[iter] = m_arrET[iter - 1];
			m_arrJitCost[iter] = m_arrJitCost[iter - 1];
			m_arrMSD[iter] = m_arrMSD[iter - 1];
		}

		msd = 0.0;
		cum_et = 0.0;
		cum_travl_time = 0.0;
		cum_travl_dist = 0.0;

	} // end of iter


	vRouteByVehicle.resize(num_of_vehicle);
	vRouteByVehicle[v_id - 1].clear();
	//for (int j = 0; j <= num_of_vehicle - 1; j++)
	//{
	for (int i = 0; i <= vBestRoutes.size() - 1; i++)
	{
		if (vBestRoutes[i].v_id == v_id - 1 + 1)
		{
			vRouteByVehicle[v_id - 1].push_back(vBestRoutes[i]);
		}
	}
	//}

	//vRouteByVehicle_Cpy = vRouteByVehicle;

	PrintDATCResult();


	for (int i = 0; i <= num_of_vehicle - 1; i++)
	{
		for (int j = 0; j <= vRouteByVehicle[i].size() - 1; j++)
			cout << vRouteByVehicle[i][j].c_id << "-";

		cout << endl;
	}

	cout << "xx" << endl;

}

double GetCurrX(double x1, double y1, double x2, double y2, double trv_time)
{
	// (x1, y1) -> (x2, y2), sojourn time = t
	double a = (y1 - y2) / (x1 - x2);
	double b = y1 - x1 * (y1 - y2) / (x1 - x2);

	//double org_dist = 2 * scale_for_dist*sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
	//double proj_dist = 2 * scale_for_dist*sqrt((x2 - x1)*(x2 - x1));

	double org_dist = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
	double proj_dist = sqrt((x2 - x1)*(x2 - x1));
	double proj_trvtime = proj_dist / v_speed;

	double x3 = x1 + (x2 - x1)*trv_time / proj_trvtime;

	return x3;
}

double GetCurrY(double x1, double y1, double x2, double y2, double trv_time)
{
	// (x1, y1) -> (x2, y2), sojourn time = t
	double a = (y1 - y2) / (x1 - x2);
	double b = y1 - x1 * (y1 - y2) / (x1 - x2);

	//double org_dist = 2 * scale_for_dist*sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
	//double proj_dist = 2 * scale_for_dist*sqrt((x2 - x1)*(x2 - x1));

	double org_dist = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
	double proj_dist = sqrt((x2 - x1)*(x2 - x1));
	double proj_trvtime = proj_dist / v_speed;

	double x3 = x1 + (x2 - x1)*trv_time / proj_trvtime;

	return a*x3 + b;
}

void Print_Pajek_Result(double current_time)
{
	ofstream ofs_pajek;
	time_t t = time(0);   // get time now
	char buff[20];
	time_t now = time(NULL);
	strftime(buff, 20, "%Y-%m-%d %H-%M-%S", localtime(&now));
	cout << buff << endl;

	//char array0[] = "exp result/";
	char* array1 = buff;
	char array2[] = "_Pajek.net";
	//char * newArray = new char[strlen(array0)+strlen(array1) + strlen(array2) + 1];
	char * newArray = new char[strlen(array1) + strlen(array2) + 1];
	//strcpy(newArray, array0);
	strcpy(newArray, array1);
	strcat(newArray, array2);

	ofs_pajek.open(newArray);

	ofs_pajek << "*Vertices    " << num_of_customer + num_of_vehicle + 1 << endl;
	ofs_pajek << 1 << " " << "c0" << " " << 0.5 << " " << 0.5 << " " << "box" << endl;

	int temp = 1;
	// print customers
	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
		{
			ofs_pajek << vRouteByVehicle[j][i].c_id + 1 << " " << "c" << vRouteByVehicle[j][i].c_id << ":" << vRouteByVehicle[j][i].comp_time << " " << vRouteByVehicle[j][i].x_pos / 100.0 << " " << vRouteByVehicle[j][i].y_pos / 100.0 << " " << "ellipse" << " " << "red" << endl;
			temp++;
		}
	}

	temp++;
	// print vehicles' current location
	for (int j = 1; j <= num_of_vehicle; j++)
	{
		ofs_pajek << temp << " " << "v" << vehicle[j].v_id << ":" << current_time << " " << vehicle[j].xPos / 100.0 << " " << vehicle[j].yPos / 100.0 << " " << "triangle" << " " << "Blue" << endl;
		temp++;
	}


	ofs_pajek << "*Arcs" << endl;

	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		ofs_pajek << 1 << " " << vRouteByVehicle[j][0].c_id + 1 << endl;

		for (int i = 1; i <= vRouteByVehicle[j].size() - 1; i++)
			ofs_pajek << vRouteByVehicle[j][i - 1].c_id + 1 << " " << vRouteByVehicle[j][i].c_id + 1 << endl;
	}
}

double SimulDatc::GetCurrentET(double current_time)
{
	bool bErase = false;
	vector<CUSTOMER>::iterator ivec_it;

	UpdateVehicleLocation(current_time);
	Print_Pajek_Result(current_time);
	RemoveCompleteCustomer(current_time);

	// Calculate current total et
	double current_et = 0.0;
	int c_id = 0;
	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
		{
			c_id = vRouteByVehicle[j][i].c_id;
			if (vBestRoutes[c_id - 1].customer_tag == 0)
				current_et += vBestRoutes[c_id - 1].et;
		}
	}

	return current_et;
}

double GetCurrentMSD(double current_time)
{
	bool bErase = false;
	vector<CUSTOMER>::iterator ivec_it;

	UpdateVehicleLocation(current_time);
	Print_Pajek_Result(current_time);
	RemoveCompleteCustomer(current_time);

	// Calculate current total MSD
	double current_msd = 0.0;
	int num_cust = 0;
	int c_id = 0;
	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
		{
			c_id = vRouteByVehicle[j][i].c_id;
			if (vBestRoutes[c_id - 1].customer_tag == 0)
			{
				current_msd += vBestRoutes[c_id - 1].et*vBestRoutes[c_id - 1].et;
				num_cust++;
			}
		}
	}

	return sqrt(current_msd) / num_cust;
}

double SimulDatc::GetCurrentCapacity(double current_time)
{
	bool bErase = false;
	vector<CUSTOMER>::iterator ivec_it;

	// Update vehicle's current location (x, y) at the current time
	UpdateVehicleLocation(current_time);
	//Print_Pajek_Result(current_time);

	// Remove completed customers (comp_time <= current_time)
	double remain_demand = 0.0;

	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
		{
			if (bErase)
				i = 0;

			if (vRouteByVehicle[j][i].comp_time <= current_time)
			{
				ivec_it = vRouteByVehicle[j].begin() + i;
				vRouteByVehicle[j].erase(ivec_it);
				bErase = true;

				if (vRouteByVehicle[j][i].order_type == 0) // if order type is pickup
					remain_demand += vRouteByVehicle[j][i].demand;
			}
			else
			{
				bErase = false;
				remain_demand += vRouteByVehicle[j][i].demand;
			}

		}
	}

	// Calculate available capacity
	double capa = 0.0;
	for (int j = 1; j <= num_of_vehicle; j++)
		capa += vehicle[j].max_capa;

	return capa - remain_demand;
}

void UpdateVehicleLocation(double current_time)
{
	double xCurrPos = 0.0;
	double yCurrPos = 0.0;
	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		if (vRouteByVehicle[j].size() >= 2)
		{
			for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
			{
				if (i == 0)
				{
					if (current_time < vRouteByVehicle[j][i].arv_time) // still at last customer
					{
						xCurrPos = vehicle[j + 1].xPos;
						yCurrPos = vehicle[j + 1].yPos;
						break;
					}

					if (vRouteByVehicle[j][i].arv_time <= current_time && current_time <= vRouteByVehicle[j][i].comp_time)
					{
						xCurrPos = GetCurrX(vehicle[j + 1].xPos, vehicle[j + 1].yPos, vRouteByVehicle[j][i].x_pos, vRouteByVehicle[j][i].y_pos, current_time - vRouteByVehicle[j][i].arv_time);
						yCurrPos = GetCurrY(vehicle[j + 1].xPos, vehicle[j + 1].yPos, vRouteByVehicle[j][i].x_pos, vRouteByVehicle[j][i].y_pos, current_time - vRouteByVehicle[j][i].arv_time);
						break;
					}
				}
				else
				{
					if (vRouteByVehicle[j][i - 1].comp_time <= current_time && current_time < vRouteByVehicle[j][i].arv_time) // still at last customer
					{
						xCurrPos = vRouteByVehicle[j][i - 1].x_pos;
						yCurrPos = vRouteByVehicle[j][i - 1].y_pos;
						break;
					}

					if (vRouteByVehicle[j][i].arv_time <= current_time && current_time <= vRouteByVehicle[j][i].comp_time)
					{
						xCurrPos = GetCurrX(vRouteByVehicle[j][i - 1].x_pos, vRouteByVehicle[j][i - 1].y_pos, vRouteByVehicle[j][i].x_pos, vRouteByVehicle[j][i].y_pos, current_time - vRouteByVehicle[j][i].arv_time);
						yCurrPos = GetCurrY(vRouteByVehicle[j][i - 1].x_pos, vRouteByVehicle[j][i - 1].y_pos, vRouteByVehicle[j][i].x_pos, vRouteByVehicle[j][i].y_pos, current_time - vRouteByVehicle[j][i].arv_time);
						break;
					}
				}
			}
		}

		if (xCurrPos != 0.0 && yCurrPos != 0.0)
		{
			vehicle[j + 1].xPos = xCurrPos;
			vehicle[j + 1].yPos = yCurrPos;
		}
	}
}

void RemoveCompleteCustomer(double current_time)
{
	bool bErase = false;
	vector<CUSTOMER>::iterator ivec_it;

	for (int j = 0; j <= num_of_vehicle - 1; j++)
	{
		for (int i = 0; i <= vRouteByVehicle[j].size() - 1; i++)
		{
			if (bErase)
				i = 0;

			if (vRouteByVehicle[j][i].comp_time <= current_time)
			{
				ivec_it = vRouteByVehicle[j].begin() + i;
				vRouteByVehicle[j].erase(ivec_it);
				bErase = true;
			}
			else
				bErase = false;
		}
	}
}

void SimulDatc::InterRunDATC(
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
	// backup current data
	vRouteByVehicle_Cpy.clear();
	vRouteByVehicle_Cpy = vRouteByVehicle;
	vCurrRoutes_Cpy.clear();
	vCurrRoutes_Cpy = vCurrRoutes;
	vBestRoutes_Cpy.clear();
	vBestRoutes_Cpy = vBestRoutes;

	bool bErase = false;
	vector<CUSTOMER>::iterator ivec_it;

	UpdateVehicleLocation(current_time);
	Print_Pajek_Result(current_time);
	RemoveCompleteCustomer(current_time);

	int new_c_id = max_c_id + 1;
	int sel_vehicle = 0;

	// Add pickup order
	CUSTOMER newcustomer_pickup;
	newcustomer_pickup.c_id = new_c_id;
	newcustomer_pickup.loc_id = new_c_id;
	newcustomer_pickup.prev_c_id = new_c_id;
	newcustomer_pickup.prev_loc_id = 0;
	newcustomer_pickup.v_id = sel_vehicle;
	newcustomer_pickup.demand = demand;
	newcustomer_pickup.minW = P_minW;
	newcustomer_pickup.maxW = P_maxW;
	newcustomer_pickup.arv_time = current_time;
	newcustomer_pickup.comp_time = 0.0;
	newcustomer_pickup.service_time = P_service_time;
	newcustomer_pickup.et = 0.0;
	newcustomer_pickup.x_pos = P_x_pos;
	newcustomer_pickup.y_pos = P_y_pos;
	newcustomer_pickup.customer_tag = 0;
	newcustomer_pickup.complete_tag = 0;
	newcustomer_pickup.v_trv_time = 0.0;
	newcustomer_pickup.v_trv_distance = 0.0;
	  
	vRouteByVehicle[sel_vehicle - 1].push_back(newcustomer_pickup);

	// Add delivery order
	new_c_id = new_c_id + 1;

	CUSTOMER newcustomer_delivery;
	newcustomer_delivery.c_id = new_c_id;
	newcustomer_delivery.loc_id = new_c_id;
	newcustomer_delivery.prev_c_id = new_c_id;
	newcustomer_delivery.prev_loc_id = 0;
	newcustomer_delivery.v_id = 0;
	newcustomer_delivery.demand = demand;
	newcustomer_delivery.minW = D_minW;
	newcustomer_delivery.maxW = D_maxW;
	newcustomer_delivery.arv_time = current_time;
	newcustomer_delivery.comp_time = 0.0;
	newcustomer_delivery.service_time = D_service_time;
	newcustomer_delivery.et = 0.0;
	newcustomer_delivery.x_pos = D_x_pos;
	newcustomer_delivery.y_pos = D_y_pos;
	newcustomer_delivery.customer_tag = 0;
	newcustomer_delivery.complete_tag = 0;
	newcustomer_delivery.v_trv_time = 0.0;
	newcustomer_delivery.v_trv_distance = 0.0;

	vRouteByVehicle[sel_vehicle - 1].push_back(newcustomer_delivery);

	vCurrRoutes = vRouteByVehicle[sel_vehicle - 1];

	// Assign vehicle
	sel_vehicle = GetVehicle(0, new_c_id - 1, P_minW, P_maxW);

	if (sel_vehicle == 0)
		sel_vehicle = 1;


	//for (int i = 0; i <= num_of_vehicle - 1; i++)
	//{
	//	for (int j = 0; j <= vRouteByVehicle[i].size() - 1; j++)
	//		cout << vRouteByVehicle[i][j].c_id << "-";
	//	cout << endl;
	//}

	//cout << vCurrRoutes.size() << endl;
	//for (int j = 0; j<=vCurrRoutes.size() - 1; j++)
	//	cout << vCurrRoutes[j].c_id << "-";

	num_of_customer = vCurrRoutes.size();
	RunDATC_for_SelectedVehicle(sel_vehicle);
}



void SimulDatc::RejectNewOrder(double current_time)
{
	// Restore previous data
	vRouteByVehicle.clear();
	vRouteByVehicle = vRouteByVehicle_Cpy;
	vCurrRoutes.clear();
	vCurrRoutes = vCurrRoutes_Cpy;
	vBestRoutes.clear();
	vBestRoutes = vBestRoutes_Cpy;
}

int SimulDatc::main_from_external()
{
	SimulDatc *m = new SimulDatc();

	InitData();

	CreateDistanceMatrix();

	m->InitialRunDATC();

	cout << best_iter << endl;

	//vector<int>::iterator ivec_it;


	m->InterRunDATC(
		10.0, // demand,
		16.0, // P_minW,
		16.1, // P_maxW,
		0.1, // P_service_time,
		20.0, // P_x_pos,
		10.0, // P_y_pos,
		18.0, // D_minW,
		18.2, // D_maxW,
		0.1, // D_service_time,
		12.0, // D_x_pos,
		12.1, // D_y_pos,
		13.5// current_time
	);

	//cout << "Current Capa: " << GetCurrentCapacity(13.5) << endl;
	//cout << "Current ET: " << GetCurrentET(13.5) << endl;

	m->RejectNewOrder(13.5 /*current_time*/);

	return 0;
}


int _tmain(int argc, _TCHAR* argv[])
{
	SimulDatc *m = new SimulDatc();

	InitData();

	CreateDistanceMatrix();

	m->InitialRunDATC();

	cout << best_iter << endl;

	//vector<int>::iterator ivec_it;


	m->InterRunDATC(
		10.0, // demand,
		16.0, // P_minW,
		16.1, // P_maxW,
		0.1, // P_service_time,
		20.0, // P_x_pos,
		10.0, // P_y_pos,
		18.0, // D_minW,
		18.2, // D_maxW,
		0.1, // D_service_time,
		12.0, // D_x_pos,
		12.1, // D_y_pos,
		13.5// current_time
	);

	//cout << "Current Capa: " << GetCurrentCapacity(13.5) << endl;
	//cout << "Current ET: " << GetCurrentET(13.5) << endl;

	m->RejectNewOrder(13.5 /*current_time*/);

	return 0;
}

