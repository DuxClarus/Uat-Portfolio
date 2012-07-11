/*
 * Main.cpp
 *
 *  Created on: Jun 21, 2011
 *      Author: ethbehar
 */

#include "StandardIncludes.h"
#include <cppunit/ui/text/TestRunner.h>
#include "LinkedListTest.h"
#include "LinkedList.h"

bool doAllFunctionTest()
{
    CppUnit::TextUi::TestRunner runner;
    runner.addTest(LinkedListTest::suite());
    return runner.run();
}

int main()
{
	if(doAllFunctionTest())
	{
		//runApplication();
	}
	return 0;
}
