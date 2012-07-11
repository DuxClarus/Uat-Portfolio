/*
 * LinkedList.h
 *
 *  Created on: Jun 21, 2011
 *      Author: ethbehar
 */

#ifndef LINKEDLIST_H_
#define LINKEDLIST_H_

#include "Node.h"
#include "StandardIncludes.h"

//Template Linked List Class
template <class T>
class LinkedList
{
public:
	LinkedList();
	virtual ~LinkedList();
	LinkedList(const LinkedList& toBeCopiedList);

	LinkedList& operator=(const LinkedList& rhs);
	bool operator==(const LinkedList& rhs);
	bool operator!=(const LinkedList& rhs);
	T& operator[](unsigned int index) const;

	void clear();
	void insert(unsigned int index, T data);
	void removeIndex(unsigned int index);
	void remove(T data);
	void push_back(T data);
	T& pop_back();
	unsigned int size() const;

private:
	Node<T>* head;
	unsigned int actualSize;
	Node<T>* findLastNode();
	Node<T>* findNodeAtIndex(int index);
	bool inspectElements(const LinkedList<T>& rhs, bool isEqual, bool isNotEqual);
};

//public methods
//Constructor Method
//Basic constructor setting head to NULL and actualSize to zero.
template <class T>
LinkedList<T>::LinkedList() : head(NULL), actualSize(0)
{
}

//Denstructor Method
//Basic destructor which calls the method clear().
template <class T>
LinkedList<T>::~LinkedList()
{
	cout<<"Destructor called"<<endl;
	clear();
}

//Copy Constructor: takes in a reference to a LinkedList
//Copy constructor sets the head to NULL immediately.
//Iterates over the toBeCopiedList and push backs all of the Nodes from toBeCopiedList.
template<class T>
LinkedList<T>::LinkedList(const LinkedList<T>& toBeCopiedList) :
	head(NULL)
{
	cout << "Copy Constructor called" << endl;
	for(unsigned int index = 0; index < toBeCopiedList.size(); ++index)
	{
		push_back(toBeCopiedList[index]);
	}
	this->actualSize = toBeCopiedList.size();
}

//Assignment Overloading: takes in a reference to a LinkedList
//Assignment checks if the lists are the same via != operator.
//Clears the list to make sure its empty then push backs all of the rhs Nodes into this list.
//returns this list
template <class T>
LinkedList<T>& LinkedList<T>::operator=(const LinkedList<T>& rhs)
{
	if(this != &rhs)
	{
		clear();
		Node<T>* toBeCopiedNode = rhs.head;
		head = new Node<T>(toBeCopiedNode->data);
		while(toBeCopiedNode->next != NULL)
		{
			toBeCopiedNode = toBeCopiedNode->next;
			push_back(toBeCopiedNode->data);
			actualSize++;
		}
	}
	return *this;
}

//Equality Overloading: takes in a reference to a LinkedList
//Equality operator first checks if the sizes are not the same if so return false.
//Then, goes to inspectElements() to check if the data is the same.
//returns boolean value
template <class T>
bool LinkedList<T>::operator==(const LinkedList<T>& rhs)
{
	if(rhs.size() != actualSize)
	{
		return false;
	}
	else
	{
		return inspectElements(rhs, false, true);
	}
}

//NotEquality Overloading: takes in a reference to a LinkedList
//NotEquality operator first checks if the sizes are not the same if so return true.
//Then, goes to inspectElements() to check if the data is the not the same.
//returns boolean value
template <class T>
bool LinkedList<T>::operator!=(const LinkedList<T>& rhs)
{
	if(rhs.size() != actualSize)
	{
		return true;
	}
	else
	{
		return inspectElements(rhs, true, false);
	}
}

//Bracet Overloading: takes in desired index value
//Checks if index is greater then actualSize if so returns error message.
//Then, iterates over the LinkedList to desired spot and returns that data.
//returns type T data, T being the type of data in the list
template <class T>
T& LinkedList<T>::operator[](unsigned int index) const
{
	Node<T>* currentNode = this->head;
	if((unsigned int) index > actualSize)
	{
		cout<<"Index is greater then size of list!"<<endl;
	}
	else
	{
		while(index > 0)
		{
			currentNode = currentNode->next;
			index--;
		}
	}
	return currentNode->data;
}

//clear Method: takes no parameters
//Check is list is greater then zero if so,
//it iterates through all the Nodes deleting the head and making head->next = head.
template <class T>
void LinkedList<T>::clear()
{
	if(actualSize > 0)
	{
		Node<T>* toBeDeleted = head->next;
		while(head->next != NULL)
		{
			delete head;
			head = toBeDeleted;
			toBeDeleted = head->next;
		}
		delete toBeDeleted;
		delete head;
	}
	actualSize = 0;
}

//insert Method : takes in the index and the data
//Checks to see if index is greater then the size of list if so push backs data.
//If desired spot is 0 check if list is empty is so define head node with data.
//If there is a head, we push the head back and make a new head with desired data.
//Lastly, if none of the above cases are true it inserts the data in desried spot.
template <class T>
void LinkedList<T>::insert(unsigned int index, T data)
{
	if(index >= this->size())
	{
		push_back(data);
	}
	else if(index == 0)
	{
		Node<T>* newNode = new Node<T>(data);
		if(head == NULL)
		{
			head = newNode;
		}
		else
		{
			newNode->next = head;
			head = newNode;
			this->actualSize++;
		}
	}
	else
	{
		Node<T>* newNode = new Node<T>(data);
		Node<T>* currentNode = findNodeAtIndex(index);
		newNode->next = currentNode->next;
		currentNode->next = newNode;
		this->actualSize++;
	}
}

//removeIndex Method: takes in desired index to be deleted
//Iterates through the list until it reaches the desired index.
//Then, proceeds to delete the Node at the index.
template <class T>
void LinkedList<T>::removeIndex(unsigned int index)
{
	Node<T>* currentNode;
	currentNode = head;
	unsigned int currentIndex = 0;
	while(currentIndex != index)
	{
		currentNode = currentNode->next;
		currentIndex++;
	}
	Node<T>* toBeDeleted = currentNode->next;
	currentNode->next = toBeDeleted->next;
	toBeDeleted->next = NULL;
	delete toBeDeleted;
	this->actualSize--;
}

//remove Method: takes in data to be deleted
//Iterates through the list until specified data is found then deletes it.
template <class T>
void LinkedList<T>::remove(T data)
{
	Node<T>* currentNode;
	currentNode = head;
	while(currentNode->next != NULL && currentNode->next->data != data)
	{
		currentNode = currentNode->next;
	}
	Node<T>* toBeDeleted = currentNode->next;
	currentNode->next = toBeDeleted->next;
	toBeDeleted->next = NULL;
	delete toBeDeleted;
	this->actualSize--;
}
//push_back Method: takes in data to add
//Checks for special case of empty list if true adds data to head.
//Else it finds the last Node and attaches it to the last Node.
template <class T>
void LinkedList<T>::push_back(T data)
{
	Node<T>* newNode = new Node<T>(data);
	if(head == NULL)
	{
		head = newNode;
	}
	else
	{
		Node<T>* lastNode = findLastNode();
		lastNode->next = newNode;
	}
	this->actualSize++;
}

//pop_back Method: takes no parameters
//Checks if list is empty if empty error message displayed.
//If LinkedList has 1 element if list has 1 element it deletes head.
//Otherwise finds last Node and deletes the Node and returns the data.
template <class T>
T& LinkedList<T>::pop_back()
{
	if(head == NULL)
	{
		cout<<"List is empty; nothing to pop."<<endl;
	}
	else if(actualSize == 1)
	{
		T data = head->data;
		delete head;
		this->actualSize--;
		return data;
	}
	else
	{
		Node<T>* currentNode = head;
		Node<T>* toBeDeleted;
		while(currentNode->next->next != NULL)
		{
			currentNode = currentNode->next;
		}
		toBeDeleted = currentNode->next;
		currentNode->next = toBeDeleted->next;
		T data = toBeDeleted->data;
		delete toBeDeleted;
		this->actualSize--;
		return data;
	}
}

//size Method: takes no parameters
//ActualSize is kept tracked in methods that add/remove Nodes properly incrementing/decrementing actualSize.
//returns int actualSize
template <class T>
unsigned int LinkedList<T>::size() const
{
	return actualSize;
}

//private methods

//findLastNode Method: takes no parameters
//The program already checks for an empty list before findLastNode() is called.
//Finds the last node in LinkedList.
//returns the last Node
template <class T>
Node<T>* LinkedList<T>::findLastNode()
{
	Node<T>* currentNode = this->head;
	while(currentNode->next != NULL)
	{
		currentNode = currentNode->next;
	}
	return currentNode;
}

//findLastNode Method: takes in desired index
//The program already checks for an empty list before findLastNode() is called.
//Finds the node at desired index in LinkedList.
//returns that Node
template <class T>
Node<T>* LinkedList<T>::findNodeAtIndex(int index)
{
	int currentIndex = 0;
	Node<T>* currentNode = NULL;
	currentNode = head;
	while(currentIndex != (index - 1))
	{
		currentNode = currentNode->next;
		currentIndex++;
	}
	return currentNode;
}

//inspectElements: takes in a LinkedList and two boolean values
//Iterates throughout two lists comparing each data in each Node.
//Depending if the != or == operator is used depends on which bool value is returned.
//returns one boolean value
template <class T>
bool LinkedList<T>::inspectElements(const LinkedList<T>& rhs, bool isNotEqual, bool isEqual)
{
	Node<T>* currentNode = head;
	Node<T>* rhsNode = rhs.head;
	while(currentNode->next != NULL)
	{
		if(rhsNode->data != currentNode->data)
		{
			return isNotEqual;
		}
		rhsNode = rhsNode->next;
		currentNode = currentNode->next;
	}
	return isEqual;
}

#endif /* LINKEDLIST_H_ */
