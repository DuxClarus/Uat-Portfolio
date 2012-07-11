/*
 * InFixToPostFixCalculator.cpp
 *
 *  Created on: Sep 25, 2011
 *      Author: Ethan Behar
 */

#include "InFixToPostFixCalculator.h"

//public methods
InFixToPostFixCalculator::InFixToPostFixCalculator() :
		expression("0"), expressionAnswer(0)
{
}

InFixToPostFixCalculator::InFixToPostFixCalculator(string mathProblem) :
		expression(mathProblem), expressionAnswer(0)
{
}

InFixToPostFixCalculator::~InFixToPostFixCalculator()
{
}

void InFixToPostFixCalculator::setExpression(string mathProblem)
{
	expression = mathProblem;
}

string InFixToPostFixCalculator::getExpression()
{
	return expression;
}

float InFixToPostFixCalculator::getExpressionAnswer()
{
	return expressionAnswer;
}

void InFixToPostFixCalculator::breakDownExpressionIntoTokens()
{
	char token;
	string number = "";
	istringstream expressionStream(expression);

	while (expressionStream.eof() == false)
	{
		token = expressionStream.get();
		if (getPriority(token) == 0)
		{
			number = token;
			while (getPriority(expressionStream.peek()) == 0)
			{
				token = expressionStream.get();
				number = number + token;
			}
			tokenItems.push_back(number);
		}
		else if (getPriority(token) > 0)
		{
			string tokenString;
			tokenString = token;
			tokenItems.push_back(tokenString);
		}
	}
}

void InFixToPostFixCalculator::convertExpressionToPostFixNotation()
{
	operatorStack.clear();
	for (unsigned int index = 0; index < tokenItems.size(); ++index)
	{
		if (atof(tokenItems[index].c_str()))
		{
			postFixExpression.push_back(tokenItems[index]);
		}

		else if (isLeftParentheses(tokenItems[index]))
		{
			operatorStack.push(tokenItems[index]);
		}

		else if (isRightParentheses(tokenItems[index]))
		{
			while (!isLeftParentheses(operatorStack.top()))
			{
				postFixExpression.push_back(operatorStack.pop());
			}
			operatorStack.pop();
		}

		else if (isOperator(tokenItems[index]))
		{
			if (operatorStack.isEmpty())
			{
				operatorStack.push(tokenItems[index]);
			}
			else
			{
				string currentToken = tokenItems.at(index);
				string operatorString = operatorStack.top();
				while ((!operatorStack.isEmpty())
						&& (getPriority(operatorString[0]) > getPriority(currentToken[0]))
						&& getPriority(operatorString[0]) != 9)
				{
					postFixExpression.push_back(operatorStack.pop());
					if (!operatorStack.isEmpty())
						operatorString = operatorStack.top();
				}
				operatorStack.push(tokenItems[index]);
			}
		}
	}
	while (!operatorStack.isEmpty())
	{
		postFixExpression.push_back(operatorStack.pop());
	}
}

void InFixToPostFixCalculator::solveExpression()
{
	float operand1 = 0;
	float operand2 = 0;
	float answer = 0;
	string element = "";
	float operandElement = 0;
	for (unsigned int index = 0; index < postFixExpression.size(); index++)
	{
		string element = postFixExpression[index];
		if (getPriority(element[0]) == 0)
		{
			operandElement = atof(element.c_str());
			streamStack.push(operandElement);
		}
		else if (getPriority(element[0]) > 0)
		{
			operand2 = streamStack.pop();
			operand1 = streamStack.pop();
			answer = performOperation(operand1, operand2, element[0]);
			streamStack.push(answer);
		}
	}
	expressionAnswer = streamStack.pop();
}

//private methods
void InFixToPostFixCalculator::printPostFixNotation()
{
	cout << "Your expression is: ";
	for (unsigned int index = 0; index < tokenItems.size(); ++index)
	{
		cout << tokenItems[index] << " ";
	}
	cout << endl;

	cout << "Postfix Notation: ";
	for (unsigned int index = 0; index < postFixExpression.size(); ++index)
	{
		cout << postFixExpression[index] << " ";
	}
	cout << endl;
}

bool InFixToPostFixCalculator::isOperator(string element)
{
	if (element == "+" || element == "-" || element == "/" || element == "*")
		return true;
	else
		return false;
}
bool InFixToPostFixCalculator::isLeftParentheses(string element)
{
	if (element == "(")
		return true;
	else
		return false;
}

bool InFixToPostFixCalculator::isRightParentheses(string element)
{
	if (element == ")")
		return true;
	else
		return false;
}

int InFixToPostFixCalculator::getPriority(char element)
{
	switch (element)
	{
	case '-':
		return 1;
	case '+':
		return 1;
	case '/':
		return 5;
	case '*':
		return 5;
	case '(':
		return 9;
	case ')':
		return 9;
	case 'ÿ':
		return -1;
	case ' ':
		return -1;
	default:
		return 0;
	}
}

float InFixToPostFixCalculator::performOperation(float operand1, float operand2,
		char operation)
{
	switch (operation)
	{
	case '-':
		return operand1 - operand2;
	case '+':
		return operand1 + operand2;
	case '/':
		return operand1 / operand2;
	case '*':
		return operand1 * operand2;
	default:
		return 0;
	}
}

