/*
 * TestStack.cpp
 *
 *  Created on: Sep 16, 2011
 *      Author: ethbehar
 */

#include "TestStack.h"
#include "StandardIncludes.h"

void TestStack::setUp()
{
	testIntStack = new Stack<int>(10);
	testInt = 0;
}

void TestStack::tearDown()
{
	delete testIntStack;
}

void TestStack::testIsEmpty()
{
	cout << "Test isEmpty starting" << endl;

	CPPUNIT_ASSERT(testIntStack->isEmpty() == true);

	cout << "Test isEmpty ending" << endl;
}

void TestStack::testPush()
{
	cout << "Test Push starting" << endl;

	testIntStack->push(5);
	testIntStack->push(10);
	testIntStack->push(15);
	testIntStack->push(20);
	testIntStack->push(25);
	testIntStack->push(30);
	testIntStack->push(35);
	testIntStack->push(40);
	testIntStack->push(45);
	CPPUNIT_ASSERT(testIntStack->size() == 9);

	cout << "Test Push Ending" << endl;
}

void TestStack::testSize()
{
	cout << "Test Size starting" << endl;

	testIntStack->clear();
	testIntStack->push(2);
	testIntStack->push(4);
	testInt = testIntStack->size();
	CPPUNIT_ASSERT(testInt == 2);

	cout << "Test Size ending" << endl;
}


void TestStack::testPop()
{
	cout << "Test Pop starting" << endl;

	testIntStack->clear();
	testIntStack->push(2);
	testIntStack->push(4);
	testInt = testIntStack->pop();
	CPPUNIT_ASSERT(testInt == 4);

	cout << "Test Pop ending" << endl;
}

void TestStack::testTop()
{
	cout << "Test Top starting" << endl;

	testIntStack->push(50);
	testInt = testIntStack->top();
	CPPUNIT_ASSERT(testInt == 50);

	cout << "Test Top Ending" << endl;
}


void TestStack::testClear()
{
	cout << "Test Clear starting" << endl;

	testIntStack->clear();
	CPPUNIT_ASSERT(testIntStack->size() == 0);

	cout << "Test Clear ending" << endl;
}

