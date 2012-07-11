package edu.uat.csc318.sp12044.etb.mastermind

public class ObserverManager implements Observable {

	private List<Observer> observerList = new ArrayList<Observer>()
	public static instanceOfObserverManager
	
	public static getDefault()
	{
		if(!instanceOfObserverManager)
		{
			instanceOfObserverManager = new ObserverManager()
		}
		return instanceOfObserverManager
	}
	
	@Override
	public void addObserver(Observer inObserver)
	{
		observerList.add(inObserver)
	}
	
	public void updateObservers()
	{
		for(observer in observerList)
		{
			observer.update()
		}
	}
}
