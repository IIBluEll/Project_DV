using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
   // 메모리 풀로 관리되는 오브젝트 정보
    private class PoolItem
    {
        public bool isActive;           // 활성화 / 비활성화 정보
        public GameObject gameObject;   // 화면에 보이는 오브젝트
    }

    private int increaseCount = 5;      // 오브젝트가 부족할 때 추가 생성되는 오브젝트 개수
    private int maxCount;               // 현재 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;            // 현재 게임에 사용되고 있는 오브젝트 개수

    private GameObject poolObject;      // 오브젝트 풀링에서 관리되는 오브젝트
    private List<PoolItem> poolItemList; // 관리되는 모든 오브젝트 리스트

    public int MaxCount => maxCount;
    public int ActiveCount => activeCount;

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }
    
    // increaseCount 단위로 오브젝트 생성
    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }
    
    // 관리중인 모든 오브젝트 삭제
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        
        poolItemList.Clear();
    }
    
    // PoolItemList에 저장되있는 오브젝트 활성화후 사용
    // 모든 오브젝트 사용중일 경우 추가 생성
    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;
        
        // 현재 생성해서 관리하는 모든 오브젝트 개수와 활성화된 오브젝트 개수 비교
        // 모든 오브젝트가 활성화 되어있을 경우 새로운 오브젝트 생성
        if (maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }
    
    // 사용이 완료된 오브젝트를 비활성화
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }
    
    // 사용중인 모든 오브젝트를 비활성화
    public void DeactiveAllPoolItem()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}
