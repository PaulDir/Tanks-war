using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 坦克，控制坦克移动和旋转
/// </summary>
public class TankMovement : MonoBehaviour
{

    private Rigidbody rigidbody; // 坦克刚体
    public float speed = 15; // 坦克的移动速度
    public float angularSpeed = 10; // 坦克的旋转速度
    public float tankID = 1; // 坦克的编号，用于控制两种不同的键盘操作

    private AudioSource audioSource; // 声音组件
    public AudioClip idleAudio; // 坦克空闲时的音效
    public AudioClip drivingAudio; // 坦克移动时的音效

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>(); // 获取刚体组件
        audioSource = this.GetComponent<AudioSource>(); // 获取 AudioSource 的组件
    }

    // MonoBehaviour.FixedUpdate():在每个固定帧帧数被调用，在处理 Rigidbody 时，应该使用 FixedUpdate 而不是 Update
    private void FixedUpdate()
    {
        // 控制坦克前后移动
        float v = Input.GetAxis("VerticalPlayer" + tankID); // 通过坦克编号区分控制
        rigidbody.velocity = transform.forward * v * speed; // 修改刚体的速度，这种方式并不符合物理规律

        // 控制坦克左右旋转
        float h = Input.GetAxis("HorizontalPlayer" + tankID);
        // angularVelocity 刚体角速度矢量，单位为弧度/秒。
        rigidbody.angularVelocity = transform.up * h * angularSpeed; // 绕 Y 轴旋转

        if (Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1) // 坦克移动时
        {
            audioSource.clip = drivingAudio; // 设置音效
            if (audioSource.isPlaying == false) // 如果没有播放，播放
                audioSource.Play();
        }
        else // 坦克空闲时
        {
            audioSource.clip = idleAudio;
            if (audioSource.isPlaying == false)
                audioSource.Play();
        }
    }
}
