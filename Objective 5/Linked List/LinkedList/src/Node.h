/*
 * Node.h
 *
 *  Created on: Jun 28, 2011
 *      Author: ethbehar
 */

#ifndef NODE_H_
#define NODE_H_

//templated Node class
//This class will use 1 varible to hold the data of the Node
//and use another variable to point to the next Node in the Linked List
template <class T>
class Node
{
public:
	Node(T data) : data(data), next(NULL)
	{
	}

	T data;
	Node* next;
};
#endif /* NODE_H_ */
