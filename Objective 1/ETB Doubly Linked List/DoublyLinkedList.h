/*
 * DoublyLinkedList.h
 *
 *  Created on: Sep 30, 2011
 *      Author: ethbehar
 */

#ifndef DOUBLYLINKEDLIST_H_
#define DOUBLYLINKEDLIST_H_

#include "StandardIncludes.h"
#include "Node.h"

template<class T>
class DoublyLinkedList
{
public:
	DoublyLinkedList();
	DoublyLinkedList(DoublyLinkedList<T>& toBeCopied);
	virtual ~DoublyLinkedList();

	DoublyLinkedList<T>& operator=(DoublyLinkedList<T>& toBeCopied);
	T& operator[](unsigned int index);
	bool operator==(DoublyLinkedList<T>& rhs);
	bool operator!=(DoublyLinkedList<T>& rhs);

	void push_back(T data); //done
	T pop_back(); //done
	void insertAtIndex(unsigned int index, T data); //done
	void clear(); //done
	void remove(T data); //done
	void removeAtIndex(unsigned int index); //done
	unsigned int size(); //done

private:
	Node<T>* head; //done
	Node<T>* findLastNode(); //done
	Node<T>* findNodeAtIndex(int index); //done
	Node<T>* findPreviousNode(Node<T>* currentNode); //done
	bool inspectElements(const DoublyLinkedList<T>& rhs, bool isEqual, bool isNotEqual);
	unsigned int actualSize;
};

template<class T>
DoublyLinkedList<T>::DoublyLinkedList() :
		head(NULL), actualSize(0)
{
}

template<class T>
DoublyLinkedList<T>::DoublyLinkedList(DoublyLinkedList<T>& toBeCopiedList) :
		head(NULL)
{
	cout << "Copy Constructor called" << endl;
	for (unsigned int index = 0; index < toBeCopiedList.size(); ++index)
	{
		push_back(toBeCopiedList[index]);
	}
	this->actualSize = toBeCopiedList.size();
}

template<class T>
DoublyLinkedList<T>::~DoublyLinkedList()
{
	clear();
}

template<class T>
DoublyLinkedList<T>& DoublyLinkedList<T>::operator=(DoublyLinkedList<T>& toBeCopiedList)
{
	if(this != &toBeCopiedList)
	{
		clear();
		Node<T>* toBeCopiedNode = toBeCopiedList.head;
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

template<class T>
T& DoublyLinkedList<T>::operator[](unsigned int index)
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

template<class T>
bool DoublyLinkedList<T>::operator==(DoublyLinkedList& rhs)
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

template<class T>
bool DoublyLinkedList<T>::operator!=(DoublyLinkedList& rhs)
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

template<class T>
void DoublyLinkedList<T>::push_back(T data)
{
	Node<T>* newNode = new Node<T>(data);
	if (head == NULL)
	{
		head = newNode;
	}
	else
	{
		Node<T>* lastNode = findLastNode();
		lastNode->next = newNode;
		newNode->previous = findPreviousNode(newNode);
	}
	this->actualSize++;
}

template<class T>
T DoublyLinkedList<T>::pop_back()
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

template<class T>
void DoublyLinkedList<T>::insertAtIndex(unsigned int index, T data)
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
		newNode->previous = currentNode;
		currentNode->next = newNode;
		this->actualSize++;
	}
}

template<class T>
void DoublyLinkedList<T>::clear()
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

template<class T>
void DoublyLinkedList<T>::remove(T data)
{
	Node<T>* currentNode;
	currentNode = head;
	while(currentNode->next != NULL && currentNode->next->data != data)
	{
		currentNode = currentNode->next;
	}
	Node<T>* toBeDeleted = currentNode->next;
	currentNode->next = toBeDeleted->next;
	toBeDeleted->next->previous = currentNode;
	toBeDeleted->next = NULL;
	toBeDeleted->previous = NULL;
	delete toBeDeleted;
	this->actualSize--;
}

template<class T>
void DoublyLinkedList<T>::removeAtIndex(unsigned int index)
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
	toBeDeleted->next->previous = currentNode;
	toBeDeleted->next = NULL;
	toBeDeleted->previous = NULL;
	delete toBeDeleted;
	this->actualSize--;
}

template<class T>
unsigned int DoublyLinkedList<T>::size()
{
	return actualSize;
}

//PRIVATE METHODS
template<class T>
Node<T>* DoublyLinkedList<T>::findLastNode()
{
	Node<T>* currentNode = this->head;
	while (currentNode->next != NULL)
	{
		currentNode = currentNode->next;
	}
	return currentNode;
}

template <class T>
Node<T>* DoublyLinkedList<T>::findNodeAtIndex(int index)
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

template<class T>
Node<T>* DoublyLinkedList<T>::findPreviousNode(Node<T>* currentNode)
{
	Node<T>* temporaryNode = this->head;
	while(temporaryNode->next != currentNode)
	{
		temporaryNode = temporaryNode->next;
	}
	return temporaryNode;
}

template <class T>
bool DoublyLinkedList<T>::inspectElements(const DoublyLinkedList<T>& rhs, bool isNotEqual, bool isEqual)
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

#endif /* DOUBLYLINKEDLIST_H_ */
