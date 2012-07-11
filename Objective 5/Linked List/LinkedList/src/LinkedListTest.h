/*
 * CppUnitTest.h
 *
 *  Created on: Oct 7, 2010
 *      Author: hartley
 */

#ifndef CPPUNITTEST_H_
#define CPPUNITTEST_H_
#include <cppunit/TestFixture.h>
#include <cppunit/extensions/HelperMacros.h>
#include "LinkedList.h"

class LinkedListTest: public CPPUNIT_NS::TestFixture
{
    CPPUNIT_TEST_SUITE (LinkedListTest);
	// TODO Repeat one of the following lines for each of your test methods
    CPPUNIT_TEST(testPushBack);
    CPPUNIT_TEST(testClear);
    CPPUNIT_TEST(testRemoveIndex);
    CPPUNIT_TEST(testRemove);
    CPPUNIT_TEST(testInsert);
    CPPUNIT_TEST(testPopBack);
    CPPUNIT_TEST(testIndex);
    CPPUNIT_TEST(testCopyConstructor);
    CPPUNIT_TEST(testAssignment);
    CPPUNIT_TEST(testEquality);
    CPPUNIT_TEST(testNotEquality);
    CPPUNIT_TEST_SUITE_END ();
public:
    void setUp(void);
    void tearDown(void);
protected:
	// TODO Define your test methods here
    void testPushBack();
    void testClear();
    void testRemoveIndex();
    void testRemove();
    void testInsert();
    void testPopBack();
    void testIndex();
    void testCopyConstructor();
    void testAssignment();
    void testEquality();
    void testNotEquality();


private:
};

#endif /* CPPUNITTEST_H_ */
