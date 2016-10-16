/*
 * based on several freezers to one market
 * cost = 0.44 * dis + 6.17/60/10 * weight * time + toll
 * dis(km) weight(0.1*t) time(min)
 */

cars:
N_FREEZER lines and N_CAR_TYPE columns
each num at (i,j) means the jth type for ith freezer

disfm, timefm, tollfm:
N_MARKET lines and N_FREEZER columns
each num at (i,j) means the distance/time/toll of jth freezer and ith market

disff, timeff, tollff£º
N_FREEZER lines and N_FREEZER columns
store a symmetric matrix

require£º
N_MARKET lines and N_FREEZER columns
each num at (i,j) means the require of ith market from jth freezer
weight is multiplied by 10, i.e. 67 means 6.7t