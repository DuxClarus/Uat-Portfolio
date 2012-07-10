#include "StandardIncludes.h"
#include <cppunit/ui/text/TestRunner.h>
#include "DoublyLinkedListTest.h"
#include "DoublyLinkedList.h"

bool doAllFunctionTest()
{
    CppUnit::TextUi::TestRunner runner;
    runner.addTest(DoublyLinkedListTest::suite());
    return runner.run();
}

int main()
{
	if(doAllFunctionTest())
	{
		cout << "Sup Dawg" << endl;
	}
	return 0;
}
