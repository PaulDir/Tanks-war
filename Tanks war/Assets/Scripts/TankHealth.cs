using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 坦克，坦克的生命处理
/// </summary>
public class TankHealth : MonoBehaviour
{

    public int hpNow = 100; // 坦克的生命值
    public GameObject tankExplosion; // 坦克死亡的爆炸效果

    public AudioClip tankExplosionAudio; // 坦克爆炸音效

    public Slider hpSlider; // 血条
    private int hpTotal; // 血量


    private void Start()
    {
        hpTotal = hpNow;
    }

    /// <summary>
    /// 收到消息，坦克受到伤害
    /// </summary>
    void TankDamage()
    {
        if (hpNow <= 0) return;
        hpNow = hpNow - Random.Range(20, 30); // 随机减少生命值
        hpSlider.value = 1 - ((float)hpNow / hpTotal); // 设置血条的值
        if (hpNow <= 0) // 收到伤害后，如果死亡
        {
            AudioSource.PlayClipAtPoint(tankExplosionAudio, transform.position); // 播放坦克爆炸的音效
            GameObject.Instantiate(tankExplosion, transform.position, transform.rotation); // 生成爆炸效果
            GameObject.Destroy(this.gameObject); // 销毁坦克
        }
    }
}
