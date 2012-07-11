/*
 * InFixToPostFixCalculator.h
 *
 *  Created on: Sep 25, 2011
 *      Author: Ethan Behar
 */

#ifndef INFIXTOPOSTFIXCALCULATOR_H_
#define INFIXTOPOSTFIXCALCULATOR_H_

#include "StandardIncludes.h"
#include "Stack.h"


class InFixToPostFixCalculator
{
public:
	InFixToPostFixCalculator();
	InFixToPostFixCalculator(string mathProblem);
	virtual ~InFixToPostFixCalculator();
	void setExpression(string mathProblem);
	string getExpression();
	float getExpressionAnswer();
	void breakDownExpressionIntoTokens();
	void convertExpressionToPostFixNotation();
	void solveExpression();
	void printPostFixNotation();

private:
	bool isOperator(string element);
	bool isLeftParentheses(string element);
	bool isRightParentheses(string element);
	int getPriority(char element);
	float performOperation(float operand1, float operand2, char operation);
	vector<string> tokenItems;
	vector<string> postFixExpression;
	string expression;
	float expressionAnswer;
	Stack<float> streamStack;
	Stack<string> operatorStack;
};

#endif /* INFIXTOPOSTFIXCALCULATOR_H_ */
