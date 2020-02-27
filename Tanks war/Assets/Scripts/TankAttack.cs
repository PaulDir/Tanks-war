using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 坦克，控制坦克开火
/// </summary>
public class TankAttack : MonoBehaviour
{
    private Transform firePosition; // 开火位置
    public GameObject shell; // 子弹，需要赋值
    public KeyCode fireKey = KeyCode.Space; // 开火按键
    public float shellSpeed = 15; // 子弹发射速度
    public AudioClip shoutAudio; // 发射子弹的声音

    // Use this for initialization
    void Start()
    {
        // Transform.Find，public Transform Find(string name); 按名称查找子元素并返回它。
        firePosition = transform.Find("FirePosition"); // 查找开火位置
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(fireKey))
        {
            AudioSource.PlayClipAtPoint(shoutAudio, transform.position); // 播放发射子弹的音效
            GameObject obj = GameObject.Instantiate(shell, firePosition.position, firePosition.rotation) as GameObject; // 实例化子弹
            obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * shellSpeed;

        }
    }
}
