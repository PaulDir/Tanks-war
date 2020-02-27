using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机：让相机跟随两个坦克的正中心
/// </summary>
public class Follow : MonoBehaviour
{

    public Transform player1; // 坦克 1
    public Transform player2; //坦克 2

    private Camera mainCamera; // 主相机
    private Vector3 offect; // 偏移，相机和两个坦克正中心

    private float size; // 相机应该处于的位置

    void Start()
    {
        offect = transform.position - (player1.position + player2.position) / 2; // 两个坦克的正中心
        mainCamera = this.GetComponent<Camera>(); // 获取相机
        mainCamera.orthographic = true; // 将相机设置为正交法

    }

    void Update()
    {
        if (player1 == null || player2 == null) return; // 如果任何一个坦克消失，游戏结束，取消相机跟随
        transform.position = offect + (player1.position + player2.position) / 2; // 两个坦克的中心位置
        float distance = Vector3.Distance(player1.position, player2.position); // 两个坦克的距离
        if (distance < 9) // 即使两个坦克的距离很近，相机的最小视野也应是一个最佳倍数
            size = 9f; // 相机视野的距离 = 初始倍数
        else // 两个坦克距离很远
            size = distance * 1; // 相机视野的距离 = 两个坦克的距离 * 一个合适的倍数
        mainCamera.orthographicSize = size; // 设置相机视野
    }
}
