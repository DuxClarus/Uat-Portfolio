//============================================================================
// Name        : ETB.cpp
// Author      :
// Version     :
// Copyright   : Your copyright notice
// Description : Hello World in C++, Ansi-style
//============================================================================


#include "StandardIncludes.h"
#include <cppunit/ui/text/TestRunner.h>
#include "Stack.h"
#include "TestStack.h"


bool doAllTestsFunction()
{
    CppUnit::TextUi::TestRunner runner;
    runner.addTest(TestStack::suite());
    return runner.run();
}

int main()
{
	if (doAllTestsFunction())
	{
		//Your application code goes here
		cout<<"WE ARE THE WINNERS"<<endl;
	}
	return 0;

	/* used during debugging mode...
	Stack<int> myStack(10);
	myStack.push(10);
	cout<<"The Size Is: "<<myStack.size()<<endl;
	myStack.push(5);
	cout<<"The Size Is: "<<myStack.size()<<endl;
	myStack.clear();
	myStack.push(5);
	cout<<"The Size Is: "<<myStack.size()<<endl;
	myStack.push(10);
	cout<<"The Size Is: "<<myStack.size()<<endl;
	myStack.push(15);
	cout<<"The Size Is: "<<myStack.size()<<endl;
	myStack.push(20);
	cout<<"The Size Is: "<<myStack.size()<<endl;
	cout<<"The Top Is: "<<myStack.top()<<endl;
	cout<<"The Size Is: "<<myStack.size()<<endl;
	cout<<"I popped off: "<<myStack.pop()<<endl;
	cout<<"The Size Is: "<<myStack.size()<<endl;
	cout<<"I popped off: "<<myStack.pop()<<endl;
	cout<<"The Size Is: "<<myStack.size()<<endl;
	cout<<"I popped off: "<<myStack.pop()<<endl;
	cout<<"The Size Is: "<<myStack.size()<<endl;
	cout<<"I popped off: "<<myStack.pop()<<endl;
	cout<<"The Size Is: "<<myStack.size()<<endl;
	cout<<"I popped off: "<<myStack.pop()<<endl;
	cout<<"The Size Is: "<<myStack.size()<<endl;
	 */
}
