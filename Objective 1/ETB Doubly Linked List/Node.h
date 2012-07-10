/*
 * Node.h
 *
 *  Created on: Sep 30, 2011
 *      Author: ethbehar
 */

#ifndef NODE_H_
#define NODE_H_

#include "StandardIncludes.h"

template<class T>
class Node
{
public:
	Node();
	Node(T data);
	virtual ~Node();
	T getData();
	T data;
	Node* next;
	Node* previous;
};

template<class T>
Node<T>::Node()
{
}

template<class T>
Node<T>::Node(T data) : data(data), next(NULL), previous(NULL)
{
}

template<class T>
Node<T>::~Node()
{
}

template<class T>
T Node<T>::getData()
{
	return data;
}


#endif /* NODE_H_ */
