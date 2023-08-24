using System.Collections;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    [SerializeField] private float deactiveTime = 3.0f;     // 탄피 생성 후 비활성화 시간
    [SerializeField] private float casingSpin = 1.0f;       // 탄피 회전 속도
    [SerializeField] private AudioClip[] audioClips;        // 탄피 충돌 사운드클립

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void SetUp(MemoryPool pool, Vector3 direction)
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;
        
        // 탄피의 속도, 회전 속도
        rigidBody.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidBody.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
            Random.Range(-casingSpin, casingSpin),
            Random.Range(-casingSpin, casingSpin));
        
        // 탄피 자동 비활성화 코루틴
        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision other)
    {
        // 탄피 사운드 랜덤 재생 
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactiveTime);
        
        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
