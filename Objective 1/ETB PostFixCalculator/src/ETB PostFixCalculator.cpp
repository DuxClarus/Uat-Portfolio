//============================================================================
// Name        : ETB.cpp
// Author      : Ethan Behar
// Version     :
// Copyright   : Copy it I dare you!
// Description : Hello World in C++, Ansi-style
//============================================================================

#include "StandardIncludes.h"
#include "InFixToPostFixCalculator.h"

int main()
{
	InFixToPostFixCalculator c;

	c.setExpression("(4*(3+3))*12+12*12/12");

	c.breakDownExpressionIntoTokens();

	c.convertExpressionToPostFixNotation();

	c.solveExpression();

	c.printPostFixNotation();

	cout << "The answer is: " << c.getExpressionAnswer();
	return 0;
}
