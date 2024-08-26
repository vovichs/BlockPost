using System.Threading;
using UnityEngine;

public class ThreadMap : MonoBehaviour
{
	private class ThreadChunkData
	{
		public int threadIndex;

		public int[,,] block;

		public Color[,,] color;

		private Mesh mesh;

		private Mesh meshGreedy;
	}

	private int counter;

	private int maxThreads = 2;

	private static bool closingApp = false;

	private static readonly object _countLock = new object();

	private static int _threadCount = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			Build(null);
		}
	}

	private void Build(Map.CChunk c)
	{
		if (c == null)
		{
			return;
		}
		ThreadChunkData threadChunkData = new ThreadChunkData();
		threadChunkData.block = new int[Map.CHUNK_SIZE_X + 2, Map.CHUNK_SIZE_Y + 2, Map.CHUNK_SIZE_Z + 2];
		threadChunkData.color = new Color[Map.CHUNK_SIZE_X + 2, Map.CHUNK_SIZE_Y + 2, Map.CHUNK_SIZE_Z + 2];
		for (int i = 0; i < Map.CHUNK_SIZE_X; i++)
		{
			for (int j = 0; j < Map.CHUNK_SIZE_Z; j++)
			{
				for (int k = 0; k < Map.CHUNK_SIZE_Y; k++)
				{
					threadChunkData.block[i + 1, k + 1, j + 1] = c.block[i, k, j];
					threadChunkData.color[i + 1, k + 1, j + 1] = c.color[i, k, j];
				}
			}
		}
		threadChunkData.threadIndex = ++counter;
		MonoBehaviour.print("Queue new thread #" + threadChunkData.threadIndex);
		ThreadPool.QueueUserWorkItem(WorkerThread, threadChunkData);
	}

	private void WorkerThread(object a)
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
			int threadIndex = (a as ThreadChunkData).threadIndex;
			Thread.Sleep(5000);
			if (!closingApp)
			{
				MonoBehaviour.print("---Finished thread #" + threadIndex);
				_threadCount--;
			}
		}
	}

	private void OnDestroy()
	{
		closingApp = true;
	}
}
