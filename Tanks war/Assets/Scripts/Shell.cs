using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹，子弹的触发检测，产生爆炸效果
/// </summary>
public class Shell : MonoBehaviour
{
    public GameObject shellExplosion; // 爆炸效果，需勾选爆炸效果的 Play On Awake，即生成就开始
    public AudioClip shellExplosionAudio;

    // 触发检测
    private void OnTriggerEnter(Collider collider)
    {
        AudioSource.PlayClipAtPoint(shellExplosionAudio, transform.position);

        GameObject.Instantiate(shellExplosion, transform.position, transform.rotation); // 实例化爆炸效果，位置就是当前子弹的位置
        GameObject.Destroy(this.gameObject); // 把自身销毁

        // 如果子弹碰撞到了 Tank，给 Tank 发送一条消息，Tank 里会创建一个 TankDamage 方法来处理消息
        if (collider.tag == "Tank")
        {
            collider.SendMessage("TankDamage");
        }
    }
}
