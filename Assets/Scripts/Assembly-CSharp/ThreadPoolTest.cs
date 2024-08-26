using System.Threading;
using UnityEngine;

public class ThreadPoolTest : MonoBehaviour
{
	private int maxThreads = 2;

	private static readonly object _countLock = new object();

	private static int _threadCount = 0;

	private static bool closingApp = false;

	private int clickCounter;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			ThreadInfo threadInfo = new ThreadInfo();
			threadInfo.myVector = Random.insideUnitSphere * 10f;
			threadInfo.threadIndex = ++clickCounter;
			MonoBehaviour.print("Queue new thread #" + threadInfo.threadIndex);
			ThreadPool.QueueUserWorkItem(MyWorkerThread, threadInfo);
		}
	}

	private void MyWorkerThread(object a)
	{
		while (!closingApp)
		{
			lock (_countLock)
			{
				if (_threadCount < maxThreads && !closingApp)
				{
					_threadCount++;
					break;
				}
			}
			Thread.Sleep(50);
		}
		if (!closingApp)
		{
			ThreadInfo obj = a as ThreadInfo;
			Vector3 myVector = obj.myVector;
			int threadIndex = obj.threadIndex;
			MonoBehaviour.print("---From thread #" + threadIndex + " processing myVector " + myVector);
			Thread.Sleep(5000);
			MonoBehaviour.print("---Finished thread #" + threadIndex);
			_threadCount--;
		}
	}

	private void OnDestroy()
	{
		closingApp = true;
	}
}
