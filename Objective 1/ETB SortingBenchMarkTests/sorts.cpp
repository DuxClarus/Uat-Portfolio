/*
 * sorts.cpp
 *
 *  Created on: Oct 24, 2011
 *      Author: Dux
 */

#include "sorts.h"

sorts::sorts()
{
	// TODO Auto-generated constructor stub

}

sorts::~sorts()
{
	// TODO Auto-generated destructor stub
}

vector<int> sorts::quickSort(vector<int> toBeSorted)
{
	vector<int> leftSide;
	vector<int> rightSide;
	if (toBeSorted.size() <= 1)
		return toBeSorted;

	int pivotIndex = rand() % toBeSorted.size();
	int pivot = toBeSorted[pivotIndex];
	toBeSorted.erase(toBeSorted.begin() + (pivotIndex));

	for (unsigned int index = 0; index < toBeSorted.size(); ++index)
	{
		if (toBeSorted[index] <= pivot)
			leftSide.push_back(toBeSorted[index]);
		else
			rightSide.push_back(toBeSorted[index]);
	}

	if (leftSide.size() >= 1)
		leftSide = quickSort(leftSide);

	if (rightSide.size() >= 1)
		rightSide = quickSort(rightSide);

	leftSide.push_back(pivot);
	toBeSorted = leftSide;
	toBeSorted.insert(toBeSorted.end(), rightSide.begin(), rightSide.end());
	return toBeSorted;
}

vector<int> sorts::selectionSort(vector<int> toBeSorted)
{
	int smallestElement = 0;
	for (int pass = 0; pass < (toBeSorted.size() - 1); ++pass)
	{
		smallestElement = pass;
		for (unsigned int index = pass + 1; index < toBeSorted.size(); index++)
		{
			if (toBeSorted[index] < toBeSorted[smallestElement])
				smallestElement = index;
		}
		toBeSorted = swap(toBeSorted, pass, smallestElement);
	}
	return toBeSorted;
}

vector<int> sorts::insertionSort(vector<int> toBeSorted)
{
	int key, pass, index;
	for (unsigned pass = 1; pass < toBeSorted.size(); ++pass)
	{
		key = toBeSorted[pass];
		for (index = pass - 1; ((index >= 0) && (toBeSorted[index] < key));
				--index)
				{
			toBeSorted[index + 1] = toBeSorted[index];
		}
		toBeSorted[index + 1] = key;
	}
	return toBeSorted;
}

//void sorts::mergeSort(vector<int> toBeSorted, int left, int right)
//{
//	if(left < right)
//	{
//		int middle = ((left + right) / 2);
//		mergeSort(toBeSorted, left, middle);
//		mergeSort(toBeSorted, middle + 1, right);
//		merge(toBeSorted, left, middle, right);
//	}
//}

//private methods
vector<int> sorts::swap(vector<int> toBeSorted, int element1, int element2)
{
	int temp = 0;
	temp = toBeSorted[element1];
	toBeSorted[element1] = toBeSorted[element2];
	toBeSorted[element2] = temp;
	return toBeSorted;
}

//void sorts::merge(vector<int> toBeSorted, int left, int middle, int right)
//{
//	vector<int> tempVector;
//	tempVector.reserve(right - left + 1);
//	int leftIndex = left;
//	int rightIndex= middle + 1;
//	int tempIndex = 0;
//
//	while((leftIndex <= middle) && (rightIndex <= right))
//	{
//		if(toBeSorted[leftIndex] < toBeSorted[rightIndex])
//			tempVector[tempIndex++] = toBeSorted[leftIndex++];
//		else
//			tempVector[tempIndex++] = toBeSorted[rightIndex++];
//	}
//
//	while(leftIndex <= middle)
//		tempVector[tempIndex] = toBeSorted[leftIndex];
//
//	while(rightIndex <= right)
//		tempVector[tempIndex++] = toBeSorted[rightIndex++];
//
//	mergeVector.swap(tempVector);
//}

