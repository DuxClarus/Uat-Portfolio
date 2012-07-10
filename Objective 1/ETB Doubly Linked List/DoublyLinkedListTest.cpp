/*
 * CppUnitTest.cpp
 *
 *  Created on: Jun 28, 2011
 *      Author: ethbehar
 */

#include "DoublyLinkedListTest.h"
#include "StandardIncludes.h"

void DoublyLinkedListTest::setUp()
{
	// TODO Your setup code goes here.  Typically you will set your instance
	// variables in this code
}

void DoublyLinkedListTest::tearDown()
{
	// TODO Your tear down code goes here. Typically, you will be deleting any
	// pointers you created in the setUp method
}

void DoublyLinkedListTest::testPushBack()
{
	cout<<"Starting test for Push_Back"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	CPPUNIT_ASSERT(list[3] == 4444);
	cout<<"Ending test for Push_Back"<<endl;
}

void DoublyLinkedListTest::testClear()
{
	cout<<"Starting test for Clear"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	list.push_back(55555);
	list.clear();
	CPPUNIT_ASSERT(list.size() == 0);
	cout<<"Ending test for Clear"<<endl;
}

void DoublyLinkedListTest::testRemoveIndex()
{
	cout<<"Starting test for RemoveIndex"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.removeAtIndex(1);
	CPPUNIT_ASSERT(list.size() == 2);
	cout<<"Ending test for RemoveIndex"<<endl;
}

void DoublyLinkedListTest::testRemove()
{
	cout<<"Starting test for Remove"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	list.push_back(55555);
	list.remove(333);
	CPPUNIT_ASSERT(list[2] == 4444);
	cout<<"Ending tests for Remove"<<endl;
}

void DoublyLinkedListTest::testInsertAtIndex()
{
	cout<<"Starting test for Insert"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.insertAtIndex(1, 4444);
	CPPUNIT_ASSERT(list[1] == 4444);
	cout<<"Ending test for Insert"<<endl;
}

void DoublyLinkedListTest::testPopBack()
{
	cout<<"Starting test for Pop Back"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	int testPopBack = list.pop_back();
	CPPUNIT_ASSERT(testPopBack == 333);
	cout<<"Ending test for Pop Back"<<endl;
}

void DoublyLinkedListTest::testIndex()
{
	cout<<"Starting test for []"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	CPPUNIT_ASSERT(list[1] == 22);
	CPPUNIT_ASSERT(list[3] == 4444);
	cout<<"Ending test for []"<<endl;
}

void DoublyLinkedListTest::testCopyConstructor()
{
	cout<<"Starting test for Copy Constructor"<<endl;
	DoublyLinkedList<int> list;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	list.push_back(4444);
	DoublyLinkedList<int> copyList(list);
	CPPUNIT_ASSERT(list.size() == copyList.size());
	cout<<"Ending test for Copy Constructor";
}

void DoublyLinkedListTest::testAssignment()
{
	cout<<"Starting test for Assignment Operator"<<endl;
	DoublyLinkedList<int> list;
	DoublyLinkedList<int> newList;
	list.push_back(1);
	list.push_back(22);
	list.push_back(333);
	newList = list;
	CPPUNIT_ASSERT(list.size() == list.size());
	cout<<"Ending test for Assignment Operator"<<endl;
}
void DoublyLinkedListTest::testEquality()
{
	cout<<"Starting test for Equality"<<endl;
	DoublyLinkedList<int> list;
	DoublyLinkedList<int> list2;
	list.push_back(1);
	list2.push_back(1);
	CPPUNIT_ASSERT(list == list2);
	cout<<"Ending test for Equality"<<endl;
}

void DoublyLinkedListTest::testNotEquality()
{
	cout<<"Starting test for Not Equality"<<endl;
	DoublyLinkedList<int> list;
	DoublyLinkedList<int> list2;
	list.push_back(1);
	list.push_back(2);
	CPPUNIT_ASSERT(list != list2);
	cout<<"Ending test for Not Equality"<<endl;
}



