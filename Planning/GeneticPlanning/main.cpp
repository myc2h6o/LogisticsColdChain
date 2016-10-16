#include <vector>
#include <time.h>
#include <stdio.h>
#include <iostream>
#include <assert.h>
#include <fstream>
using namespace std;

#define CARS_FILE "resources/cars.txt"
#define DIS_FM_FILE "resources/disfm.txt"
#define TIME_FM_FILE "resources/timefm.txt"
#define TOLL_FM_FILE "resources/tollfm.txt"
#define DIS_FF_FILE "resources/disff.txt"
#define TIME_FF_FILE "resources/timeff.txt"
#define TOLL_FF_FILE "resources/tollff.txt"
#define REQUIRE_FILE "resources/require.txt"

#define N_FREEZER 8
#define N_MARKET 10
#define N_CAR_TYPE 3

#define N_POPULATION 4
int N_ITER = 0;    //warning: should be larger than 99, just for printing


/*
 *weight of cars: 5  for 0.5t
                  20 for 2t
                  40 for 4t
 */

const int car_type[N_CAR_TYPE] = { 5, 20, 40 };

//nMaxPlan: max plan for a car per day
//maxTime: max time a car should work
int maxTime = 600;
int nMaxPlan = 2;    //warning: now nMaxPlan = 1 seems wrong


int disfm[N_MARKET][N_FREEZER];
int timefm[N_MARKET][N_FREEZER];
int tollfm[N_MARKET][N_FREEZER];
int disff[N_FREEZER][N_FREEZER];
int timeff[N_FREEZER][N_FREEZER];
int tollff[N_FREEZER][N_FREEZER];

//m for markets
//f for freezers
struct Plan {
    //warning: split one path with same dst into two to save money (this is abandoned for user feeling)
    int codem;
    int numf;
    int weight[N_FREEZER];
    int route[N_FREEZER];
};

struct Require {
    int codef;
    int codem;
    int weight;
};

bool checkDemand(int g);

struct Car {
    int weight;
    int codef;
    vector<vector<Plan>> plans;
    int getWorkTime() { return 0; }    //warning: now do not consider time window

    double cost(int nGroup) {
        /*
         * based on several freezers to one market
         * cost = 0.44 * dis + 6.17/60/10 * weight * time + toll
         * dis(km) weight(0.1*t) time(min)
         */
        double ret = 0;
        int dis = 0;
        int time = 0;
        int toll = 0;

        dis += disff[codef][plans[nGroup][0].route[0]];
        time += timeff[codef][plans[nGroup][0].route[0]];
        toll += tollff[codef][plans[nGroup][0].route[0]];
        for (int i = 0; i < plans[nGroup].size(); ++i) {
            for (int j = 0; j < plans[nGroup][i].numf - 1; ++j) {
                dis += disff[plans[nGroup][i].route[j]][plans[nGroup][i].route[j + 1]];
                time += timeff[plans[nGroup][i].route[j]][plans[nGroup][i].route[j + 1]];
                toll += tollff[plans[nGroup][i].route[j]][plans[nGroup][i].route[j + 1]];
            }
            dis += disfm[plans[nGroup][i].codem][plans[nGroup][i].route[plans[nGroup][i].numf - 1]];
            time += timefm[plans[nGroup][i].codem][plans[nGroup][i].route[plans[nGroup][i].numf - 1]];
            toll += tollfm[plans[nGroup][i].codem][plans[nGroup][i].route[plans[nGroup][i].numf - 1]];
            
            /*if (i != plans[nGroup].size() - 1) {
                //not the last plan
                dis += disfm[plans[nGroup][i].codem][plans[nGroup][i + 1].route[0]];
                time += timefm[plans[nGroup][i].codem][plans[nGroup][i + 1].route[0]];
                toll += tollfm[plans[nGroup][i].codem][plans[nGroup][i + 1].route[0]];
            }*/
            //tmp
            if (i != plans[nGroup].size() - 1) {
                //not the last plan
                dis += disff[codef][plans[nGroup][i + 1].route[0]];
                time += timeff[codef][plans[nGroup][i + 1].route[0]];
                toll += tollff[codef][plans[nGroup][i + 1].route[0]];
            }
        }
        //now do not consider backing to home freezer
        /*dis += disfm[plans[nGroup][plans[nGroup].size() - 1].codem][codef];
        time += timefm[plans[nGroup][plans[nGroup].size() - 1].codem][codef];
        toll += tollfm[plans[nGroup][plans[nGroup].size() - 1].codem][codef];
        */
        return 0.44 * dis + 0.010283 * weight * time + toll;
    }

    void optimizePlan(int nGroup) {
        //warning: now use random to replace this
        return;
    }

    int addWork(const Require &require, int nGroup, int flag) {
        //flag 0: do not need to full the require weigt
        //       1: need to do
        int nPlans = plans[nGroup].size();
        for (int i = 0; i < nPlans; ++i) {
            if (plans[nGroup][i].codem == require.codem) {
                //to same dst
                if (plans[nGroup][i].weight[require.codef] == 0) {
                    //not in plan
                    int totalWeight = 0;
                    for (int j = 0; j < plans[nGroup][i].numf; ++j) {
                        totalWeight += plans[nGroup][i].weight[plans[nGroup][i].route[j]];
                    }
                    if (flag == 0 && totalWeight == weight) return 0;
                    else if (flag == 1 && totalWeight + require.weight > weight) return 0;

                    //merge them
                    plans[nGroup][i].weight[require.codef] = (require.weight < (weight - totalWeight)) ? require.weight : (weight - totalWeight);
                    plans[nGroup][i].route[plans[nGroup][i].numf] = require.codef;
                    plans[nGroup][i].numf++;
                    //optimizePlan(nGroup);
                    return plans[nGroup][i].weight[require.codef];
                }
            }
        }
        if (nPlans == nMaxPlan) return 0;
        //can not merge or not to same dst, add new plan
        Plan tmpPlan;
        tmpPlan.numf = 1;
        tmpPlan.codem = require.codem;
        for (int i = 0; i < N_FREEZER; ++i)
            tmpPlan.weight[i] = 0;

        if ((flag == 1) && (require.weight > weight)) return 0;
        tmpPlan.weight[require.codef] = (require.weight < weight) ? require.weight : weight;
        tmpPlan.route[0] = require.codef;
        plans[nGroup].push_back(tmpPlan);
        //optimizePlan(nGroup);
        return tmpPlan.weight[require.codef];
    }

    bool deleteRandomWork(Require &req, int nGroup) {
        if (plans[nGroup].size() == 0) return false;
        int planNum = rand() % plans[nGroup].size();
        int routeNum = rand() % plans[nGroup][planNum].numf;
        //copy to req
        req.codef = plans[nGroup][planNum].route[routeNum];
        req.codem = plans[nGroup][planNum].codem;
        req.weight = plans[nGroup][planNum].weight[plans[nGroup][planNum].route[routeNum]];

        //delete from and weight route, and minus numf
        plans[nGroup][planNum].weight[plans[nGroup][planNum].route[routeNum]] = 0;
        plans[nGroup][planNum].numf--;
        if (plans[nGroup][planNum].numf == 0) {
            //[TODO]warning: now just consider max plan number == 2
            if (planNum == 1) plans[nGroup].pop_back();
            else if (planNum == 0 && plans[nGroup].size() == 1)
                plans[nGroup].pop_back();
            else if (planNum == 0 && plans[nGroup].size() != 1) {
                Plan tmpPlan = plans[nGroup][1];
                plans[nGroup].clear();
                plans[nGroup].push_back(tmpPlan);
            }
        }else {
            if (planNum >= plans[nGroup].size()) return true;
            for (int i = routeNum; i < plans[nGroup][planNum].numf; ++i) {
                plans[nGroup][planNum].route[i] = plans[nGroup][planNum].route[i + 1];
            }
        }
        return true;
    }

    void changeRouteRandom(int groupNum, int planNum) {
    //[TDOD] now just consider a car with 2 nMaxPlan
        int f0 = 0;
        int f1 = 0;
        switch (plans[groupNum][planNum].numf) {
        case 2:
            f0 = 0;
            f1 = 1;
            break;
        case 3:
            f0 = rand() % 3;
            f1 = rand() % 3;
            if (f0 == f1) f0 = rand() % 3;
            if (f0 == f1) f1 = rand() % 3;
            break;
        case 4:
            f0 = rand() % 4;
            f1 = rand() % 4;
            if (f0 == f1) f1 = rand() % 4;
            break;
        default:
            break;
        }
        if (f0 != f1) {
            double oldCost = cost(groupNum);
            int tmp = plans[groupNum][planNum].route[f0];
            plans[groupNum][planNum].route[f0] = plans[groupNum][planNum].route[f1];
            plans[groupNum][planNum].route[f1] = tmp;
            if (cost(groupNum) > oldCost) {
                //change back
                plans[groupNum][planNum].route[f1] = plans[groupNum][planNum].route[f0];
                plans[groupNum][planNum].route[f0] = tmp;
            }
        }
    }
};

vector<Car> cars;
vector<Require> requires;

int initCarInherit() {
    FILE *fp = NULL;
    fp = fopen(CARS_FILE, "r");
    if (!fp) return 1;

    int nCar = 0;
    Car tmpCar;
    for (int i = 0; i < N_FREEZER; ++i) {
        for (int j = 0; j < N_CAR_TYPE; ++j) {
            fscanf(fp, "%d", &nCar);
            while (nCar > 0) {
                tmpCar.codef = i;
                tmpCar.weight = car_type[j];
                cars.push_back(tmpCar);
                nCar--;
            }
        }
    }
    fclose(fp);
    return 0;
};

int initMarketFreezer() {
    FILE *fp = NULL;
    //freezer to market
    //disfm
    fp = fopen(DIS_FM_FILE, "r");
    if (!fp) return 1;
    for (int i = 0; i < N_MARKET; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &(disfm[i][j]));
        }
    }
    fclose(fp);
    //timefm
    fp = fopen(TIME_FM_FILE, "r");
    if (!fp) return 1;
    for (int i = 0; i < N_MARKET; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &(timefm[i][j]));
        }
    }
    fclose(fp);
    //tollfm
    fp = fopen(TOLL_FM_FILE, "r");
    if (!fp) return 1;
    for (int i = 0; i < N_MARKET; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &(tollfm[i][j]));
        }
    }
    fclose(fp);

    //freezer to freezer
    //disff
    fp = fopen(DIS_FF_FILE, "r");
    if (!fp) return 1;
    for (int i = 0    ; i < N_FREEZER; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &(disff[i][j]));
        }
    }
    fclose(fp);
    //timeff
    fp = fopen(TIME_FF_FILE, "r");
    if (!fp) return 1;
    for (int i = 0; i < N_FREEZER; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &(timeff[i][j]));
        }
    }
    fclose(fp);
    //tollff
    fp = fopen(TOLL_FF_FILE, "r");
    if (!fp) return 1;
    for (int i = 0; i < N_FREEZER; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &(tollff[i][j]));
        }
    }
    fclose(fp);
    return 0;
}


int initRequirement() {
    FILE *fp = NULL;
    fp = fopen(REQUIRE_FILE, "r");
    if (!fp) return 1;

    Require tmpReq;
    int tmpWeight;
    for (int i = 0; i < N_MARKET; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            fscanf(fp, "%d", &tmpWeight);
            if (tmpWeight == 0) continue;
            tmpReq.codem = i;
            tmpReq.codef = j;
            tmpReq.weight = tmpWeight;
            requires.push_back(tmpReq);
        }
    }
    fclose(fp);
}

void findInitGroup(int nGroup) {
    int nReq = requires.size();
    int nCar = cars.size();
    int time = 0;
    vector<Plan> tmpPlans;
    //initialize car plan group
    for (int i = 0; i < nCar; ++i)
        cars[i].plans.push_back(tmpPlans);
    //distribute tasks
    for (int i = 0; i < nReq; ++i) {
        //replace this for more generalized case due to weight
        int typeNum = N_CAR_TYPE - 1;
        while (typeNum >= 0) {    //warning, typeNum should not be unsigned
            while (requires[i].weight > car_type[typeNum]) {
                if (nGroup % 2 == 0) {
                    for (int j = 0; j < nCar; ++j) {
                        int retWeight = 0;
                        if ((retWeight = cars[j].addWork(requires[i], nGroup, 0)) != 0) {
                            requires[i].weight -= retWeight;
                            break;
                        }
                    }
                }
                else {
                    for (int j = nCar-1; j >= 0; --j) {
                        int retWeight = 0;
                        if ((retWeight = cars[j].addWork(requires[i], nGroup, 0)) != 0) {
                            requires[i].weight -= retWeight;
                            break;
                        }
                    }
                }
            }
            typeNum--;
        }
        //deal with the remainder
        while (requires[i].weight != 0) {
            bool noCar = true;
            if (nGroup % 3 == 0) {
                for (int j = 0; j < nCar; ++j) {
                    int retWeight = 0;
                    if ((retWeight = cars[j].addWork(requires[i], nGroup, 0)) != 0) {
                        requires[i].weight -= retWeight;
                        noCar = false;
                        break;
                    }
                }
            }
            else {
                for (int j = nCar-1; j >= 0; --j) {
                    int retWeight = 0;
                    if ((retWeight = cars[j].addWork(requires[i], nGroup, 0)) != 0) {
                        requires[i].weight -= retWeight;
                        noCar = false;
                        break;
                    }
                }
            }
            assert(noCar == false);
        }
    }
}

void inheritPlan(int nInitGroup) {
    //nInitGroup:size of initial group
    for (int i = 0; i < nInitGroup; ++i) {
        vector<Require> tmpReq = requires;
        findInitGroup(i);
        requires = tmpReq;
    }
    vector<double> costs;
    int nCar = cars.size();
    for (int i = 0; i < nInitGroup; ++i) {
        double sum = 0;
        for (int j = 0; j < nCar; ++j) {
            if(cars[j].plans[i].size() != 0)
                sum += cars[j].cost(i);
        }
        costs.push_back(sum);
    }

    for (int iterNum = 0; iterNum < N_ITER; ++iterNum) {
        if (iterNum % (N_ITER / 10) == 0) {
            printf("---------------%d%%---------------\n", iterNum / (N_ITER / 100));
        }
        vector<Car> tmpCars = cars;
        vector<Require> tmpReqs;
        Require tmpReq;
        int groupNum = rand() % nInitGroup;
        int nCarChosen = rand() % 8 + 2;
        //warning: min tasks is required, i.e. task should not be too less
        tmpReqs.clear();
        //remove one random requirement from random car

        int curCarCnt = 0;
        while (nCarChosen > 0) {
            nCarChosen--;
            curCarCnt = rand() % nCar;
            if (tmpCars[curCarCnt].deleteRandomWork(tmpReq, groupNum)) {
                tmpReqs.push_back(tmpReq);
            }
        }


        //randomly add new requirement
        bool dealed = false;
        for (int i = 0; i < tmpReqs.size(); ++i) {
            dealed = false;
            int checkNum = 10;
            while (true) {
                checkNum--;
                curCarCnt = rand() % nCar;
                if (tmpCars[curCarCnt].addWork(tmpReqs[i], groupNum, 1) != 0) {
                    dealed = true;
                    break;
                }
                if (checkNum == 0) break;
            }
            if (!dealed) {
                for (int j = 0; j < nCar; ++j) {
                    if (tmpCars[i].addWork(tmpReqs[i], groupNum, 1) != 0) {
                        dealed = true;
                        break;
                    }
                }
            }
            if (!dealed) {
                break;
            }
        }

        if (!dealed) continue;

        //assert(checkDemand(groupNum) == true);

        //optimize route in each plan
        //[TDOD] now just consider a car with 2 nMaxPlan
        for (int i = 0; i < nCar; ++i) {
            if (tmpCars[i].plans[groupNum].size() == 0) {
                continue;
            }
            else if (tmpCars[i].plans[groupNum].size() == 1) {
                tmpCars[i].changeRouteRandom(groupNum, 0);
            }
            else if (tmpCars[i].plans[groupNum].size() == 2) {
                //change two plans' sequence
                double oldCost = tmpCars[i].cost(groupNum);
                swap(tmpCars[i].plans[groupNum][0], tmpCars[i].plans[groupNum][1]);
                if (tmpCars[i].cost(groupNum) >= oldCost) {    //change two plans could generate same cost, so use ">="
                    //!note: plan position changed
                    //swap two plans does not desrease the cost
                    //try changing one plan's route to make cost less
                    tmpCars[i].changeRouteRandom(groupNum, 0);
                    if (tmpCars[i].cost(groupNum) >= oldCost) continue;
                        
                    tmpCars[i].changeRouteRandom(groupNum, 1);
                    if (tmpCars[i].cost(groupNum) >= oldCost) continue;

                    //!note: if changing does not make cost less, chageRouteRandom will recover the chage
                    //failed after trying changing each plan's route respectively
                    swap(tmpCars[i].plans[groupNum][0], tmpCars[i].plans[groupNum][1]);
                }
                else {
                    //!note: plan position not changed
                    //swap two plans decreased the cost successfully
                    //change each plan's route
                    tmpCars[i].changeRouteRandom(groupNum, 0);
                    tmpCars[i].changeRouteRandom(groupNum, 1);
                }
            }
        }

        //update the new generation
        //kick away the max in the 4+1
        double maxCost = costs[0];
        int maxIndex = 0;
        for (int i = 1; i < costs.size(); ++i) {
            if (maxCost < costs[i]) {
                maxCost = costs[i];
                maxIndex = i;
            }
        }

        double newCost = 0;
        for (int i = 0; i < nCar; ++i) {
            if (tmpCars[i].plans[groupNum].size() != 0)
                newCost += tmpCars[i].cost(groupNum);
        }
        if (newCost < maxCost) {
            //change
            for (int i = 0; i < nCar; ++i) {
                cars[i].plans[maxIndex] = tmpCars[i].plans[groupNum];
            }
            costs[maxIndex] = newCost;    //kick away the max cost
            //print the new population
            printf("%d: ", iterNum);
            for (int i = 0; i < N_POPULATION; ++i) {
                printf("%8.1f ", costs[i]);
            }
            printf("\n");
        }
    }
}

void printResult() {
    vector<double> costs;
    int nCar = cars.size();
    for (int i = 0; i < N_POPULATION; ++i) {
        double sum = 0;
        for (int j = 0; j < nCar; ++j) {
            if (cars[j].plans[i].size() != 0)
                sum += cars[j].cost(i);
        }
        costs.push_back(sum);
    }

    double cost = costs[0];
    int groupNum = 0;
    for (int i = 1; i < N_POPULATION; ++i) {
        if (costs[i] < cost) {
            cost = costs[i];
            groupNum = i;
        }
    }

    for (int i = 0; i < nCar; ++i) {
        for (int j = 0; j < cars[i].plans[groupNum].size(); ++j) {
            printf("car_%02d: ", i);
            switch (cars[i].codef) {
            case 0:
                printf("A -> ");
                break;
            case 1:
                printf("B -> ");
                break;
            case 2:
                printf("C -> ");
                break;
            case 3:
                printf("D -> ");
                break;
            case 4:
                printf("E -> ");
                break;
            case 5:
                printf("F -> ");
                break;
            case 6:
                printf("G -> ");
                break;
            case 7:
                printf("H -> ");
                break;
            default:
                break;
            }
            for (int k = 0; k < cars[i].plans[groupNum][j].numf; ++k) {
                switch (cars[i].plans[groupNum][j].route[k]) {
                case 0:
                    printf("A");
                    break;
                case 1:
                    printf("B");
                    break;
                case 2:
                    printf("C");
                    break;
                case 3:
                    printf("D");
                    break;
                case 4:
                    printf("E");
                    break;
                case 5:
                    printf("F");
                    break;
                case 6:
                    printf("G");
                    break;
                case 7:
                    printf("H");
                    break;
                default:
                    break;
                }
                printf(": %d -> ", cars[i].plans[groupNum][j].weight[cars[i].plans[groupNum][j].route[k]]);
            }
            printf("%d\n", cars[i].plans[groupNum][j].codem + 1);
        }
    }
    printf("total cost:%f\n", cost);
}

bool checkDemand(int g) {
    int demand[N_MARKET][N_FREEZER];
    for (int i = 0; i < N_MARKET; ++i)
        for (int j = 0; j < N_FREEZER; ++j)
            demand[i][j] = 0;
    for (int i = 0; i < cars.size(); ++i) {
        for (int j = 0; j < cars[i].plans[g].size(); ++j) {
            for (int k = 0; k < cars[i].plans[g][j].numf; ++k) {
                demand[cars[i].plans[g][j].codem][cars[i].plans[g][j].route[k]] += cars[i].plans[g][j].weight[cars[i].plans[g][j].route[k]];
            }
        }
    }

    //for testing, magic number
    int obj[N_MARKET][N_FREEZER];
    for (int i = 0; i < N_MARKET; ++i)
        for (int j = 0; j < N_FREEZER; ++j)
            obj[i][j] = 0;
    obj[0][0] = 67;
    obj[1][0] = 24;
    obj[1][1] = 58;
    obj[2][2] = 64;
    obj[3][1] = 45;
    obj[3][2] = 27;
    obj[4][0] = 38;
    obj[4][1] = 53;
    obj[4][3] = 4;
    //end test
    for (int i = 0; i < N_MARKET; ++i) {
        for (int j = 0; j < N_FREEZER; ++j) {
            if (obj[i][j] != demand[i][j])
                return false;
        }
    }
    return true;
}


int main() {
    srand(time(NULL));
    assert(initCarInherit() == 0);
    assert(initMarketFreezer() == 0);
    assert(initRequirement() == 0);
    printf("iter num(>99 please): ");
    scanf("%d", &N_ITER);
    clock_t timeStart = clock();
    inheritPlan(N_POPULATION);
    double timeCost = (clock() - timeStart) / CLOCKS_PER_SEC;
    printf("---------------100%%---------------\n");
    printResult();
    printf("total time: %.1fs\n", timeCost);
    cin.get();
    cin.get();
    return 0;
}
