/*
 *
 * University of Advancing Technology Standard C++ Template Driver
 *
 * Name: Driver.cpp
 * Student: Default Student
 *
 */

#include "StandardIncludes.h"
#include "sorts.h"
#include <boost/timer.hpp>


sorts sorter;
boost::timer testTimer;
vector<int> fillVectorWithRandomInts(int MAX);
float testInsertionSort(int vectorSize, int iterations);
float testSelectionSort(int vectorSize, int iterations);
float testQuickSort(int vectorSize, int iterations);

int main()
{
	//Selection sort works here... but not down there :(
//	vector<int> quick;
//	sorts sorter;
//	quick.reserve(100);
//	quick = fillVectorWithRandomInts(100);
//	for(unsigned int index = 0; index < quick.size(); index++)
//		cout << quick.at(index) << " ";
//	quick = sorter.selectionSort(quick);
//
//	cout << endl;
//
//	for(unsigned int index = 0; index < quick.size(); ++index)
//	{
//		cout << quick[index] << " ";
//	}

	float testTime = 0;
	float insertionTime = 0;
	float selectionTime = 0;
	float mergeTime = 0;
	float quickTime = 0;
	for (int vectorSize = 5; vectorSize <= 100000;)
	{
		for (int iterations = 100000; iterations >= 10;)
		{
			testTime = testInsertionSort(vectorSize, iterations);
			cout << "Insertion Sort - Vector Size: " << vectorSize << " , Iterations: " << iterations << " = " << testTime * 1000 << " milliseconds per iteration of CPU usage" << endl;
			insertionTime += testTime;

//			testTime = testSelectionSort(vectorSize, iterations);
//			cout << "Selection Sort - Vector Size: " << vectorSize << " , Iterations: " << iterations << " = " << testTime * 1000 << " milliseconds per iteration of CPU usage" << endl;
//			selectionTime += testTime;

			testTime = testQuickSort(vectorSize, iterations);
			cout << "Quick Sort - Vector Size: " << vectorSize << " , Iterations: " << iterations << " = " << testTime * 1000 << " milliseconds per iteration of CPU usage" << endl;
			quickTime += testTime;

			if(vectorSize == 5)
			{
				vectorSize = 10;
			}
			else
			{
				vectorSize *= 10;
				iterations /= 10;
			}
		}
	}

	cout << endl;
	cout << "Results: Quick Sort average time = " << (quickTime / 6) * 1000 << endl;
	cout << "Results: Merge Sort average time = " << (mergeTime / 6) * 1000 << endl;
	cout << "Results: Selection Sort average time = " << (selectionTime / 6) * 1000 << endl;
	cout << "Results: Insertion Sort average time = " << (insertionTime / 6) * 1000 << endl;
	cout << "Selection Sort crashes when running; Merge Sort incomplete.";

	return 0;
}

vector<int> fillVectorWithRandomInts(int MAX)
{
	vector<int> temp;
	temp.reserve(MAX);

	for(int index = 0; index < MAX; ++index)
	{
		temp.push_back(rand() % MAX);
	}
	return temp;
}

float testInsertionSort(int vectorSize, int numberOfIterations)
{
	vector<int> insertion;
	insertion.reserve(vectorSize);
	float elapsedTime = 0;

	for (int iterations = 0; iterations < numberOfIterations; ++iterations)
	{
		insertion = fillVectorWithRandomInts(insertion.size());
		testTimer.restart();
		insertion = sorter.insertionSort(insertion);
		elapsedTime += testTimer.elapsed();
	}
	return (elapsedTime / numberOfIterations);
}

float testSelectionSort(int vectorSize, int numberOfIterations)
{
	vector<int> selection;
	selection.reserve(vectorSize);
	float elapsedTime = 0;

	for (int iterations = 0; iterations < numberOfIterations; ++iterations)
	{
		selection = fillVectorWithRandomInts(selection.size());
		testTimer.restart();
		selection = sorter.selectionSort(selection);
		elapsedTime += testTimer.elapsed();
	}
	return (elapsedTime / numberOfIterations);
}

float testQuickSort(int vectorSize, int numberOfIterations)
{
	vector<int> quick;
	quick.reserve(vectorSize);
	float elapsedTime = 0;

	for (int iterations = 0; iterations < numberOfIterations; ++iterations)
	{
		quick = fillVectorWithRandomInts(quick.size());
		testTimer.restart();
		quick = sorter.quickSort(quick);
		elapsedTime += testTimer.elapsed();
	}
	return (elapsedTime / numberOfIterations);
}
