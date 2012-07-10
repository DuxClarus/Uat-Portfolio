/*
 * TestStack.h
 *
 *  Created on: Sep 16, 2011
 *      Author: ethbehar
 */

#ifndef TESTSTACK_H_
#define TESTSTACK_H_
#include <cppunit/TestFixture.h>
#include <cppunit/extensions/HelperMacros.h>
#include "Stack.h"
#include "StandardIncludes.h"

class TestStack: public CPPUNIT_NS::TestFixture
{
    CPPUNIT_TEST_SUITE(TestStack);
	// TODO Repeat one of the following lines for each of your test methods
    CPPUNIT_TEST(testIsEmpty);
    CPPUNIT_TEST(testPush);
    CPPUNIT_TEST(testSize);
    CPPUNIT_TEST(testPop);
    CPPUNIT_TEST(testTop);
    CPPUNIT_TEST(testClear);
    CPPUNIT_TEST_SUITE_END();
public:
    void setUp(void);
    void tearDown(void);
protected:
	// TODO Define your test methods here
    void testIsEmpty();
    void testPush();
    void testSize();
    void testPop();
    void testTop();
    void testClear();
private:
	// TODO Define any instance variables your tests will use here
    Stack<int> *testIntStack;
    int testInt;

};

#endif /* TESTSTACK_H_ */
