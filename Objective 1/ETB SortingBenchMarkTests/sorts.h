/*
 * sorts.h
 *
 *  Created on: Oct 24, 2011
 *      Author: Dux
 */

#ifndef SORTS_H_
#define SORTS_H_
#include "StandardIncludes.h"

class sorts
{
public:
	sorts();
	virtual ~sorts();
	vector<int> quickSort(vector<int>);
	vector<int> selectionSort(vector<int> toBeSorted);
	vector<int> insertionSort(vector<int> toBeSorted);
	//void mergeSort(vector<int> toBeSorted, int left, int right);

private:
	vector<int> swap(vector<int>, int element1, int element2);
	//void merge(vector<int> toBeSorted, int left, int right, int mid);
	//vector<int> mergeVector;

};

#endif /* SORTS_H_ */
