/*
 * CppUnitTest.cpp
 *
 *  Created on: Jun 28, 2011
 *      Author: ethbehar
 */

#include "LinkedListTest.h"
#include "StandardIncludes.h"

void LinkedListTest::setUp()
{
	// TODO Your setup code goes here.  Typically you will set your instance
	// variables in this code
}

void LinkedListTest::tearDown()
{
	// TODO Your tear down code goes here. Typically, you will be deleting any
	// pointers you created in the setUp method
}

void LinkedListTest::testPushBack()
{
	cout<<"Starting test for Push_Back"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	CPPUNIT_ASSERT(list[3] == 4444);
	cout<<"Ending test for Push_Back"<<endl;
}

void LinkedListTest::testClear()
{
	cout<<"Starting test for Clear"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	list.push_back(55555);
	list.clear();
	CPPUNIT_ASSERT(list.size() == 0);
	cout<<"Ending test for Clear"<<endl;
}

void LinkedListTest::testRemoveIndex()
{
	cout<<"Starting test for RemoveIndex"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.removeIndex(1);
	CPPUNIT_ASSERT(list.size() == 2);
	cout<<"Ending test for RemoveIndex"<<endl;
}

void LinkedListTest::testRemove()
{
	cout<<"Starting test for Remove"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	list.push_back(55555);
	list.remove(333);
	CPPUNIT_ASSERT(list[2] == 4444);
	cout<<"Ending tests for Remove"<<endl;
}

void LinkedListTest::testInsert()
{
	cout<<"Starting test for Insert"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.insert(1, 4444);
	CPPUNIT_ASSERT(list[1] == 4444);
	cout<<"Ending test for Insert"<<endl;
}

void LinkedListTest::testPopBack()
{
	cout<<"Starting test for Pop Back"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	int testPopBack = list.pop_back();
	CPPUNIT_ASSERT(testPopBack == 333);
	cout<<"Ending test for Pop Back"<<endl;
}

void LinkedListTest::testIndex()
{
	cout<<"Starting test for []"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	CPPUNIT_ASSERT(list[1] == 22);
	CPPUNIT_ASSERT(list[3] == 4444);
	cout<<"Ending test for []"<<endl;
}

void LinkedListTest::testCopyConstructor()
{
	cout<<"Starting test for Copy Constructor"<<endl;
	LinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	LinkedList<int> copyList(list);
	CPPUNIT_ASSERT(list.size() == copyList.size());
	cout<<"Ending test for Copy Constructor";
}

void LinkedListTest::testAssignment()
{
	cout<<"Starting test for Assignment Operator"<<endl;
	LinkedList<int> list;
	LinkedList<int> newList;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	newList = list;
	CPPUNIT_ASSERT(list.size() == list.size());
	cout<<"Ending test for Assignment Operator"<<endl;
}
void LinkedListTest::testEquality()
{
	cout<<"Starting test for Equality"<<endl;
	LinkedList<int> list;
	LinkedList<int> list2;
	list.push_back(1);
	list2.push_back(1);
	CPPUNIT_ASSERT(list == list2);
	cout<<"Ending test for Equality"<<endl;
}

void LinkedListTest::testNotEquality()
{
	cout<<"Starting test for Not Equality"<<endl;
	LinkedList<int> list;
	LinkedList<int> list2;
	list.push_back(1);
	list.push_back(2);
	CPPUNIT_ASSERT(list != list2);
	cout<<"Ending test for Not Equality"<<endl;
}



