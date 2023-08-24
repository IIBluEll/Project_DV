/// <summary>
/// 차후 무기의 다양성을 추가하기 위해 공용으로 사용하는 변수들을 구조체로 묶어 정의
/// </summary>

public enum WeaponName {AssaultRifle}

[System.Serializable]
public class WeaponSetting
{
    public WeaponSetting weaponSetting;

    public float attackRate;        // 발사 속도
    public float attackDistance;    // 최대 사거리
    public bool isAutomaticAttack;  // 연사 가능 여부

    public int currentAmmo;         // 현재 탄약 수
    public int amountAmmo;             // 전체 탄약 수
    public int maxSizeMagazine;     // 탄창에 들어가는 최대 탄약 수

}
