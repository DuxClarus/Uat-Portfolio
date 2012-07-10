/*
 * Stack.h
 *
 *  Created on: Sep 16, 2011
 *      Author: Ethan Behar
 */

#ifndef STACK_H_
#define STACK_H_

#include "StandardIncludes.h"

template<class T>
class Stack
{

public:
	const static unsigned int DEFAULT_NUMBER_OF_ELEMENTS = 100;
	Stack();
	Stack(unsigned int numberOfElements);
	virtual ~Stack();
	void push(T item);
	T pop();
	T top();
	bool isEmpty();
	unsigned int size();
	void clear();

private:
	unsigned int numberOfElements;
	unsigned int itemsOnStack;
	T* stackElements;
	unsigned int stackPointer;
};

//deafult Constructor: numberOfElements defaulted to 100
template<class T>
Stack<T>::Stack() : numberOfElements(DEFAULT_NUMBER_OF_ELEMENTS), itemsOnStack(0), stackElements(new T[numberOfElements]), stackPointer(0)
{
}

//Constructor: numberOfElements is user-picked
template<class T>
Stack<T>::Stack(unsigned int numberOfElements) :  numberOfElements(numberOfElements), itemsOnStack(0), stackElements(new T[numberOfElements]), stackPointer(0)
{
}

//Destructor: deletes the stack
template<class T>
Stack<T>::~Stack()
{
	delete [] stackElements;
}

//Push method: puts given item on top of stack
//Increments stackPointer, and itemsOnStack by one. Also, checks if memory is avaiable, if not tells user.
template<class T> void Stack<T>::push(T item)
{
	if(numberOfElements == itemsOnStack)
	{
		cout<<"Stack is full"<<endl;
	}
	else{
		stackElements[stackPointer] = item;
		stackPointer++;
		itemsOnStack++;
	}
}

//Pop method: removes and returns item on top of stack
//Decrements stackPointer, and itemsOnStack by one. Also, checks if the stack has elements, if not tells user.
template<class T> T Stack<T>::pop()
{
	if(isEmpty())
	{
		cout<<"Nothing to pop"<<endl;
		return 0;
	}
	else{
		stackPointer--;
		T topItem = stackElements[stackPointer];
		itemsOnStack--;
		return topItem;
	}
}

//Top method: returns item on top of stack
//Checks if the stack has elements, if not tells user.
template<class T> T Stack<T>::top()
{
	if(isEmpty())
	{
		cout<<"Nothing on top"<<endl;
		return 0;
	}
	else{
		return stackElements[stackPointer-1];
	}
}

//isEmpty method: checks if stack is empty
//Returns true or false.
template<class T> bool Stack<T>::isEmpty()
{
	if(size() == 0) return true;
	return false;
}

//Size Method: returns the size
template<class T> unsigned int Stack<T>::size()
{
	return itemsOnStack;
}

//Clear Method: clears the stack
//Decrements stackPointer and itemsOnStack to zero.
template<class T> void Stack<T>::clear()
{
	while(itemsOnStack > 0)
	{
		stackPointer--;
		itemsOnStack--;
	}
}



#endif /* STACK_H_ */
